﻿using JET.Launcher.Structures;
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
		internal string GetServerLocation { get { return launcherConfig.ServerPath; } }

		internal string Email {
			get { return launcherConfig.Email; }
            set { 
				launcherConfig.Email = value;
				Save();
			}
		}
		internal string Password
		{
			get { return launcherConfig.Password; }
			set
			{
				launcherConfig.Password = value;
				Save();
			}
		}
		internal bool StartServerAtLaunch()
		{
			return launcherConfig.AutoStartServer;
		}
		internal void ChangeStartServerAtLaunch(bool set)
		{
			launcherConfig.AutoStartServer = set;
			Save();
		}
		internal void ChangeServerAtLaunch(bool change)
		{
			launcherConfig.AutoStartServer = change;
			Save();
		}
		internal void ChangeServerLocation(string location) {
			launcherConfig.ServerPath = location;
			Save();
		}
		internal List<string> GetServers() {
			return launcherConfig.Servers;
		}
		internal int GetServersCount() {
			return launcherConfig.Servers.Count;
		}
		internal void AddServer(string BackendUrl) {
			if(!launcherConfig.Servers.Contains(BackendUrl))
				launcherConfig.Servers.Add(BackendUrl);
			Save();
		}
		internal void RemoveServer(int index) {
			if(launcherConfig.Servers.Count > 1)
				launcherConfig.Servers.RemoveAt(index);
			Save();
		}
		internal void RemoveServer(string BackendUrl) {
			for (int i = 0; i < launcherConfig.Servers.Count; i++) {
				if (launcherConfig.Servers[i] == BackendUrl) {
					launcherConfig.Servers.RemoveAt(i);
				}
			}
			Save();
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
