using FilesChecker;
using JET.Utilities.Patching;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace JET.Patches
{
    class EnsureConsistencyPatch : GenericPatch<EnsureConsistencyPatch>
    {
		public EnsureConsistencyPatch() : base(prefix: nameof(PatchPrefix))
		{
		}

		protected override MethodBase GetTargetMethod()
		{
			return Assembly.GetAssembly(typeof(ICheckResult))
				.GetTypes().Single(x => x.Name == "ConsistencyController")
				.GetMethod("EnsureConsistency", BindingFlags.Public | BindingFlags.Instance);
		}

		private static bool PatchPrefix(ref Task<ICheckResult> __result)
		{
			__result = Task.FromResult<ICheckResult>(new ScanResult());
			return false;
		}
	}
	class ScanResult : ICheckResult
	{
		public TimeSpan ElapsedTime { get; private set; }
		public Exception Exception { get; private set; }

		public ScanResult()
		{
			ElapsedTime = new TimeSpan(10);
			Exception = null;
		}
	}
}
