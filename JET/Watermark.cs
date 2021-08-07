using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JET
{
    class Watermark
    {
#if B13487
        private static string Version = "13487 | JET";
#endif
#if B13074
        private static string Version = "13074 | JET";
#endif
#if B12102
        private static string Version = "12102 | JET";
#endif
#if B11661
        private static string Version = "11661 | JET";
#endif
#if B10988
        private static string Version = "10988 | JET";
#endif
#if B9767
        private static string Version = "9767 | JET";
#endif
#if B9018
        private static string Version = "9018 | JET";
#endif
#if DEBUG
        private static string Version = "Debug | JET";
#endif
        EFT.UI.LocalizedText localizedText;
        internal void Do()
        {
            if (MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance != null)
            {
                if (localizedText == null)
                {
                    if (typeof(EFT.UI.PreloaderUI).GetField("_alphaVersionLabel", BindingFlags.NonPublic | BindingFlags.Instance) == null)
                        return;
                    localizedText = typeof(EFT.UI.PreloaderUI)
                    .GetField("_alphaVersionLabel", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance) as EFT.UI.LocalizedText;
                }
                if (localizedText.LocalizationKey == null)
                    return;
                localizedText.LocalizationKey = Version;
            }
        }
    }
}
