using UnityEngine;
using JET.Core.Patches;
using JET.Common.Utils.Patching;

namespace JET.Core
{
	public class Instance : MonoBehaviour
	{
		private void Start()
		{
            Debug.LogError("JET.Core: Loaded");

            PatcherUtil.Patch<BattleEyePatch>();
            PatcherUtil.Patch<SslCertificatePatch>();
            PatcherUtil.Patch<UnityWebRequestPatch>();
            PatcherUtil.Patch<NotificationSslPatch>();
            int i = 0;
            int cnt = 0;
            while (true)
            {
                i++;
                if (GClass310.Config.BackendUrl.Length > 0) {
                    Debug.LogError(GClass310.Config.BackendUrl);
                    Debug.LogError("found at:" + i);
                    break;
                }
            }
            
        }
        
	}
}
