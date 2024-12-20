﻿using System;
using System.Collections.Generic;
using System.Reflection;
using EFT;
using JET.Utilities;
using JET.Utilities.HTTP;
using JET.Utilities.Patching;
using UnityEngine;
#if B16029
using WaveInfo = GClass1226; // search for: Difficulty and chppse gclass with lower number whic hcontains Role and Limit variables
using BotsPresets = GClass586; // Method: GetNewProfile (higher GClass number)
#endif
#if B14687
using WaveInfo = GClass1192; // search for: Difficulty and chppse gclass with lower number whic hcontains Role and Limit variables
using BotsPresets = GClass564; // Method: GetNewProfile (higher GClass number)
#endif
#if B13487
using WaveInfo = GClass984; // search for: Difficulty and chppse gclass with lower number whic hcontains Role and Limit variables
using BotsPresets = GClass378; // Method: GetNewProfile (higher GClass number)
#endif
#if B13074
using WaveInfo = GClass984; // search for: Difficulty and chppse gclass with lower number whic hcontains Role and Limit variables
using BotsPresets = GClass379; // Method: GetNewProfile (higher GClass number)
#endif
#if B11661 || B12102
using WaveInfo = GClass956; // Field: Role (choose first one(or middle one) displayed as "Role")
using BotsPresets = GClass363; // Method: GetNewProfile (higher GClass number)
#endif
#if B10988
using WaveInfo = GClass929; // Field: Role (choose first one displayed as "Role")
using BotsPresets = GClass362; // Method: GetNewProfile (higher GClass number)
#endif
#if B9767
using WaveInfo = GClass904; // Field: Role (choose first one displayed as "Role")
using BotsPresets = GClass337; // Method: GetNewProfile (higher GClass number)
#endif
#if B9018
using WaveInfo = GClass897; // Field: Role (choose first one displayed as "Role")
using BotsPresets = GClass334; // Method: GetNewProfile (higher GClass number)
#endif
#if DEBUG
using WaveInfo = GClass929; // Field: Role (choose first one displayed as "Role")
using BotsPresets = GClass362; // Method: GetNewProfile (higher GClass number)
#endif


namespace JET.Patches.Bots
{
    public class BotTemplateLimitPatch : GenericPatch<BotTemplateLimitPatch>
    {
        public BotTemplateLimitPatch() : base(postfix: nameof(PatchPostfix))
        {
            // compile-time checks
            _ = nameof(BotsPresets.CreateProfile);
            _ = nameof(WaveInfo.Difficulty);
        }

        protected override MethodBase GetTargetMethod()
        {
            foreach (var type in PatcherConstants.TargetAssembly.GetTypes()) {
                if (type.Name.StartsWith("GClass")) {
                    var BoolCheck = type.GetMethod("GetNewProfile", BindingFlags.NonPublic | BindingFlags.Instance) == null;
                    if (BoolCheck) continue;
                    // its proper gclass now lets check if our targeted method exists there
                    var TargetedMethod = type.GetMethod("method_1", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    if (TargetedMethod != null)
                        return TargetedMethod;
                }
            }
            return null;
        }

        public static void PatchPostfix(List<WaveInfo> __result, List<WaveInfo> wavesProfiles, List<WaveInfo> delayed)
        {
            /*
                In short this method sums Limits by grouping wavesPropfiles collection by Role and Difficulty
                then in each group sets Limit to 30, the remainder is stored in "delayed" collection.
                So we change Limit of each group.
                Clear delayed waves, we don't need them if we have enough loaded profiles and in method_2 it creates a lot of garbage.
            */

            delayed?.Clear();
            
            foreach (WaveInfo wave in __result)
            {
                wave.Limit = Request(wave.Role);
            }
        }

        private static int Request(WildSpawnType role)
        {
            var json = new Request(null, Config.BackendUrl).GetJson("/singleplayer/settings/bot/limit/" + role.ToString());

            if (string.IsNullOrWhiteSpace(json))
            {
                Debug.LogError("[JET]: Received bot " + role.ToString() + " limit data is NULL, using fallback");
                return 30;
            }
            Debug.LogError("[JET]: Successfully received bot " + role.ToString() + " limit data");
            return Convert.ToInt32(json);
        }
    }
}
