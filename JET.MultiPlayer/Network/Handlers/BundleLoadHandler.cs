﻿using System;
using Comfort.Common;
using EFT;
using ServerLib.Network.Messages;
using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Network.Handlers
{
    public static class BundleLoadHandler
    {
        public static void OnReportProgressLoading(NetworkMessage networkMessage)
        {
            var server = Singleton<ServerInstance>.Instance;
            var reportProgressMessage = networkMessage.ReadMessage<LoadingBundlesMessage>();
            int taskId = reportProgressMessage.TaskId;

            Console.WriteLine(
                $" OnReportProgressLoading received, Profile is: {reportProgressMessage.ProfileId}," +
                $" OperationId is: {reportProgressMessage.TaskId}," +
                $" ProgressValue is: {reportProgressMessage.ProgressValue}"
            );

            if (!(reportProgressMessage.ProgressValue >= 1f)) return;

            if (taskId == SomeBundleMessage.BundlesQueue)
            {
                if (server.NetworkClients.TryGetValue(networkMessage.conn.connectionId, out var player))
                {
                    player.Session.isInLoadBundlesState = false;
                }

                return;
            }

            SpawnPlayerObject(networkMessage.conn);
        }

        private static void SpawnPlayerObject(NetworkConnection connection)
        {
            var serverInstance = Singleton<ServerInstance>.Instance;
            if (!serverInstance.NetworkClients.TryGetValue(connection.connectionId, out var player))
            {
                Console.WriteLine($"ERROR!!! Session with id {connection.connectionId} is not found in gameSessions");
                return;
            }

            Console.WriteLine($"SpawnPlayerObject with Client Authority for id {connection.connectionId}");

            NetworkServer.SpawnWithClientAuthority(
                player.Session.gameObject,
                AbstractSession.AuthorityHash,
                connection
            );
        }
    }
}
