﻿using System.Reflection;
using JET.Common.Utils.Patching;
using JET.SinglePlayer.Utils;
using BotDifficultyHandler = GClass280;

namespace JET.SinglePlayer.Patches.Bots
{
	public class CoreDifficultyPatch : AbstractPatch
	{
		public CoreDifficultyPatch()
		{
			methodName = "LoadCoreByString";
			flags = BindingFlags.Public | BindingFlags.Static;
		}

		public override MethodInfo TargetMethod()
		{
			return typeof(BotDifficultyHandler).GetMethod(methodName, flags);
		}

		public static bool Prefix(ref string __result)
		{
			__result = Settings.CoreDifficulty;

			if (string.IsNullOrEmpty(__result))
			{
				return true;
			}

			return false;
		}
	}
}
