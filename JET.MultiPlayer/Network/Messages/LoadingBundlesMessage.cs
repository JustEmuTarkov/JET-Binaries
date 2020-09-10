using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Network.Messages
{
    public class LoadingBundlesMessage : MessageBase
    {
        public override void Deserialize(NetworkReader reader)
        {
            ProfileId = reader.ReadString();
            TaskId = reader.ReadInt32();
            ProgressValue = reader.ReadSingle();
            base.Deserialize(reader);
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(ProfileId);
            writer.Write(TaskId);
            writer.Write(ProgressValue);
            base.Serialize(writer);
        }

        internal string ProfileId;

        internal int TaskId;

        internal float ProgressValue;

        public const short MessageId = 190;
    }
}
