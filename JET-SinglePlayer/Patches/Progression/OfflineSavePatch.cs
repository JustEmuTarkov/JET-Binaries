using System;
using System.Linq;
using System.Reflection;
using Comfort.Common;
using EFT;
using JET.Utilities.Patching;
using JET.Utilities.Player;
using UnityEngine;


namespace JET.Patches.Progression
{
    class OfflineSaveProfilePatch : GenericPatch<OfflineSaveProfilePatch>
    {

        public OfflineSaveProfilePatch() : base(prefix: nameof(PatchPrefix))
        {
            // compile-time check
            //_ = nameof(ClientMetrics.Metrics);
        }

        protected override MethodBase GetTargetMethod()
        {
            foreach (var method in PatcherConstants.MainApplicationType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
                //if (method.Name == "method_44") {
                //    Debug.LogError($"{method.GetParameters().Length} {method.GetParameters()[0].ParameterType.Name} {method.GetParameters()[3].Name} {method.GetParameters()[3].ParameterType.Name}");
                //}
                if(method.Name.StartsWith("method") &&
                    method.GetParameters().Length == 6 &&
                    method.GetParameters()[0].Name == "profileId" &&
                    method.GetParameters()[1].Name == "savageProfile" &&
                    method.GetParameters()[2].Name == "location" &&
                    method.GetParameters()[3].Name == "isLocal" &&
                    method.GetParameters()[4].Name == "result" &&
                    method.GetParameters()[5].Name == "timeHasComeScreenController")
                {
                    return method;
                }
            }
            return null;
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
