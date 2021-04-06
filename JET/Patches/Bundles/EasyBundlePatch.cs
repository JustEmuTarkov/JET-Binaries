﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Diz.DependencyManager;
using JET.Utilities.Patching;
using JET.Utilities;
#if B11661
using IEasyBundle = GInterface253; //Property: SameNameAsset 
using IBundleLock = GInterface254; //Property: IsLocked
// go to actual gclass and search for gclassXXXX<T> with initial value gparam_0 and base.method_0(value) call
using BindableState = GClass2206<Diz.DependencyManager.ELoadState>; //Construct method parameter: initialValue 
#endif
#if B10988
using IEasyBundle = GInterface250; //Property: SameNameAsset 
using IBundleLock = GInterface251; //Property: IsLocked
// go to actual gclass and search for gclassXXXX<T> with initial value gparam_0 and base.method_0(value) call
using BindableState = GClass2166<Diz.DependencyManager.ELoadState>; //Construct method parameter: initialValue 
#endif
#if B9767
using IEasyBundle = GInterface238; //Property: SameNameAsset 
using IBundleLock = GInterface239; //Property: IsLocked
// go to actual gclass and search for gclassXXXX<T> with initial value gparam_0 and base.method_0(value) call
using BindableState = GClass2100<Diz.DependencyManager.ELoadState>; //Construct method parameter: initialValue 
#endif
#if B9018
using IEasyBundle = GInterface223; //Property: SameNameAsset 
using IBundleLock = GInterface224; //Property: IsLocked
// go to actual gclass and search for gclassXXXX<T> with initial value gparam_0 and base.method_0(value) call
using BindableState = GClass2046<Diz.DependencyManager.ELoadState>; //Construct method parameter: initialValue 
#endif
#if DEBUG
using IEasyBundle = GInterface250; //Property: SameNameAsset 
using IBundleLock = GInterface251; //Property: IsLocked
// go to actual gclass and search for gclassXXXX<T> with initial value gparam_0 and base.method_0(value) call
using BindableState = GClass2166<Diz.DependencyManager.ELoadState>; //Construct method parameter: initialValue 
#endif


namespace JET.Patches
{
    public class EasyBundlePatch : GenericPatch<EasyBundlePatch>
    {
        public EasyBundlePatch() : base(prefix: nameof(PatchPrefix)) {}

        protected override MethodBase GetTargetMethod()
        {
            return PatcherConstants.TargetAssembly.GetTypes().Single(IsTargetType).GetConstructors()[0];
        }

        private static bool IsTargetType(Type type)
        {
            return type.IsClass && type.GetProperty("SameNameAsset") != null;
        }

        static bool PatchPrefix(IEasyBundle __instance, string key, string rootPath, UnityEngine.AssetBundleManifest manifest, IBundleLock bundleLock)
        {
            var easyBundle = new EasyBundleHelper(__instance);
            easyBundle.Key = key;

            var path = rootPath + key;
            var bundle = (BundleInfo)null;

            if (Settings.bundles.TryGetValue(key, out bundle))
            {
                path = bundle.Path;
            }

            easyBundle.Path = path;
            easyBundle.KeyWithoutExtension = Path.GetFileNameWithoutExtension(key);

            var dependencyKeys = manifest.GetDirectDependencies(key);

            foreach (KeyValuePair<string, BundleInfo> kvp in Settings.bundles)
            {
                if (!key.Equals(kvp.Key))
                {
                    continue;
                }

                var result = dependencyKeys == null ? new List<string>() : dependencyKeys.ToList();
                dependencyKeys = result.Union(kvp.Value.DependencyKeys).ToList().ToArray<string>();
                break;
            }

            easyBundle.DependencyKeys = dependencyKeys;
            easyBundle.LoadState = new BindableState(ELoadState.Unloaded, null);
            easyBundle.BundleLock = bundleLock;

            return false;
        }
    }
}
