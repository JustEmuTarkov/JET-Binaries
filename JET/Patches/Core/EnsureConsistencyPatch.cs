using FilesChecker;
using JET.Utilities.Patching;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace JET.Patches
{
    class EnsureConsistencyPatch : GenericPatch<EnsureConsistencyPatch>
    {
		public EnsureConsistencyPatch() : base(prefix: nameof(PatchPrefix)){}

		protected override MethodBase GetTargetMethod()
		{
			/*return PatcherConstants.TargetAssembly.GetTypes().First(x =>
					x.IsClass &&
					x.GetMethod("Awake", BindingFlags.Public | BindingFlags.Instance) != null)
				.GetNestedTypes(BindingFlags.NonPublic)
				.Single(y => y.GetConstructor(new[] { typeof(int) }) != null).GetMethod("method_0",
					BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);*/

			return typeof(EFT.MainApplication)
				.GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance)
				.Single(x => x.Name == "Class1000")
				.GetMethod("method_0", BindingFlags.NonPublic | BindingFlags.Instance);
		}

		static bool PatchPrefix(ref Task<ICheckResult> __result)
		{
			__result = Task.FromResult<ICheckResult>(new ScanResult());
			Debug.LogError(__result.Result.Exception == null);
			Debug.LogError("FUCKING CALLED ?? MAYBE ??");
			return false;
		}
	}
	class ScanResult : ICheckResult
	{
		public TimeSpan ElapsedTime { get; private set; }
		public Exception Exception { get; private set; }

		public ScanResult()
		{
			ElapsedTime = new TimeSpan(5);
			Exception = null;

		}
	}
}
