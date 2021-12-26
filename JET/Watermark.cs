using Comfort.Common;
using JET.Utilities;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace JET
{
    class Watermark
    {
        EFT.UI.PreloaderUI PreloaderUIInstance { get 
            {
#if B16338
                return Singleton<EFT.UI.PreloaderUI>.Instance;
#else
                return MonoBehaviourSingleton<EFT.UI.PreloaderUI>.Instance;
#endif
            }
        }

        EFT.UI.LocalizedText localizedText;
        internal void Do()
        {
            if (PreloaderUIInstance != null)
            {
                if (localizedText == null)
                {
                    if (typeof(EFT.UI.PreloaderUI).GetField("_alphaVersionLabel", BindingFlags.NonPublic | BindingFlags.Instance) == null)
                        return;
                    localizedText = typeof(EFT.UI.PreloaderUI)
                    .GetField("_alphaVersionLabel", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(PreloaderUIInstance) as EFT.UI.LocalizedText;
                }
                if (localizedText.LocalizationKey == null)
                    return;
                localizedText.LocalizationKey = Game.Version.Split('.').Last() + " | JET";
            }
        }
    }
}
