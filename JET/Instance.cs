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

namespace JET
{
    [ObfuscationAttribute(Exclude = true)]
    public class Instance : MonoBehaviour
	{
        [ObfuscationAttribute(Exclude = true)]
        private void Start()
		{

            PatcherUtil.Patch<BattleEyePatch>();
            PatcherUtil.Patch<SslCertificatePatch>();
            PatcherUtil.Patch<UnityWebRequestPatch>();
            PatcherUtil.Patch<NotificationSslPatch>();
            Debug.Log("Core: Loaded");

            new Settings(null, Config.BackendUrl);
            
            PatcherUtil.Patch<EasyAssetsPatch>();
            PatcherUtil.Patch<EasyBundlePatch>();
            PatcherUtil.Patch<BundleLoadPatch>();
            Debug.Log("RuntimeBundles: Loaded");

            if (Offline.IsEnabled())
            {
                OfflineModePatchRoutes();
                return;
            }
            Debug.Log("SinglePlayer: Disabled");
        }

        private void OfflineModePatchRoutes() {
            PatcherUtil.Patch<OfflineLootPatch>();
            PatcherUtil.Patch<OfflineSaveProfilePatch>();
            PatcherUtil.Patch<OfflineSpawnPointPatch>();
            PatcherUtil.Patch<WeaponDurabilityPatch>();
            PatcherUtil.Patch<SingleModeJamPatch>();
            PatcherUtil.Patch<ExperienceGainPatch>();

            PatcherUtil.Patch<MainMenuControllerPatch>();
            PatcherUtil.Patch<PlayerPatch>();

            PatcherUtil.Patch<MatchmakerOfflineRaidPatch>();
            PatcherUtil.Patch<MatchMakerSelectionLocationScreenPatch>();
            PatcherUtil.Patch<InsuranceScreenPatch>();

            PatcherUtil.Patch<BossSpawnChancePatch>();
            PatcherUtil.Patch<BotTemplateLimitPatch>();
            PatcherUtil.Patch<GetNewBotTemplatesPatch>();
            PatcherUtil.Patch<RemoveUsedBotProfilePatch>();
            PatcherUtil.Patch<SpawnPmcPatch>();
            PatcherUtil.Patch<CoreDifficultyPatch>();
            PatcherUtil.Patch<BotDifficultyPatch>();

            PatcherUtil.Patch<OnDeadPatch>();
            PatcherUtil.Patch<OnShellEjectEventPatch>();
            PatcherUtil.Patch<BotStationaryWeaponPatch>();

            PatcherUtil.Patch<BeaconPatch>();
            PatcherUtil.Patch<DogtagPatch>();

            PatcherUtil.Patch<LoadOfflineRaidScreenPatch>();
            PatcherUtil.Patch<ScavPrefabLoadPatch>();
            PatcherUtil.Patch<ScavProfileLoadPatch>();
            PatcherUtil.Patch<ScavSpawnPointPatch>();
            PatcherUtil.Patch<ScavExfilPatch>(); // if its not required by players can be removed or moved into config loading patches

            PatcherUtil.Patch<EndByTimerPatch>();

            Debug.Log("SinglePlayer: Loaded");
        }
	}
}
