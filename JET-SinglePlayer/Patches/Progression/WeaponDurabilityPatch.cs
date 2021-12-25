using System.Linq;
using System.Reflection;
using EFT;
using UnityEngine;
using JET.Utilities;
using JET.Utilities.Patching;
#if B16338
using AmmoInfo = GClass2105;
// AmmoLifeTimeSec
#endif
#if B16029
using AmmoInfo = GClass2058; // AmmoLifeTimeSec
#endif
#if B14687
using AmmoInfo = GClass1992; // AmmoLifeTimeSec
#endif
#if B13074 || B13487
using AmmoInfo = GClass1774; // AmmoLifeTimeSec
#endif
#if B11661 || B12102
using AmmoInfo = GClass1746;
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
#if B13074  || B13487 || B14687 || B16029 || B16338
            // do not patch at those versions cause they already have that inplace
            return false;
#else
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
#endif
        }

        public static void PatchPostfix(Player.FirearmController __instance, AmmoInfo ammo)
        {
#if B13074 || B13487 || B14687 || B16029 || B16338
            // do not patch at those versions cause they already have that inplace
            return;
#else
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
#endif
        }
    }
}
