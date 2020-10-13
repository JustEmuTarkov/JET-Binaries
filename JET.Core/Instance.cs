using UnityEngine;
using JET.Core.Patches;
using JET.Common.Utils.Patching;

namespace JET.Core
{
	public class Instance : MonoBehaviour
	{
		private void Start()
		{
            Debug.Log("JET.Core: Loaded");

            PatcherUtil.Patch<BattleEyePatch>();
            PatcherUtil.Patch<SslCertificatePatch>();
            PatcherUtil.Patch<UnityWebRequestPatch>();
            PatcherUtil.Patch<NotificationSslPatch>();
            Debug.Log("JET.Core: BackendUrl Status:" + ((GClass310.Config.BackendUrl.Length > 0) ? "OK" : "BAD"));
            
        }
        
	}
}
