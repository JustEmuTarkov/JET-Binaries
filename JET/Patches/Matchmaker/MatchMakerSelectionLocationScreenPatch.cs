using System.Reflection;
using EFT.UI;
using EFT.UI.Matchmaker;
using JET.Utilities.Patching;
using UnityEngine;
#if B10988
using UI_Button = EFT.UI.DefaultUIButton; // Method: CheckOnExcude, LoadCoreByString
#endif
#if B9767
using UI_Button = EFT.UI.UIButtonSpawner; // Method: CheckOnExcude, LoadCoreByString
#endif
#if B9018
using UI_Button = EFT.UI.UIButtonSpawner; // Method: CheckOnExcude, LoadCoreByString
#endif
#if DEBUG
using UI_Button = EFT.UI.DefaultUIButton; // Method: CheckOnExcude, LoadCoreByString
#endif
namespace JET.Patches.Matchmaker
{
    class MatchMakerSelectionLocationScreenPatch : GenericPatch<MatchMakerSelectionLocationScreenPatch>
    {
        public MatchMakerSelectionLocationScreenPatch() : base(postfix: nameof(PatchPostfix))
        {
        }

        public static void PatchPostfix(ref UI_Button ____readyButton)
        {
            ____readyButton.gameObject.SetActive(false);
        }

        protected override MethodBase GetTargetMethod()
        {
            return typeof(MatchMakerSelectionLocationScreen).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}