﻿using System;
using System.Reflection;
using Comfort.Common;
using EFT;
using JET.Common.Utils.Patching;
using JET.SinglePlayer.Utils.Player;
using ClientMetrics = GClass1304;

// will need to disable this if multiplayer is ON
namespace JET.SinglePlayer.Patches.Progression
{
    class OfflineSaveProfilePatch : AbstractPatch
    {
        private readonly string overridedMethod = "method_38";
        static OfflineSaveProfilePatch()
        {
            // compile-time check
            _ = nameof(ClientMetrics.Metrics);
        }

        public override MethodInfo TargetMethod()
        {
            return PatcherConstants.MainApplicationType.GetMethod(overridedMethod, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        public static void Prefix(ESideType ___esideType_0, Result<ExitStatus, TimeSpan, ClientMetrics> result)
        {
            var session = Utils.Config.BackEndSession;
            string backendUrl = Utils.Config.BackendUrl;
            bool isPlayerScav = false;
            Profile profile = session.Profile;

            if (___esideType_0 == ESideType.Savage)
            {
                profile = session.ProfileOfPet;
                isPlayerScav = true;
            }

            var currentHealth = Utils.Player.HealthListener.Instance.CurrentHealth;

            SaveLootUtil.SaveProfileProgress(backendUrl, session.GetPhpSessionId(), result.Value0, profile, currentHealth, isPlayerScav);
        }
    }
}