using JET.Utilities;
using JET.Utilities.Patching;
using System.Linq;
using System.Reflection;

namespace JET.Patches.Logging
{
    class ResetHookPatch : GenericPatch<ResetHookPatch>
    {
        public ResetHookPatch() : base(postfix: nameof(PatchPostifx)) { }
        protected override MethodBase GetTargetMethod()
        {
            var loggerClass = PatcherConstants.TargetAssembly.GetTypes().First(x => x.IsClass && x.GetProperty("UnityDebugLogsEnabled") != null);
            var childClass = loggerClass.GetNestedTypes(BindingFlags.NonPublic).First(x => x.GetMethod("Release", BindingFlags.Public | BindingFlags.Instance) != null);
            return childClass.GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).First();
        }

        static void PatchPostifx() => UnityEngine.Debug.unityLogger.logHandler = UnityLogger.Instance;
    }
}
