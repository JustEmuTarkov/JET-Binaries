﻿using System;
using System.Reflection;
using EFT;
using EFT.InventoryLogic;
using JET.Utilities.Patching;
using UnityEngine;
#if B16029
using Equipment = GClass2040; // GetSlot
using DamageInfo = GStruct247; // HittedBallisticCollider
#endif
#if B14687
using Equipment = GClass1975; // GetSlot
using DamageInfo = GStruct240; // HittedBallisticCollider
#endif
#if B13074 || B13487
using Equipment = GClass1757; // GetSlot
using DamageInfo = GStruct241; // HittedBallisticCollider
#endif
#if B11661 || B12102
using Equipment = GClass1729; // GetSlot
using DamageInfo = GStruct240; // HittedBallisticCollider
#endif
#if B10988
using Equipment = GClass1692;
using DamageInfo = GStruct239;
#endif
#if B9767
using Equipment = GClass1632;
using DamageInfo = GStruct232;
#endif
#if B9018
using Equipment = GClass1602;
using DamageInfo = GStruct227;
#endif
#if DEBUG
using Equipment = GClass1692;
using DamageInfo = GStruct239;
#endif
namespace JET.Patches.Quests
{
    class DogtagPatch : GenericPatch<DogtagPatch>
    {
        private static Func<Player, Equipment> getEquipmentProperty;

        public DogtagPatch() : base(postfix: nameof(PatchPostfix))
        {
            // compile-time checks
            _ = nameof(Equipment.GetSlot);
            _ = nameof(DamageInfo.Weapon);

            getEquipmentProperty = typeof(Player)
                .GetProperty("Equipment", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetGetMethod(true)
                .CreateDelegate(typeof(Func<Player, Equipment>)) as Func<Player, Equipment>;
        }

        protected override MethodBase GetTargetMethod() => typeof(Player)
            .GetMethod("OnBeenKilledByAggressor", BindingFlags.NonPublic | BindingFlags.Instance);

        public static void PatchPostfix(Player __instance, Player aggressor, DamageInfo damageInfo)
        {
            if (__instance.Profile.Info.Side == EPlayerSide.Savage)
            {
                return;
            }

            var equipment = getEquipmentProperty(__instance);
            var dogtagSlot = equipment.GetSlot(EquipmentSlot.Dogtag);
            var dogtagItem = dogtagSlot.ContainedItem as Item;

            if (dogtagItem == null)
            {
                Debug.LogError("[DogtagPatch] error > DogTag slot item is null somehow.");
                return;
            }

            var itemComponent = dogtagItem.GetItemComponent<DogtagComponent>();

            if (itemComponent == null)
            {
                Debug.LogError("[DogtagPatch] error > DogTagComponent on dog tag slot is null. Something went horrifically wrong!");
                return;
            }

            var victimProfileInfo = __instance.Profile.Info;

            itemComponent.AccountId = __instance.Profile.AccountId;
            itemComponent.ProfileId = __instance.Profile.Id;
            itemComponent.Nickname = victimProfileInfo.Nickname;
            itemComponent.Side = victimProfileInfo.Side;
            itemComponent.KillerName = aggressor.Profile.Info.Nickname;
            itemComponent.Time = DateTime.Now;
            itemComponent.Status = "Killed by ";
            itemComponent.KillerAccountId = aggressor.Profile.AccountId;
            itemComponent.KillerProfileId = aggressor.Profile.Id;
            itemComponent.WeaponName = damageInfo.Weapon.Name;

            if (__instance.Profile.Info.Experience > 0)
            {
                itemComponent.Level = victimProfileInfo.Level;
            }
        }
    }
}
