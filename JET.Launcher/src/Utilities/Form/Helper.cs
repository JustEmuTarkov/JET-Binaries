using JET.Launcher.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JET.Launcher.Utilities.Form
{
    internal class Helper
    {
        internal Helper() { }

        internal void DisplayGrid_LoginConnect(string name = "")
        {
            switch (name)
            {
                case "Connect":
                    MainWindow.Instance.__Connect.Visibility = System.Windows.Visibility.Visible;
                    MainWindow.Instance.__Login.Visibility = System.Windows.Visibility.Hidden;
                    MainWindow.Instance.__LoggedIn.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case "Login":
                    MainWindow.Instance.__Connect.Visibility = System.Windows.Visibility.Hidden;
                    MainWindow.Instance.__Login.Visibility = System.Windows.Visibility.Visible;
                    MainWindow.Instance.__LoggedIn.Visibility = System.Windows.Visibility.Hidden;
                    break;
                default:
                    MainWindow.Instance.__Connect.Visibility = System.Windows.Visibility.Hidden;
                    MainWindow.Instance.__Login.Visibility = System.Windows.Visibility.Hidden;
                    MainWindow.Instance.__LoggedIn.Visibility = System.Windows.Visibility.Visible;
                    break;
            }
        }

        #region Tick Updater
        internal void Updater_Tick(object sender, EventArgs e)
        {
            ServerCheck();
            UpdateStartServerButton();
            UpdateDeleteServerButton();
        }
        #region Functions
        private void UpdateDeleteServerButton()
        {
            if (MainWindow.Instance.__LauncherConfigL.GetServersCount() > 0)
                MainWindow.Instance.__DeleteSelectedServer.IsEnabled = true;
            else
                MainWindow.Instance.__DeleteSelectedServer.IsEnabled = false;

        }
        private void UpdateStartServerButton()
        {
            if (ProcessManager.consoleProcessName != "")
            {
                //Process running
                MainWindow.Instance.__StartStopServer.Content = "Stop Server";
            }
            else
            {
                //Process not present
                MainWindow.Instance.__StartStopServer.Content = "Start Server";
            }
        }
        private bool LocalServerFound = false;
        private int LastServersCheckCount = 0;
        private void ServerCheck()
        {
            if (!RequestManager.OngoingRequest)
            {
                if (ServerManager.AvailableServers.Count <= 0)
                {
                    Task.Factory.StartNew(() => {
                        ServerManager.LoadServer(); // load actual selected server
                    });
                }
                else
                {
                    if (MainWindow.Instance.__ServerList.Items.Count <= 0 || LastServersCheckCount != ServerManager.AvailableServers.Count)
                    {
                        
                        MainWindow.Instance.__ServerList.Items.Clear();

                        foreach (var server in ServerManager.AvailableServers.ToList())
                        {
                            if (ServerManager.LoadServerFromDifferentBackend(server.backendUrl))
                            {
                                if(server.backendUrl.Contains("localhost") || server.backendUrl.Contains("127.0.0.1"))
                                    LocalServerFound = true;
                                MainWindow.Instance.__ServerList.Items.Add(server.name);
                            }
                        }
                        if (MainWindow.Instance.__ServerList.Items.Count > 0)
                        {
                            MainWindow.Instance.__ServerTab.IsEnabled = LocalServerFound;
                            MainWindow.Instance.__ApplyButton.IsEnabled = true;
                            MainWindow.Instance.__ServerList.SelectedIndex = 0;
                            ServerManager.SelectServer(0);
                        }
                        else
                        {
                            //MainWindow.Instance.__ServerList.Text = "No Servers Found";
                        }
                        LastServersCheckCount = MainWindow.Instance.__ServerList.Items.Count;
                    }
                }
            }
        }
        #endregion
        #endregion

    }
}
