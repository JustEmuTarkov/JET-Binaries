using System.Linq;
using EFT;
using EFT.InventoryLogic;
using Inventory = GClass1529;

namespace ServerLib.Utils.Server
{
    public static class PrefabUtils
    {
        public static ResourceKey[] GetAllInventoryPrefabs(this Inventory inventory)
        {
            Item[] source = inventory.AllPlayerItems.Concat(inventory.GetAllEquipmentItems()).ToArray();
            return source.SelectMany(x => x.Template.AllResources).ToArray();
        }
    }
}
