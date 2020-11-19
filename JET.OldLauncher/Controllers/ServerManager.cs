using System;
using System.Collections.Generic;
using JET.Utilities.App;

namespace JET.OldLauncher
{
	public class ServerManager
	{
		public static List<ServerInfo> AvailableServers = new List<ServerInfo>();
		public static ServerInfo SelectedServer = new ServerInfo();

		/*public ServerManager()
		{
		}*/
		public static bool requestSended = false;
		public static void SelectServer(int index)
		{
			if (index < 0 || index >= AvailableServers.Count)
			{
				SelectedServer = null;
				return;
			}
			SelectedServer = AvailableServers[index];
		}

		public static void LoadServer(string backendUrl)
		{
            string json;
            try
			{
				RequestHandler.ChangeBackendUrl(backendUrl);
				json = RequestHandler.RequestConnect();
			}
			catch
			{
				return;
			}

			AvailableServers.Add(Json.Deserialize<ServerInfo>(json));
		}

		public static void LoadServers(string[] servers)
		{
			AvailableServers.Clear();

			foreach (string backendUrl in servers)
			{
				LoadServer(backendUrl);
			}
			requestSended = false;
		}
	}
}
