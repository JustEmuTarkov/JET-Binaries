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
        /*
        this.bool_0 = reader.ReadBoolean();
		this.bool_1 = reader.ReadBoolean();
		this.gclass907_0 = GClass907.Deserialize(reader);
		this.byte_0 = reader.ReadBytesAndSize();
		this.byte_1 = reader.ReadBytesAndSize();
		this.byte_2 = reader.ReadBytesAndSize();
		this.bool_2 = reader.ReadBoolean();
		this.ememberCategory_0 = (EMemberCategory) reader.ReadInt32();
		this.float_0 = reader.ReadSingle();
		this.byte_3 = reader.ReadBytesAndSize();
		this.byte_4 = reader.ReadBytesAndSize();
		Vector3 min = reader.ReadVector3();
        Vector3 max = reader.ReadVector3();
		this.bounds_0 = new Bounds
		{
			min = min,
			max = max
        };

        this.ushort_0 = reader.ReadUInt16();

        this.enetLogsLevel_0 = (ENetLogsLevel) reader.ReadByte();

        this.gclass312_0 = GClass312.Deserialize(reader);

        this.float_1 = reader.ReadSingle();
		base.Deserialize(reader);
        */


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
