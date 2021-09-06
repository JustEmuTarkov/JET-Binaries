using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using EFT;
using HarmonyLib;
using JET.Utilities;
using JET.Utilities.Patching;
using JET.Utilities.Reflection.CodeWrapper;

namespace JET.Patches.ScavMode
{
    public class ScavPrefabLoadPatch : GenericPatch<ScavPrefabLoadPatch>
    {
        public ScavPrefabLoadPatch() : base(transpiler: nameof(PatchTranspile)) {}

        protected override MethodBase GetTargetMethod()
        {
            return typeof(MainApplication)
                .GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Single(x =>
                    x.Name.StartsWith("Struct") &&
                    x.GetField("entryPoint") != null
                    && x.GetField("timeAndWeather") != null
                    && x.GetField("location") != null
                    && x.GetField("mainApplication_0") != null
                    && x.GetField("timeHasComeScreenController") == null)
                .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .FirstOrDefault(x => x.Name == "MoveNext");

            //foreach (var type in PatcherConstants.MainApplicationType.GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)) {
            //    if (type.Name.StartsWith("Struct")) {
            //        if (type.GetField("entryPoint") != null &&
            //            type.GetField("timeAndWeather") != null &&
            //            type.GetField("location") != null &&
            //            type.GetField("mainApplication_0") != null &&
            //            type.GetField("timeHasComeScreenController") != null) {
            //            var TargetedMethod = type.GetMethod("MoveNext", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            //            if (TargetedMethod != null)
            //                return TargetedMethod;
            //        }
            //    }
            //}
            //return null;
        }

        static IEnumerable<CodeInstruction> PatchTranspile(ILGenerator generator, IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);

            // Search for code where backend.Session.getProfile() is called.
            var searchCode = new CodeInstruction(OpCodes.Callvirt, AccessTools.Method(PatcherConstants.SessionInterfaceType, "get_Profile"));
            var searchIndex = -1;

            for (var i = 0; i < codes.Count; i++)
            {
                if (codes[i].opcode == searchCode.opcode && codes[i].operand == searchCode.operand)
                {
                    searchIndex = i;
                    break;
                }
            }

            // Patch failed.
            if (searchIndex == -1)
            {
                PatchLogger.LogTranspileSearchError(MethodBase.GetCurrentMethod());
                return instructions;
            }

            // Move back by 3. This is the start of IL chain that we're interested in.
            searchIndex -= 3;

            var brFalseLabel = generator.DefineLabel();
            var brLabel = generator.DefineLabel();

            var newCodes = CodeGenerator.GenerateInstructions(new List<Code>()
            {
                new Code(OpCodes.Ldloc_1),
                new Code(OpCodes.Ldfld, typeof(ClientApplication), "_backEnd"),
                new Code(OpCodes.Callvirt, PatcherConstants.BackendInterfaceType, "get_Session"),
                new Code(OpCodes.Ldloc_1),
                new Code(OpCodes.Ldfld, typeof(MainApplication), "esideType_0"),
                new Code(OpCodes.Ldc_I4_0),
                new Code(OpCodes.Ceq),
                new Code(OpCodes.Brfalse, brFalseLabel),
                new Code(OpCodes.Callvirt, PatcherConstants.SessionInterfaceType, "get_Profile"),
                new Code(OpCodes.Br, brLabel),
                new CodeWithLabel(OpCodes.Callvirt, brFalseLabel, PatcherConstants.SessionInterfaceType, "get_ProfileOfPet"),
                new CodeWithLabel(OpCodes.Ldc_I4_1, brLabel)
            });

            codes.RemoveRange(searchIndex, 5);
            codes.InsertRange(searchIndex, newCodes);

            return codes.AsEnumerable();
        }
    }
}
