using System;
using System.IO;
using System.Diagnostics;
using System.Windows;
using JET.Launcher.Utilities;
using System.Windows.Controls;
using System.Threading;
using JET.Launcher.Utilities.Form;

namespace JET.Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;
        FileManager __FileM = new FileManager();
        Utilities.Form.Manager __FormM;
        internal ProcessManager __ProcM;
        internal LauncherConfigLoader __LauncherConfigL;
        public MainWindow()
        {
            //Environment.CurrentDirectory = @"J:\TarkovGame\EFT_10988 - Emulator"; // Change to debug path
#if DEBUG
            Environment.CurrentDirectory = @"C:\Emu Tarkov\12.9.10988\original\client"; // Change to debug path
#endif
            var managedPath = Path.Combine(Environment.CurrentDirectory, "EscapeFromTarkov_Data/Managed");
            foreach (var file in Directory.GetFiles(managedPath))
                Helper.UnblockFile(Path.Combine(managedPath, file));

            Instance = this;

            /*
             * Enable that after you finish developing :)
             * */
#if !DEBUG
            if (!ProgramManager.isGameFilesFound()) {
                Application.Current.Shutdown();
                return;
            }
#endif
            Application.Current.DispatcherUnhandledException += (sender, args) => ProgramManager.HandleException(args.Exception);
            AppDomain.CurrentDomain.UnhandledException += ProgramManager.CurrentDomainOnUnhandledException;

            // load missing assemblies from EFT's managed directory
            AppDomain.CurrentDomain.AssemblyResolve += ProgramManager.AssemblyResolveEvent;
            InitializeStartups();
            InitializeComponent();
            InitializeLauncherProps();
        }
        #region Initialazers
        private void InitializeStartups()
        {

            // initialize launcher config
            __LauncherConfigL = new LauncherConfigLoader();

            // confirm server location
            __FileM.FindServerDirectory(__LauncherConfigL.GetServerLocation);

            // did location of the server changed ?? then save it if yes
            if (Global.ServerLocation != __LauncherConfigL.GetServerLocation)
                __LauncherConfigL.ChangeServerLocation(Global.ServerLocation);
        }
        private void InitializeLauncherProps()
        {
            __AutoServerStart_RadioButton.IsChecked = __LauncherConfigL.StartServerAtLaunch();
            _AutoServerStart_RadioButton = __LauncherConfigL.StartServerAtLaunch();
            __FormM = new Utilities.Form.Manager(); // initialize Form Manager
            __ProcM = new ProcessManager();

            if (__LauncherConfigL.StartServerAtLaunch())
            {
                __ProcM.StartConsoleInsideLauncher();
                if (ProcessManager.consoleProcessName != "")
                {
                    __StartStopServer.Content = "Stop Server";
                }
            }

            // set launcher title
            Title = Global.LauncherName + " " + Global.LauncherVersion;
            __FormM.UpdateApplyButton("connect");
            __FormM.SetupIntervalUpdater(__ProcM);
        }
        #endregion

        /*
         * Managing Events for Objects in form below
         */
        #region Change Combobox ServerList
        private void __ServerList_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ServerManager.SelectServer(__ServerList.SelectedIndex);
        }
        #endregion
        #region Handle Clicking
        private void __ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            __FormM.ApplyButtonClickEvent(sender, e);
        }
        private void __RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            __FormM.RegisterButtonClickEvent(sender, e);
        }
        private void __WipeButton_Click(object sender, RoutedEventArgs e)
        {
            __FormM.RegisterButtonClickEvent(sender, e);
        }
        private void OpenLocalServerWeb_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(RequestManager.GetBackendUrl());
        }
        private void bnt7_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://justemutarkov.eu");
        }

        private void bnt8_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/justemutarkov");
        }
        private void OpenServerLogs_Click(object sender, RoutedEventArgs e)
        {
            FileManager.OpenDirectory(Global.Server.LogsFolderDir);
        }
        private void OpenGameLogs_Click(object sender, RoutedEventArgs e)
        {
            FileManager.OpenDirectory(Path.Combine(Environment.CurrentDirectory, Global.PATH.GameLogs));
        }
        private void ClearCache_Click(object sender, RoutedEventArgs e)
        {
            if (FileManager.DeleteDirectoryFiles(Path.Combine(Global.ServerLocation, Global.PATH.ServerCache)))
            {
                MessageBoxManager.Show("Cache properly wiped.", "Information:");
            }
        }
        private void OpenServerMods_Click(object sender, RoutedEventArgs e)
        {
            FileManager.OpenDirectory(Global.Server.ModsFolderDir);
        }
        private void OpenTarkovTemp_Click(object sender, RoutedEventArgs e)
        {
            FileManager.OpenDirectory(Global.Server.TempFolderDir_TarkovTemp);
        }
        private void StartStopServer_Click(object sender, RoutedEventArgs e)
        {
            __ProcM.StartOrStop();
        }
        private void AddServer_Click(object sender, RoutedEventArgs e)
        {
            __LauncherConfigL.AddServer(___NewServerBackend.Text);
        }
        private void DeleteSelectedServer_Click(object sender, RoutedEventArgs e)
        {
            __LauncherConfigL.RemoveServer(__ServerList.SelectedIndex);
        }

        private void CreateShortcut_Click(object sender, RoutedEventArgs e)
        {
            ProcessManager.CreateShortcut();
        }
        private void MoveBack_Click(object sender, RoutedEventArgs e)
        {
            __FormM.BackButtonClickEvent(sender, e);
        }
        private void CreateAccountButton_Click(object sender, RoutedEventArgs e)
        {
           // _LoginField.Text;
        }
        bool _AutoServerStart_RadioButton;
        private void __AutoServerStart_RadioButton_Click(object sender, RoutedEventArgs e)
        {
            _AutoServerStart_RadioButton = !_AutoServerStart_RadioButton;
            var s = sender as RadioButton;
            s.IsChecked = _AutoServerStart_RadioButton;
            Console.WriteLine(s.IsChecked);
            __LauncherConfigL.ChangeStartServerAtLaunch(_AutoServerStart_RadioButton);

        }
        #endregion
        #region Server Text changed event
        private void TextChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            var rtbox = sender as RichTextBox;
            if (rtbox.Document == null)
                return;
            // content of server changed event !!!
            //TextRange documentRange = new TextRange(rtbox.Document.ContentStart, rtbox.Document.ContentEnd);
            //documentRange.ClearAllProperties();

        }
        #endregion

        private void Main_Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Bring window to front when starting
            WindowState = WindowState.Minimized;
            Show();
            WindowState = WindowState.Normal;
        }


    }
}

