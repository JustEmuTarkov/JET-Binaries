using System.Linq;
using System.Reflection;
using UnityEngine;
using EFT;
using JET.Utilities.Patching;
using JET.Utilities;
#if B11661
using AmmoInfo = GClass1746; // AmmoLifeTimeSec
#endif
#if B10988
using AmmoInfo = GClass1709;
#endif
#if B9767
using AmmoInfo = GClass1649;
#endif
#if B9018
using AmmoInfo = GClass1619;
#endif
#if DEBUG
using AmmoInfo = GClass1709;
#endif
namespace JET.Patches.Progression
{
    public class WeaponDurabilityPatch : GenericPatch<WeaponDurabilityPatch>
    {
        public WeaponDurabilityPatch() : base(postfix: nameof(PatchPostfix))
        {
            // compile-time check
            _ = nameof(AmmoInfo.AmmoLifeTimeSec);
        }

        protected override MethodBase GetTargetMethod()
        {
            //private void method_46(GClass1564 ammo)
            return typeof(Player.FirearmController).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Single(IsTargetMethod);
        }

        private static bool IsTargetMethod(MethodInfo methodInfo)
        {
            if (methodInfo.IsVirtual)
            {
                return false;
            }

            var parameters = methodInfo.GetParameters();
            var methodBody = methodInfo.GetMethodBody();

            if (parameters.Length != 1
            || parameters[0].ParameterType != typeof(AmmoInfo)
            || parameters[0].Name != "ammo")
            {
                return false;
            }

            if (methodBody.LocalVariables.Any(x => x.LocalType == typeof(Vector3)))
            {
                return true;
            }

            return false;
        }

        public static void PatchPostfix(Player.FirearmController __instance, AmmoInfo ammo)
        {
            if (!Config.WeaponDurabilityEnabled)
            {
                return;
            }

            var item = __instance.Item;
            var durability = item.Repairable.Durability;
            var deterioration = ammo.Deterioration;
            var operatingResource = (item.Template.OperatingResource > 0) ? item.Template.OperatingResource : 1;

            if (durability <= 0f)
            {
                return;
            }

            durability -= item.Repairable.MaxDurability / operatingResource * deterioration;
            item.Repairable.Durability = (durability > 0) ? durability : 0;
        }
    }
}
