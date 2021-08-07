using EFT;
using JET.Utilities;
using JET.Utilities.Patching;
using System.Linq;
using System.Reflection;

namespace JET.Patches.Logging
{
    class InitialHookPatch : GenericPatch<InitialHookPatch>
    {
        public InitialHookPatch() : base(postfix: nameof(PatchPostifx)) {}

        protected override MethodBase GetTargetMethod()
            => typeof(MainApplication).GetConstructors().First();

        static void PatchPostifx() => UnityEngine.Debug.unityLogger.logHandler = UnityLogger.Instance;
    }
}
