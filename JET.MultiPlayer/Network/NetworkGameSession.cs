using System;
using System.Collections.Generic;
using Comfort.Common;
using ServerLib.Network.Bots;
using ServerLib.Network.Messages;
using ServerLib.Network.Player;
using ServerLib.Network.World;
using ServerLib.Utils.Game;
using UnityEngine;

#pragma warning disable 618

namespace ServerLib.Network
{
    public class NetworkGameSession : AbstractGameSession
    {
        private float _nextActionTime = 5f;
        private float ShortPeriod = 2f;
        public ServerPlayer player;
        public bool[] availableChannels = new bool[ServerInstance.MaxPlayersOnMap];
        private readonly Queue<int> _spawnedQueue = new Queue<int>();

        public bool isInLoadBundlesState;
        public bool isInSpawnObserverState;

        public readonly Queue<PlayerSpawnMessage> SpawnQueue = new Queue<PlayerSpawnMessage>();
        public readonly Queue<SomeBundleMessage> BundlesQueue = new Queue<SomeBundleMessage>();


        private void Start()
        {
            Console.WriteLine(
                $"NetworkGameSession started, con_id: {connection.connectionId}," +
                $" {connection.address}, " +
                $"channel id: {chanelId}, " +
                $"channel index: {chanelIndex}");
        }

        private void FixedUpdate()
        {
            if (!(Time.time > _nextActionTime)) return;

            _nextActionTime = Time.time + ShortPeriod;

            if (sessionIsSpawned)
            {
                UpdatePerTenSec();
                return;
            }

            RpcGameMatching(5, 1, 10);
        }

        private void UpdatePerTenSec()
        {
            if (gameSyncTimeIsSent && !worldMessageIsSent)
            {
                var worldSpawnMessage = new WorldSpawnMessage();

                connection.Send(WorldSpawnMessage.MessageId, worldSpawnMessage);
                worldMessageIsSent = true;
                return;
            }

            if (worldMessageIsSent && !subWorldMessageIsSent)
            {
                var subWorldSpawnMessage = new SubWorldSpawnMessage();
                connection.Send(SubWorldSpawnMessage.MessageId, subWorldSpawnMessage);
                subWorldMessageIsSent = true;
                return;
            }

            if (subWorldMessageIsSent && !playerSpawnIsSent)
            {
                var serverInstance = Singleton<ServerInstance>.Instance;

                var spawnMessage = PlayerSpawnMessage.FromInstance(player, chanelId);
                var pos = new Vector3(spawnMessage.Position.x, 0.1f, spawnMessage.Position.z);
                spawnMessage.Position = pos;
                spawnMessage.SpawnPosition = pos;
                spawnMessage.IsInSpawnOperation = false;

                connection.Send(PlayerSpawnMessage.MessageId, spawnMessage);

                foreach (var gameSession in serverInstance.GameSessions.Values)
                {
                    if (gameSession.chanelId == chanelId) continue;
                    gameSession.SpawnQueue.Enqueue(spawnMessage);
                }

                player.Teleport(spawnMessage.Position);
                playerSpawnIsSent = true;
                return;
            }

            if (BundlesQueue.Count > 0)
            {
                ShortPeriod = 0.1f;
                if (isInLoadBundlesState) return;

                var bundleMessage = BundlesQueue.Dequeue();
                connection.Send(SomeBundleMessage.MessageId, bundleMessage);
                isInLoadBundlesState = true;
                return;
            }

            if (SpawnQueue.Count > 0)
            {
                if (isInSpawnObserverState) return;

                var spawnMessage = SpawnQueue.Dequeue();

                connection.Send(ObserverSpawnMessage.MessageId, spawnMessage);
                isInSpawnObserverState = true;
                return;
            }

            if (playerIsSpawned && !gameSpawnedIsSent)
            {
                RpcGameSpawned();
                gameSpawnedIsSent = true;
                return;
            }

            if (gameSpawnedIsSent && !gameStartingIsSent)
            {
                RpcGameStarting(5);
                gameStartingIsSent = true;
                return;
            }

            if (gameStartingIsSent && !gameStartedIsSent)
            {
                RpcGameStarted(1, 3322);
                gameStartedIsSent = true;

                player.InventoryController.SetExamined(true);
                player.ManageGameQuests();
                RegisterEnemy();
            }
        }

        public void RegisterEnemy()
        {
            try
            {
                LocalGameUtils.Get()?.AllPlayers.Add(player.GetPlayer);
                LocalGameUtils.Get()?.BotsController.AddActivePLayer(player.GetPlayer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected override void CmdSpawnConfirm(int spawnedChannel)
        {
            var serverInstance = Singleton<ServerInstance>.Instance;
            var allBots = BotsMonitor.AllBots;

            Console.WriteLine(
                $"CmdSpawnConfirm from client {connection.connectionId} " +
                $"Spawned with channelId {spawnedChannel} " +
                $"Self channelId {chanelId}"
            );
            isInSpawnObserverState = false;

            if (chanelId == spawnedChannel)
            {
                if (playerIsSpawned) return;

                foreach (var gameSession in serverInstance.GameSessions.Values)
                {
                    if (gameSession.chanelId == chanelId) continue;
                    var spawnMessage = PlayerSpawnMessage.FromInstance(gameSession.player, gameSession.chanelId);

                    SpawnQueue.Enqueue(spawnMessage);
                }

                foreach (var bot in allBots.Values)
                {
                    var emitter = bot.GetComponent<BotStateEmitter>();
                    var spawnMessage = PlayerSpawnMessage.FromInstance(bot.GetPlayer, emitter.channelId);
                    spawnMessage.PlayerId = emitter.channelId;

                    SpawnQueue.Enqueue(spawnMessage);
                }

                playerIsSpawned = true;
            }
            else
            {
                if (allBots.TryGetValue(spawnedChannel, out var bot))
                {
                    var emitter = bot.GetComponent<BotStateEmitter>();
                    emitter.availableChannels[chanelId] = true;
                    return;
                }

                if (serverInstance.GameSessions.TryGetValue(spawnedChannel, out var session))
                {
                    session.availableChannels[chanelId] = true;

                    return;
                }
            }

            RpcSyncGameTime(DateTime.UtcNow.ToBinary());
            Singleton<ServerWorld>.Instance.AddInteractiveObjectsStatusPacket();
        }
    }
}
