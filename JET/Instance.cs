using UnityEngine;
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
using JET.Patches.Other;
using JET.Patches.Ragfair;
using JET.Patches.Logging;

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.IO;
using System;
//using System.Text.Json;
//using System.Text.Json.Serialization;

namespace JET
{
    [ObfuscationAttribute(Exclude = true)]
    public class Instance : MonoBehaviour
    {
        public delegate void Void();
        public static event Void ApplicationQuitEvent;
        private static Settings _settings;
#if B13074
        private string Watermark = "13074 | JET";
#endif
#if B12102
        private string Watermark = "12102 | JET";
#endif
#if B11661
        private string Watermark = "11661 | JET";
#endif
#if B10988
        private string Watermark = "10988 | JET";
#endif
#if B9767
        private string Watermark = "9767 | JET";
#endif
#if B9018
        private string Watermark = "9018 | JET";
#endif
#if DEBUG
        private string Watermark = "Debug | JET";
#endif
        [ObfuscationAttribute(Exclude = true)]
        private void Awake() {

            PatcherUtil.Patch<EnsureConsistencyPatch>();
        }
        [ObfuscationAttribute(Exclude = true)]
        private void Start()
        {
            //try
            //{ //client.game.profile.list
            //    var stringProfiles = File.ReadAllText("J:\\_git\\_JET_REPOS\\JET-Backend-Server\\user\\profiles\\AID8131647517931710690RF\\character.json");
            //    var player_data = JsonConvert.DeserializeObject<EFT.Profile>(stringProfiles);
            //    // var serialized = JsonConvert.SerializeObject(player_data);
            //    Debug.LogError("FUCKING WORKING !!!!!");
            //    //Debug.LogError(serialized);
            //}
            //catch (Exception e) { Debug.LogError(e); }
            Debug.Log("[Starting]: " + Watermark);
            PatcherUtil.Patch<InitialHookPatch>();
            PatcherUtil.Patch<ResetHookPatch>();
            PatcherUtil.Patch<LoggingPatch>();
            PatcherUtil.Patch<BattleEyePatch>();
            PatcherUtil.Patch<SslCertificatePatch>();
            PatcherUtil.Patch<UnityWebRequestPatch>();
            PatcherUtil.Patch<NotificationSslPatch>();
#if !B13074
            PatcherUtil.Patch<UnlockItemsIdLength>();
#endif
            PatcherUtil.Patch<BarterSchemeAutoFill>();
            //PatcherUtil.Patch<BarterSchemeAutoFillPersist>();
            // allows to turn on and off the PreloaderUI.SetStreamMode(bool)
            // PatcherUtil.Patch<Patches.Core.StreamerModePatch>(); 
            // PatcherUtil.Patch<Patches.Core.FixChatOnDestroyPatch>(); 

            Debug.Log("Core: Loaded");

            //_settings = new Settings(null, Config.BackendUrl);

            PatcherUtil.Patch<EasyAssetsPatch>();
            PatcherUtil.Patch<EasyBundlePatch>();
            PatcherUtil.Patch<BundleLoadPatch>();
            //Debug.Log("RuntimeBundles: Loaded");

            OfflineModePatchRoutes(Offline.LoadModules());


            WatermarkOverrider();
        }
        private void LateUpdate()
        {
            WatermarkOverrider();

            // DISABLE ADD OFFER BUTTON IN FLEA MARKET
            //try
            //{
            //    var addOfferButton = typeof(EFT.UI.Ragfair.RagfairScreen).GetField("_addOfferButton", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(typeof(EFT.UI.Ragfair.RagfairScreen)) as EFT.UI.DefaultUIButton;
            //    if(addOfferButton != null)
            //        addOfferButton.Interactable = false;
            //}
            //catch { }
        }
        EFT.UI.LocalizedText localizedText;
        private void WatermarkOverrider()
        {
            try
            {
                if (localizedText == null)
                    localizedText = typeof(EFT.UI.PreloaderUI)
                    .GetField("_alphaVersionLabel", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance) as EFT.UI.LocalizedText;
                localizedText.LocalizationKey = Watermark;
            }
            catch { }
        }
        private void OfflineModePatchRoutes(Offline.OfflineMode EnabledElements)
        {
            if (EnabledElements.JetLogger)
            {
                PatcherUtil.Patch<InitialHookPatch>();
                PatcherUtil.Patch<ResetHookPatch>();
                PatcherUtil.Patch<LoggingPatch>();
            }

            //if (EnabledElements.OfflineLootPatch)
            //    PatcherUtil.Patch<OfflineLootPatch>();
            if (EnabledElements.OfflineSaveProfilePatch)
                PatcherUtil.Patch<OfflineSaveProfilePatch>();
            //if (EnabledElements.OfflineSpawnPointPatch)
            //    PatcherUtil.Patch<OfflineSpawnPointPatch>();
            //if (EnabledElements.WeaponDurabilityPatch)
            //    PatcherUtil.Patch<WeaponDurabilityPatch>();
            //if (EnabledElements.SingleModeJamPatch)
            //    PatcherUtil.Patch<SingleModeJamPatch>();
            if (EnabledElements.ExperienceGainPatch)
                PatcherUtil.Patch<ExperienceGainPatch>();

            if (EnabledElements.MainMenuControllerPatch)
                PatcherUtil.Patch<MainMenuControllerPatch>();
            if (EnabledElements.PlayerPatch)
                PatcherUtil.Patch<PlayerPatch>();

            if (EnabledElements.MatchmakerOfflineRaidPatch)
                PatcherUtil.Patch<MatchmakerOfflineRaidPatch>();
            if (EnabledElements.MatchMakerSelectionLocationScreenPatch)
            {
                PatcherUtil.Patch<MatchMakerSelectionLocationScreenPatch>();
                //PatcherUtil.Patch<MatchMakerAfterSelectLocation>();
            }

            if (EnabledElements.RemoveAddOfferButton)
            {
                PatcherUtil.Patch<RemoveAddOfferButton_Awake>();
                PatcherUtil.Patch<RemoveAddOfferButton_Call>();
            }

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
            //if (EnabledElements.ScavProfileLoadPatch)
            //    PatcherUtil.Patch<ScavProfileLoadPatch>();
            //if (EnabledElements.ScavSpawnPointPatch)
            //    PatcherUtil.Patch<ScavSpawnPointPatch>();
            //if (EnabledElements.ScavExfilPatch)
            //    PatcherUtil.Patch<ScavExfilPatch>(); // if its not required by players can be removed or moved into config loading patches

            if (EnabledElements.EndByTimerPatch)
                PatcherUtil.Patch<EndByTimerPatch>();
            if (EnabledElements.SpawnRandomizationPatch)
                PatcherUtil.Patch<SpawnRandomizationPatch>();
            if (EnabledElements.NoFiltersPatch)
                PatcherUtil.Patch<NoFiltersPatch>();

            Debug.Log("SinglePlayer: Loaded");
        }

        public void OnApplicationQuit() => ApplicationQuitEvent?.Invoke();
    }
}
