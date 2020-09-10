using System;
using System.Collections.Generic;
using System.Linq;
using BitPacking;
using Comfort.Common;
using EFT;
using EFT.Ballistics;
using EFT.HealthSystem;
using EFT.InventoryLogic;
using EFT.NetworkPackets;
using ServerLib.Network.Bots;
using ServerLib.Network.Packets;
using ServerLib.Network.Player;
using ServerLib.Network.Utils;
using ServerLib.Network.World;
using ServerLib.Utils;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Network.Handlers
{
    public static class InteractiveHandler
    {
        private static readonly GStruct90[] ClientsPrev = new GStruct90[ServerInstance.MaxChannels];
        private static readonly GStruct127[] ObserversPrev = new GStruct127[ServerInstance.MaxChannels];

        // temporary log
        private static readonly int[] Log = new int[ServerInstance.MaxChannels];

        public static void OnInteractivePacketHandler(NetworkMessage message)
        {
            var serverInstance = Singleton<ServerInstance>.Instance;
            var chanelId = message.channelId;

            if (!serverInstance.NetworkClients.TryGetValue(message.conn.connectionId, out var player))
            {
                Console.WriteLine(
                    $"Channel {message.channelId} Can't get server player object from NetworkClients");
                return;
            }

            if (ClientsPrev[chanelId].FrameId == 0)
            {
                ClientsPrev[chanelId] = new GStruct90 {FrameId = ulong.MaxValue};
            }

            if (ObserversPrev[chanelId].FrameIndex == 0)
            {
                var prevObserverPacket = GStruct127.INITIAL_FRAME_INFO;
                prevObserverPacket.SetDeathPacket(new GStruct126 {IsAlive = true});
                prevObserverPacket.Time = Time.time;
                ObserversPrev[chanelId] = prevObserverPacket;
            }

            ushort num1 = message.reader.ReadUInt16();
            IBitReaderStream reader = new BitReaderStream(message.reader.ReadBytes(num1));
            int rtt = NetworkTransport.GetCurrentRTT(message.conn.hostId, message.conn.connectionId, out _);
            if (num1 > 1350)
                Console.WriteLine("Received big frameSize");

            try
            {
                if (ClientsPrev[chanelId].FrameId == ulong.MaxValue)
                {
                    player.currentPacket.AddClientConfirmCallbackPacket(
                        0, null, player.Profile.Inventory.CreateInventoryHashSum(), 0
                    );
                }

                var sessions = serverInstance.GameSessions.Values;

                int size = reader.ReadLimitedInt32(0, 127);
                for (int i = 0; i < size; i++)
                {
                    GStruct90 clientPacket = GStruct90.DEFAULT_CLIENT2_SERVER_PACKET;

                    var success = GStruct90.DeserializeDiffUsing(reader, ref clientPacket, ClientsPrev[chanelId]);

                    if (!success)
                    {
                        Console.WriteLine($"GStruct90.DeserializeDiffUsing Channel: {chanelId}, success = false   ");
                        continue;
                    }

                    if (clientPacket.HandsChangePacket.OperationType != GStruct103.EOperationType.None)
                    {
                        var packet = clientPacket.HandsChangePacket;

                        Console.WriteLine(
                            $"HandsChangePacket CallBack: {packet.CallbackId}," +
                            $" ItemId: {packet.ItemId} operationType {packet.OperationType}"
                        );
                    }

                    if (clientPacket.FirearmPacket.method_0())
                    {
                        var forApproveShots = clientPacket.FirearmPacket.ShotsForApprovement;
                        if (forApproveShots.Length > 0)
                        {
                            for (int j = 0; j < forApproveShots.Length; j++)
                            {
                                ShotInfo shotInfo = forApproveShots[j];
                                var playerId = shotInfo.HittedId;
                                EFT.Player hittingPlayer = null;

                                if (Singleton<ServerInstance>.Instance.GameSessions.TryGetValue(playerId,
                                    out var session))
                                {
                                    hittingPlayer = session.player;
                                }

                                if (BotsMonitor.AllBots.TryGetValue(playerId, out var bot))
                                {
                                    hittingPlayer = bot.GetPlayer;
                                }

                                if (hittingPlayer != null)
                                {
                                    hittingPlayer.ActiveHealthController.ApplyDamage(
                                        shotInfo.BodyPart,
                                        shotInfo.Damage, EDamageType.Shoot
                                    );
                                }
                            }

                            Console.WriteLine(
                                $"ShotsForApprovement: {clientPacket.FirearmPacket.ShotsForApprovement.Length}"
                            );
                            clientPacket.FirearmPacket.ShotsForApprovement = new OneAndList<ShotInfo>();
                        }
                    }

                    var firePacket = clientPacket.FirearmPacket.EnableInventoryPacket;
                    if (firePacket.EnableInventory || firePacket.InventoryStatus)
                    {
                        Console.WriteLine(
                            $"EnableInventoryPacket EnableInventory {firePacket.EnableInventory} InventoryStatus {firePacket.InventoryStatus}"
                        );
                    }

                    if (clientPacket.FirearmPacket.ExamineWeapon)
                    {
                        Console.WriteLine("FirearmPacket.ExamineWeapon");
                    }

                    if (clientPacket.MovementInfoPacket.LootInteractionPacket.Interact)
                    {
                        var lootInteractionPacket = clientPacket.MovementInfoPacket.LootInteractionPacket;

                        player.currentPacket.AddClientConfirmCallbackPacket(
                            lootInteractionPacket.CallbackId, null,
                            player.Profile.Inventory.CreateInventoryHashSum(new[] {EquipmentSlot.SecuredContainer}),
                            0
                        );

                        Console.WriteLine(
                            $"LootInteractionPacket callback {lootInteractionPacket.CallbackId} LootId: {lootInteractionPacket.LootId}"
                        );
                        clientPacket.MovementInfoPacket.LootInteractionPacket = default;
                    }

                    if (clientPacket.MovementInfoPacket.InteractWithDoorPacket.HasInteraction)
                    {
                        var interactionPacket = clientPacket.MovementInfoPacket.InteractWithDoorPacket;
                        Console.WriteLine($"InteractWithDoorPacket id: {interactionPacket.Id}");
                        Console.WriteLine($"InteractWithDoorPacket itemId: {interactionPacket.ItemId}");
                    }

                    if (clientPacket.StartSearchContentPacket.HasValue)
                    {
                        var startSearch = clientPacket.StartSearchContentPacket.Value;
                        Console.WriteLine($"StartSearchContentPacket {startSearch.ItemId}");
                        player.ProcessStartSearchContent(startSearch.ItemId, err =>
                        {
                            player.currentPacket.AddOperationStatus(new GStruct132
                            {
                                Error = err,
                                Id = (ushort) startSearch.CallbackId,
                                Status = string.IsNullOrEmpty(err)
                                    ? NetworkPlayer.EOperationStatus.Finished
                                    : NetworkPlayer.EOperationStatus.Failed
                            });
                        });
                    }

                    if (clientPacket.InventoryCommandPackets != null && clientPacket.InventoryCommandPackets.Count > 0)
                    {
                        Console.WriteLine(
                            $"WARNING!!!! InventoryCommandPackets. Count {clientPacket.InventoryCommandPackets.Count}"
                        );
                        clientPacket.InventoryCommandPackets.ForEach(command =>
                        {
                            var operation = command.ReadInventoryCommand(player);
                            if (operation.Error != null)
                                Console.WriteLine($"ERROR!!! operation is null. Error: {operation.Error}");

                            operation.Value.vmethod_0(err =>
                            {
                                player.currentPacket.AddOperationStatus(new GStruct132
                                {
                                    Error = err,
                                    Id = (ushort) command.CallbackId,
                                    Status = string.IsNullOrEmpty(err)
                                        ? NetworkPlayer.EOperationStatus.Finished
                                        : NetworkPlayer.EOperationStatus.Failed
                                });
                            });

                            Console.WriteLine(
                                $"InventoryCommandPacket. Hash: {command.InventoryHashSum}, CallbackId: {command.CallbackId}"
                            );
                        });
                        clientPacket.InventoryCommandPackets = null;
                    }

                    var observerPacket = new GStruct127(
                        clientPacket.FrameId,
                        new GStruct126 {IsAlive = true},
                        clientPacket.MovementInfoPacket,
                        clientPacket.HandsChangePacket,
                        clientPacket.HandsTypePacket,
                        clientPacket.FirearmPacket,
                        clientPacket.GrenadePacket,
                        clientPacket.KnifePacket,
                        new List<GStruct116>(),
                        clientPacket.PhraseCommandPacket,
                        player.currentPacket.CommonPacket,
                        null, null,
                        player.currentPacket.SyncHealthPacket)
                    {
                        FrameIndex = clientPacket.FrameId, Time = ObserversPrev[chanelId].Time + clientPacket.DeltaTime
                    };

                    var important = observerPacket.GetPriority() == PacketPriority.Critical;
                    foreach (var otherSession in sessions)
                    {
                        if (otherSession.chanelId == chanelId) continue;

                        otherSession.connection.SendMessageForPlayers(
                            chanelId + (important ? 0 : 1),
                            ref observerPacket,
                            ObserversPrev[chanelId]
                        );
                    }

                    player.ProcessPacket(clientPacket, ObserversPrev[chanelId].Time);

                    ClientsPrev[chanelId] = clientPacket;
                    ObserversPrev[chanelId].Time = observerPacket.Time;
                    ObserversPrev[chanelId].FrameIndex = observerPacket.FrameIndex;
                }

                if (!player.currentPacket.HasData()) return;

                ChanelTransport.SendMessageForPlayer(
                    message.conn.hostId,
                    message.conn.connectionId,
                    message.channelId, player.currentPacket
                );
                player.currentPacket = new GStruct128();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
