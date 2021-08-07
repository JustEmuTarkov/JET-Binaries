using UnityEngine;
using JET.Utilities.Patching;
using JET.Patches;
using System.Reflection;
using System.IO;
using JET.Patches.Logging;
using System.Linq;
using System;
using JET.Utilities;

namespace JET
{
    [Obfuscation(Exclude = true)]
    public class Instance : MonoBehaviour
    {
        public delegate void Void();
        public static event Void ApplicationQuitEvent;
        public void OnApplicationQuit() => ApplicationQuitEvent?.Invoke();

        //private static CustomMods _customMods = new CustomMods();
        private readonly Watermark _watermark = new Watermark();

        internal static bool FullLoggerEnabled { 
            get 
            { 
                return File.Exists(Path.Combine(CustomMods.GetGameDirectory, "LoggerEnable"));
            }
        }

        [Obfuscation(Exclude = true)]
        private void Awake() {

            if (FullLoggerEnabled)
            {
                FullLogger.Independent();
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
        [Obfuscation(Exclude = true)]
        private void Start()
        {
            CustomMods.Load();
            _watermark.Do();
            if (FullLoggerEnabled)
            {
                FullLogger.Independent();
            }
        }
        [Obfuscation(Exclude = true)]
        private void LateUpdate()
        {
            _watermark.Do();
        }

    }
}
