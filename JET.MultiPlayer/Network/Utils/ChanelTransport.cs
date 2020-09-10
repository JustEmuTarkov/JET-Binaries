using System;
using System.Collections;
using BitPacking;
using EFT;
using ServerLib.Network.Extensions;
using UnityEngine;
using UnityEngine.Networking;
using PacketStruct = GStruct128;
#pragma warning disable 618

namespace ServerLib.Network.Utils
{
    public static class ChanelTransport
    {
        public static class MessageFor
        {
            public const int Player = 1;
            public const int World = 0;
        }

        public static IEnumerator SendPacketToPlayerWithDelay(
            this NetworkConnection conn, PacketStruct packet, int channel, float timeDelay
        )
        {
            yield return new WaitForSeconds(timeDelay);

            SendMessageForPlayer(conn.hostId, conn.connectionId, channel, packet);
        }

        public static IEnumerator SendPacketToWorldWithDelay(int hostId, int connectionId, int channelId,
            GStruct67 packet, float timeDelay)
        {
            yield return new WaitForSeconds(timeDelay);

            //  SendWorldMessage(hostId, connectionId, channelId, packet);
        }

        public static void SendWorldMessage(this NetworkGameSession session, ref GStruct67 packet, GStruct67 previous)
        {
            var bitWriterStream = new BitWriterStream(new byte[1350]);
            bitWriterStream.WriteLimitedInt32(MessageFor.World, 0, 1);
            bitWriterStream.SerializePacket(ref packet, previous);
            bitWriterStream.Flush();

            byte[] buffer = new byte[bitWriterStream.BytesWritten];
            Array.Copy(bitWriterStream.Buffer, buffer, bitWriterStream.BytesWritten);

            SendPacket(session.connection.hostId, session.connection.connectionId, session.chanelId, buffer);
        }

        public static void SendMessageForPlayers(this NetworkConnection con, int fromId, ref GStruct127 cur,
            GStruct127 prev)
        {
            var writer = new BitWriterStream(new byte[1350]);

            writer.WriteLimitedInt32(MessageFor.Player, 0, 1);
            bool lowPriority = prev.FrameIndex > 0U;

            if (lowPriority)
            {
                lowPriority = cur.MovementInfo.Equals(prev.MovementInfo); // EqualsTo(prev.MovementInfo);
                if (lowPriority)
                {
                    lowPriority = cur.FirearmPacket.method_0();
                    if (lowPriority)
                    {
                        lowPriority = cur.FirearmPacket.HasShotsRealShots();
                    }
                }
            }

            writer.Write(lowPriority);
            if (lowPriority)
            {
                writer.WriteLimitedInt32((int) (cur.FrameIndex - prev.FrameIndex), 1, 5);
            }
            else
            {
                writer.WriteLimitedInt32((int) cur.FrameIndex, 0, 2097151);
            }

            GStruct127.SerializeDiffUsing(writer, ref cur, prev);
            writer.Flush();

            byte[] buffer = new byte[writer.BytesWritten];
            Array.Copy(writer.Buffer, buffer, writer.BytesWritten);

            buffer = FinalizePacket(buffer);

            con.TransportSend(buffer, buffer.Length, fromId, out _);
        }

        public static void SendMessageForPlayer(int hostId, int connectionId, int channelId, PacketStruct packet)
        {
            var bitWriterStream = new BitWriterStream(new byte[1350]);
            bitWriterStream.WriteLimitedInt32(MessageFor.Player, 0, 1);
            PacketStruct.Serialize(bitWriterStream, ref packet);
            bitWriterStream.Flush();

            byte[] buffer = new byte[bitWriterStream.BytesWritten];
            Array.Copy(bitWriterStream.Buffer, buffer, bitWriterStream.BytesWritten);

            SendPacket(hostId, connectionId, channelId, buffer);
        }

        public static void SendPacket(int hostId, int connectionId, int channelId, byte[] packetData)
        {
            try
            {
                var networkWriter = new NetworkWriter();
                networkWriter.Write((byte) 0);
                networkWriter.Write((byte) 0);
                networkWriter.Write((byte) 170);
                networkWriter.Write((byte) 0);

                networkWriter.WriteBytesAndSize(packetData, packetData.Length);
                networkWriter.FinishMessage();

                byte[] buffer = new byte[networkWriter.Position];
                Array.Copy(networkWriter.AsArray(), buffer, networkWriter.Position);

                NetworkTransport.Send(
                    hostId,
                    connectionId,
                    channelId, buffer, buffer.Length, out _);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        public static byte[] FinalizePacket(byte[] data)
        {
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 170);
            networkWriter.Write((byte) 0);

            networkWriter.WriteBytesAndSize(data, data.Length);
            networkWriter.FinishMessage();

            byte[] buffer = new byte[networkWriter.Position];
            Array.Copy(networkWriter.AsArray(), buffer, networkWriter.Position);
            return buffer;
        }
    }
}
