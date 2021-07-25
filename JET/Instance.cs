using UnityEngine;
using JET.Utilities.Patching;
using JET.Utilities;
using JET.Patches;
using System.Reflection;

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using JET.Patches.Logging;
//using System.Text.Json;
//using System.Text.Json.Serialization;

namespace JET
{
    [ObfuscationAttribute(Exclude = true)]
    public class Instance : MonoBehaviour
    {
        public delegate void Void();
        public static event Void ApplicationQuitEvent;
        public void OnApplicationQuit() => ApplicationQuitEvent?.Invoke();

        private static CustomMods _customMods = new CustomMods();
        private Watermark _watermark = new Watermark();

        private static bool FULL_LOGGER_ENABLED = false;
        [ObfuscationAttribute(Exclude = true)]
        private void Awake() {
            if (File.Exists(Path.Combine(CustomMods.GetGameDirectory, "LoggerEnable")))
            {
                FULL_LOGGER_ENABLED = true;
                PatcherUtil.Patch<InitialHookPatch>();
                PatcherUtil.Patch<LoggingPatch>();
                PatcherUtil.Patch<ResetHookPatch>();
            }
#if B13074
            PatcherUtil.Patch<EnsureConsistencyPatch>(); // this occurs only in ~13074 and above
#endif
            PatcherUtil.Patch<BattleEyePatch>();
            PatcherUtil.Patch<SslCertificatePatch>();
            PatcherUtil.Patch<UnityWebRequestPatch>();
            PatcherUtil.Patch<NotificationSslPatch>();
            
        }
        [ObfuscationAttribute(Exclude = true)]
        private void Start()
        {
            Debug.Log("JET JET JET JET JET");
            CustomMods.Load();
            _watermark.Do();
        }
        [ObfuscationAttribute(Exclude = true)]
        private void LateUpdate()
        {
            _watermark.Do();
#if B13074
            FullLogger();
#endif
        }

        private static bool PatchedIngameLogger = false;
        private void FullLogger() {
            if (!FULL_LOGGER_ENABLED) return;
            // if logger is enabled enable all features
            GClass389.IsLogsEnabled = true;
            GClass389.UnityDebugLogsEnabled = true;
            Debug.unityLogger.logEnabled = true;
            Application.SetStackTraceLogType(LogType.Assert, StackTraceLogType.Full);
            Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.Full);
            Application.SetStackTraceLogType(LogType.Exception, StackTraceLogType.Full);
            Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.Full);
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.Full);
        }

    }
}
