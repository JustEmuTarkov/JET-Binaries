﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using HarmonyLib;
using Diz.Jobs;
using Diz.Resources;
using JetBrains.Annotations;
using JET.Utilities.Patching;
using JET.Utilities;
using JET.Patches.Bundles;
using System.IO;
using JET.Utilities.HTTP;
using Newtonsoft.Json;
using System.Net;
using System.Net.Cache;
#if B13074
using IEasyBundle = GInterface263; //Property: SameNameAsset 
using IBundleLock = GInterface264; //Property: IsLocked
using BundleLock = GClass2265; //Property: MaxConcurrentOperations
using DependencyGraph = GClass2266<GInterface263>; // Method: GetDefaultNode() / Inside <T> goes IEasyBundle // mostly its +1 number from BundleLock
#endif
#if B11661 || B12102
using IEasyBundle = GInterface253; //Property: SameNameAsset 
using IBundleLock = GInterface254; //Property: IsLocked
using BundleLock = GClass2220; //Property: MaxConcurrentOperations
using DependencyGraph = GClass2221<GInterface253>; // Method: GetDefaultNode() / Inside <T> goes IEasyBundle
#endif
#if B10988
using IEasyBundle = GInterface250; //Property: SameNameAsset 
using IBundleLock = GInterface251; //Property: IsLocked
using BundleLock = GClass2180; //Property: MaxConcurrentOperations
using DependencyGraph = GClass2181<GInterface250>; // Method: GetDefaultNode() / Inside <T> goes IEasyBundle
#endif
#if B9767
using IEasyBundle = GInterface238; //Property: SameNameAsset 
using IBundleLock = bundleLock; //Property: IsLocked
using BundleLock = GClass2114; //Property: MaxConcurrentOperations
using DependencyGraph = GClass2115<GInterface238>; // Method: GetDefaultNode() / Inside <T> goes IEasyBundle
#endif
#if B9018
using IEasyBundle = GInterface223; //Property: SameNameAsset 
using IBundleLock = GInterface224; //Property: IsLocked
using BundleLock = GClass2061; //Property: MaxConcurrentOperations
using DependencyGraph = GClass2062<GInterface223>; // Method: GetDefaultNode() / Inside <T> goes IEasyBundle
#endif
#if DEBUG
using IEasyBundle = GInterface250; //Property: SameNameAsset 
using IBundleLock = GInterface251; //Property: IsLocked
using BundleLock = GClass2180; //Property: MaxConcurrentOperations
using DependencyGraph = GClass2181<GInterface250>; // Method: GetDefaultNode() / Inside <T> goes IEasyBundle
#endif

namespace JET.Patches
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
