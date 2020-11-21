using JET.Launcher.Structures;
using JET.Utilities.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace JET.Launcher.Utilities
{
    internal class FormManager
    {
        private MainWindow mainWindow;
        internal FormManager(MainWindow _mainWindow) { mainWindow = _mainWindow; }
        internal static string ApplyButtonAction = "connect";
        internal void EnableApplyButton(bool enable) => mainWindow.__ApplyButton.IsEnabled = true;
        private void DisplayGrid(string name = "") {
            switch (name) {
                case "Connect":
                    mainWindow.__Connect.Visibility = System.Windows.Visibility.Visible;
                    mainWindow.__Login.Visibility = System.Windows.Visibility.Hidden;
                    break;
                case "Login":
                    mainWindow.__Connect.Visibility = System.Windows.Visibility.Hidden;
                    mainWindow.__Login.Visibility = System.Windows.Visibility.Visible;
                    break;
                default:
                    mainWindow.__Connect.Visibility = System.Windows.Visibility.Hidden;
                    mainWindow.__Login.Visibility = System.Windows.Visibility.Hidden;
                    break;
            }
        }
        /// <summary>
        /// Handles ApplyButton in the WPF Form
        /// Possible Actions: "connect", "login", "startgame", ""(disable)
        /// </summary>
        /// <param name="action"></param>
        internal void UpdateApplyButton(string action) {
            switch (action) {
                case "connect":
                    mainWindow.__ApplyButton.Content = "Connect to Server";
                    ApplyButtonAction = "connect";
                    DisplayGrid("Connect");
                    break;
                case "login":
                    mainWindow.__ApplyButton.Content = "Login";
                    ApplyButtonAction = "login";
                    DisplayGrid("Login");
                    break;
                case "startgame":
                    mainWindow.__ApplyButton.Content = "Start";
                    ApplyButtonAction = "startgame";
                    DisplayGrid();
                    break;
                default:
                    ApplyButtonAction = "null";
                    mainWindow.__ApplyButton.IsEnabled = false;
                    mainWindow.__ApplyButton.Content = "Unavaliable";
                    DisplayGrid();
                    break;
            }
        }
        internal void ApplyButtonClickEvent(object sender, System.Windows.RoutedEventArgs e) {
            if(ApplyButtonAction == "connect")
                UpdateApplyButton("login");
            // Check if can login
            if(ApplyButtonAction == "login")
                UpdateApplyButton("startgame");

        }
        internal static DispatcherTimer TickUpdater = new DispatcherTimer();
        private ProcessManager __ProcM;
        internal void SetupIntervalUpdater(ProcessManager _procM) {
            __ProcM = _procM;
            TickUpdater.Tick += Updater_Tick;
            TickUpdater.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            TickUpdater.Start();
        }
        private void Updater_Tick(object sender, EventArgs e)
        {
                if (!RequestManager.OngoingRequest)
                {
                    if (ServerManager.AvailableServers.Count <= 0)
                    {
                        Console.WriteLine("TryConnect using SavedList");
                        UpdateTick_TryConnect();
                    }
                    else
                    {
                        if (mainWindow.__ServerList.Items.Count <= 0)
                        {
                            Console.WriteLine("Update View List");
                            UpdateTick_UpdateServerListView();
                        }
                    }
                }
            if(__ProcM != null)
                __ProcM.SetConsoleOutputText();
            // code goes here
        }
        private void UpdateTick_TryConnect() {
            Task.Factory.StartNew(() => {
                ServerManager.LoadServers(mainWindow.__LauncherConfigL.GetServers()); // load actual selected server
            });
        }
        private void UpdateTick_UpdateServerListView() {
            //
            mainWindow.__ServerList.Items.Clear();
            //mainWindow.__ServerList.Text = "";

            foreach (RequestData.ServerInfo server in ServerManager.AvailableServers)
            {
                Console.WriteLine(server.name);
                mainWindow.__ServerList.Items.Add(server.name);
            }
            if (mainWindow.__ServerList.Items.Count > 0)
            {
                mainWindow.__ServerTab.IsEnabled = true;
                mainWindow.__ApplyButton.IsEnabled = true;
                mainWindow.__ServerList.SelectedIndex = 0;
                ServerManager.SelectServer(0);
            }
            else
            {
                mainWindow.__ServerList.Text = "No Servers Found";
            }
        }
    }
}
