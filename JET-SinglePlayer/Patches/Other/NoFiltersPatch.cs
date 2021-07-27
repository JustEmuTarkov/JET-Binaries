using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using JET.Utilities.Patching;

namespace JET.Patches.Other
{
    class NoFiltersPatch : GenericPatch<NoFiltersPatch>
    {
        public NoFiltersPatch() : base(prefix: nameof(PatchPrefix)) { }
        protected override MethodBase GetTargetMethod()
        {
            var type = PatcherConstants.TargetAssembly.GetTypes().Single(x => x.GetMethod("CheckItemFilter", BindingFlags.Public | BindingFlags.Static) != null
                                                                  && x.GetMethod("CheckItemFilter", BindingFlags.Public | BindingFlags.Static).IsDefined(typeof(ExtensionAttribute), true));
            return type.GetMethod("smethod_0", BindingFlags.NonPublic | BindingFlags.Static);
        }

        public static bool PatchPrefix(ref bool __result)
        {
            __result = true;
            return false;
        }
    }
}
