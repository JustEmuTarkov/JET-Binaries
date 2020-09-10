﻿using Comfort.Common;
using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Network.Handlers
{
    public static class SceneReadyHandler
    {
        public static void OnSceneReady(NetworkConnection conn)
        {
            var serverInstance = Singleton<ServerInstance>.Instance;
            var player = serverInstance.NetworkClients[conn.connectionId];
            string sessionName = $"Play session({player.Profile.Id})";

            var channelId = player.channelIndex;

            PlayerReadyHandler.OnPlayerReadyMessage(new NetworkMessage {conn = conn});

            var session = AbstractGameSession.Create<NetworkGameSession>(
                player.gameObject.transform,
                sessionName,
                player.Profile.Id
            );

            session.tag = "Finish";
            session.player = player;
            session.connection = conn;
            session.chanelId = channelId;
            session.chanelIndex = channelId;
            player.Session = session;

            serverInstance.GameSessions.TryAdd(channelId, session);
        }
    }
}
