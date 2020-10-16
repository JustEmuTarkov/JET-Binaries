using NLog.Targets;
using JET.Utilities.Hook;
using System.Reflection;

namespace JET.SinglePlayer.Hook
{
	[ObfuscationAttribute(Exclude = true)]
	[Target("JET.SinglePlayer")]
	public sealed class Target : TargetWithLayout
	{
		public Target()
		{
			Loader<Instance>.Load();
		}
	}
}
