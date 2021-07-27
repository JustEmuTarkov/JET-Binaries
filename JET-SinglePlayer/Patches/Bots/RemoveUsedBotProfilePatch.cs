using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using EFT;
using HarmonyLib;
using JET.Utilities.Patching;
#if B13074
using BotData = GInterface17; // find ChooseProfile and get ginterface off that
#endif
#if B11661 || B12102
using BotData = GInterface16;
#endif
#if B10988
using BotData = GInterface15;
#endif
#if B9767
using BotData = GInterface14;
#endif
#if B9018
using BotData = GInterface13;
#endif
#if DEBUG
using BotData = GInterface15;
#endif

namespace JET.Patches.Bots
{
    public class RemoveUsedBotProfilePatch : GenericPatch<RemoveUsedBotProfilePatch>
    {
        private static Type targetInterface;
        private static Type targetType;
        private static AccessTools.FieldRef<object, List<Profile>> profilesField;

        public RemoveUsedBotProfilePatch() : base(prefix: nameof(PatchPrefix))
        {
            // compile-time check
            _ = nameof(BotData.ChooseProfile);

            targetInterface = PatcherConstants.TargetAssembly.GetTypes().Single(IsTargetInterface);
            targetType = PatcherConstants.TargetAssembly.GetTypes().Single(IsTargetType);
            profilesField = AccessTools.FieldRefAccess<List<Profile>>(targetType, "list_0");
        }

        private static bool IsTargetInterface(Type type)
        {
            if (!type.IsInterface || type.GetProperty("StartProfilesLoaded") == null || type.GetMethod("CreateProfile") == null)
            {
                return false;
            }
            
            return true;
        }

        private bool IsTargetType(Type type)
        {
            if (!targetInterface.IsAssignableFrom(type) || !targetInterface.IsAssignableFrom(type.BaseType))
            {
                return false;
            }

            return true;
        }

        protected override MethodBase GetTargetMethod()
        {
            return targetType.GetMethod("GetNewProfile", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        }

        public static bool PatchPrefix(ref Profile __result, object __instance, BotData data)
        {
            var profiles = profilesField(__instance);

            if (profiles.Count > 0)
            {
                // second parameter makes client remove used profiles
                __result = data.ChooseProfile(profiles, true);
            }
            else
            {
                __result = null;
            }

            return false;
        }
    }
}
