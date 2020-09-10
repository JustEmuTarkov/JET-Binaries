using System;
using UnityEngine;

namespace JET.Common.Utils.App
{
    public class Logger
    {
        private static void Log(string type, string text)
        {
            Debug.Log($"{type} | {text}");
        }

        public static void LogData(string text)
        {
            Debug.Log($"{text}");
        }

        public static void LogInfo(string text)
        {
            Log("INFO", text);
        }

        public static void LogWarning(string text)
        {
            Log("WARNING", text);
        }

        public static void LogError(string text)
        {
            Log("ERROR", text);
        }
    }
}
