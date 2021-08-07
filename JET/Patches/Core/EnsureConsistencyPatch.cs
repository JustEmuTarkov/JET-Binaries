#if B13074 || B13487
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
			return typeof(ICheckResult)
				.Assembly.GetTypes()
				.Single(Class => Class.Name == "ConsistencyMetadataProvider")
				.GetMethod("GetConsistencyMetadata", BindingFlags.Public | BindingFlags.Instance);
		}

		static bool PatchPrefix(ref System.Collections.Generic.IReadOnlyList<FileConsistencyMetadata> __result)
		{
			Debug.LogError("No Files to check");
			__result = new FileConsistencyMetadata[] {};
			// you can add your own files to check to disallow game to run if something is not found 
			// filename, size, checksum, is critical
			//new FileConsistencyMetadata("EscapeFromTarkov.exe", 661712L, 72195026, true),
			//__result = Task.FromResult<ICheckResult>(new ScanResult());
			return false;
		}
	}
	//class ScanResult : ICheckResult
	//{
	//	public TimeSpan ElapsedTime { get; private set; }
	//	public Exception Exception { get; private set; }

	//	public ScanResult()
	//	{
	//		ElapsedTime = new TimeSpan(5);
	//		Exception = null;

	//	}
	//}
}
#endif
