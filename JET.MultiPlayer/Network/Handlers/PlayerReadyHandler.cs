using System;
using Comfort.Common;
using EFT;
using ServerLib.Network.Messages;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Network.Handlers
{
    public static class PlayerReadyHandler
    {
        public static void OnPlayerReadyMessage(NetworkMessage message)
        {
            var someBundleMessage = new SomeBundleMessage
            {
                Id = 0, LootItems = Singleton<ServerInstance>.Instance.AllPrefabs.ToArray()
            };

            NetworkServer.SendToClient(
                message.conn.connectionId,
                SomeBundleMessage.MessageId,
                someBundleMessage
            );

            Console.WriteLine($"OnPlayerReady received, conn id is: {message.conn.connectionId}");
        }
    }
}
