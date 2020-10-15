using System;
using System.Linq;
using System.Reflection;
using JET.Common.Utils.Patching;
using UnityEngine.Networking;

namespace JET.Core.Patches
{
	public class SslCertificatePatch : GenericPatch<SslCertificatePatch>
	{
		public SslCertificatePatch() : base(prefix: nameof(PatchPrefix)) {}

		protected override MethodBase GetTargetMethod()
		{
			return PatcherConstants.TargetAssembly.GetTypes()
				.Single((Type x) => x.BaseType == typeof(CertificateHandler))
				.GetMethod("ValidateCertificate", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.NonPublic);
		}

		static bool PatchPrefix(ref bool __result)
		{
			__result = true;
			return false;
		}
	}
}
