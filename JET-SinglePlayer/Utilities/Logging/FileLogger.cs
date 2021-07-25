using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using System.Timers;
using EFT;

namespace JET.Utilities.Logging
{
    public static class FileLogger
    {
        private static string CurrentFileName { 
            get {
                if (_currentLogFile == null)
                    _currentLogFile = Path.Combine(Application.dataPath, "../Logs/JET Debug/", DateTime.Now.ToString("MM.dd.yyyy_HH-mm-ss-fff") + ".log");
                if (!Directory.Exists(Path.GetDirectoryName(_currentLogFile)))
                    Directory.CreateDirectory(Path.GetDirectoryName(_currentLogFile));
                return _currentLogFile;
            } 
        }
        private static string _currentLogFile = null;
        private static List<string> LogCache = new List<string>();
        private static Timer WriteTimer = new Timer(5000); // 5 seconds

        static FileLogger()
        {
            WriteTimer.AutoReset = true;
            WriteTimer.Elapsed += WriteTimer_Elapsed;
            WriteTimer.Start();
            SinglePlayer.ApplicationQuitEvent += Application_quitting;
        }

        private static void Application_quitting() => SaveLogFile();

        private static void WriteTimer_Elapsed(object sender, ElapsedEventArgs e) => SaveLogFile();

        private static void SaveLogFile()
        {
            lock (LogCache)
            {
                if(LogCache.Count > 0)
                    File.AppendAllLines(CurrentFileName, LogCache);
                LogCache.Clear();
            }
        }

        public static void Log(string message)
        {
            lock (LogCache)
            {
                LogCache.Add(message);
            }
        }
    }
}
