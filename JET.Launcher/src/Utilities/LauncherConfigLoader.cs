using JET.Launcher.Structures;
using JET.Utilities.App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JET.Launcher.Utilities
{
    class LauncherConfigLoader
    {
        private LauncherConfig launcherConfig;
        internal AccountInfo SelectedAccount { get; private set; }

		internal bool ConfigFileExists() => File.Exists(Path.Combine(Environment.CurrentDirectory, "launcher.config.json"));
		internal LauncherConfig Load()
		{
			return Json.Load<LauncherConfig>(Path.Combine(Environment.CurrentDirectory, "launcher.config.json"));
		}

		internal void Save(LauncherConfig data)
		{
			Json.Save(Path.Combine(Environment.CurrentDirectory, "launcher.config.json"), data);
		}
	}
}
