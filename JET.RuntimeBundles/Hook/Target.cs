using NLog.Targets;
using JET.Common.Utils.Hook;
using JET.RuntimeBundles;

namespace JET.RuntimeBundles.Hook
{
	[Target("JET.RuntimeBundles")]
	public sealed class Target : TargetWithLayout
	{
		public Target()
		{
			// not working at .7294 > used in lastest game versions
			//Loader<Instance>.Load();
		}
	}
}
