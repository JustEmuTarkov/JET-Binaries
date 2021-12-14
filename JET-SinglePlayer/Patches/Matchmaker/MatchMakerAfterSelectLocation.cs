using System.Reflection;
using EFT.UI.Matchmaker;
using JET.Utilities.Patching;
#if B10988 || B11661 || B12102 || B13074 || B13487 || B14687 || B16029
using UI_Button = EFT.UI.DefaultUIButton;
#endif
#if B9018 || B9767
using UI_Button = EFT.UI.UIButtonSpawner;
#endif
#if DEBUG
using UI_Button = EFT.UI.DefaultUIButton;
#endif
namespace JET.Patches.Matchmaker
{
    class MatchMakerAfterSelectLocation : GenericPatch<MatchMakerAfterSelectLocation>
    {
        public MatchMakerAfterSelectLocation() : base(postfix: nameof(PatchPostfix))
        {
        }
        public static void PatchPostfix(ref UI_Button ____readyButton)
        {
            ____readyButton.gameObject.SetActive(false);
        }

        protected override MethodBase GetTargetMethod()
        {
            foreach (var method in typeof(MatchMakerSelectionLocationScreen).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                return method?.SetMethod; // there is only 1 here so lets just return first one...
            }
            return null;
            //return typeof(MatchMakerSelectionLocationScreen).GetProperty(PropertyName, BindingFlags.NonPublic | BindingFlags.Instance)?.SetMethod; 
        }
    }
}
