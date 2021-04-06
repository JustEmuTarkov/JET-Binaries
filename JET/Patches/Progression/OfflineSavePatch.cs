using System;
using System.Reflection;
using Comfort.Common;
using EFT;
using JET.Utilities.Patching;
using JET.Utilities.Player;
#if B11661
using ClientMetrics = GClass1408; // GameUpdateBinMetricCollector (lower Gclass number)
#endif
#if B10988
using ClientMetrics = GClass1367; // GameUpdateBinMetricCollector (lower Gclass number)
#endif
#if B9767
using ClientMetrics = GClass1325; // GameUpdateBinMetricCollector (lower Gclass number)
#endif
#if B9018
using ClientMetrics = GClass1304; // GameUpdateBinMetricCollector (lower Gclass number)
#endif
#if DEBUG
using ClientMetrics = GClass1367; // GameUpdateBinMetricCollector (lower Gclass number)
#endif

namespace JET.Patches.Progression
{
    class OfflineSaveProfilePatch : GenericPatch<OfflineSaveProfilePatch>
    {
#if B11661
        string methodNumber = "41";
#endif
#if B10988
        string methodNumber = "41";
#endif
#if B9767
        string methodNumber = "40";
#endif
#if B9018
        string methodNumber = "38";
#endif
#if DEBUG
        string methodNumber = "41";
#endif
        public OfflineSaveProfilePatch() : base(prefix: nameof(PatchPrefix))
        {
            // compile-time check
            _ = nameof(ClientMetrics.Metrics);
        }

        protected override MethodBase GetTargetMethod()
        {
            return PatcherConstants.MainApplicationType.GetMethod($"method_{methodNumber}", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        public static void PatchPrefix(ESideType ___esideType_0, Result<ExitStatus, TimeSpan, ClientMetrics> result)
        {
            var session = Utilities.Config.BackEndSession;
            var isPlayerScav = false;
            var profile = session.Profile;

            if (___esideType_0 == ESideType.Savage)
            {
                profile = session.ProfileOfPet;
                isPlayerScav = true;
            }

            var currentHealth = Utilities.Player.HealthListener.Instance.CurrentHealth;

            SaveLootUtil.SaveProfileProgress(Utilities.Config.BackendUrl, session.GetPhpSessionId(), result.Value0, profile, currentHealth, isPlayerScav);
        }
    }
}
