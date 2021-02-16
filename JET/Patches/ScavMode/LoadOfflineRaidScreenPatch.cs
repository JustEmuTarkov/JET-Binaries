using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using EFT;
using EFT.UI.Matchmaker;
using EFT.UI.Screens;
using JET.Utilities.Patching;
using JET.Utilities.Reflection;
using MenuController = GClass1194; // .SelectedKeyCard
using WeatherSettings = GStruct92; // IsRandomTime and IsRandomWeather
using BotsSettings = GStruct232; // IsScavWars and BotAmount
using WavesSettings = GStruct93; // IsTaggedAndCursed and IsBosses
namespace JET.Patches.ScavMode
{
    using OfflineRaidAction = Action<bool, WeatherSettings, BotsSettings, WavesSettings>;

    public class LoadOfflineRaidScreenPatch : GenericPatch<LoadOfflineRaidScreenPatch>
    {
        private const string k_TargetMethodName = "method_2";
        private const string k_MenuControllerInnerType = "Class816";
        private const string k_LoadReadyScreenMethodName = "method_35";
        private const string k_ReadyMethodName = "method_50";

        public LoadOfflineRaidScreenPatch() : base(transpiler: nameof(PatchTranspiler)) { }

        protected override MethodBase GetTargetMethod() => typeof(MenuController).GetNestedTypes(BindingFlags.NonPublic)
                .Single(x => x.Name == k_MenuControllerInnerType)
                .GetMethod(k_TargetMethodName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        static IEnumerable<CodeInstruction> PatchTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            var index = 26;
            var callCode = new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(LoadOfflineRaidScreenPatch), "LoadOfflineRaidScreenForScav"));

            codes[index].opcode = OpCodes.Nop;
            codes[index + 1] = callCode;
            codes.RemoveAt(index + 2);

            return codes.AsEnumerable();
        }

        private static MenuController GetMenuController() => PrivateValueAccessor
            .GetPrivateFieldValue(
                typeof(MainApplication),
                $"{typeof(MenuController).Name.ToLower()}_0", 
                ClientAppUtils.GetMainApp()) as MenuController;

        // Refer to MatchmakerOfflineRaid's subclass's OnShowNextScreen action definitions if these structs numbers change.
        public static void LoadOfflineRaidNextScreen(bool local, WeatherSettings weatherSettings, BotsSettings botsSettings, WavesSettings wavesSettings)
        {
            var menuController = GetMenuController();

            if (menuController.SelectedLocation.Id == "laboratory")
            {
                wavesSettings.IsBosses = true;
            }

            SetMenuControllerFieldValue(menuController, "bool_0", local);
            SetMenuControllerFieldValue(menuController, $"{typeof(BotsSettings).Name.ToLower()}_0", botsSettings);
            SetMenuControllerFieldValue(menuController, $"{typeof(WeatherSettings).Name.ToLower()}_0", wavesSettings);
            SetMenuControllerFieldValue(menuController, $"{typeof(WavesSettings).Name.ToLower()}_0", weatherSettings);

            typeof(MenuController).GetMethod(k_LoadReadyScreenMethodName, BindingFlags.NonPublic | BindingFlags.Instance).Invoke(menuController, null);
        }

        public static void LoadOfflineRaidScreenForScav()
        {
            var menuController = (object)GetMenuController();
            var gclass = new MatchmakerOfflineRaid.GClass2026();

            gclass.OnShowNextScreen += LoadOfflineRaidNextScreen;
            gclass.OnShowReadyScreen += (OfflineRaidAction)Delegate.CreateDelegate(typeof(OfflineRaidAction), menuController, k_ReadyMethodName);
            gclass.ShowScreen(EScreenState.Queued);
        }

        private static void SetMenuControllerFieldValue(MenuController instance, string fieldName, object value)
        {
            PrivateValueAccessor.SetPrivateFieldValue(typeof(MenuController), fieldName, instance, value);
        }
    }
}
