﻿using System;
using System.Linq;
using System.Reflection;
using Comfort.Common;
using EFT;
using JET.Utilities.Patching;
using JET.Utilities.Player;
#if B13074 || B13487
using ClientMetrics = GClass1432; // GameUpdateBinMetricCollector (lower Gclass number)
#endif
#if B11661 || B12102
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
#if B13074
        string methodNumber = "44"; // search main application for those:
                                    // method_xx(string profileId, Profile savageProfile, GClass783.GClass785 location, bool isLocal, Result<ExitStatus, TimeSpan, GClass1403> result, MatchmakerTimeHasCome.GClass2068 timeHasComeScreenController = null)
#endif
#if B11661 || B12102
        string methodNumber = "41"; // could be wrong one
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
            return PatcherConstants.MainApplicationType
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(method => 
                    method.Name.StartsWith("method_") && 
                    method.GetParameters().Length == 6 && 
                    method.GetParameters()[0].GetType() == typeof(string) && 
                    method.GetParameters()[3].Name == "isLocal" && 
                    method.GetParameters()[3].GetType() == typeof(bool))
                .First();
            //return PatcherConstants.MainApplicationType.GetMethod($"method_{methodNumber}", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        public static void PatchPrefix(ESideType ___esideType_0, Result<ExitStatus, TimeSpan, object> result)
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
