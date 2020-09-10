using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Network.Messages
{
    public class AuthRequestMessage : MessageBase
    {
        public override void Deserialize(NetworkReader reader)
        {
            ProfileId = reader.ReadString();
            IsObserveOnly = reader.ReadBoolean();
            base.Deserialize(reader);
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(ProfileId);
            writer.Write(IsObserveOnly);
            base.Serialize(writer);
        }

        internal string ProfileId;
        internal bool IsObserveOnly;

        public const short MessageId = 147;
    }
}
