using JET.Utilities.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JET.Patches
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
        public static bool PatchPrefix(ref bool ___bool_1)
        {
            ___bool_1 = true;
            PlayerPrefs.SetInt("AutoFillRequirements", 1);
            PlayerPrefs.Save();
            return true;
        }
        protected override MethodBase GetTargetMethod()
        {
            return typeof(EFT.UI.BarterSchemePanel).GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Single(x => x.Name == "method_6");
        }
    }
}
