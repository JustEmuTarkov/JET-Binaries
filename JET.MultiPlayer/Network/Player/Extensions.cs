using System;
using BitPacking;
using EFT;
using EFT.InventoryLogic;

namespace ServerLib.Network.Player
{
    public static class Extensions
    {
        public static (string, EController) GetHandsControllerType(
            this EFT.Player.AbstractHandsController handsController)
        {
            var firearmController = handsController as EFT.Player.FirearmController;
            var grenadeController = handsController as EFT.Player.GrenadeController;
            var knifeController = handsController as EFT.Player.KnifeController;
            var quickGrenadeThrowController = handsController as EFT.Player.QuickGrenadeThrowController;
            var quickKnifeKickController = handsController as EFT.Player.QuickKnifeKickController;
            var medsController = handsController as EFT.Player.MedsController;
            var emptyHandsController = handsController as EFT.Player.EmptyHandsController;

            if (firearmController != null)
                return (firearmController.Item.Id, EController.Firearm);

            if (grenadeController != null)
                return (grenadeController.Item.Id, EController.Grenade);

            if (knifeController != null)
                return (knifeController.Item.Id, EController.Knife);

            if (quickGrenadeThrowController != null)
                return (quickGrenadeThrowController.Item.Id, EController.QuickGrenade);

            if (quickKnifeKickController != null)
                return (quickKnifeKickController.Item.Id, EController.QuickKnife);

            if (medsController != null)
                return (medsController.Item.Id, EController.Meds);

            return emptyHandsController != null
                ? (emptyHandsController.Item.Id, EController.Empty)
                : ("", EController.None);
        }

        public static byte[] GetInventoryBytes(this EFT.Player player)
        {
            var inventory = player.Profile.Inventory;

            var writer = new BitWriterStream(new byte[ushort.MaxValue]);
            var inventorySync = new GStruct125(inventory);
            GStruct125.Serialize(writer, inventorySync);
            writer.Flush();

            byte[] buffer = new byte[writer.BytesWritten];
            Array.Copy(writer.Buffer, buffer, writer.BytesWritten);

            return buffer;
        }
    }
}
