using System.Reflection;
using JET.Utilities.Patching;
#if B16338
using MainMenuController = GClass1536;
// SelectedDateTime
using IHealthController = GInterface190;
// CarryingWeightAbsoluteModifier
#endif
#if B16029
using MainMenuController = GClass1504; // SelectedDateTime
using IHealthController = GInterface188; // CarryingWeightAbsoluteModifier
#endif
#if B14687
using MainMenuController = GClass1454; // SelectedDateTime
using IHealthController = GInterface187; // CarryingWeightAbsoluteModifier
#endif
#if B13074 || B13487
using MainMenuController = GClass1253; // SelectedDateTime
using IHealthController = GInterface177; // CarryingWeightAbsoluteModifier
#endif
#if B11661 || B12102
using MainMenuController = GClass1224; // SelectedDateTime
using IHealthController = GInterface171; // CarryingWeightAbsoluteModifier
#endif
#if B10988
using MainMenuController = GClass1194; // SelectedDateTime
using IHealthController = GInterface169; // CarryingWeightAbsoluteModifier
#endif
#if B9767
using MainMenuController = GClass1157; // SelectedDateTime
using IHealthController = GInterface163; // CarryingWeightAbsoluteModifier
#endif
#if B9018
using MainMenuController = GClass1144; // SelectedDateTime
using IHealthController = GInterface157; // CarryingWeightAbsoluteModifier
#endif
#if DEBUG
using MainMenuController = GClass1194; // SelectedDateTime
using IHealthController = GInterface169; // CarryingWeightAbsoluteModifier
#endif

namespace JET.Patches.Healing
{
    class MainMenuControllerPatch : GenericPatch<MainMenuControllerPatch>
    {
        static MainMenuControllerPatch()
        {
            _ = nameof(IHealthController.HydrationChangedEvent);
            _ = nameof(MainMenuController.HealthController);
        }

        public MainMenuControllerPatch() : base(postfix: nameof(PatchPostfix)) { }

        protected override MethodBase GetTargetMethod()
        {
            return typeof(MainMenuController).GetMethod("method_1", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        static void PatchPostfix(MainMenuController __instance)
        {
            var healthController = __instance.HealthController;
            var listener = Utilities.Player.HealthListener.Instance;
            listener.Init(healthController, false);
        }
    }
}
