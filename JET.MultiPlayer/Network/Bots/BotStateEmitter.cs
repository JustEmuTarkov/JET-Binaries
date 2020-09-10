using System;
using BitPacking;
using Comfort.Common;
using EFT;
using EFT.InventoryLogic;
using EFT.NetworkPackets;
using ServerLib.Network.Messages;
using ServerLib.Network.Utils;
using UnityEngine;

#pragma warning disable 618

namespace ServerLib.Network.Bots
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class BotStateEmitter : MonoBehaviour
    {
        public BotOwner botOwner;
        public int channelId;
        public bool stop;
        public GStruct127 prevPacket;
        public bool[] availableChannels = new bool[ServerInstance.MaxPlayersOnMap];

        public void Start()
        {
            if (botOwner != null && botOwner.GetPlayer == null)
            {
                Console.WriteLine("ERROR!!! botOwner.GetPlayer is null");
                return;
            }

            prevPacket = GStruct127.INITIAL_FRAME_INFO;
            prevPacket.Time = Time.time;
            prevPacket.SetDeathPacket(new GStruct126 {IsAlive = true});

            SpawnBot();
        }

        private void SpawnBot()
        {
            var serverInstance = Singleton<ServerInstance>.Instance;
            var playerSpawnMessage = PlayerSpawnMessage.FromInstance(botOwner.GetPlayer, channelId);
            playerSpawnMessage.PlayerId = channelId;
            playerSpawnMessage.IsInSpawnOperation = false;
            playerSpawnMessage.EncumberDisabled = true;

            foreach (var session in serverInstance.GameSessions.Values)
            {
                session.SpawnQueue.Enqueue(playerSpawnMessage);
            }
        }

        private void LateUpdate()
        {
            SendBotState();
        }

        internal void SendBotState(float stamp = 0f)
        {
            var player = botOwner.GetPlayer as BotPlayer;
            if (player == null || stop) return;

            var serverInstance = Singleton<ServerInstance>.Instance;
            var cliPacket = GStruct90.DEFAULT_CLIENT2_SERVER_PACKET;
            var handsType = EHandsTypePacket.None;

            byte commandMask = player.movementContext.CommandMask;
            player.movementContext.ClearCommandMask();

            var fireArmsController = player.HandsController as BotFireArmsController;
            if (fireArmsController != null)
            {
                cliPacket.Set(fireArmsController.FirearmPacket);
                fireArmsController.FirearmPacket = default;
                handsType = EHandsTypePacket.Firearm;
            }

            (GStruct126 deathPacket, GStruct144? deadSyncPacket) = GetDeathPacket();

            var movementInfoPacket = new GStruct94(0,
                player.MovementContext.TransformPosition, player.MovementContext.Rotation,
                commandMask, player.MovementContext.CurrentState.Name, player.CurrentAnimatorStateIndex,
                GClass986.ConvertToMovementDirection(player.MovementContext.MovementDirection),
                player.MovementContext.PoseLevel,
                player.MovementContext.ClampSpeed(player.MovementContext.CharacterMovementSpeed),
                player.MovementContext.Tilt,
                player.MovementContext.Step, player.MovementContext.BlindFire, player.interactWithDoorPacket,
                player.lootInteractionPacket, new GStruct96(), new GStruct97(),
                player.MovementContext.SoftSurface, player.HeadRotation, player.Physical.SerializationStruct
            );
            if (!deathPacket.IsAlive) movementInfoPacket = player.lastMovementContext;
            movementInfoPacket.LocalIndex = serverInstance.LocalIndex;


            var packet = new GStruct127(
                serverInstance.LocalIndex, deathPacket, movementInfoPacket,
                player.handsChangePacket, handsType, cliPacket.FirearmPacket, new GStruct115(),
                new KnifePacket(), player.inventoryCommands, player.phraseCommandPacket,
                new GClass1057(), deadSyncPacket, null, player.syncHealthPacket
            ) {Time = prevPacket.Time + player.LastDelta + stamp};

            foreach (var session in serverInstance.GameSessions.Values)
            {
                if (!availableChannels[session.chanelId]) continue;

                session.connection.SendMessageForPlayers(
                    channelId, ref packet, prevPacket
                );
            }

            player.handsChangePacket = default;
            player.lootInteractionPacket = default;
            player.interactWithDoorPacket = default;
            player.syncHealthPacket = null;
            player.phraseCommandPacket = default;
            player.inventoryCommands = null;

            prevPacket.Time = packet.Time;
            prevPacket.FrameIndex = packet.FrameIndex;
        }

        private (GStruct126, GStruct144?) GetDeathPacket()
        {
            var deathPacket = new GStruct126 {IsAlive = botOwner.HealthController.IsAlive};
            if (deathPacket.IsAlive) return (deathPacket, null);

            var inventory = botOwner.GetPlayer.Profile.Inventory;
            deathPacket.Side = (int) botOwner.Side;
            deathPacket.InventoryHashSum = inventory.CreateInventoryHashSum(new[] {EquipmentSlot.SecuredContainer});

            var writer = new BitWriterStream(new byte[1350 * 5]);
            var inventorySync = new GStruct125(inventory);
            GStruct125.Serialize(writer, inventorySync);
            writer.Flush();

            byte[] buffer = new byte[writer.BytesWritten];
            Array.Copy(writer.Buffer, buffer, writer.BytesWritten);

            if (buffer.Length <= 1350)
            {
                return (deathPacket, new GStruct144 {Bytes = buffer});
            }

            Console.WriteLine($"bot is deaaaaaaaaaaaaaaaaaaaad!!!!!!!!!!!!!!!! Big frame size {buffer.Length}");
            return (deathPacket, null);
        }

        public BotStateEmitter Init(BotOwner botOwner1)
        {
            botOwner = botOwner1;
            channelId = botOwner1.Id;

            return this;
        }
    }
}
