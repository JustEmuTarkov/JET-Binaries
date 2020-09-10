using System;
using EFT;

namespace ServerLib.Network.Bots
{
    public class BotHeathController : GClass1264
    {
        protected override bool _sendNetworkSyncPackets => true;

        public BotHeathController(Profile.GClass1049 healthInfo, EFT.Player player, bool addExistence) : base(
            healthInfo, player, addExistence)
        {
        }

        protected override void SendNetworkSyncPacket(GStruct165 packet)
        {
            var player = Player as BotPlayer;

            try
            {
                if (player == null) throw new Exception("ERROR!!! SendNetworkSyncPacket player is null");
                packet.AttachTo(ref player.syncHealthPacket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
