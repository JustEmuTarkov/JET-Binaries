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
using UI_Button = DefaultUIButton; // Method: CheckOnExcude, LoadCoreByString
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
