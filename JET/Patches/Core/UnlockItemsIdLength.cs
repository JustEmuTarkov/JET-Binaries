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

/// TODO: CHECK STRINGS IF THEY ARE PROPER ONES

namespace JET.Patches
{
    class UnlockItemsIdLength : GenericPatch<UnlockItemsIdLength>
    {
        public UnlockItemsIdLength() : base(transpiler: nameof(PatchTranspile)) { }

        protected override MethodBase GetTargetMethod()
        {
#if B13074
            // its fixed from now on...
            return null;
#else

            return PatcherConstants.TargetAssembly.GetTypes()
                .Single(x => {
                   // x.Name.StartsWith("Class") &&
                   // x.GetMethod("method_29") != null;
                   // Find Logout >> Find Class with 3 number like 189 etc. >> 
                    if (x.Name.StartsWith("Class"))
                    {
                        var method_29_info = x.GetMethod("method_29", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (method_29_info != null)
                        {
                            ParameterInfo[] paramInfo = method_29_info.GetParameters();
                            return  paramInfo.Length == 1 && 
                                    paramInfo[0].Name == "item" && 
                                    paramInfo[0].ParameterType.Name.StartsWith("GStruct") && 
                                    method_29_info.ReturnType.Name.StartsWith("Class");
                        }
                    }
                    return false;
                }).GetMethod("method_29", BindingFlags.NonPublic | BindingFlags.Instance);
#endif
        }

        static IEnumerable<CodeInstruction> PatchTranspile(IEnumerable<CodeInstruction> instructions)
        {
#if B13074
            return instructions;
#else

            var codes = new List<CodeInstruction>(instructions);

            // that should be the fastest way cause its at index 3 and we need to remov e3 instructions from there
            for (var i = 0; i < 3; i++)
                codes.RemoveAt(3);
            /* more advanced code if the first one will fail after some time ...
            List<CodeInstruction> searchCode = new List<CodeInstruction> {
                new CodeInstruction(OpCodes.Ldc_I4_0),
                new CodeInstruction(OpCodes.Ldc_I4_S),
                new CodeInstruction(OpCodes.Callvirt)
            };
            FoundItemToDeleteReturning:
            if(searchCode.Count > 0)
                for (var i = 0; i < codes.Count; i++) {
                    if (codes[i].opcode == searchCode[0].opcode) {
                        searchCode.RemoveAt(0);
                        codes.RemoveAt(i);
                        goto FoundItemToDeleteReturning;
                    }
                }

            if (searchCode.Count > 0){
                Debug.LogError("Patch Failed!! too much codes to search should be 0...");
                PatchLogger.LogTranspileSearchError(MethodBase.GetCurrentMethod());
                return instructions;
            }
            */
            if (codes.Count != 9)
            {
                Debug.LogError($"Patch Failed!! strange number of opcodes {codes.Count} [originalCode count is: {instructions.ToList().Count}]");
                PatchLogger.LogTranspileSearchError(MethodBase.GetCurrentMethod());
                return instructions;
            }
            return codes.AsEnumerable();
#endif
        }
    }
}
