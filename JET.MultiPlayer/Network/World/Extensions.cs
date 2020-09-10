using System;
using System.Linq;
using BitPacking;
using EFT;
using EFT.Interactive;
using EFT.InventoryLogic;

namespace ServerLib.Network.World
{
    public static class Extensions
    {
        internal static ArraySegment<byte> SerializeInteractiveObjectsStatus(this ServerWorld serverWorld)
        {
            IBitWriterStream writer = new BitWriterStream(new byte[1024]);
            var objects = serverWorld.GetInteractiveStatusInfos();

            writer.WriteUInt32UsingBitRange((uint) objects.Length, new[] {4, 8, 10});
            foreach (var door in objects)
            {
                var state = door.GetStatusInfo(true);

                writer.WriteLimitedInt32(door.NetId, 0, 2047);
                writer.WriteBits(state.State, 5);

                if ((state.State & (uint) EDoorState.Interacting) == 0) continue;

                var angle = (state.Angle + 180f) / 15U;
                writer.WriteBits((uint) angle, 5);
            }

            writer.Flush();
            var buffer = new byte[writer.BytesWritten];
            Array.Copy(writer.Buffer, buffer, writer.BytesWritten);

            return new ArraySegment<byte>(buffer);
        }

        public static ResourceKey[] GetAllLootPrefabs(this GameWorld gameWorld)
        {
            var items = gameWorld.GetJsonLootItems();
            Item[] source = items.Select(x => x.Item).ToArray();
            return source
                .OfType<IContainerCollection>()
                .GetAllItemsFromCollections()
                .Concat(source.Where(x => !(x is IContainerCollection)))
                .SelectMany(x => x.Template.AllResources)
                .ToArray();
        }
    }
}
