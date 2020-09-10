using System;
using EFT;

namespace ServerLib.Network.Player
{
    sealed class ServerHealthController : GClass1264
    {
        public ServerHealthController(Profile.GClass1049 healthInfo, EFT.Player player, bool addExistence) : base(
            healthInfo, player, addExistence)
        {
        }



        protected override void SendNetworkSyncPacket(GStruct165 packet)
        {
            var player = Player as ServerPlayer;

            try
            {
                if (player == null)
                    throw new Exception("ERROR!!! ServerHealthController SendNetworkSyncPacket player is null");

                player.currentPacket.AddSyncHealthPacket(packet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected override bool _sendNetworkSyncPackets => true;
    }
}
