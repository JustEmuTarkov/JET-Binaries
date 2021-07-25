using JET.Utilities.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace JET.Utilities
{
    class UnityLogger : ILogHandler
    {
        private static UnityLogger _instance = null;
        public static UnityLogger Instance => _instance ?? (_instance = new UnityLogger());

        public void LogFormat(LogType logType, Object context, string format, params object[] args)
        {
            var str = $"[{logType}] ";
            if (context != null)
                str += "Object " + context.name + ": ";
            str += format;
            str = string.Format(str, args);
            FileLogger.Log(str);
        }

        public void LogException(Exception exception, Object context)
        {
            LogFormat(LogType.Exception, context, exception.ToString());
        }
    }
}
