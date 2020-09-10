using EFT;
using EFT.InventoryLogic;

namespace ServerLib.Network.Bots
{
    class BotFireArmsController : ClientFirearmController
    {
        internal static BotFireArmsController Create(BotPlayer player, Weapon weapon)
        {
            return smethod_5<BotFireArmsController>(player, weapon);
        }

        internal static BotFireArmsController Create(BotPlayer player, Item item, bool isStationaryWeapon)
        {
            return Create(player, (Weapon) item);
        }

        public override bool CanPressTrigger() => true;

        public override bool CanStartReload()
        {
            GClass1477 currentMagazine = this.Item.GetCurrentMagazine();
            if (currentMagazine != null && !this._player.GClass1544_0.Examined(currentMagazine))
            {
                return false;
            }
            return this.CurrentOperation.CanStartReload();
        }
    }
}
