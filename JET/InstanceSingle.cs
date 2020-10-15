using UnityEngine;
using JET.Common.Utils.Patching;
using JET.SinglePlayer.Patches.Bots;
using JET.SinglePlayer.Patches.Matchmaker;
using JET.SinglePlayer.Patches.Progression;
using JET.SinglePlayer.Patches.Quests;
using JET.SinglePlayer.Patches.RaidFix;
using JET.SinglePlayer.Patches.ScavMode;

namespace JET.SinglePlayer
{
    public class Instance : MonoBehaviour
    {
        private void Start()
		{
            Debug.Log("JET.SinglePlayer: Loaded");

			PatcherUtil.Patch<OfflineLootPatch>();
			PatcherUtil.Patch<OfflineSaveProfilePatch>();
            PatcherUtil.Patch<OfflineSpawnPointPatch>();
            PatcherUtil.Patch<WeaponDurabilityPatch>();
            PatcherUtil.Patch<SingleModeJamPatch>();
            
            PatcherUtil.Patch<Patches.Healing.MainMenuControllerPatch>();
			PatcherUtil.Patch<Patches.Healing.PlayerPatch>();

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
            PatcherUtil.Patch<ScavExfilPatch>();

            PatcherUtil.Patch<EndByTimerPatch>();
        }
    }
}
