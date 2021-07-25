using HarmonyLib;
using JET.Utilities;
using JET.Utilities.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
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
/// TODO: CHECK STRINGS IF THEY ARE PROPER ONES

namespace JET.Patches
{
    class BotSettingsLoadPatch : GenericPatch<BotSettingsLoadPatch>
    {
        public BotSettingsLoadPatch() : base(transpiler: nameof(PatchTranspile)) { }

        protected override MethodBase GetTargetMethod()
        {
            return typeof(BotDifficultyHandler).GetMethod("Load", BindingFlags.Public | BindingFlags.Static);
        }

        static IEnumerable<CodeInstruction> PatchTranspile(IEnumerable<CodeInstruction> instructions)
        {

            var codes = new List<CodeInstruction>(instructions);
            // that should be the fastest way cause its at index 3 and we need to remov e3 instructions from there
            for (var i = 0; i < 14; i++)
                codes.RemoveAt(6);

            if (codes.Count != 20)
            {
                Debug.LogError($"Patch Failed!! strange number of opcodes {codes.Count} [originalCode count is: {instructions.ToList().Count}]");
                PatchLogger.LogTranspileSearchError(MethodBase.GetCurrentMethod());
                return instructions;
            }
            return codes.AsEnumerable();
        }
    }
}
