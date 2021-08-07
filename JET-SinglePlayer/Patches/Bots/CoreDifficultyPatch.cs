using System.Linq;
using System.Reflection;
using JET.Utilities;
using JET.Utilities.HTTP;
using JET.Utilities.Patching;
using UnityEngine;
//#if B13074
//using BotDifficultyHandler = GClass320; // Method: CheckOnExcude, LoadCoreByString, LoadDifficultyStringInternal, LoadInternalCoreByString
//#endif
//#if B11661 || B12102
//using BotDifficultyHandler = GClass304; // Method: CheckOnExcude, LoadCoreByString
//#endif
//#if B10988
//using BotDifficultyHandler = GClass303; // Method: CheckOnExcude, LoadCoreByString
//#endif
//#if B9767
//using BotDifficultyHandler = GClass283; // Method: CheckOnExcude, LoadCoreByString
//#endif
//#if B9018
//using BotDifficultyHandler = GClass280; // Method: CheckOnExcude, LoadCoreByString
//#endif
//#if DEBUG
//using BotDifficultyHandler = GClass303; // Method: CheckOnExcude, LoadCoreByString
//#endif
namespace JET.Patches.Bots
{
	public class CoreDifficultyPatch : GenericPatch<CoreDifficultyPatch>
	{
		public CoreDifficultyPatch() : base(prefix: nameof(PatchPrefix))
		{
        }

        protected override MethodBase GetTargetMethod()
        {
            var getBotDifficultyHandler = typeof(EFT.MainApplication).Assembly.GetTypes().Where(type => type.Name.StartsWith("GClass") && type.GetMethod("CheckOnExcude", BindingFlags.Public | BindingFlags.Static) != null).First();
            if (getBotDifficultyHandler == null)
                return null;
            return getBotDifficultyHandler.GetMethod("LoadCoreByString", BindingFlags.Public | BindingFlags.Static);
		}

		public static bool PatchPrefix(ref string __result)
		{
            __result = Request();

			return string.IsNullOrWhiteSpace(__result);
        }

        private static string Request()
        {
            var json = new Request(null, Config.BackendUrl).GetJson("/singleplayer/settings/bot/difficulty/core/core");

            if (string.IsNullOrWhiteSpace(json))
            {
                Debug.LogError("[JET]: Received core bot difficulty data is NULL, using fallback");
                return null;
            }

            Debug.LogError("[JET]: Successfully received core bot difficulty data");
            return json;
        }
    }
}
