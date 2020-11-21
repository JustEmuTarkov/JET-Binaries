using JET.Launcher.Structures;
using JET.Utilities.App;
using System;
using System.Collections.Generic;
using System.IO;

namespace JET.Launcher.Utilities
{
    class LauncherConfigLoader
    {
		internal LauncherConfigLoader() {
			if (ConfigFileExists())
				Load();
			else
				Save(new LauncherConfig());
		}
		private LauncherConfig launcherConfig;
        internal AccountInfo SelectedAccount { get; private set; }
		internal string GetServerLocation { get { return launcherConfig.ServerPath; } }

		internal void ChangeServerLocation(string location) {
			launcherConfig.ServerPath = location;
			Save();
		}
		internal List<string> GetServers() {
			return launcherConfig.Servers;
		}
		internal void AddServer(string BackendUrl) {
			launcherConfig.Servers.Add(BackendUrl);
		}
		internal void RemoveServer(int index) {
			launcherConfig.Servers.RemoveAt(index);
		}
		internal void RemoveServer(string BackendUrl) {
			for (int i = 0; i < launcherConfig.Servers.Count; i++) {
				if (launcherConfig.Servers[i] == BackendUrl) {
					launcherConfig.Servers.RemoveAt(i);
				}
			}
		}
		internal bool ConfigFileExists() => File.Exists(Path.Combine(Environment.CurrentDirectory, "launcher.config.json"));
		internal void Load()
		{
			launcherConfig = Json.Load<LauncherConfig>(Path.Combine(Environment.CurrentDirectory, "launcher.config.json"));
		}

		internal void Save(LauncherConfig data)
		{
			Json.Save(Path.Combine(Environment.CurrentDirectory, "launcher.config.json"), data);
		}
		private void Save()
		{
			Json.Save(Path.Combine(Environment.CurrentDirectory, "launcher.config.json"), launcherConfig);
		}
	}
}
