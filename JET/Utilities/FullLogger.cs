using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JET.Utilities
{
    class FullLogger
    {
        private static Type loggerClass;
        private static FieldInfo isLogsEnabled;
        private static FieldInfo unityDebugLogsEnabled;
        internal static void Independent() {
            if (!Instance.FullLoggerEnabled) return;
            // if logger is enabled enable all features
            if (loggerClass == null)
            {
                var list = typeof(EFT.AbstractGame).Assembly.GetTypes()
                    .Where(type =>
                        type.Name.StartsWith("GClass") &&
                        type.GetField("IsLogsEnabled", BindingFlags.Public | BindingFlags.Static) != null &&
                        type.GetField("UnityDebugLogsEnabled", BindingFlags.Public | BindingFlags.Static) != null
                    ).ToList();

                if (list.Count > 0)
                {
                    loggerClass = list[0];
                    isLogsEnabled = loggerClass.GetField("IsLogsEnabled", BindingFlags.Public | BindingFlags.Static);
                    unityDebugLogsEnabled = loggerClass.GetField("IsLogsEnabled", BindingFlags.Public | BindingFlags.Static);
                }
            }
            if (isLogsEnabled != null)
                isLogsEnabled.SetValue(null, true);
            if (unityDebugLogsEnabled != null)
                unityDebugLogsEnabled.SetValue(null, true);
            Debug.unityLogger.logEnabled = true;
            Application.SetStackTraceLogType(LogType.Assert, StackTraceLogType.Full);
            Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.Full);
            Application.SetStackTraceLogType(LogType.Exception, StackTraceLogType.Full);
            Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.Full);
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.Full);
        }
    }
}
