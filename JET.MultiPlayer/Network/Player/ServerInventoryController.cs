using System;
using Comfort.Common;
using EFT;
using EFT.InventoryLogic;

namespace ServerLib.Network.Player
{
    sealed class ServerInventoryController : EFT.Player.PlayerInventoryController
    {
        internal ServerInventoryController(EFT.Player player, Profile profile) : base(player, profile, 0, true)
        {
        }

        public new bool Examined(Item item)
        {
            return true;
        }

        protected override void Execute(GClass1613 operation, Callback callback)
        {
            base.Execute(operation, callback);
            Console.WriteLine($"ServerInventoryController Execute GClass1613 operation {operation.gclass1486_0.Id}");
        }

        internal override void Execute(GClass1591 operation, Callback callback)
        {
            Console.WriteLine($"Called Operation: {operation.string_0}");
            if (operation == null)
            {
                callback?.Invoke("operation is null");
                Console.WriteLine("ServerInventoryController Execute GClass1591 operation null");
                return;
            }

            base.Execute(operation, callback);
            Console.WriteLine($"ServerInventoryController Execute GClass1591 operation {operation.string_0}");
        }

        public override void StartSearchingAction(GClass1486 item)
        {
            Console.WriteLine($"StartSearchingAction item {item.Id}");
            base.StartSearchingAction(item);
        }

        public override void SetSearched(Item item, GClass1552 gridItemAddress, bool silent = false)
        {
            Console.WriteLine($"SetSearched item {item.Id}");
            base.SetSearched(item, gridItemAddress, silent);
            var player = player_0 as ServerPlayer;
            if (player == null) return;

            Console.WriteLine($"SetSearched item update access for item {item.Id}");
            player.currentPacket.CommonPacket ??= new GClass1057();

            player.currentPacket.CommonPacket.AddSetSearchedPacket(
                item.Id,
                new GStruct187 {Id = gridItemAddress.Grid.Id, ParentId = gridItemAddress.Grid.ParentItem.Id},
                silent
            );
            player.currentPacket.CommonPacket.AddUpdateSearchedState(
                item.Parent.Container.ParentItem.Id, SearchedState.Searched
            );
        }

        public override void StopSearchingAction(GClass1486 item)
        {
            var player = player_0 as ServerPlayer;
            if (player != null)
            {
                player.currentPacket.CommonPacket ??= new GClass1057();

                player.currentPacket.CommonPacket.AddStopSearchingPacket(item.Id);
                player.currentPacket.CommonPacket.AddStopSearchingActionPacket(item.Id);

                player.currentPacket.CommonPacket.AddUpdateSearchedState(
                    item.Id,
                    item.GetSearchState(Profile.Id).Value
                );
            }

            Console.WriteLine("ERROR!!!! StopSearchingAction(GClass1485 item)");
            base.StopSearchingAction(item);
        }

        public override void StopSearching(string itemId)
        {
            Console.WriteLine("ERROR!!!! StopSearching(string itemId)");
            base.StopSearching(itemId);
        }

        public override void StopSearching(GClass1440 item)
        {
            Console.WriteLine("ERROR!!!! StopSearching(GClass1439 item)");
            base.StopSearching(item);
        }
    }
}
