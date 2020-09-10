using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Network.Messages
{
    public class BeClientMessage : MessageBase
    {
        // Token: 0x02000AC7 RID: 2759
        //internal sealed class Class455 : MessageBase
        // Token: 0x06003EA9 RID: 16041 RVA: 0x000C7464 File Offset: 0x000C5664
        public override void Serialize(NetworkWriter writer)
        {
            writer.WriteBytesFull(PacketBytes);
            base.Serialize(writer);
        }

        // Token: 0x06003EAA RID: 16042 RVA: 0x000C7479 File Offset: 0x000C5679
        public override void Deserialize(NetworkReader reader)
        {
            PacketBytes = reader.ReadBytesAndSize();
            base.Deserialize(reader);
        }

        // Token: 0x04004416 RID: 17430
        internal byte[] PacketBytes;
        public const short MessageId = 168;
    }
}
