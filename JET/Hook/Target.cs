﻿using NLog.Targets;
using JET.Utilities.Hook;

namespace JET.SinglePlayer.Hook
{
	[Target("JET.SinglePlayer")]
	public sealed class Target : TargetWithLayout
	{
		public Target()
		{
			Loader<Instance>.Load();
		}
	}
}
