using NLog.Targets;
using JET.Common.Utils.Hook;

namespace JET.Common.Hook
{
	[Target("JET.Common")]
	public sealed class Target : TargetWithLayout
	{
		public Target()
		{
			Loader<Instance>.Load();
		}
	}
}
