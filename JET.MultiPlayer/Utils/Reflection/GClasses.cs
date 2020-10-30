using System.IO;
using Comfort.Common;
using UnityEngine.Networking;
using Inventory = GClass1529;
using InventorySerializer = GClass907;
using ItemsFactory = GClass879;

#pragma warning disable 618

namespace ServerLib.Utils.Reflection
{
    public static class GClasses
    {
        public static ItemsFactory ItemsFactory => Singleton<ItemsFactory>.Instance;

        public static Inventory DeserializeInventory(this BinaryReader reader)
        {
            return InventorySerializer.DeserializeInventory(ItemsFactory, reader.ReadEFTInventoryDescriptor());
        }

        public static void SerializeInventory(this BinaryWriter writer, Inventory inventory)
        {
            writer.Write(InventorySerializer.SerializeInventory(inventory));
        }

        public static GClass1424[] DeSerializeSearchableInfo(this NetworkReader reader)
        {
            return GClass1423.Deserialize(reader.ReadBytesAndSize());
        }

        public static void SerializeSearchableInfo(this NetworkWriter writer, GClass804[] searchableInfo)
        {
            GClass803.Serialize(writer, searchableInfo);
        }
    }
}
