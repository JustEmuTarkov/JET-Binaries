﻿using UnityEngine;
using JET.Utilities.Patching;
using JET.Utilities;
using JET.Patches;
using JET.Patches.Progression;
using JET.Patches.Healing;
using JET.Patches.Matchmaker;
using JET.Patches.Bots;
using JET.Patches.RaidFix;
using JET.Patches.Quests;
using JET.Patches.ScavMode;
using System.Reflection;
using System;

namespace JET
{
    [ObfuscationAttribute(Exclude = true)]
    public class Instance : MonoBehaviour
	{
        private string Watermark = "10901 | JET";
        [ObfuscationAttribute(Exclude = true)]
        private void Start()
		{

            PatcherUtil.Patch<BattleEyePatch>();
            PatcherUtil.Patch<SslCertificatePatch>();
            PatcherUtil.Patch<UnityWebRequestPatch>();
            PatcherUtil.Patch<NotificationSslPatch>();
            // allows to turn on and off the PreloaderUI.SetStreamMode(bool)
            // PatcherUtil.Patch<Patches.Core.StreamerModePatch>(); 
            // PatcherUtil.Patch<Patches.Core.FixChatOnDestroyPatch>(); 

            Debug.LogError("Core: Loaded");

            new Settings(null, Config.BackendUrl);
            
            PatcherUtil.Patch<EasyAssetsPatch>();
            PatcherUtil.Patch<EasyBundlePatch>();
            PatcherUtil.Patch<BundleLoadPatch>();
            Debug.LogError("RuntimeBundles: Loaded");

            OfflineModePatchRoutes(Offline.LoadModules());
            
            WatermarkOverrider();
        }

        private void WatermarkOverrider() {
            var _barVariable = typeof(EFT.UI.PreloaderUI)
                .GetField("_alphaVersionLabel", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance) as EFT.UI.LocalizedText;
            _barVariable.LocalizationKey = Watermark;
        }
        private void OfflineModePatchRoutes(Offline.OfflineMode EnabledElements) {
            
            if(EnabledElements.OfflineLootPatch)
                PatcherUtil.Patch<OfflineLootPatch>();
            if (EnabledElements.OfflineSaveProfilePatch)
                PatcherUtil.Patch<OfflineSaveProfilePatch>();
            if (EnabledElements.OfflineSpawnPointPatch)
                PatcherUtil.Patch<OfflineSpawnPointPatch>();
            if (EnabledElements.WeaponDurabilityPatch)
                PatcherUtil.Patch<WeaponDurabilityPatch>();
            if (EnabledElements.SingleModeJamPatch)
                PatcherUtil.Patch<SingleModeJamPatch>();
            if (EnabledElements.ExperienceGainPatch)
                PatcherUtil.Patch<ExperienceGainPatch>();

            if (EnabledElements.MainMenuControllerPatch)
                PatcherUtil.Patch<MainMenuControllerPatch>();
            if (EnabledElements.PlayerPatch)
                PatcherUtil.Patch<PlayerPatch>();

            if (EnabledElements.MatchmakerOfflineRaidPatch)
                PatcherUtil.Patch<MatchmakerOfflineRaidPatch>();
            if (EnabledElements.MatchMakerSelectionLocationScreenPatch)
                PatcherUtil.Patch<MatchMakerSelectionLocationScreenPatch>();
            if (EnabledElements.InsuranceScreenPatch)
                PatcherUtil.Patch<InsuranceScreenPatch>();

            if (EnabledElements.BossSpawnChancePatch)
                PatcherUtil.Patch<BossSpawnChancePatch>();
            if (EnabledElements.BotTemplateLimitPatch)
                PatcherUtil.Patch<BotTemplateLimitPatch>();
            if (EnabledElements.GetNewBotTemplatesPatch)
                PatcherUtil.Patch<GetNewBotTemplatesPatch>();
            if (EnabledElements.RemoveUsedBotProfilePatch)
                PatcherUtil.Patch<RemoveUsedBotProfilePatch>();
            if (EnabledElements.SpawnPmcPatch)
                PatcherUtil.Patch<SpawnPmcPatch>();
            if (EnabledElements.CoreDifficultyPatch)
                PatcherUtil.Patch<CoreDifficultyPatch>();
            if (EnabledElements.BotDifficultyPatch)
                PatcherUtil.Patch<BotDifficultyPatch>();

            if (EnabledElements.OnDeadPatch)
                PatcherUtil.Patch<OnDeadPatch>();
            if (EnabledElements.OnShellEjectEventPatch)
                PatcherUtil.Patch<OnShellEjectEventPatch>();
            if (EnabledElements.BotStationaryWeaponPatch)
                PatcherUtil.Patch<BotStationaryWeaponPatch>();

            if (EnabledElements.BeaconPatch)
                PatcherUtil.Patch<BeaconPatch>();
            if (EnabledElements.DogtagPatch)
                PatcherUtil.Patch<DogtagPatch>();

            if (EnabledElements.LoadOfflineRaidScreenPatch)
                PatcherUtil.Patch<LoadOfflineRaidScreenPatch>();
            if (EnabledElements.ScavPrefabLoadPatch)
                PatcherUtil.Patch<ScavPrefabLoadPatch>();
            if (EnabledElements.ScavProfileLoadPatch)
                PatcherUtil.Patch<ScavProfileLoadPatch>();
            //if (EnabledElements.ScavSpawnPointPatch)
            //    PatcherUtil.Patch<ScavSpawnPointPatch>();
            if (EnabledElements.ScavExfilPatch)
                PatcherUtil.Patch<ScavExfilPatch>(); // if its not required by players can be removed or moved into config loading patches

            if (EnabledElements.EndByTimerPatch)
                PatcherUtil.Patch<EndByTimerPatch>();

            Debug.LogError("SinglePlayer: Loaded");
        }
	}
}
