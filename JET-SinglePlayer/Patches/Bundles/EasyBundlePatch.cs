using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Diz.DependencyManager;
using JET.Utilities.Patching;
using JET.Utilities;
using JET.Patches;
using System.Threading.Tasks;
using UnityEngine;
#if B13074
using IEasyBundle = GInterface263; //Property: SameNameAsset 
using IBundleLock = GInterface264; //Property: IsLocked
// go to actual gclass and search for gclassXXXX<T> with initial value gparam_0 and base.method_0(value) call
using BindableState = GClass2251<Diz.DependencyManager.ELoadState>; //Construct method parameter: initialValue 
#endif
#if B11661 || B12102
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
using IBundleLock = bundleLock; //Property: IsLocked
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
        public EasyBundlePatch() : base(prefix: nameof(PatchPrefix)) { }

        protected override MethodBase GetTargetMethod()
        {
            return Shared.LoaderType.GetConstructors().First();
        }

        static bool PatchPrefix(object __instance, string key, string rootPath, AssetBundleManifest manifest, object bundleLock, ref string ___string_1, ref string ___string_0, ref Task ___task_0)
        {
            var easyBundle = new EasyBundleHelper(__instance)
            {
                Key = key
            };
            ___string_1 = rootPath + key;
            ___string_0 = Path.GetFileNameWithoutExtension(key);
            if (manifest != null)
                easyBundle.DependencyKeys = manifest.GetDirectDependencies(key);

            var newInst = Activator.CreateInstance(Shared.LoadState.PropertyType, ELoadState.Unloaded, null);
            Shared.LoadState.SetValue(__instance, newInst);
            ___task_0 = null;

            Shared.BundleLockField.SetValue(__instance, bundleLock);

            return false;
        }
    }
}
