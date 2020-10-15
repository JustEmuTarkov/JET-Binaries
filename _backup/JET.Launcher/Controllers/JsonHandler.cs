using System;
using System.IO;
using JET.Common.Utils.App;

namespace JET.Launcher
{
	public static class JsonHandler
	{
		private static readonly string filepath;
		private static StaticData staticData = new StaticData();
		static JsonHandler()
		{
			filepath = Environment.CurrentDirectory;
		}

		public static LauncherConfig LoadLauncherConfig()
		{
			return Json.Load<LauncherConfig>(Path.Combine(filepath, staticData.launcherConfigFile));
		}

		public static void SaveLauncherConfig(LauncherConfig data)
		{
			Json.Save(Path.Combine(filepath, staticData.launcherConfigFile), data);
		}

		public static ClientConfig LoadClientConfig()
		{
			return Json.Load<ClientConfig>(Path.Combine(filepath, staticData.clientConfigFile));
		}

		public static void SaveClientConfig(ClientConfig data)
		{
			Json.Save(Path.Combine(filepath, staticData.clientConfigFile), data);
		}
	}
}
