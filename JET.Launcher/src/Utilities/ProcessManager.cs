using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JET.Launcher.Utilities
{
    internal class ProcessManager
    {
		MainWindow mainWindow;
		internal ProcessManager(MainWindow _mainWindow) {
			mainWindow = _mainWindow;
		}
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
			Console.WriteLine("Server started");
		}
		internal void Terminate() {
			consoleProcessName = "";
			consoleProcessHandle.Kill();
		}
		internal void ServerTerminated(object sender, EventArgs e)
		{
			// server closed what now ?
		}
		List<string> TagsToRemoveFromConsoleOutput = new List<string>() {
			"(\\e\\[[0-1];3[0-9])m",
			"\\[2J\\[0;0f",
			"\\[[0-4][0-7]m",
			"\\[0m",
			"[┌│┐┘└─]"
			//""
		};
		private void RemoveConsoleTags(ref string _ConsoleOutput) {
			_ConsoleOutput = _ConsoleOutput.Replace($"", "");
			var consoleEnum = TagsToRemoveFromConsoleOutput.GetEnumerator();
			while (consoleEnum.MoveNext())
			{
				_ConsoleOutput = Regex.Replace(_ConsoleOutput, consoleEnum.Current, "");
			}
		}
		private void ServerOutputDataReceived(object sender, DataReceivedEventArgs e)
		{
			if (e.Data != null)
			{
				string tConsoleOutput = e.Data;
				RemoveConsoleTags(ref tConsoleOutput);
				//adding to the stack
				_ConsoleOutput.Add(tConsoleOutput + Environment.NewLine);
				while (_ConsoleOutput.Count >= Global.LimitConsoleOutput)
				{
					_ConsoleOutput.RemoveAt(0);
				}
			}
		}
		internal void SetConsoleOutputText()
		{
			var consoleEnum = _ConsoleOutput.GetEnumerator();
			string fullOutput = "";
			while (consoleEnum.MoveNext()) {
				fullOutput += consoleEnum.Current;
			}
			mainWindow.__ServerConsole.Text = fullOutput;
		}
	}

}
