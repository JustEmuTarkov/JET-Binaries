using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Comfort.Common;
using ComponentAce.Compression.Libs.zlib;
using EFT;
using EFT.InventoryLogic;
using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Network.Messages
{
    public class SubWorldSpawnMessage : MessageBase
    {
        public override void Serialize(NetworkWriter writer)
        {
            var gameWorld = Singleton<GameWorld>.Instance;

            writer.Write(true);

            var lootItems = gameWorld.GetJsonLootItems().ToArray();

            Console.WriteLine($"SubWorldSpawnMessage serialize loot count {lootItems.Length}");

            using (var memoryStream = new MemoryStream())
            {
                using var binaryWriter = new BinaryWriter(memoryStream);
                binaryWriter.Write(GClass907.SerializeLootData(lootItems));
                byte[] buffer = SimpleZlib.CompressToBytes(memoryStream.ToArray(), 9);
                writer.WriteBytesFull(buffer);
            }

            IEnumerable<Item> allItemsFromCollections = lootItems
                .Select(x => x.Item)
                .OfType<IContainerCollection>()
                .GetAllItemsFromCollections();

            GClass803.Serialize(
                writer,
                GClass803.ExtractSearchInfo(allItemsFromCollections).ToArray()
            );

            base.Serialize(writer);
        }

        // Item[] source = this.location.Loot.Select(new Func<GClass714, Item>(BaseLocalGame<T>.Class1029.class1029_0.method_10)).ToArray<Item>();
        public const short MessageId = 153;
    }
}
