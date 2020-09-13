using System.IO;
using Comfort.Common;
using EFT;

namespace ServerLib.Network.Bots
{
    internal class BotInventoryController : EFT.Player.PlayerInventoryController
    {
        private BotPlayer _player => player_0 as BotPlayer;

        internal BotInventoryController(EFT.Player player, Profile profile) : base(player, profile, 0, true)
        {
        }

        internal override void Execute(GClass1591 operation, Callback callback)
        {
            /* this is crashing game client not server
            using MemoryStream memoryStream = new MemoryStream(new byte[1350]);
            using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

            binaryWriter.WritePolymorph(GClass980.FromInventoryOperation(operation));
            this._player.AddInventoryCommand(new GStruct116
            {
                Tag = GStruct116.ETag.Command,
                CommandPacket = new GStruct117
                {
                    CallbackId = operation.Id,
                    CommandBytes = memoryStream.ToArray(),
                    InventoryHashSum = base.Inventory.CreateInventoryHashSum()
                }
            });
            base.Execute(operation, x =>
            {
                _player.AddInventoryCommand(new GStruct116
                {
                    Tag = GStruct116.ETag.Status,
                    StatusPacket = new GStruct132
                    {
                        Error = x,
                        Id = operation.Id,
                        Status = string.IsNullOrEmpty(x)
                            ? NetworkPlayer.EOperationStatus.Finished
                            : NetworkPlayer.EOperationStatus.Failed
                    }
                });
                callback?.Invoke(x);
            });*/
        }
    }
}
