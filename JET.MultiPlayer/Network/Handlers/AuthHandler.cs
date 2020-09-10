using System;
using System.Linq;
using Comfort.Common;
using EFT;
using ServerLib.Network.Messages;
using ServerLib.Network.Player;
using ServerLib.Network.World;
using ServerLib.Utils.HTTP;
using ServerLib.Utils.Server;
using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Network.Handlers
{
    public static class AuthHandler
    {
        public static async void OnAuthMessage(NetworkMessage message)
        {
            int count = 0;
            var authMessage = message.ReadMessage<AuthRequestMessage>();
            var serverInstance = Singleton<ServerInstance>.Instance;
            Console.WriteLine(count);count++;
            Profile profile = new HttpUtils.Create<Profile>(authMessage.ProfileId)
                .Get($"/server/profile/{authMessage.ProfileId}");

            Console.WriteLine(profile);
            Console.WriteLine(count);
            count++;

            var channelId = ServerInstance.GetNextChannelId();
            var spawnMessage = PlayerSpawnMessage.FromProfile(channelId, profile);
            Console.WriteLine(count);
            count++;

            var player = ServerPlayer.Create(
                spawnMessage.PlayerId, spawnMessage.Position,
                Singleton<ServerInstance>.Instance
            );
            Console.WriteLine(count);
            count++;
            await player.Init(spawnMessage);
            Console.WriteLine(count);
            count++;

            player.channelIndex = (byte) channelId;
            serverInstance.NetworkClients.TryAdd(message.conn.connectionId, player);

            Console.WriteLine(count);
            count++;
            var playerPrefabs = profile.Inventory.GetAllInventoryPrefabs();
            serverInstance.AllPrefabs.AddRange(playerPrefabs);
            Console.WriteLine(count);
            count++;

            var msg = new SomeBundleMessage {LootItems = playerPrefabs.ToArray(), Id = SomeBundleMessage.BundlesQueue};
            foreach (var gameSession in serverInstance.GameSessions.Values)
            {
                if (gameSession.connection.connectionId == message.conn.connectionId) continue;
                gameSession.BundlesQueue.Enqueue(msg);
            }
            Console.WriteLine(count);
            count++;

            var authResponseMessage = AuthResponseMessage.GetAuthResponseMessage();
            authResponseMessage.MemberCategory = player.Profile.Info.MemberCategory;
            authResponseMessage.Prefabs = Singleton<GameWorld>.Instance.GetAllLootPrefabs();
            Console.WriteLine(count);
            count++;

            NetworkServer.SendToClient(
                message.conn.connectionId,
                AuthResponseMessage.MessageId,
                authResponseMessage
            );
            Console.WriteLine("END");
        }
    }
}
