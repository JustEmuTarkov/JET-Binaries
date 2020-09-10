using System.Reflection;
using JET.Common.Utils.Patching;
using JET.SinglePlayer.Utils.Reflection;

namespace JET.SinglePlayer.Patches.RaidFix
{
    public class OnShellEjectEventPatch : GenericPatch<OnShellEjectEventPatch>
    {
        public OnShellEjectEventPatch() : base(prefix: nameof(PatchPrefix)) { }

        protected override MethodBase GetTargetMethod()
        {
            return PatcherConstants.FirearmControllerType.GetMethod("OnShellEjectEvent");
        }

        static bool PatchPrefix(object __instance)
        {
            object weaponController = PrivateValueAccessor.GetPrivateFieldValue(PatcherConstants.FirearmControllerType, PatcherConstants.WeaponControllerFieldName, __instance);
            return (weaponController.GetType().GetField("RemoveFromChamberResult").GetValue(weaponController) == null) ? false : true;
        }
    }
}
