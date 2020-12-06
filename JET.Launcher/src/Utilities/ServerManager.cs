using System;
using System.Collections.Generic;
using JET.Launcher.Structures;
using JET.Launcher.Utilities;
using JET.Utilities.App;

namespace JET.Launcher.Utilities
{
    class ServerManager
    {
        public static List<RequestData.ServerInfo> AvailableServers = new List<RequestData.ServerInfo>();
        public static RequestData.ServerInfo SelectedServer = new RequestData.ServerInfo();

        public static void SelectServer(int index)
        {
            if (index < 0 || index >= AvailableServers.Count)
            {
                SelectedServer = null;
                MessageBoxManager.Show("Selected server is out of bounds from saved server list", "Out of bounds: Error", MessageBoxManager.Button.OK, MessageBoxManager.Image.Error);
                return;
            }
            SelectedServer = AvailableServers[index];
        }
        public static void SelectServer(string BackendName)
        {
            for (int i = 0; i < AvailableServers.Count; i++)
            {
                if(AvailableServers[i].backendUrl == BackendName)
                    SelectedServer = AvailableServers[i];
            }
        }
        internal static bool LoadServer()
        {
            RequestManager.Busy();
            try
            {
                string json = RequestManager.Connect();
                Console.WriteLine(json);
                if (json == "")
                {
                    RequestManager.Free();
                    return false;
                }
                RequestData.ServerInfo serverInfo = Json.Deserialize<RequestData.ServerInfo>(json);
                AvailableServers.Add(serverInfo);
            }
            catch (Exception e) { Console.WriteLine(e); }
            RequestManager.Free();
            return true;
        }
        internal static bool LoadServerFromDiffrentBackend(string backend, bool save = false)
        {
            RequestManager.Busy();
            try
            {
                RequestManager.ChangeBackendUrl(backend);
                string json = RequestManager.Connect();
                if (json == "")
                {
                    RequestManager.Free();
                    return false;
                }
                RequestData.ServerInfo serverInfo = Json.Deserialize<RequestData.ServerInfo>(json);
                if(save)
                    AvailableServers.Add(Json.Deserialize<RequestData.ServerInfo>(json));
            }
            catch (Exception e) { Console.WriteLine(e); }
            RequestManager.Free();
            return true;
        }
    }
}
