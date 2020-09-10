using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using JET.Launcher.Controllers;

namespace JET.Launcher
{
	public static class Program
	{
        private static StaticData staticText = new StaticData();
		[STAThread]
		private static void Main()
		{
            // set rendering
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // enable logs
            Application.ThreadException += (sender, args) => HandleException(args.Exception);
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

            // load assemblies from EFT's managed directory
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEvent);

            Application.Run(new Main());
        }

        private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exception)
            {
                HandleException(exception);
            }
            else
            {
                HandleException(new Exception(staticText.EXCEPTIONS.unknException));
            }
        }

        private static void HandleException(Exception exception)
        {
            var text = $"{staticText.EXCEPTIONS.exception} {staticText.EXCEPTIONS.message}:{exception.Message}{Environment.NewLine}StackTrace:{exception.StackTrace}";
			LogManager.Instance.Error(text);
            MessageBox.Show(text, staticText.EXCEPTIONS.exception, MessageBoxButtons.OK,MessageBoxIcon.Error);
        }

        private static Assembly AssemblyResolveEvent(object sender, ResolveEventArgs args)
        {
            string assembly = new AssemblyName(args.Name).Name;
            string filename = Path.Combine(Environment.CurrentDirectory, $"{staticText.eft_managed}{assembly}{staticText.dll_ext}");

            // resources are embedded inside assembly
            if (filename.Contains("resources"))
            {
                return null;
            }

            return Assembly.LoadFrom(filename);
        }
    }
}
