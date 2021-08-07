using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Diz.DependencyManager;
using JET.Utilities;
using JET.Utilities.Patching;
using UnityEngine;

namespace JET.Patches.Bundles
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
