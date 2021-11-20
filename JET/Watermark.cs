using JET.Utilities;
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
        EFT.UI.LocalizedText localizedText;
        internal void Do()
        {
            if (Comfort.Common.Singleton<EFT.UI.PreloaderUI>.Instance != null)
            {
                if (localizedText == null)
                {
                    if (typeof(EFT.UI.PreloaderUI).GetField("_alphaVersionLabel", BindingFlags.NonPublic | BindingFlags.Instance) == null)
                        return;
                    localizedText = typeof(EFT.UI.PreloaderUI)
                    .GetField("_alphaVersionLabel", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(Comfort.Common.Singleton<EFT.UI.PreloaderUI>.Instance) as EFT.UI.LocalizedText;
                }
                if (localizedText.LocalizationKey == null)
                    return;
                localizedText.LocalizationKey = Game.Version.Split('.').Last() + " | JET";
            }
        }
    }
}
