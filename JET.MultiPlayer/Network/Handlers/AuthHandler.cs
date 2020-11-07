using System;
using System.Linq;
using System.Linq.Expressions;
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
            try
            {
                var authMessage = message.ReadMessage<AuthRequestMessage>();
                var serverInstance = Singleton<ServerInstance>.Instance;
                // changed cause server is sending whole profile without an array
                Profile profile = new HttpUtils.Create<Profile>(authMessage.ProfileId)
                    .Get($"/server/profile/{authMessage.ProfileId}");

                var channelId = ServerInstance.GetNextChannelId();
                var spawnMessage = PlayerSpawnMessage.FromProfile(channelId, profile);

                var player = ServerPlayer.Create(
                    spawnMessage.PlayerId, spawnMessage.Position,
                    Singleton<ServerInstance>.Instance
                );
                await player.Init(spawnMessage);

                player.channelIndex = (byte) channelId;
                serverInstance.NetworkClients.TryAdd(message.conn.connectionId, player);

                var playerPrefabs = profile.Inventory.GetAllEquipmentItems();//.GetAllInventoryPrefabs();
                serverInstance.AllPrefabs.AddRange(playerPrefabs); // TODO: this need a rewrite !!!!!!

                var msg = new SomeBundleMessage { LootItems = playerPrefabs.ToArray(), Id = SomeBundleMessage.BundlesQueue };
                foreach (var gameSession in serverInstance.GameSessions.Values)
                {
                    if (gameSession.connection.connectionId == message.conn.connectionId)
                        continue;
                    gameSession.BundlesQueue.Enqueue(msg);
                }

                var authResponseMessage = AuthResponseMessage.GetAuthResponseMessage();
                authResponseMessage.MemberCategory = player.Profile.Info.MemberCategory;
                authResponseMessage.Prefabs = Singleton<GameWorld>.Instance.GetAllLootPrefabs();

                NetworkServer.SendToClient(
                    message.conn.connectionId,
                    AuthResponseMessage.MessageId,
                    authResponseMessage
                );
                Console.WriteLine($"AuthHandler Copleted Action! for {authMessage.ProfileId}");
            } catch(Exception e){
                Console.WriteLine($"AuthHandler ERROR!!!");
                Console.WriteLine($"{e}");
            }
            
        }
    }
}
