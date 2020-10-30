using System;
using EFT;

namespace ServerLib.Network.Bots
{
    public class BotHeathController : GClass1368
    {
        protected override bool _sendNetworkSyncPackets => true;

        public BotHeathController(Profile.GClass1088 healthInfo, EFT.Player player, bool addExistence) : base(
            healthInfo, player, player.GClass1652_0, player.Skills, addExistence)
        {
        }

        protected override void SendNetworkSyncPacket(GStruct190 packet)
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
