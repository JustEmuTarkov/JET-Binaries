using JET.Utilities.Logging;
using JET.Utilities.Patching;
using NLog;
using System;
using System.Linq;
using System.Reflection;

namespace JET.Patches.Logging
{
    class LoggingPatch : GenericPatch<LoggingPatch>
    {
        public LoggingPatch() : base(prefix: nameof(LoggerPrefix)) { }
        static bool LoggerPrefix(string nlogFormat, string unityFormat, LogLevel logLevel, params object[] args)
        {
            try
            {
                FileLogger.Log($"[{logLevel.Name}] {string.Format(nlogFormat, args)}");
            }
            catch (FormatException)
            {
                FileLogger.Log($"[{logLevel.Name}] {nlogFormat}");
            }

            return true;
        }

        protected override MethodBase GetTargetMethod()
        {
            var loggerClass = PatcherConstants.TargetAssembly.GetTypes().First(x => x.IsClass && x.GetProperty("UnityDebugLogsEnabled") != null);
            return loggerClass.GetMethods().First(x => x.GetParameters().Length == 4 && x.Name == "Log");
        }
    }
}
