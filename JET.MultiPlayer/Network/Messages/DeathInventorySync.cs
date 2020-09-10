using System;
using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Network.Messages
{
    public class DeathInventorySync : MessageBase
    {
        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(PlayerId);
            writer.WriteBytesAndSize(InventoryBytes, InventoryBytes.Length);
            base.Serialize(writer);

            Console.WriteLine(
                $"DeathInventorySync Serialize bytes length {InventoryBytes.Length} " +
                $"Writer buffer length {writer.ToArray().Length} " +
                $"Writer position {writer.Position}"
            );
        }

        internal int PlayerId;
        internal byte[] InventoryBytes;
        public const short MessageId = 160;
    }
}
