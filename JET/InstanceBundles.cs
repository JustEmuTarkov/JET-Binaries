using JET.Common.Utils.Patching;
using JET.RuntimeBundles.Patches;
using JET.RuntimeBundles.Utils;
using UnityEngine;


namespace JET.RuntimeBundles
{
    public class Instance : MonoBehaviour
    {
        private void Start()
		{
            Debug.Log("JET.RuntimeBundles: Loaded");

            new Settings(null, Config.BackendUrl);

            PatcherUtil.Patch<EasyAssetsPatch>();
            PatcherUtil.Patch<EasyBundlePatch>();
            PatcherUtil.Patch<BundleLoadPatch>();
        }
    }
}
