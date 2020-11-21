using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JET.Launcher.Structures
{
	public class LauncherConfig
	{
		public List<string> Servers;
		public string Email;
		public string Password;
		public string ServerPath;
		public bool MinimizeToTray;
		public bool AutoStartServer;

		public LauncherConfig()
		{
			Servers = new List<string>() { "https://127.0.0.1:443" };
			Email = "";
			Password = "";
			ServerPath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "../"));
			MinimizeToTray = true;
			AutoStartServer = false;
		}
	}
}
