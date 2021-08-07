using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using HarmonyLib;
using JET.Utilities;
using JET.Utilities.HTTP;
using JET.Utilities.Patching;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;

namespace JET.Patches.Bundles
{
    public class EasyAssetsPatch : GenericPatch<EasyAssetsPatch>
    {
        private const string BUNDLE_URL = "/singleplayer/bundles";
        private static WebClient Client = new WebClient { CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore) };

        public EasyAssetsPatch() : base(prefix: nameof(PatchPrefix)) { }

        protected override MethodBase GetTargetMethod()
        {
            var nodeInterfaceType = PatcherConstants.TargetAssembly.GetTypes().First(x => x.IsInterface && x.GetProperty("SameNameAsset") != null);
            var targetType = PatcherConstants.TargetAssembly.GetTypes().Single(IsTargetType).MakeGenericType(nodeInterfaceType);
            
            return targetType.GetConstructors().First();
        }

        private static bool IsTargetType(Type type)
        {
            return type.IsClass && type.GetMethod("GetNode") != null && string.IsNullOrWhiteSpace(type.Namespace);
        }

        private static bool PatchPrefix(ref object[] loadables, string defaultKey, [CanBeNull] Func<string, bool> shouldExclude)
        {
            CacheServerBundles();
            var newInstances = new List<object>();

            foreach (var bundle in Shared.CachedBundles)
            {
                var bundleLock = Shared.BundleLockConstructor.Invoke(new object[] { 1 });
                var loaderInstance =
                    Activator.CreateInstance(Shared.LoaderType, bundle.Key, string.Empty, null, bundleLock);
                AccessTools.Property(Shared.LoaderType, "DependencyKeys").SetValue(loaderInstance, 
                    Shared.ManifestCache.ContainsKey(bundle.Key)
                        ? File.ReadAllLines(Shared.ManifestCache[bundle.Key])
                        : new string[] { });
                newInstances.Add(loaderInstance);
                Debug.Log("Adding custom bundle " + bundle.Key + " to the game");
            }

            loadables = loadables.Concat(newInstances.ToArray()).ToArray();

            return true;
        }
        private static string GetLocalBundlePath(Bundle bundle)
        {
            try
            {
                Debug.LogError(bundle.key);
                Debug.LogError(bundle.path);
                var local = false;
                var backend = new Uri(Config.BackendUrl);
                if (IPAddress.TryParse(backend.Host, out var ip))
                    if (ip.MapToIPv4().ToString().StartsWith("127"))
                        local = true;


                if (local && File.Exists(bundle.path))
                    return bundle.path;

                // Check local bundles folder
                var possibleLocalPath = Path.Combine(Shared.LOCAL_BUNDLES_PATH, bundle.key);
                if (File.Exists(possibleLocalPath))
                    return possibleLocalPath;

                // Check local cache
                var cachePath = Path.Combine(Shared.CACHE_BUNDLES_PATH, backend.Host, bundle.key);
                if (File.Exists(cachePath))
                    return cachePath;

                // Download bundle and put it in the cache folder
                var url = Config.BackendUrl + "/files/bundle/" + bundle.key;
                var dirPath = Path.GetDirectoryName(cachePath);
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);

                Debug.Log("Downloading bundle from " + url);

                Client.DownloadFile(url, cachePath);

                return cachePath;
            }
            catch (Exception e)
            {
                Debug.Log(e.Data);
                return null;
            }
        }

        private static void CacheServerBundles()
        {
            try
            {
                var text = new Request(null, Config.BackendUrl).GetJson(BUNDLE_URL);
                var serverBundles = JsonConvert.DeserializeObject<Bundle[]>(text);
                foreach (var bundle in serverBundles)
                {
                    var localPath = GetLocalBundlePath(bundle);
                    AssetBundle customBundle = null;


                    if (bundle.dependencyKeys.Length > 0)
                        File.WriteAllLines(localPath + ".manifest", bundle.dependencyKeys);

                    try
                    {
                        customBundle = AssetBundle.LoadFromFile(localPath);
                        var bundlePath = Path.Combine(AppContext.BaseDirectory,
                            "EscapeFromTarkov_Data/StreamingAssets/Windows/", customBundle.name);
                        if (!File.Exists(bundlePath))
                        {
                            Shared.CachedBundles.Add(customBundle.name, localPath);
                            Debug.Log("Cached modded bundle " + customBundle.name);
                            var manifestPath = localPath + ".manifest";
                            if (File.Exists(manifestPath))
                            {
                                Shared.ManifestCache.Add(customBundle.name, manifestPath);
                                Debug.Log("Cached manifest for " + customBundle.name);
                            }
                        }

                        else
                        {
                            var assets = customBundle.GetAllAssetNames();
                            foreach (var assetName in assets)
                            {
                                if (!Shared.ModdedAssets.ContainsKey(customBundle.name))
                                    Shared.ModdedAssets.Add(customBundle.name, new List<string>());
                                if (!Shared.ModdedBundlePaths.ContainsKey(customBundle.name))
                                    Shared.ModdedBundlePaths.Add(customBundle.name, localPath);
                                if (!Shared.ModdedAssets[customBundle.name].Contains(assetName))
                                    Shared.ModdedAssets[customBundle.name].Add(assetName);
                            }

                            Debug.Log("Cached modded assets for " + customBundle.name);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Failed to Load modded bundle " + localPath + ": " + e);
                    }

                    try
                    {
                        customBundle.Unload(true);
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
        internal class Bundle
        {
            public string key { get; set; }
            public string path { get; set; }
            public string[] dependencyKeys { get; set; }
        }
    }
}
