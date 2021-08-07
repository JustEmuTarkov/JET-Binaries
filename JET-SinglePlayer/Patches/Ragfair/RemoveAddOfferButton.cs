using System.Reflection;
using JET.Utilities.Patching;
#if B10988 || B11661 || B12102 || B13074 || B13487
using UI_Button = EFT.UI.DefaultUIButton;
#endif
#if B9767 || B9018
using UI_Button = EFT.UI.UIButtonSpawner;
#endif
#if DEBUG
using UI_Button = EFT.UI.DefaultUIButton;
#endif
namespace JET.Patches.Ragfair
{
    class RemoveAddOfferButton_Awake : GenericPatch<RemoveAddOfferButton_Awake>
    {
        public RemoveAddOfferButton_Awake() : base(postfix: nameof(PatchPostfix))
        {
        }

        public static void PatchPostfix(ref UI_Button ____addOfferButton)
        {
            //____addOfferButton.gameObject.SetActive(false);
            ____addOfferButton.Interactable = false;
#if B10988
            ____addOfferButton.OnClick.RemoveAllListeners();
#endif
        }

        protected override MethodBase GetTargetMethod()
        {
            return typeof(EFT.UI.Ragfair.RagfairScreen).GetMethod("Awake", BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
    class RemoveAddOfferButton_Call : GenericPatch<RemoveAddOfferButton_Call>
    {
        public RemoveAddOfferButton_Call() : base(postfix: nameof(PatchPostfix))
        {
        }

        public static void PatchPostfix(ref UI_Button ____addOfferButton)
        {
            //____addOfferButton.gameObject.SetActive(false);
            ____addOfferButton.Interactable = false;
#if B10988
            ____addOfferButton.OnClick.RemoveAllListeners();
#endif
        }

        protected override MethodBase GetTargetMethod()
        {
            return typeof(EFT.UI.Ragfair.RagfairScreen).GetMethod("method_6", BindingFlags.NonPublic | BindingFlags.Instance);
        }
    }
}
