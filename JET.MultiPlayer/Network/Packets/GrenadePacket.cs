using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Network.Packets
{
    public class GrenadePacket
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Velocity;
        public Vector3 AngularVelocity;
        public int Id;

        public static void Serialize(NetworkWriter writer, ref GrenadePacket packet)
        {
            writer.Write(packet.Position);
            writer.Write(packet.Rotation);
            writer.Write(packet.Velocity);
            writer.Write(packet.AngularVelocity);
            writer.Write(packet.Id);
        }
    }
}
