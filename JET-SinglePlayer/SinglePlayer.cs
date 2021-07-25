using JET.Patches;
using JET.Utilities;
using JET.Utilities.Patching;

namespace JET
{
    public class SinglePlayer
    {
        public delegate void Void();
        public static event Void ApplicationQuitEvent;
        public void OnApplicationQuit() => ApplicationQuitEvent?.Invoke();

        private static Settings _settings;
#if B13074
        public static string GAME_VERSION = "0.12.11.2.13074";
#endif
#if B12102
        public static string GAME_VERSION = "";
#endif
#if B11661
        public static string GAME_VERSION = "";
#endif
#if B10988
        public static string GAME_VERSION = "0.12.9.10988";
#endif
#if B9767
        public static string GAME_VERSION = "";
#endif
#if B9018
        public static string GAME_VERSION = "";
#endif
#if DEBUG
        public static string GAME_VERSION = "";
#endif
        public static void Initialize() {
            // DEFAULT PATCHES
            PatcherUtil.Patch<UnlockItemsIdLength>();
            PatcherUtil.Patch<BarterSchemeAutoFill>();

            // BUNDLE LOADING PATCHES
            PatcherUtil.Patch<EasyAssetsPatch>();
            PatcherUtil.Patch<EasyBundlePatch>();
            PatcherUtil.Patch<BundleLoadPatch>();

            OfflineModePatchRoutes(Offline.LoadModules());

        }
        private static void OfflineModePatchRoutes(Offline.OfflineMode EnabledElements)
        {
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
                PatcherUtil.Patch<MatchMakerAfterSelectLocation>();
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

            PatcherUtil.Patch<BotSettingsLoadPatch>();

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
        }
    }
}
