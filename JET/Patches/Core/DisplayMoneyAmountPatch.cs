using EFT.InventoryLogic;
using JET.Utilities.Patching;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace JET.Patches
{
    class DisplayMoneyAmountPatch : GenericPatch<DisplayMoneyAmountPatch>
    {
        private string _MethodName = "Show";
        private string _FieldName = "Succeed";

        public DisplayMoneyAmountPatch() : base(prefix: nameof(PatchPrefix)) { }

        protected override MethodBase GetTargetMethod()
        {
            return typeof(EFT.UI.DisplayMoneyPanel).GetMethod(_MethodName, BindingFlags.Public | BindingFlags.Instance);
        }
        /* need to be patched using transpiler and edited a part where GetMoneySums is called then returned the one below ;)
        EFT.ItemRequirementsPanel.CreateItemsViews(GClass1560, GClass1928[], Action<bool>) : void @06007242
        EFT.UI.HealthTreatmentServiceView.method_3() : void @0600ACC1
        EFT.UI.Insurance.InsurerParametersPanel.UpdateLabels(int, bool, bool) : void @0600BCFA
        EFT.UI.QuestObjectiveView.method_0(GClass1448) : void @0600AB9D
        EFT.UI.Ragfair.AddOfferWindow.method_3() : void @0600C030
        EFT.UI.Ragfair.RenewOfferWindow.method_2() : void @0600C17E
        EFT.UI.RepairerParametersPanel.UpdateLabels() : void @0600A4BC
        GClass1460.method_16(GClass1781, ConditionFindItem) : int @06007FF2
        */
        private static bool PatchPrefix(
            IEnumerable<EFT.InventoryLogic.Item> inventoryItems,
            EFT.UI.DisplayMoneyPanel __instance,
            CustomTextMeshProUGUI ___roubles,
            CustomTextMeshProUGUI ___euros,
            CustomTextMeshProUGUI ___dollars)
        {
            __instance.ShowGameObject(false);
            var items = inventoryItems.SelectMany(new Func<Item, IEnumerable<Item>>(GetAllItems));
            var OnlyMoney = items.Where(new Func<EFT.InventoryLogic.Item, bool>(IsMoney));
            long CNT_ROUBLES = 0;
            long CNT_DOLLARS = 0;
            long CNT_EUROS = 0;
            foreach (var money in OnlyMoney) {
                switch (money.TemplateId) {
                    case "5449016a4bdc2d6f028b456f": // Roubles
                        CNT_ROUBLES += money.StackObjectsCount;
                        break;
                    case "5696686a4bdc2da3298b456a": // Dollars
                        CNT_DOLLARS += money.StackObjectsCount;
                        break;
                    case "569668774bdc2da2298b4568": // EUROS
                        CNT_EUROS += money.StackObjectsCount;
                        break;
                }
            }
            NumberFormatInfo provider = new NumberFormatInfo
            {
                NumberGroupSeparator = " "
            };
            ___roubles.text = CNT_ROUBLES.ToString("N0", provider);
            ___dollars.text = CNT_DOLLARS.ToString("N0", provider);
            ___euros.text = CNT_EUROS.ToString("N0", provider);
            return false;
        }
        internal static IEnumerable<Item> GetAllItems(Item item) {
            return item.GetAllItems(false);
        }
        internal static bool IsMoney(EFT.InventoryLogic.Item item)
        {
            return item.TemplateId == "5449016a4bdc2d6f028b456f" ||
                item.TemplateId == "5696686a4bdc2da3298b456a" ||
                item.TemplateId == "569668774bdc2da2298b4568";
        }


        // for transpiler patching
        internal static Dictionary<ECurrencyType, long> GetMoneySums_Patched(IEnumerable<Item> items) {
            Dictionary<ECurrencyType, long> MoneyDict = new Dictionary<ECurrencyType, long>() {
                { ECurrencyType.EUR, 0 },
                { ECurrencyType.RUB, 0 },
                { ECurrencyType.USD, 0 }
            };
            foreach (var money in items)
            {
                switch (money.TemplateId)
                {
                    case "5449016a4bdc2d6f028b456f": // Roubles
                        MoneyDict[ECurrencyType.RUB] += money.StackObjectsCount;
                        break;
                    case "5696686a4bdc2da3298b456a": // Dollars
                        MoneyDict[ECurrencyType.USD] += money.StackObjectsCount;
                        break;
                    case "569668774bdc2da2298b4568": // EUROS
                        MoneyDict[ECurrencyType.EUR] += money.StackObjectsCount;
                        break;
                }
            }
            return MoneyDict;
        }
    }
}
