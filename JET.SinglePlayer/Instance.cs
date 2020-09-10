using UnityEngine;
using JET.Common.Utils.Patching;
using JET.SinglePlayer.Patches.Bots;
using JET.SinglePlayer.Patches.Matchmaker;
using JET.SinglePlayer.Patches.Progression;
using JET.SinglePlayer.Patches.Quests;
using JET.SinglePlayer.Patches.RaidFix;
using JET.SinglePlayer.Patches.ScavMode;
using JET.SinglePlayer.Utils;
using System.Reflection;

namespace JET.SinglePlayer
{
    public class Instance : MonoBehaviour
    {
        [ObfuscationAttribute(Exclude = true)]
        private void Start()
		{
            Debug.LogError("JET.SinglePlayer: Loaded");

			// todo: find a way to get php session id
			//new Settings(null, Utils.Config.BackendUrl);

			PatcherUtil.PatchPrefix<OfflineLootPatch>();
			PatcherUtil.PatchPrefix<OfflineSaveProfilePatch>();
            PatcherUtil.PatchPrefix<OfflineSpawnPointPatch>();
            PatcherUtil.PatchPostfix<WeaponDurabilityPatch>();
            PatcherUtil.PatchPostfix<SingleModeJamPatch>();
            
            PatcherUtil.Patch<Patches.Healing.MainMenuControllerPatch>();
			PatcherUtil.Patch<Patches.Healing.PlayerPatch>();

			PatcherUtil.PatchPostfix<MatchmakerOfflineRaidPatch>(); // offline buttons
			PatcherUtil.PatchPostfix<MatchMakerSelectionLocationScreenPatch>();
			PatcherUtil.Patch<InsuranceScreenPatch>();

            PatcherUtil.Patch<BossSpawnChancePatch>();
			PatcherUtil.PatchPostfix<BotTemplateLimitPatch>();
            PatcherUtil.PatchPrefix<GetNewBotTemplatesPatch>();
            PatcherUtil.PatchPrefix<RemoveUsedBotProfilePatch>();
           // PatcherUtil.PatchPrefix<SpawnPmcPatch>(); // PMC Simulation
			PatcherUtil.PatchPrefix<CoreDifficultyPatch>();
			PatcherUtil.PatchPrefix<BotDifficultyPatch>();
            
            PatcherUtil.Patch<OnDeadPatch>();
            PatcherUtil.Patch<OnShellEjectEventPatch>();
            PatcherUtil.Patch<BotStationaryWeaponPatch>();

            PatcherUtil.PatchPrefix<BeaconPatch>();
			PatcherUtil.PatchPostfix<DogtagPatch>();

           // PatcherUtil.Patch<LoadOfflineRaidScreenPatch>();
            PatcherUtil.Patch<ScavPrefabLoadPatch>();
            PatcherUtil.Patch<ScavProfileLoadPatch>();
            PatcherUtil.Patch<ScavSpawnPointPatch>();
            PatcherUtil.Patch<ScavExfilPatch>();

            PatcherUtil.Patch<EndByTimerPatch>();
        }
    }
}
