using EFT;

namespace ServerLib.Network.Player
{
    public class ServerStatisticsManager : GClass1077
    {
        protected override void ShowStatNotification(LocalizationKey localizationKey1, LocalizationKey localizationKey2,
            int value)
        {
            var player = player_0 as ServerPlayer;
            if (player == null) return;

            player
                .currentPacket
                .AddShowStatNotificationPacket((uint) localizationKey1, (uint) localizationKey2, value);
        }
    }
}
