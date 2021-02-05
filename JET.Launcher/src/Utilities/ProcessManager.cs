﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace JET.Launcher.Utilities
{
    
    internal class ProcessManager
    {
		internal ProcessManager() {}
        #region SERVER STARTER

        internal static List<string> _ConsoleOutput = new List<string>();
		internal static string consoleProcessName = ""; // we dont need that but whatever ... we always can ask process variable for anything...
		internal static Process consoleProcessHandle = new Process();
        internal void StartConsoleInsideLauncher() 
        {
            consoleProcessHandle = new Process();// incase resets what was in the variable before
            // initialize process parameters
            consoleProcessHandle.StartInfo.WorkingDirectory = Global.ServerLocation;
			consoleProcessHandle.StartInfo.FileName = Path.Combine(Global.ServerLocation, Global.ServerName);
			consoleProcessHandle.StartInfo.CreateNoWindow = true;
			consoleProcessHandle.StartInfo.UseShellExecute = false;
			consoleProcessHandle.StartInfo.RedirectStandardError = true;
			consoleProcessHandle.StartInfo.RedirectStandardInput = true;
			consoleProcessHandle.StartInfo.RedirectStandardOutput = true;
			consoleProcessHandle.StartInfo.StandardOutputEncoding = Encoding.UTF8;
			consoleProcessHandle.EnableRaisingEvents = true;
			consoleProcessHandle.Exited += ServerTerminated;
			// Start Process
			consoleProcessHandle.Start();
			// last setters
			consoleProcessHandle.BeginOutputReadLine();
			consoleProcessHandle.OutputDataReceived += ServerOutputDataReceived;
			consoleProcessName = consoleProcessHandle.ProcessName;
		}
        #endregion
        #region SERVER FUNCTIONS
        internal void StartOrStop() {
            if (consoleProcessName == "")
            {
                //Process running
                MainWindow.Instance.__StartStopServer.Content = "Stop Server";
                StartConsoleInsideLauncher();
            }
            else
            {
                //Process not present
                MainWindow.Instance.__StartStopServer.Content = "Start Server";
                Terminate();
                MainWindow.Instance.__ServerConsole.Document.Blocks.Clear();
            }
        }
        internal void Terminate() {
			consoleProcessHandle.Kill();
			consoleProcessName = "";
		}
		internal void ServerTerminated(object sender, EventArgs e)
		{
			Console.WriteLine("***** Server Closed *****");
			// server closed what now ?
		}
        #region ConsoleRegex Removal strange artifacts
        List<string> TagsToRemoveFromConsoleOutput = new List<string>() {
			"\\[2J\\[0;0f",
			"[┌│┐┘└─]"
		};
		private void RemoveConsoleTags(ref string _ConsoleOutput) {
			_ConsoleOutput = _ConsoleOutput.Replace($"", "");
			var consoleEnum = TagsToRemoveFromConsoleOutput.GetEnumerator();
			while (consoleEnum.MoveNext())
			{
				_ConsoleOutput = Regex.Replace(_ConsoleOutput, consoleEnum.Current, "");
			}
		}
        #endregion

        private Color SwitchableColor;
		private void ServerOutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (e.Data != null)
			{
				string tConsoleOutput = e.Data;
				RemoveConsoleTags(ref tConsoleOutput);
                string[] splitLine = tConsoleOutput.Split(new string[] { "[0m" }, StringSplitOptions.None);
                //adding to the stack
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    for (int index = 0; index < splitLine.Length; index++) {
                        if (index == 0)
                        {
                            SwitchableColor = Colors.White;
                            ProcessText(ref SwitchableColor, ref splitLine[index]);
                            RichTextBox_AppendText(splitLine[index], SwitchableColor);
                        }
                        else 
                        {
                            MainWindow.Instance.__ServerConsole.AppendText(splitLine[index]);
                        }
                    }
                });
			}
		}

        private void ProcessText(ref Color switchableColor, ref string text)
        {
            /*if (text.Contains("[30m") || text.Contains("[40m"))
            {
                switchableColor = Colors.Black;
            }
            else*/
            if (text.Contains("[31m") || text.Contains("[41m"))
            {
                switchableColor = Colors.Red;
            }
            else if (text.Contains("[32m") || text.Contains("[42m"))
            {
                switchableColor = Colors.Green;
            }
            else if (text.Contains("[33m") || text.Contains("[43m"))
            {
                switchableColor = Colors.Yellow;
            }
            else if (text.Contains("[34m") || text.Contains("[44m"))
            {
                switchableColor = Colors.Blue;
            }
            else if (text.Contains("[35m") || text.Contains("[45m"))
            {
                switchableColor = Colors.Magenta;
            }
            else if (text.Contains("[36m") || text.Contains("[46m"))
            {
                switchableColor = Colors.Cyan;
            }
            else 
            {
                switchableColor = Colors.White;
            }
            text = Regex.Replace(text, "\\[[3-4][0-7]m", "");
            //text = Regex.Replace(text, "\\[0m", "");
        }
        #region RichTextBox - Server Console Text Appender
        private Run _textRenderer;
        private Paragraph _paragraphRenderer;
        private void RichTextBox_AppendText(string text, Color color)
        {
            _textRenderer = new Run(text);
            _textRenderer.Foreground = new SolidColorBrush(color); // My Color
            _paragraphRenderer = new Paragraph(_textRenderer);
            MainWindow.Instance.__ServerConsole.Document.Blocks.Add(_paragraphRenderer);
        }
        #endregion
        #endregion
        public int LaunchGame()
        {
            if (IsInstalledInLive())
            {
                return -1;
            }

            SetupGameFiles();

            // TODO: check if files exists in game directory; in less retarded way

            if (!EmulatorInstalled())
                return -4;

            if (!File.Exists("EscapeFromTarkov.exe"))
            {
                return -3;
            }

            try
            {
                ProcessStartInfo clientProcess = new ProcessStartInfo("EscapeFromTarkov.exe")
                {
                    Arguments = GenerateArguments(),
                    UseShellExecute = false,
                    WorkingDirectory = Environment.CurrentDirectory
                };
                Process.Start(clientProcess);
            } catch (Exception) 
            { 
                return -2; 
            }

            return 1;
        }
        private bool EmulatorInstalled() {
            if (File.Exists($"EscapeFromTarkov_Data/Managed/0Harmony.dll"))
            {
                if (File.Exists($"EscapeFromTarkov_Data/Managed/Assembly-CSharp.dll"))
                {
                    if (File.Exists($"EscapeFromTarkov_Data/Managed/NLog.dll.nlog"))
                    {
                        if (File.Exists($"EscapeFromTarkov_Data/Managed/NLog.JET.dll"))
                        {
                            return true;
                        }
                        MessageBoxManager.Show("Missing file: EscapeFromTarkov_Data/Managed/NLog.JET.dll", "Missing Files!", MessageBoxManager.Button.OK, MessageBoxManager.Image.Information);
                        return false;
                    }
                    MessageBoxManager.Show("Missing file: EscapeFromTarkov_Data/Managed/NLog.dll.nlog", "Missing Files!", MessageBoxManager.Button.OK, MessageBoxManager.Image.Information);
                    return false;
                }
                MessageBoxManager.Show("Missing file: EscapeFromTarkov_Data/Managed/Assembly-CSharp.dll", "Missing Files!", MessageBoxManager.Button.OK, MessageBoxManager.Image.Information);
                return false;
            }
            MessageBoxManager.Show("Missing file: EscapeFromTarkov_Data/Managed/0Harmony.dll", "Missing Files!", MessageBoxManager.Button.OK, MessageBoxManager.Image.Information);
            return false;
        }
        private bool IsInstalledInLive()
        {
            // TODO: rework this shit

            var value0 = false;
            /*
            try
            {
                var value1 = Registry.LocalMachine.OpenSubKey(@"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\EscapeFromTarkov", false).GetValue("UninstallString");
                var value2 = (value1 != null) ? value1.ToString() : "";
                var value3 = new FileInfo(value2);
                var value4 = new FileInfo[]
                {
                    new FileInfo(value2.Replace(value3.Name, @"JET Launcher.exe")),
                    new FileInfo(value2.Replace(value3.Name, @"EscapeFromTarkov_Data\Managed\0Harmony.dll")),
                    new FileInfo(value2.Replace(value3.Name, @"EscapeFromTarkov_Data\Managed\NLog.dll.nlog")),
                    new FileInfo(value2.Replace(value3.Name, @"EscapeFromTarkov_Data\Managed\Nlog.JET.dll"))
                };

                foreach (var value in value4)
                {
                    if (File.Exists(value.FullName))
                    {
                        File.Delete(value.FullName);
                        value0 = true;
                    }
                }

                if (value0)
                {
                    File.Delete(@"EscapeFromTarkov_Data\Managed\Assembly-CSharp.dll");
                }
            }
            catch
            {
            }
            */
            return value0;
        }
        private List<string> FilesToRemove = new List<string>() {
            Path.Combine(Environment.CurrentDirectory, "BattlEye"),
            Path.Combine(Environment.CurrentDirectory, "Logs"),
            Path.Combine(Environment.CurrentDirectory, "EscapeFromTarkov_BE.exe"),
            Path.Combine(Environment.CurrentDirectory, "Uninstall.exe"),
            Path.Combine(Environment.CurrentDirectory, "UnityCrashHandler64.exe")
        };
        private void SetupGameFiles()
        {
            foreach (string file in FilesToRemove)
            {
                try
                {
                    if (Directory.Exists(file))
                    {
                        Directory.Delete(file, true);
                    }

                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }
                }
                catch (Exception) { 
                    MessageBoxManager.Show("Something got fucked up. I could not delete a file:\r\n" + file, "Deletion Failed!", MessageBoxManager.Button.OK, MessageBoxManager.Image.Information); 
                }
            }
        }
        
        private void RemoveRegisteryKeys()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Battlestate Games\EscapeFromTarkov", true);

                foreach (string value in key.GetValueNames())
                {
                    key.DeleteValue(value);
                }
            }
            catch
            {
                MessageBoxManager.Show("Unable to remove registry from:\r\nSoftware\\Battlestate Games\\EscapeFromTarkov", "Deletion Failed!", MessageBoxManager.Button.OK, MessageBoxManager.Image.Information);
            }
        }

        internal static void CleanTempFiles()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(Path.GetTempPath(), $@"Battlestate Games\EscapeFromTarkov"));

            if (!Directory.Exists(Path.Combine(Path.GetTempPath(), $@"Battlestate Games\EscapeFromTarkov")))
            {
                return;
            }

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
            {
                directory.Delete(true);
            }
        }
        #region EFT Game Process Auto Login Arguments
        private static string GenerateArguments()
        {
            return "-bC5vLmcuaS5u={\"email\":\""+ RequestManager.SelectedAccount.email +"\",\"password\":\""+ RequestManager.SelectedAccount.password + "\",\"toggle\":true,\"timestamp\":0} -token=" + RequestManager.SelectedAccount.id + " -config={\"BackendUrl\":\"" + ServerManager.SelectedServer.backendUrl + "\",\"Version\":\"live\"}";
        }
        #endregion
        #region Shortcut Creator
        public static void CreateShortcut()
        {
            IShellLink link = (IShellLink)new ShellLink();
            link.SetDescription("Start as " + RequestManager.SelectedAccount.email);
            link.SetArguments(GenerateArguments());
            link.SetPath(Global.TarkovExecutable);
            link.SetIconLocation(Global.TarkovExecutable, 0);
            IPersistFile file = (IPersistFile)link;
            file.Save(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "JustEmuTarkov ("+ RequestManager.SelectedAccount.email + ").lnk"), false);
        }
        #region Import Shortcut Libraries
        [ComImport]
        [Guid("00021401-0000-0000-C000-000000000046")]
        internal class ShellLink
        {
        }

        [ComImport]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("000214F9-0000-0000-C000-000000000046")]
        internal interface IShellLink
        {
            void GetPath([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile, int cchMaxPath, out IntPtr pfd, int fFlags);
            void GetIDList(out IntPtr ppidl);
            void SetIDList(IntPtr pidl);
            void GetDescription([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszName, int cchMaxName);
            void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetWorkingDirectory([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir, int cchMaxPath);
            void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            void GetArguments([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs, int cchMaxPath);
            void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            void GetHotkey(out short pwHotkey);
            void SetHotkey(short wHotkey);
            void GetShowCmd(out int piShowCmd);
            void SetShowCmd(int iShowCmd);
            void GetIconLocation([Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszIconPath, int cchIconPath, out int piIcon);
            void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);
            void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);
            void Resolve(IntPtr hwnd, int fFlags);
            void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        }
        #endregion
        #endregion
    }
}