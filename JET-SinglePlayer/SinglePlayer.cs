using System;
using JET.Patches;
using JET.Patches.Bots;
using JET.Patches.Bundles;
using JET.Patches.Healing;
using JET.Patches.Matchmaker;
using JET.Patches.Other;
using JET.Patches.Progression;
using JET.Patches.Quests;
using JET.Patches.Ragfair;
using JET.Patches.RaidFix;
using JET.Patches.ScavMode;
using JET.Utilities;
using JET.Utilities.Patching;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JET.Modding;
using UnityEngine;

/*
 Files To Edit GClasses:
	Utilities.EasyBundleHelper.cs
	Utilities.Config.cs
	Utilities.Player.HealthListener.cs
	Patches.ScavMode.LoadOfflineRaidScreenPatch.cs
	Patches.Quests.DogtagPatch.cs
	Patcher.Progression.WeaponDurabilityPatch.cs
	Patcher.Progression.OfflineSpawnPointPatch.cs
	Patcher.Progression.OfflineSavePatch.cs
	Patcher.Progression.OfflineLootPatch.cs (disabled after 12.11 cause we use build in one)
	Patcher.Other.HideoutRequirementIndicator.cs
	Patcher.Matchmaker.MatchMakerSelectionLocationScreenPatch.cs >> menu button
	Patcher.Matchmaker.MatchMakerAfterSelectLocation.cs >> menu button
	Patcher.Matchmaker.InsuranceScreenPatch.cs
	Patcher.Healing.MainMenuControllerPatch.cs
	Patcher.Bots.RemoveUsedBotProfilePatch.cs
	Patcher.Bots.GetNewBotTemplatesPatch.cs
	Patcher.Bots.CoreDifficultyPatch.cs
	Patcher.Bots.BotSettingsLoadPatch.cs
	Patcher.Bots.BotDifficultyPatch.cs
	
	
Edit finder so its not searching static names
	RemoveAddOfferButton.cs + BarterSchemeAutoFill.cs
	UnlockItemsIdLength.cs
 */



namespace JET
{
	public class SinglePlayer : JetMod
	{
		public delegate void Void();
		public static event Void ApplicationQuitEvent;
		public void OnApplicationQuit() => ApplicationQuitEvent?.Invoke();

#if B16029
		public static string GAME_VERSION = "0.12.12.0.16029";
#endif
#if B14687
		public static string GAME_VERSION = "0.12.11.5.14687";
#endif
#if B13487
		public static string GAME_VERSION = "0.12.11.1.13487";
#endif
#if B13074
		public static string GAME_VERSION = "0.12.11.0.13074";
#endif
#if B10988
		public static string GAME_VERSION = "0.12.9.10988";
#endif
#if B9767
		public static string GAME_VERSION = "0.12.8.9767";
#endif
#if B9018
		public static string GAME_VERSION = "0.12.7.9018";
#endif
#if DEBUG
		public static string GAME_VERSION = "DEBUG";
#endif

		private static void CheckVersion() 
		{
			string inGameVersion = Game.Version;
			if (GAME_VERSION != inGameVersion)
			{
				Debug.LogError($"This 50IQ is trying to launch Singleplayer module precompiled for {GAME_VERSION} on {inGameVersion}");
				Application.Quit(0);
			}
		}


		protected override void Initialize(IReadOnlyDictionary<Type, JetMod> dependencies, string gameVersion) {
			CheckVersion();
			// DEFAULT PATCHES
			PatcherUtil.Patch<UnlockItemsIdLength>();
			PatcherUtil.Patch<BarterSchemeAutoFill>();
#if B13074 || B13487 || B14687 || B16029
			//PatcherUtil.Patch<HideoutRequirementIndicator>(); // this displays hideout requirements on item hover
#endif
			// BUNDLE LOADING PATCHES
			PatcherUtil.Patch<EasyAssetsPatch>();
			//PatcherUtil.Patch<EasyBundlePatch>();
			//PatcherUtil.Patch<BundleLoadPatch>();

			OfflineModePatchRoutes(Offline.LoadModules());

		}
		private static void OfflineModePatchRoutes(Offline.OfflineMode EnabledElements)
		{
			#if !B13074 && !B13487 && !B14687 && !B16029
			if (EnabledElements.OfflineLootPatch)
				PatcherUtil.Patch<OfflineLootPatch>(); // this changes backend url for map loot to api/location
			#endif
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

			if (EnabledElements.BotSettingsLoadPatch)
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
			  //  PatcherUtil.Patch<ScavSpawnPointPatch>();
			if (EnabledElements.ScavExfilPatch)
			    PatcherUtil.Patch<ScavExfilPatch>(); // if its not required by players can be removed or moved into config loading patches

			if (EnabledElements.EndByTimerPatch)
				PatcherUtil.Patch<EndByTimerPatch>();
			if (EnabledElements.SpawnRandomizationPatch)
				PatcherUtil.Patch<SpawnRandomizationPatch>();
			if (EnabledElements.NoFiltersPatch)
				PatcherUtil.Patch<NoFiltersPatch>();
			if (EnabledElements.AllDifficultiesAvaliable)
			{
				// this unfortunatly crashes game client and game doesnt know what to do... propably lacks a few more places that need to be edited for that to work
				//GClass320.ExcludedDifficulties = new System.Collections.Generic.Dictionary<EFT.WildSpawnType, System.Collections.Generic.List<BotDifficulty>>()
				//{
				//	{
				//		EFT.WildSpawnType.assaultGroup,
				//		new List<BotDifficulty>
				//		{
				//			BotDifficulty.normal,
				//			BotDifficulty.easy,
				//			BotDifficulty.hard,
				//			BotDifficulty.impossible
				//		}
				//	},
				//	{
				//		EFT.WildSpawnType.followerTest,
				//		new List<BotDifficulty>
				//		{
				//			BotDifficulty.easy,
				//			BotDifficulty.hard,
				//			BotDifficulty.impossible
				//		}
				//	},
				//	{
				//		EFT.WildSpawnType.bossTest,
				//		new List<BotDifficulty>
				//		{
				//			BotDifficulty.easy,
				//			BotDifficulty.hard,
				//			BotDifficulty.impossible
				//		}
				//	},
				//	{
				//		EFT.WildSpawnType.test,
				//		new List<BotDifficulty>
				//		{
				//			BotDifficulty.easy,
				//			BotDifficulty.hard,
				//			BotDifficulty.impossible
				//		}
				//	}
				//};
			}
		}
	}
}
