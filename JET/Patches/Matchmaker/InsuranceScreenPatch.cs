using System.Linq;
using System.Reflection;
using JET.Utilities.Patching;
#if B11661 || B12102
using MainMenuController = GClass1224;  // SelectedDateTime
#endif
#if B10988
using MainMenuController = GClass1194;  // SelectedDateTime
#endif
#if B9767
using MainMenuController = GClass1157;  // SelectedDateTime
#endif
#if B9018
using MainMenuController = GClass1144;  // SelectedDateTime
#endif
#if DEBUG
using MainMenuController = GClass1194;  // SelectedDateTime
#endif

namespace JET.Patches.Matchmaker
{
    class InsuranceScreenPatch : GenericPatch<InsuranceScreenPatch>
    {
        static InsuranceScreenPatch()
        {
            _ = nameof(MainMenuController.InventoryController);
        }

        public InsuranceScreenPatch() : base(prefix: nameof(PrefixPatch), postfix: nameof(PostfixPatch)) { }

        // don't forget 'ref'
        static void PrefixPatch(ref bool local)
        {
            local = false;
        }

        static void PostfixPatch(ref bool ___bool_0)
        {
            ___bool_0 = true;
        }

        protected override MethodBase GetTargetMethod()
        {
            // find method 
            // private void method_53(bool local, GStruct73 weatherSettings, GStruct177 botsSettings, GStruct74 wavesSettings)
            return typeof(MainMenuController)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .FirstOrDefault(IsTargetMethod);    // controller contains 2 methods with same signature. Usually target method is first of them.
        }

        private static bool IsTargetMethod(MethodInfo mi)
        {
            var parameters = mi.GetParameters();

            if (parameters.Length != 4
            || parameters[0].ParameterType != typeof(bool)
            || parameters[0].Name != "local"
            || parameters[1].Name != "weatherSettings"
            || parameters[2].Name != "botsSettings"
            || parameters[3].Name != "wavesSettings")
            {
                return false;
            }

            return true;
        }
    }
}