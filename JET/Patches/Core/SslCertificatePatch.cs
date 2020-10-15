using System.Linq;
using System.Reflection;
using JET.Utilities.Patching;
using UnityEngine.Networking;

namespace JET.Patches
{
	public class SslCertificatePatch : GenericPatch<SslCertificatePatch>
	{
		public SslCertificatePatch() : base(prefix: nameof(PatchPrefix)) {}

		protected override MethodBase GetTargetMethod()
		{
			return PatcherConstants.TargetAssembly
				.GetTypes().Single(x => x.BaseType == typeof(CertificateHandler))
				.GetMethod("ValidateCertificate", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
		}

		static bool PatchPrefix(ref bool __result)
		{
			__result = true;

			return false;
		}
	}
}
