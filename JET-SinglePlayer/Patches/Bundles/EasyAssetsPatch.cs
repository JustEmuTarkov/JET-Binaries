using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Reflection;
using System.Threading.Tasks;
using Diz.Jobs;
using Diz.Resources;
using HarmonyLib;
using JET.Utilities;
using JET.Utilities.HTTP;
using JET.Utilities.Patching;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Build.Pipeline;

namespace JET.Patches.Bundles
{
    public class EasyAssetsPatch : GenericPatch<EasyAssetsPatch>
    {
        private const string BundleUrl = "/singleplayer/bundles";
        private static readonly WebClient _client = new WebClient { CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore) };

        public EasyAssetsPatch() : base(prefix: nameof(PatchPrefix)) { }

        protected override MethodBase GetTargetMethod()
        {
#if B16029
            var targetType = PatcherConstants.TargetAssembly.GetTypes().First(IsTargetType);
            return targetType.GetMethods().Single(x => x.Name == "method_0");
#else
            var nodeInterfaceType = PatcherConstants.TargetAssembly.GetTypes().First(x => x.IsInterface && x.GetProperty("SameNameAsset") != null);
            var targetType = PatcherConstants.TargetAssembly.GetTypes().Single(IsTargetType).MakeGenericType(nodeInterfaceType);
            
            return targetType.GetConstructors().First();
#endif
        }

        private static bool IsTargetType(Type type)
        {
#if B16029
            return type.IsClass && type.Name.EndsWith("EasyAssets");
#else
            return type.IsClass && type.GetMethod("GetNode") != null && string.IsNullOrWhiteSpace(type.Namespace);
#endif
        }

#if B16029
        private static bool PatchPrefix(EasyAssets __instance, object bundleLock, string defaultKey, string rootPath, string platformName, Func<string, bool> shouldExclude, Func<string, Task> bundleCheck)
        {
            CacheServerBundles();

            var text = rootPath.Replace("file:///", "").Replace("file://", "") + "/" + platformName + "/";
            var results =
            JsonConvert.DeserializeObject<Dictionary<string, Shared.BundleDetailStruct>>(File.ReadAllText(text + platformName + ".json"))
                .ToDictionary(k => k.Key, v => new BundleDetails
                {
                    FileName = v.Value.FileName,
                    Crc = v.Value.Crc,
                    Dependencies = v.Value.Dependencies
                });
            //TODO: Generate CRC and add bundles to results.
            __instance.Manifest = ScriptableObject.CreateInstance<CompatibilityAssetBundleManifest>();
            __instance.Manifest.SetResults(results);

            var allAssetBundles = __instance.Manifest.GetAllAssetBundles();
            var bundles = new object[allAssetBundles.Length];
            bundleLock ??= Shared.BundleLockConstructor.Invoke(new object[] {int.MaxValue});
            for (var i = 0; i < allAssetBundles.Length; i++)
            {
                bundles[i] = Activator.CreateInstance(Shared.LoaderType, allAssetBundles[i], string.Empty, __instance.Manifest, bundleLock, bundleCheck);
                JobScheduler.Yield().GetAwaiter();
            }

            AccessTools.Property(__instance.GetType(), "System").SetValue(__instance, Activator.CreateInstance(Shared.NodeType, bundles, defaultKey, shouldExclude));
            return false;
        }
#else
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
#endif

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

                _client.DownloadFile(url, cachePath);

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
                var text = new Request(null, Config.BackendUrl).GetJson(BundleUrl);
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
