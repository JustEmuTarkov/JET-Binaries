using JET.Utilities.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JET.Patches.Ragfair
{
    class BarterSchemeAutoFill : GenericPatch<BarterSchemeAutoFill>
    {
        public BarterSchemeAutoFill() : base(postfix: nameof(PatchPostfix)) { }
        public static void PatchPostfix(ref bool ___bool_1)
        {
            ___bool_1 = true;
            PlayerPrefs.SetInt("AutoFillRequirements", 1);
        }
        protected override MethodBase GetTargetMethod()
        {
            return typeof(EFT.UI.BarterSchemePanel).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
    class BarterSchemeAutoFillPersist : GenericPatch<BarterSchemeAutoFillPersist>
    {
        public BarterSchemeAutoFillPersist() : base(prefix: nameof(PatchPrefix)) { }
        public static bool PatchPrefix(ref bool arg, ref bool ___bool_1)
        {
            arg = true;
            ___bool_1 = true;
            return false;
        }
        protected override MethodBase GetTargetMethod()
        {
            return typeof(EFT.UI.BarterSchemePanel).GetMethod("method_6", BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}
