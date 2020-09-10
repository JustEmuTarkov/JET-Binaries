using NLog.Targets;
using JET.Common.Utils.Hook;

namespace JET.Core.Hook
{
	[Target("JET.Core")]
	public sealed class Target : TargetWithLayout
	{
		public Target()
		{
			Loader<Instance>.Load();
		}
	}
}
