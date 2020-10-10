using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Network.Messages
{
    public class ObserverSpawnMessage : PlayerSpawnMessage
    {
        public new const short MessageId = 157;

        public override void Deserialize(NetworkReader reader) {
            _playerId = reader.ReadInt32();
            _gClass1015 = reader.ReadByte();
            _position = reader.ReadVector3();
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(_playerId);
            writer.Write(_gClass1015);
            writer.Write(_position);
            base.Serialize(writer);
        }

        int _playerId = 0;
        byte _gClass1015;
        Vector3 _position;
    }
}
