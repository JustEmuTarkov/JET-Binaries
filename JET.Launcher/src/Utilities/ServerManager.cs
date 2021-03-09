using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Threading;
using JET.Launcher.Structures;
using JET.Launcher.Utilities;
using JET.Utilities.App;

namespace JET.Launcher.Utilities
{
    internal class ServerManager
    {
        public static ObservableCollection<RequestData.ServerInfo> AvailableServers = new ObservableCollection<RequestData.ServerInfo>();
        public static RequestData.ServerInfo SelectedServer = new RequestData.ServerInfo();
        private static bool updatingCollection = false;
        static ServerManager()
        {
            AvailableServers.CollectionChanged += (sender, args) =>
                {
                    if (updatingCollection) return;
                    MainWindow.Instance.Dispatcher.Invoke(() =>
                        {
                            var newCollection = AvailableServers.Select(x => x.name ?? x.backendUrl).ToArray();
                            var currentCollection = MainWindow.Instance.__ServerList.Items.SourceCollection.Cast<string>().ToArray();
                            if (newCollection.Length != currentCollection.Length || newCollection.SequenceEqual(currentCollection))
                            {
                                MainWindow.Instance.__ServerList.ItemsSource = AvailableServers.Select(x => x.name ?? x.backendUrl);
                                if (newCollection.Length <= 0) return;
                                MainWindow.Instance.__ServerList.SelectedIndex = 0;
                                SelectServer(0);
                            }
                        });
                };
        }
        public static void SelectServer(int index)
        {
            if (index < 0 || index >= AvailableServers.Count)
            {
                SelectedServer = null;
                MessageBoxManager.Show("Selected server is out of bounds from saved server list", "Out of bounds: Error", MessageBoxManager.Button.OK, MessageBoxManager.Image.Error);
                return;
            }
            SelectedServer = AvailableServers[index];
            RequestManager.ChangeBackendUrl(SelectedServer.backendUrl);
        }
        public static void SelectServer(string BackendName)
        {
            var selected = AvailableServers.FirstOrDefault(server => server.backendUrl == BackendName);
            if (selected != default(RequestData.ServerInfo))
            {
                SelectedServer = selected;
            }
        }
        internal static bool LoadServer()
        {
            RequestManager.Busy();
            try
            {
                var json = RequestManager.Connect();
                Console.WriteLine(json);
                if (json == "")
                {
                    RequestManager.Free();
                    return false;
                }
                var serverInfo = Json.Deserialize<RequestData.ServerInfo>(json);
                AvailableServers.Add(serverInfo);
            }
            catch (Exception e) { Console.WriteLine(e); }
            RequestManager.Free();
            return true;
        }

        private static readonly object ListLock = new object();
        internal static bool LoadServerFromDifferentBackend(string backend, bool save = false)
        {
            if (RequestManager.OngoingRequest) return false;
            RequestManager.Busy();
            try
            {
                var json = RequestManager.Connect(backend);
                if (json == "")
                {
                    RequestManager.Free();
                    return false;
                }
                var serverInfo = Json.Deserialize<RequestData.ServerInfo>(json);
                if (save)
                {
                    lock (ListLock)
                    {
                        updatingCollection = true;
                        if (AvailableServers.Any(x => x.backendUrl == backend))
                            AvailableServers.Remove(AvailableServers.First(x => x.backendUrl == backend));
                        updatingCollection = false;
                        AvailableServers.Add(serverInfo);
                    }
                }

            }
            catch (Exception e) { Console.WriteLine(e); }
            RequestManager.Free();
            return true;
        }
    }
}
