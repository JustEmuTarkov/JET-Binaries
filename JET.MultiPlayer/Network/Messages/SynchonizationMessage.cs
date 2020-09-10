using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Network.Messages
{
    public class SynchronizationMessage : MessageBase
    {
        public override void Deserialize(NetworkReader reader)
        {
            Progress = reader.ReadSingle();
            base.Deserialize(reader);
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(Progress);
            base.Serialize(writer);
        }

        internal float Progress;

        public const short MessageId = 189;
    }
}
