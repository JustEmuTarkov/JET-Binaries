using EFT;

namespace ServerLib.Network.Bots
{
    class BotGrenadeController : ClientGrenadeController
    {
        internal static BotGrenadeController Create(BotPlayer player, GClass1525 item)
        {
            return smethod_8<BotGrenadeController>(player, item);
        }


        public override bool CanThrow() => this._player.StateIsSuitableForHandInput;
    }
}
