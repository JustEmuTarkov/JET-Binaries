using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace JET.Launcher.Utilities
{
    class ProgramManager
    {
        internal static bool isGameFilesFound() { 
            if(File.Exists(Path.Combine(Environment.CurrentDirectory, "EscapeFromTarkov.exe"))){
                return true;
            }
            MessageBoxManager.Show("Game Files Not Found", "Oopss!!", MessageBoxManager.Button.OK, MessageBoxManager.Image.Error);
            return false;
        }
        internal static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exception)
            {
                HandleException(exception);
            }
            else
            {
                HandleException(new Exception("Unknown Exception"));
            }
        }
        internal static void HandleException(Exception exception)
        {
            var text = $"Exception; Message:{exception.Message}{Environment.NewLine}StackTrace:{exception.StackTrace}";
            //LogManager.Instance.Error(text);
            MessageBoxManager.Show(text, "Exception", MessageBoxManager.Button.OK, MessageBoxManager.Image.Error);
        }
        private static string _FileName;
        internal static Assembly AssemblyResolveEvent(object sender, ResolveEventArgs args)
        {
            try
            {
                var assembly = new AssemblyName(args.Name).Name;
                _FileName = Path.Combine(Environment.CurrentDirectory, $"EscapeFromTarkov_Data/Managed/{assembly}.dll");
                // resources are embedded inside assembly
                if (_FileName.Contains("resources"))
                {
                    return null;
                }
                return Assembly.LoadFrom(_FileName);
            }
            catch (Exception e)
            {
                MessageBoxManager.Show(
                    $"Cannot find a file(or file is not unlocked) named:\r\n{_FileName}\r\nWith an exception: {e.Message}\r\nApplication will close after pressing OK.",
                    "File load error!", MessageBoxManager.Button.OK, MessageBoxManager.Image.Error);
                Application.Current.Shutdown();
            }
            return null;
        }
        internal static bool NoOfficialHyh = true;
        internal static string FoundGameFiles = "";
        internal static string FoundGameVersions = "";
        internal static bool IsGameFound() {
            try
            {
                List<byte[]> varList = new List<byte[]>() {
                    new byte[80] { 83, 111, 102, 116, 119, 97, 114, 101, 92, 87, 111, 119, 54, 52, 51, 50, 78, 111, 100, 101, 92, 77, 105, 99, 114, 111, 115, 111, 102, 116, 92, 87, 105, 110, 100, 111, 119, 115, 92, 67, 117, 114, 114, 101, 110, 116, 86, 101, 114, 115, 105, 111, 110, 92, 85, 110, 105, 110, 115, 116, 97, 108, 108, 92, 69, 115, 99, 97, 112, 101, 70, 114, 111, 109, 84, 97, 114, 107, 111, 118 },
                    new byte[15] { 73, 110, 115, 116, 97, 108, 108, 76, 111, 99, 97, 116, 105, 111, 110 },
                    new byte[14] { 68, 105, 115, 112, 108, 97, 121, 86, 101, 114, 115, 105, 111, 110 },
                    new byte[20] { 69, 115, 99, 97, 112, 101, 70, 114, 111, 109, 84, 97, 114, 107, 111, 118, 46, 101, 120, 101 }
                };
                //@"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\EscapeFromTarkov"
                RegistryKey key = Registry.LocalMachine.OpenSubKey(Encoding.ASCII.GetString(varList[0]));
                if (key != null)
                {
                    //"InstallLocation"
                    object path = key.GetValue(Encoding.ASCII.GetString(varList[1]));
                    //"DisplayVersion"
                    object version = key.GetValue(Encoding.ASCII.GetString(varList[2]));
                    if (path != null && version != null)
                    {
                        FoundGameFiles = path.ToString();
                        FoundGameVersions = version.ToString();
                        //"EscapeFromTarkov.exe"
                        string gamefilepath = Path.Combine(FoundGameFiles, Encoding.ASCII.GetString(varList[3]));
                        if (File.Exists(gamefilepath)) {
                            FileVersionInfo myFileVersionInfo = FileVersionInfo.GetVersionInfo(gamefilepath);
                            string file_version = myFileVersionInfo.FileVersion.Split('.').Last();
                            string file_version2 = FoundGameVersions.Split('.').Last();
                            if (file_version == file_version2) {
                                NoOfficialHyh = false;
                                return true;
                            }
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }
    }
}
