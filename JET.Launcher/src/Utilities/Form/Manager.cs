using JET.Launcher.Structures;
using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace JET.Launcher.Utilities.Form
{
    internal class Manager
    {
        private Helper _Helper;
        internal Manager() { _Helper = new Helper(); }

        internal static string ApplyButtonAction = "connect";
        internal void EnableApplyButton(bool enable) => MainWindow.Instance.__ApplyButton.IsEnabled = true;
        
        /// <summary>
        /// Handles ApplyButton in the WPF Form
        /// Possible Actions: "connect", "login", "startgame", ""(disable)
        /// </summary>
        /// <param name="action"></param>
        internal void UpdateApplyButton(string action) {
            switch (action) {
                case "connect":
                    MainWindow.Instance.__ApplyButton.Content = "Connect to Server";
                    ApplyButtonAction = "connect";
                    _Helper.DisplayGrid_LoginConnect("Connect");
                    break;
                case "login":
                    MainWindow.Instance.__ApplyButton.Content = "Login";
                    ApplyButtonAction = "login";
                    _Helper.DisplayGrid_LoginConnect("Login");
                    break;
                case "startgame":
                    MainWindow.Instance.__ApplyButton.Content = "Start";
                    ApplyButtonAction = "startgame";
                    _Helper.DisplayGrid_LoginConnect();
                    break;
                default:
                    ApplyButtonAction = "null";
                    MainWindow.Instance.__ApplyButton.IsEnabled = false;
                    MainWindow.Instance.__ApplyButton.Content = "Unavaliable";
                    _Helper.DisplayGrid_LoginConnect();
                    break;
            }
        }
        private int responseCode = 0;
        internal void ApplyButtonClickEvent(object sender, System.Windows.RoutedEventArgs e) {

            switch (ApplyButtonAction) {
                case "connect":
                    UpdateApplyButton("login");
                    break;
                case "login":
                    //Console.WriteLine($"Login: {MainWindow.Instance._LoginField.Text}, Pass: {MainWindow.Instance._PasswordField.Password}");
                    responseCode = RequestManager.ProfileLogin(MainWindow.Instance._LoginField.Text, MainWindow.Instance._PasswordField.Password);
                    switch (responseCode) {
                        case 1:
                            UpdateApplyButton("startgame");
                            MainWindow.Instance.__Label_LoggedAs.Content = RequestManager.SelectedAccount.email;
                            break;
                        case -1: 
                            MessageBoxManager.Show("Wrong login data. Email or password not exist on the server", "Login Error!!", MessageBoxManager.Button.OK, MessageBoxManager.Image.Error);
                            break;
                        case -2: 
                            MessageBoxManager.Show("Couldnt finish a request script crashed at login or getting account data", "Request Error!!", MessageBoxManager.Button.OK, MessageBoxManager.Image.Error);
                            break;
                        default:
                            MessageBoxManager.Show($"Something goes wrong and returned code {responseCode}", "Unknown Error!!", MessageBoxManager.Button.OK, MessageBoxManager.Image.Error);
                            break;
                    }
                    break;
                case "startgame":
                    responseCode = MainWindow.Instance.__ProcM.LaunchGame();
                    switch (responseCode)
                    {
                        case 1:
                            break;
                        case -1:
                            MessageBoxManager.Show("Emulator Installed in official game", "Error!!", MessageBoxManager.Button.OK, MessageBoxManager.Image.Error);
                            break;
                        case -2:
                            MessageBoxManager.Show("Failed Starting Game Somethign goes wrong...", "Error!!", MessageBoxManager.Button.OK, MessageBoxManager.Image.Error);
                            break;
                        case -3:
                            MessageBoxManager.Show("EscapeFromTarkov.exe is missing", "File Missing Error!!", MessageBoxManager.Button.OK, MessageBoxManager.Image.Error);
                            break;
                        case -4:
                            //Emulator not installed
                            break;
                        default:
                            MessageBoxManager.Show($"Something goes wrong and returned code {responseCode}", "Unknown Error!!", MessageBoxManager.Button.OK, MessageBoxManager.Image.Error);
                            break;
                    }
                    break;
            }
        }
        internal void BackButtonClickEvent(object sender, System.Windows.RoutedEventArgs e)
        {
            switch (ApplyButtonAction)
            {
                case "connect":
                    break;
                case "login":
                    UpdateApplyButton("connect");
                    break;
                case "startgame":
                    UpdateApplyButton("login");
                    break;
            }
        }

        #region Interval Tick Timer
        internal static DispatcherTimer TickUpdater = new DispatcherTimer();
        internal void SetupIntervalUpdater(ProcessManager _procM) {
            TickUpdater.Tick += _Helper.Updater_Tick;
            TickUpdater.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            TickUpdater.Start();
        }
        #endregion
    }
}
