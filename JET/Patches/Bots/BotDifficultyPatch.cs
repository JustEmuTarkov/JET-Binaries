using System.Reflection;
using UnityEngine;
using EFT;
using JET.Utilities.HTTP;
using JET.Utilities.Patching;
using JET.Utilities;
using System;
using System.Collections.Generic;
#if B13074
using BotDifficultyHandler = GClass320; // Method: CheckOnExcude, LoadDifficultyStringInternal
#endif
#if B11661 || B12102
using BotDifficultyHandler = GClass304; // Method: CheckOnExcude, LoadDifficultyStringInternal
#endif
#if B10988
using BotDifficultyHandler = GClass303; // Method: CheckOnExcude, LoadDifficultyStringInternal
#endif
#if B9767
using BotDifficultyHandler = GClass283; // Method: CheckOnExcude, LoadDifficultyStringInternal
#endif
#if B9018
using BotDifficultyHandler = GClass280; // Method: CheckOnExcude, LoadDifficultyStringInternal
#endif
#if DEBUG
using BotDifficultyHandler = GClass303; // Method: CheckOnExcude, LoadDifficultyStringInternal
#endif

namespace JET.Patches.Bots
{
    public class BotDifficultyPatch : GenericPatch<BotDifficultyPatch>
    {
        public BotDifficultyPatch() : base(prefix: nameof(PatchPrefix))
        {
        }

        protected override MethodBase GetTargetMethod()
        {
            //return typeof(BotDifficultyHandler).GetMethod("LoadInternal", BindingFlags.Public | BindingFlags.Static);
            return typeof(BotDifficultyHandler).GetMethod("LoadDifficultyStringInternal", BindingFlags.Public | BindingFlags.Static);
        }

        //private static bool PatchPrefix(ref bool __result, out GClass319 core)
        //{
        //    string text = GClass320.LoadCoreByString();
        //    if (text == null) {
        //        core = null;
        //        __result = false;
        //        return false;
        //    }

        //    core = GClass319.Create(text);
        //    List<string> botList = new List<string>();
        //    foreach (object obj in Enum.GetValues(typeof(WildSpawnType)))
        //    {
        //        botList.Add(((WildSpawnType)obj).ToString());
        //    }

        //    string RequestedData = Request();
        //    if (RequestedData == null)
        //    {
        //        return true;
        //    }

        //    List<KeyValuePair<string, object>> botDifficulty = Newtonsoft.Json.JsonConvert.DeserializeObject<List<KeyValuePair<string, object>>>(RequestedData);

        //    foreach (KeyValuePair<string, object> entry in botDifficulty) {
        //        var split = entry.Key.Split('.');
        //        BotDifficulty difficulty = GetDifficulty(split[0]);
        //        WildSpawnType type = GetTypeOfBot(split[1]);
        //        Debug.LogError(entry.Value);
        //        string difficultySetting = Newtonsoft.Json.JsonConvert.SerializeObject(entry.Value);
        //        Debug.LogError("difficultySetting");
        //        Debug.LogError(difficultySetting);
        //        if (difficultySetting == null || difficultySetting == "")
        //            continue;

        //        GClass322 gclass = GClass322.Create(difficultySetting);
        //        if (!GClass320.AllSettings.ContainsKey(difficulty, type))
        //        {
        //            GClass320.AllSettings.Add(difficulty, type, gclass);
        //        }
        //    }
        //    Debug.LogError("[JET]: Successfully loaded difficulty data");
        //    __result = true;
        //    return false;
        //}

        //internal static BotDifficulty GetDifficulty(string text)
        //{
        //    foreach (object botDifficulty in Enum.GetValues(typeof(BotDifficulty)))
        //    {
        //        if (text == ((BotDifficulty)botDifficulty).ToString())
        //        {
        //            return (BotDifficulty)botDifficulty;
        //        }
        //    }
        //    return BotDifficulty.normal;
        //}

        //internal static WildSpawnType GetTypeOfBot(string text)
        //{
        //    foreach (object botDifficulty in Enum.GetValues(typeof(WildSpawnType)))
        //    {
        //        if (text == ((WildSpawnType)botDifficulty).ToString())
        //        {
        //            return (WildSpawnType)botDifficulty;
        //        }
        //    }
        //    return WildSpawnType.assault;
        //}
        private static bool PatchPrefix(ref string __result, BotDifficulty botDifficulty, WildSpawnType role)
        {
            __result = Request(role, botDifficulty);
            return string.IsNullOrWhiteSpace(__result);
        }

        //private static string Request() {
        //    var json = new Request(null, Config.BackendUrl).GetJson("/singleplayer/settings/bot/difficulty");
        //    if (string.IsNullOrWhiteSpace(json))
        //    {
        //        Debug.LogError("[JET]: difficulty data is NULL, using fallback");
        //        return null;
        //    }
        //    Debug.LogError("[JET]: Successfully received difficulty data");
        //    return json;
        //}

        private static string Request(WildSpawnType role, BotDifficulty botDifficulty)
        {
            var json = new Request(null, Config.BackendUrl).GetJson("/singleplayer/settings/bot/difficulty/" + role.ToString() + "/" + botDifficulty.ToString());

            if (string.IsNullOrWhiteSpace(json))
            {
                Debug.LogError("[JET]: Received bot " + role.ToString() + " " + botDifficulty.ToString() + " difficulty data is NULL, using fallback");
                return null;
            }

            Debug.LogError("[JET]: Successfully received bot " + role.ToString() + " " + botDifficulty.ToString() + " difficulty data");
            return json;
        }
    }
}
