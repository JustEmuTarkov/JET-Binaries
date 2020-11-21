using System;
using System.IO;
using System.Diagnostics;
using System.Windows;
using JET.Launcher.Utilities;
using JET.Launcher.Structures;

namespace JET.Launcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileManager __FileM = new FileManager();
        FormManager __FormM;
        ProcessManager __ProcM;
        internal LauncherConfigLoader __LauncherConfigL;
        public MainWindow()
        {
            /*if (!ProgramManager.isGameFilesFound()) {
                Application.Current.Shutdown();
                return;
            }*/
            Application.Current.DispatcherUnhandledException += (sender, args) => ProgramManager.HandleException(args.Exception);
            AppDomain.CurrentDomain.UnhandledException += ProgramManager.CurrentDomainOnUnhandledException;
            // load missing assemblies from EFT's managed directory
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ProgramManager.AssemblyResolveEvent);
            InitializeStartups();
            InitializeComponent();
            InitializeLauncherProps();
        }
        private void InitializeStartups() {
            // initialize launcher config
            __LauncherConfigL = new LauncherConfigLoader();
            // confirm server location
            __FileM.FindServerDirectory(__LauncherConfigL.GetServerLocation);
            // did location of the server changed ?? then save it if yes
            if(Global.ServerLocation != __LauncherConfigL.GetServerLocation)
                __LauncherConfigL.ChangeServerLocation(Global.ServerLocation);
        }
        private void InitializeLauncherProps()
        {
            __FormM = new FormManager(this); // initialize Form Manager
            __ProcM = new ProcessManager(this);

            //if (_LauncherConfig.AutoStartServer)
            //{
                __ProcM.StartConsoleInsideLauncher();
            //}


            // set launcher title
            this.Title = Global.LauncherName + " " + Global.LauncherVersion;
            __FormM.UpdateApplyButton("connect");
            __FormM.SetupIntervalUpdater(__ProcM);
        }
        
        /*
         * Managing Events for Objects in form below
         */
        private void __ServerList_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ServerManager.SelectServer(__ServerList.SelectedIndex);
        }

        private void __ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            __FormM.ApplyButtonClickEvent(sender, e);
        }

        private void bnt1_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(RequestManager.GetBackendUrl());
        }

        private void bnt2_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", Path.Combine(Global.ServerLocation, Global.PATH.ServerCache));
        }

        private void bnt3_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, Global.PATH.GameLogs));
        }

        private void bnt4_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Directory.Delete(Path.Combine(Global.ServerLocation, Global.PATH.ServerCache), true);
                Directory.CreateDirectory(Path.Combine(Global.ServerLocation, Global.PATH.ServerCache));
                MessageBoxManager.Show("Cache properly wiped.", "Information:", MessageBoxManager.Button.OK, MessageBoxManager.Image.Information);
            }
            catch (Exception deleteException) 
            {
                MessageBoxManager.Show($"Error occured on deleting files\r\nMessage: {deleteException.Message}", "Error:", MessageBoxManager.Button.OK, MessageBoxManager.Image.Error);
            }
        }
        private void bnt5_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, Global.PATH.ServerMods));
        }

        private void __StartStopServer_Click(object sender, RoutedEventArgs e)
        {
            if (ProcessManager.consoleProcessName != "")
            {
                //Process running
                __StartStopServer.Content = "Stop Server";
                __ProcM.StartConsoleInsideLauncher();
            }
            else
            {
                //Process not present
                __StartStopServer.Content = "Start Server";
                ProcessManager.consoleProcessName = "";
                __ProcM.Terminate();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
