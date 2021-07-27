using System.Reflection;
using EFT.UI;
using EFT.UI.Matchmaker;
using JET.Utilities.Patching;
using UnityEngine;
#if B10988 || B11661 || B12102 || B13074
using UI_Button = EFT.UI.DefaultUIButton;
#endif
#if B9018 || B9767
using UI_Button = EFT.UI.UIButtonSpawner;
#endif
#if DEBUG
using UI_Button = EFT.UI.DefaultUIButton;
#endif
namespace JET.Patches
{
    class MatchMakerAfterSelectLocation : GenericPatch<MatchMakerAfterSelectLocation>
    {
        public MatchMakerAfterSelectLocation() : base(postfix: nameof(PatchPostfix))
        {
        }
#if B13074
        private const string PropertyName = "GClass806_0";
#else
        private string propertyName = "SelectedLocation";
#endif
        public static void PatchPostfix(ref UI_Button ____readyButton)
        {
            ____readyButton.gameObject.SetActive(false);
        }

        protected override MethodBase GetTargetMethod()
        {
            return typeof(MatchMakerSelectionLocationScreen).GetProperty(PropertyName, BindingFlags.NonPublic | BindingFlags.Instance)?.SetMethod; 
        }
    }
}
