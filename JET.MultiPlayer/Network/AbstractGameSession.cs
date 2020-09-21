using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EFT;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Network
{
    public class AbstractGameSession : AbstractSession
    {

        public static T Create<T>(Transform parent, string name, string profileId) where T : AbstractGameSession
        {
            var t = Instantiate<T>(parent, name, profileId);
            t.GetOrAddComponent<NetworkIdentity>().localPlayerAuthority = true;
            return t;
        }

        public override float GetNetworkSendInterval()
        {
            return 0f;
        }

        public override int GetNetworkChannel()
        {
            Console.WriteLine("trying to get GetNetworkChannel");
            return 0;
        }

        #region Client Cmd / Rpc
        protected virtual void CmdSpawn()
        {
            Console.WriteLine("Call CmdSpawn from client.");

            sessionIsSpawned = true;
        }

        protected virtual void CmdReSpawn()
        {
            Console.WriteLine("Call CmdReSpawn from client.");
        }

        protected virtual void CmdStartGame()
        {
            Console.WriteLine("Call CmdStartGame CmdStartGame from client.");
        }

        protected virtual void CmdStartGameAfterTeleport()
        {
            Console.WriteLine("Call CmdStartGameAfterTeleport from client.");
        }

        protected virtual void CmdRestartGameInitiate()
        {
            Console.WriteLine("Call CmdRestartGameInitiate from client.");
        }

        protected virtual void CmdRestartGame()
        {
            Console.WriteLine("Call CmdRestartGame from client.");
        }

        protected virtual void CmdStopGame()
        {
            Console.WriteLine("Call CmdStopGame command from client.");
            RpcSoftStopNotification(5);
            RpcGameStopped();
        }

        protected virtual void CmdSyncGameTime()
        {
            Console.WriteLine("Call CmdSyncGameTime CmdSyncGameTime from client.");
            RpcSyncGameTime(DateTime.UtcNow.ToBinary());
            gameSyncTimeIsSent = true;
        }

        protected virtual void CmdDevelopRequestBot()
        {
            Console.WriteLine("Call CmdDevelopRequestBot from client.");
        }

        protected virtual void CmdDevelopmentSpawnBotRequest(EPlayerSide side)
        {
            Console.WriteLine("Call CmdDevelopmentSpawnBotRequest from client.");
        }

        protected virtual void CmdDevelopmentSpawnBotOnServer(EPlayerSide side)
        {
            Console.WriteLine("Call CmdDevelopmentSpawnBotOnServer from client.");
        }

        protected virtual void CmdDevelopmentSpawnBotOnClient(EPlayerSide side, int instanceId)
        {
            Console.WriteLine("Call CmdDevelopmentSpawnBotOnClient from client.");
        }

        protected virtual void CmdDisconnectAcceptedOnClient()
        {
            Console.WriteLine("Call CmdDisconnectAcceptedOnClient from client.");
        }

        protected virtual void CmdSpawnConfirm(int playerId)
        {
            Console.WriteLine("Call CmdSpawnConfirm from client.");
        }

        protected virtual void vmethod_14()
        {
            Console.WriteLine("Call RpcGameSpawned from client.");
        }

        protected virtual void vmethod_15(ushort activitiesCounter, ushort minCounter, int seconds)
        {
            Console.WriteLine("Call RpcGameMatching from client.");
        }

        protected virtual void vmethod_16(int seconds)
        {
            Console.WriteLine("Call RpcGameStarting from client. vm-16");
        }

        protected virtual void vmethod_17(Vector3 position, int exfiltrationId, string spawnAreaName, string entryPoint)
        {
            Console.WriteLine("Call RpcGameStartingWithTeleport from client. vm-17");
        }

        protected virtual void vmethod_18(float pastTime, int escapeSeconds)
        {
            Console.WriteLine("Call RpcGameStarted from client.");
        }

        protected virtual void vmethod_19()
        {
            Console.WriteLine("Call RpcGameRestarting from client. vm-19");
        }

        protected virtual void vmethod_20()
        {
            Console.WriteLine("Call RpcGameRestarted from client. vm-20");
        }

        protected virtual void vmethod_21(ExitStatus exitStatus, int playTime)
        {
            Console.WriteLine("Call RpcGameStopped from client. vm-21");
        }

        protected virtual void vmethod_22()
        {
            Console.WriteLine("Call RpcGameStopping from client. vm-22");
        }

        protected virtual void vmethod_23(long time)
        {
            Console.WriteLine("Call RpcSyncGameTime from client. vm-23");
        }

        protected virtual void vmethod_24(byte[] data)
        {
            Console.WriteLine("Call RpcDevelopSendBotData from client. vm-24");
        }

        protected virtual void vmethod_25(EPlayerSide side, int instanceId)
        {
            Console.WriteLine("Call RpcDevelopmentSpawnBotResponse from client. vm-25");
        }

        protected virtual void vmethod_26(int escapeSeconds)
        {
            Console.WriteLine("Call RpcSoftStopNotification from client. vm-26");
        }

        protected virtual void vmethod_27(int disconnectionCode, string additionalInfo, string technicalMessage)
        {
            Console.WriteLine("Call RpcStartDisconnectionProcedure from client.");
            RpcStartDisconnectionProcedure(disconnectionCode, additionalInfo, technicalMessage);
        }

        // EFT.AbstractGameSession // Token: 0x06003F41 RID: 16193 RVA: 0x000A1F19 File Offset: 0x000A0119
        private void method_1()
        {
            Console.WriteLine("Call command from client. vm-28");
        }
        #endregion 

        #region Client Network Reader
        protected static void CmdSpawn(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("Command CmdSpawn called on client.");
            ((AbstractGameSession) obj).CmdSpawn();
        }

        protected static void CmdRespawn(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                return;
            }
            Console.WriteLine("Command CmdRespawn called on client.");
            ((AbstractGameSession) obj).CmdReSpawn();
        }

        protected static void CmdStartGame(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                return;
            }
            Console.WriteLine("Command CmdStartGame called on client.");
            ((AbstractGameSession) obj).CmdStartGame();
        }

        protected static void CmdStartGameAfterTeleport(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("Command CmdStartGameAfterTeleport called on client.");
            ((AbstractGameSession) obj).CmdStartGameAfterTeleport();
        }

        protected static void CmdRestartGameInitiate(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("Command CmdRestartGameInitiate called on client.");
            ((AbstractGameSession) obj).CmdRestartGameInitiate();
        }

        protected static void CmdRestartGame(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("Command CmdRestartGame called on client.");
            ((AbstractGameSession) obj).CmdRestartGame();
        }

        protected static void CmdStopGame(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("Command CmdStopGame called on client.");
            ((AbstractGameSession) obj).CmdStopGame();
        }

        protected static void CmdSyncGameTime(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("Command CmdSyncGameTime called on client.");
            ((AbstractGameSession) obj).CmdSyncGameTime();
        }

        protected static void CmdDevelopRequestBot(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("Command CmdDevelopRequestBot called on client.");
            ((AbstractGameSession) obj).CmdDevelopRequestBot();
        }

        protected static void CmdDevelopmentSpawnBotRequest(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("Command CmdDevelopmentSpawnBotRequest called on client.");
            ((AbstractGameSession) obj).CmdDevelopmentSpawnBotRequest((EPlayerSide) reader.ReadInt32());
        }

        protected static void CmdDevelopmentSpawnBotOnServer(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("Command CmdDevelopmentSpawnBotOnServer called on client.");
            ((AbstractGameSession) obj).CmdDevelopmentSpawnBotOnServer((EPlayerSide) reader.ReadInt32());
        }

        protected static void CmdDevelopmentSpawnBotOnClient(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                return;
            }
            Console.WriteLine("Command CmdDevelopmentSpawnBotOnClient called on client.");
            ((AbstractGameSession) obj).CmdDevelopmentSpawnBotOnClient((EPlayerSide) reader.ReadInt32(),
                (int) reader.ReadPackedUInt32());
        }

        protected static void CmdDisconnectAcceptedOnClient(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("Command CmdDisconnectAcceptedOnClient called on client.");
            ((AbstractGameSession) obj).CmdDisconnectAcceptedOnClient();
        }

        protected static void CmdSpawnConfirm(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("Command CmdSpawnConfirm called on client.");
            ((AbstractGameSession) obj).CmdSpawnConfirm((int) reader.ReadPackedUInt32());
        }
        #endregion

        #region Server/Client Handler 
        public void CallCmdSpawn()
        {
            if (!NetworkClient.active)
            {
                return;
            }

            if (isServer)
            {
                Console.WriteLine("Command function CmdSpawn called on server.");
                CmdSpawn();
                return;
            }

            Console.WriteLine("Command internal CmdSpawn called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_CmdSpawn);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendCommandInternal(networkWriter, 0, "CmdSpawn");
        }

        public void CallCmdReSpawn()
        {
            if (!NetworkClient.active)
            {
                return;
            }

            if (isServer)
            {
                Console.WriteLine("Command function CmdRespawn called on server.");
                CmdReSpawn();
                return;
            }

            Console.WriteLine("Command function CmdRespawn called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_CmdRespawn);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendCommandInternal(networkWriter, 0, "CmdRespawn");
        }

        public void CallCmdStartGame()
        {
            if (!NetworkClient.active)
            {
                return;
            }

            if (isServer)
            {
                Console.WriteLine("Command function CmdStartGame called on server.");
                CmdStartGame();
                return;
            }

            Console.WriteLine("Command function CmdStartGame called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_CmdStartGame);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendCommandInternal(networkWriter, 0, "CmdStartGame");
        }

        public void CallCmdStartGameAfterTeleport()
        {
            if (!NetworkClient.active)
            {
                return;
            }

            if (isServer)
            {
                Console.WriteLine("Command function CmdStartGameAfterTeleport called on server.");
                CmdStartGameAfterTeleport();
                return;
            }

            Console.WriteLine("Command function CmdStartGameAfterTeleport called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_CmdStartGameAfterTeleport);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendCommandInternal(networkWriter, 0, "CmdStartGameAfterTeleport");
        }

        public void CallCmdRestartGameInitiate()
        {
            if (!NetworkClient.active)
            {
                return;
            }

            if (isServer)
            {
                Console.WriteLine("Command function CmdRestartGameInitiate called on server.");
                CmdRestartGameInitiate();
                return;
            }

            Console.WriteLine("Command function CmdRestartGameInitiate called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_CmdRestartGameInitiate);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendCommandInternal(networkWriter, 0, "CmdRestartGameInitiate");
        }

        public void CallCmdRestartGame()
        {
            if (!NetworkClient.active)
            {
                return;
            }

            if (isServer)
            {
                Console.WriteLine("Command function CmdRestartGame called on server.");
                CmdRestartGame();
                return;
            }

            Console.WriteLine("Command function CmdRestartGame called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_CmdRestartGame);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendCommandInternal(networkWriter, 0, "CmdRestartGame");
        }

        public void CallCmdStopGame()
        {
            if (!NetworkClient.active)
            {
                return;
            }

            if (isServer)
            {
                Console.WriteLine("Command function CmdStopGame called on server.");
                CmdStopGame();
                return;
            }

            Console.WriteLine("Command function CmdStopGame called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_CmdStopGame);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendCommandInternal(networkWriter, 0, "CmdStopGame");
        }

        public void CallCmdSyncGameTime()
        {
            if (!NetworkClient.active)
            {
                return;
            }

            if (isServer)
            {
                Console.WriteLine("Command function CmdSyncGameTime called on server.");
                CmdSyncGameTime();
                return;
            }

            Console.WriteLine("Command function CmdSyncGameTime called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_CmdSyncGameTime);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendCommandInternal(networkWriter, 0, "CmdSyncGameTime");
        }

        public void CallCmdDevelopRequestBot()
        {
            if (!NetworkClient.active)
            {
                return;
            }

            if (isServer)
            {
                Console.WriteLine("Command function CmdDevelopRequestBot called on server.");
                CmdDevelopRequestBot();
                return;
            }

            Console.WriteLine("Command function CmdDevelopRequestBot called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_CmdDevelopRequestBot);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendCommandInternal(networkWriter, 0, "CmdDevelopRequestBot");
        }

        public void CallCmdDevelopmentSpawnBotRequest(EPlayerSide side)
        {
            if (!NetworkClient.active)
            {
                return;
            }

            if (isServer)
            {
                Console.WriteLine("Command function CmdDevelopmentSpawnBotRequest called on server.");
                CmdDevelopmentSpawnBotRequest(side);
                return;
            }

            Console.WriteLine("Command function CmdDevelopmentSpawnBotRequest called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_CmdDevelopmentSpawnBotRequest);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.Write((int) side);
            SendCommandInternal(networkWriter, 0, "CmdDevelopmentSpawnBotRequest");
        }

        public void CallCmdDevelopmentSpawnBotOnServer(EPlayerSide side)
        {
            if (!NetworkClient.active)
            {
                return;
            }

            if (isServer)
            {
                Console.WriteLine("Command function CmdDevelopmentSpawnBotOnServer called on server.");
                CmdDevelopmentSpawnBotOnServer(side);
                return;
            }

            Console.WriteLine("Command function CmdDevelopmentSpawnBotOnServer called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_CmdDevelopmentSpawnBotOnServer);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.Write((int) side);
            SendCommandInternal(networkWriter, 0, "CmdDevelopmentSpawnBotOnServer");
        }

        public void CallCmdDevelopmentSpawnBotOnClient(EPlayerSide side, int instanceId)
        {
            if (!NetworkClient.active)
            {
                return;
            }

            if (isServer)
            {
                Console.WriteLine("Command function CmdDevelopmentSpawnBotOnClient called on server.");
                CmdDevelopmentSpawnBotOnClient(side, instanceId);
                return;
            }

            Console.WriteLine("Command function CmdDevelopmentSpawnBotOnClient called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_CmdDevelopmentSpawnBotOnClient);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.Write((int) side);
            networkWriter.WritePackedUInt32((uint) instanceId);
            SendCommandInternal(networkWriter, 0, "CmdDevelopmentSpawnBotOnClient");
        }

        public void CallCmdDisconnectAcceptedOnClient()
        {
            if (!NetworkClient.active)
            {
                return;
            }

            if (isServer)
            {
                Console.WriteLine("Command function CmdDisconnectAcceptedOnClient called on server.");
                CmdDisconnectAcceptedOnClient();
                return;
            }

            Console.WriteLine("Command function CmdDisconnectAcceptedOnClient called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_CmdDisconnectAcceptedOnClient);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendCommandInternal(networkWriter, 0, "CmdDisconnectAcceptedOnClient");
        }

        public void CallCmdSpawnConfirm(int playerId)
        {
            if (!NetworkClient.active)
            {
                return;
            }

            if (isServer)
            {
                Console.WriteLine("Command function CmdSpawnConfirm called on server.");
                CmdSpawnConfirm(playerId);
                return;
            }

            Console.WriteLine("Command function CmdSpawnConfirm called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_CmdSpawnConfirm);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.WritePackedUInt32((uint) playerId);
            SendCommandInternal(networkWriter, 0, "CmdSpawnConfirm");
        }
        #endregion

        #region Server Network Reader
        protected static void sm_RpcGameSpawned(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                return;
            }

            Console.WriteLine("RPC RpcGameSpawned called on server.");
            ((AbstractGameSession) obj).vmethod_14();
        }

        protected static void sm_RpcGameMatching(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                return;
            }

            Console.WriteLine("RPC RpcGameMatching called on server.");
            ((AbstractGameSession) obj).vmethod_15((ushort) reader.ReadPackedUInt32(),
                (ushort) reader.ReadPackedUInt32(), (int) reader.ReadPackedUInt32());
        }

        protected static void sm_RpcGameStarting(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                return;
            }

            Console.WriteLine("RPC RpcGameStarting called on server.");
            ((AbstractGameSession) obj).vmethod_16((int) reader.ReadPackedUInt32());
        }

        protected static void sm_RpcGameStartingWithTeleport(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                return;
            }

            Console.WriteLine("RPC RpcGameStartingWithTeleport called on server.");
            ((AbstractGameSession) obj).vmethod_17(reader.ReadVector3(), (int) reader.ReadPackedUInt32(),
                reader.ReadString(), reader.ReadString());
        }

        protected static void sm_RpcGameStarted(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                return;
            }

            Console.WriteLine("RPC RpcGameStarted called on server.");
            ((AbstractGameSession) obj).vmethod_18(reader.ReadSingle(), (int) reader.ReadPackedUInt32());
        }

        protected static void sm_RpcGameRestarting(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                return;
            }

            Console.WriteLine("RPC RpcGameRestarting called on server.");
            ((AbstractGameSession) obj).vmethod_19();
        }

        protected static void sm_RpcGameRestarted(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                return;
            }

            Console.WriteLine("RPC RpcGameRestarted called on server.");
            ((AbstractGameSession) obj).vmethod_20();
        }

        protected static void sm_RpcGameStopping(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                return;
            }

            Console.WriteLine("RPC RpcGameStopping called on server.");
            ((AbstractGameSession) obj).vmethod_21((ExitStatus) reader.ReadInt32(), (int) reader.ReadPackedUInt32());
        }

        protected static void sm_RpcGameStopped(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                return;
            }

            Console.WriteLine("RPC RpcGameStopped called on server.");
            ((AbstractGameSession) obj).vmethod_22();
        }

        protected static void sm_RpcSyncGameTime(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                return;
            }

            Console.WriteLine("RPC RpcSyncGameTime called on server.");
            ((AbstractGameSession) obj).vmethod_23((long) reader.ReadPackedUInt64());
        }

        protected static void sm_RpcDevelopSendBotData(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                return;
            }

            Console.WriteLine("RPC RpcDevelopSendBotData called on server.");
            ((AbstractGameSession) obj).vmethod_24(reader.ReadBytesAndSize());
        }

        protected static void sm_RpcDevelopmentSpawnBotResponse(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                return;
            }

            Console.WriteLine("RPC RpcDevelopmentSpawnBotResponse called on server.");
            ((AbstractGameSession) obj).vmethod_25((EPlayerSide) reader.ReadInt32(), (int) reader.ReadPackedUInt32());
        }

        protected static void sm_RpcSoftStopNotification(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                return;
            }

            Console.WriteLine("RPC RpcSoftStopNotification called on server.");
            ((AbstractGameSession) obj).vmethod_26((int) reader.ReadPackedUInt32());
        }

        protected static void sm_RpcStartDisconnectionProcedure(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                return;
            }

            Console.WriteLine("RPC RpcStartDisconnectionProcedure called on server.");
            ((AbstractGameSession) obj).vmethod_27((int) reader.ReadPackedUInt32(), reader.ReadString(),
                reader.ReadString());
        }
        #endregion

        #region Client RPC Writer
        public void RpcGameSpawned()
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("RPC Function RpcGameSpawned called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_sm_RpcGameSpawned);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);

            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcGameSpawned");
        }

        public void RpcGameMatching(ushort activitiesCounter, ushort minCounter, int seconds)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("RPC Function RpcGameMatching called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_sm_RpcGameMatching);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.WritePackedUInt32((uint) activitiesCounter);
            networkWriter.WritePackedUInt32((uint) minCounter);
            networkWriter.WritePackedUInt32((uint) seconds);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcGameMatching");
        }

        public void RpcGameStarting(int seconds)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("RPC Function RpcGameStarting called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_sm_RpcGameStarting);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.WritePackedUInt32((uint) seconds);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcGameStarting");
        }

        public void RpcGameStartingWithTeleport(Vector3 position, int exfiltrationId, string spawnAreaName,
            string entryPoint)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("RPC Function RpcGameStartingWithTeleport called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_sm_RpcGameStartingWithTeleport);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.Write(position);
            networkWriter.WritePackedUInt32((uint) exfiltrationId);
            networkWriter.Write(spawnAreaName);
            networkWriter.Write(entryPoint);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcGameStartingWithTeleport");
        }

        public void RpcGameStarted(float pastTime, int escapeSeconds)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("RPC Function RpcGameStarted called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_sm_RpcGameStarted);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.Write(pastTime);
            networkWriter.WritePackedUInt32((uint) escapeSeconds);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcGameStarted");
        }

        public void RpcGameRestarting()
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("RPC Function RpcGameRestarting called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_sm_RpcGameRestarting);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcGameRestarting");
        }

        public void RpcGameRestarted()
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("RPC Function RpcGameRestarted called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_sm_RpcGameRestarted);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcGameRestarted");
        }

        public void RpcGameStopping(ExitStatus exitStatus, int playTime)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("RPC Function RpcGameStopping called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_sm_RpcGameStopping);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.Write((int) exitStatus);
            networkWriter.WritePackedUInt32((uint) playTime);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcGameStopping");
        }

        public void RpcGameStopped()
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("RPC Function RpcGameStopped called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_sm_RpcGameStopped);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcGameStopped");
        }

        public void RpcSyncGameTime(long time)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("RPC Function RpcSyncGameTime called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_sm_RpcSyncGameTime);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.WritePackedUInt64((ulong) time);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcSyncGameTime");
        }

        public void RpcDevelopSendBotData(byte[] data)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("RPC Function RpcDevelopSendBotData called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_sm_RpcDevelopSendBotData);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.WriteBytesFull(data);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcDevelopSendBotData");
        }

        public void RpcDevelopmentSpawnBotResponse(EPlayerSide side, int instanceId)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("RPC Function RpcDevelopmentSpawnBotResponse called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_sm_RpcDevelopmentSpawnBotResponse);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.Write((int) side);
            networkWriter.WritePackedUInt32((uint) instanceId);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcDevelopmentSpawnBotResponse");
        }

        public void RpcSoftStopNotification(int escapeSeconds)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("RPC Function RpcSoftStopNotification called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_sm_RpcSoftStopNotification);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.WritePackedUInt32((uint) escapeSeconds);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcSoftStopNotification");
        }

        public void RpcStartDisconnectionProcedure(int disconnectionCode, string additionalInfo,
            string technicalMessage)
        {
            if (!NetworkServer.active)
            {
                return;
            }

            Console.WriteLine("RPC Function RpcStartDisconnectionProcedure called on client.");
            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) i_sm_RpcStartDisconnectionProcedure);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.WritePackedUInt32((uint) disconnectionCode);
            networkWriter.Write(additionalInfo);
            networkWriter.Write(technicalMessage);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcStartDisconnectionProcedure");
        }
        #endregion

        protected static void sm_CmdGameStarted(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdGameStarted - server not active");
                return;
            }

            Console.WriteLine("Command CmdGameStarted called from client.");
        }

        #region INIT
        static void OverrideInitCodes()
        {
            i_CmdSpawn = -1723132743;
            i_CmdRespawn = 740792038;
            i_CmdStartGame = -1220356686; //FB B2 D5 42 B7‬ - ‭1081037111991‬
            i_CmdStartGameAfterTeleport = 1792897173;
            i_CmdRestartGameInitiate = 273195288;
            i_CmdRestartGame = -1501005473; // FB 5F 79 88 A6‬ - ‭1079638591654‬
            i_CmdStopGame = -750099178; // FB 16 65 4A D3 ‬‭10 - 78412528339‬
            i_CmdSyncGameTime = 463608476;
            i_CmdDevelopRequestBot = -1035840717; // FB 33 53 42 C2 - ‬‭1078897885890‬
            i_CmdDevelopmentSpawnBotRequest = -1581543574; // FB 6A 8F BB A1 - ‬‭1079824595873‬
            i_CmdDevelopmentSpawnBotOnServer = 102630535;
            i_CmdDevelopmentSpawnBotOnClient = -349255409; // FB 0F C9 2E EB -‬ ‭1078301634283‬
            i_CmdDisconnectAcceptedOnClient = -1733636721; // FB 8F CD AA 98 - ‬‭1080449411736‬
            i_CmdSpawnConfirm = -1317447737; // FB C7 57 79 B1‬ - ‭1081381190065‬
            i_sm_RpcGameSpawned = -1952818640; // FB 30 5A 9A 8B - ‬‭1078848035467‬
            i_sm_RpcGameMatching = 2117859815;
            i_sm_RpcGameStarting = -1157222870; // FB 2A 2E 06 BB - ‭1078744450747‬
            i_sm_RpcGameStartingWithTeleport = 1572370779;
            i_sm_RpcGameStarted = -1838445225; // FB 57 8D 6B 92 - ‬‭1079505677202‬
            i_sm_RpcGameRestarting = 94275293;
            i_sm_RpcGameRestarted = -1243884988; // FB 44 D2 DB B5 - ‬‭1079191460789‬
            i_sm_RpcGameStopping = -758380962; // FB 5E 06 CC D2 -‬ ‭1079614295250‬
            i_sm_RpcGameStopped = -1825579357; // FB A3 DE 2F 93 - ‬‭1080786038675‬
            i_sm_RpcSyncGameTime = 547040626;
            i_sm_RpcDevelopSendBotData = 1152897188;
            i_sm_RpcDevelopmentSpawnBotResponse = -1269941968; // FB 30 39 4E B4‬ - ‭1078845853364‬
            i_sm_RpcSoftStopNotification = -435294673; // FB 2F EE 0D E6 -‬ ‭1078840921574‬
            i_sm_RpcStartDisconnectionProcedure = 1124901489;
            i_sm_CmdGameStarted = -40021267;
        }
        static AbstractGameSession()
        {
            Type invokedType = typeof(AbstractGameSession);
            OverrideInitCodes();
            RegisterCommandDelegate(invokedType, i_CmdSpawn, CmdSpawn);
            RegisterCommandDelegate(invokedType, i_CmdRespawn, CmdRespawn);
            RegisterCommandDelegate(invokedType, i_CmdStartGame, CmdStartGame);
            RegisterCommandDelegate(invokedType, i_CmdStartGameAfterTeleport, CmdStartGameAfterTeleport);
            RegisterCommandDelegate(invokedType, i_CmdRestartGameInitiate, CmdRestartGameInitiate);
            RegisterCommandDelegate(invokedType, i_CmdRestartGame, CmdRestartGame);
            RegisterCommandDelegate(invokedType, i_CmdStopGame, CmdStopGame);
            RegisterCommandDelegate(invokedType, i_CmdSyncGameTime, CmdSyncGameTime);
            RegisterCommandDelegate(invokedType, i_CmdDevelopRequestBot, CmdDevelopRequestBot);
            RegisterCommandDelegate(invokedType, i_CmdDevelopmentSpawnBotRequest, CmdDevelopmentSpawnBotRequest);
            RegisterCommandDelegate(invokedType, i_CmdDevelopmentSpawnBotOnServer, CmdDevelopmentSpawnBotOnServer);
            RegisterCommandDelegate(invokedType, i_CmdDevelopmentSpawnBotOnClient, CmdDevelopmentSpawnBotOnClient);
            RegisterCommandDelegate(invokedType, i_CmdDisconnectAcceptedOnClient, CmdDisconnectAcceptedOnClient);
            RegisterCommandDelegate(invokedType, i_CmdSpawnConfirm, CmdSpawnConfirm);
            RegisterRpcDelegate(invokedType, i_sm_RpcGameSpawned, sm_RpcGameSpawned);
            RegisterRpcDelegate(invokedType, i_sm_RpcGameMatching, sm_RpcGameMatching);
            RegisterRpcDelegate(invokedType, i_sm_RpcGameStarting, sm_RpcGameStarting);
            RegisterRpcDelegate(invokedType, i_sm_RpcGameStartingWithTeleport, sm_RpcGameStartingWithTeleport);
            RegisterRpcDelegate(invokedType, i_sm_RpcGameStarted, sm_RpcGameStarted);
            RegisterRpcDelegate(invokedType, i_sm_RpcGameRestarting, sm_RpcGameRestarting);
            RegisterRpcDelegate(invokedType, i_sm_RpcGameRestarted, sm_RpcGameRestarted);
            RegisterRpcDelegate(invokedType, i_sm_RpcGameStopping, sm_RpcGameStopping);
            RegisterRpcDelegate(invokedType, i_sm_RpcGameStopped, sm_RpcGameStopped);
            RegisterRpcDelegate(invokedType, i_sm_RpcSyncGameTime, sm_RpcSyncGameTime);
            RegisterRpcDelegate(invokedType, i_sm_RpcDevelopSendBotData, sm_RpcDevelopSendBotData);
            RegisterRpcDelegate(invokedType, i_sm_RpcDevelopmentSpawnBotResponse, sm_RpcDevelopmentSpawnBotResponse);
            RegisterRpcDelegate(invokedType, i_sm_RpcSoftStopNotification, sm_RpcSoftStopNotification);
            RegisterRpcDelegate(invokedType, i_sm_RpcStartDisconnectionProcedure, sm_RpcStartDisconnectionProcedure);
            RegisterCommandDelegate(invokedType, i_sm_CmdGameStarted, sm_CmdGameStarted);
            NetworkCRC.RegisterBehaviour("AbstractGameSession", 0);
        }
        #endregion

        public override bool OnSerialize(NetworkWriter writer, bool forceAll)
        {
            var flag = base.OnSerialize(writer, forceAll);
            return false || flag;
        }

        #region Command Codes
        private static int i_CmdSpawn = -1723132743;
        private static int i_CmdRespawn = 740792038;
        private static int i_CmdStartGame = -1220356686;
        private static int i_CmdStartGameAfterTeleport = 1792897173;
        private static int i_CmdRestartGameInitiate = 273195288;
        private static int i_CmdRestartGame = -1501005473;
        private static int i_CmdStopGame = -750099178;
        private static int i_CmdSyncGameTime = 463608476;
        private static int i_CmdDevelopRequestBot = -1035840717;
        private static int i_CmdDevelopmentSpawnBotRequest = -1581543574;
        private static int i_CmdDevelopmentSpawnBotOnServer = 102630535;
        private static int i_CmdDevelopmentSpawnBotOnClient = -349255409;
        private static int i_CmdDisconnectAcceptedOnClient = -1733636721;
        private static int i_CmdSpawnConfirm = -1317447737;
        private static int i_sm_RpcGameSpawned = -1952818640;
        private static int i_sm_RpcGameMatching = 2117859815;
        private static int i_sm_RpcGameStarting = -1157222870;
        private static int i_sm_RpcGameStartingWithTeleport = 1572370779;
        private static int i_sm_RpcGameStarted = -1838445225;
        private static int i_sm_RpcGameRestarting = 94275293;
        private static int i_sm_RpcGameRestarted = -1243884988;
        private static int i_sm_RpcGameStopping = -758380962;
        private static int i_sm_RpcGameStopped = -1825579357;
        private static int i_sm_RpcSyncGameTime = 547040626;
        private static int i_sm_RpcDevelopSendBotData = 1152897188;
        private static int i_sm_RpcDevelopmentSpawnBotResponse = -1269941968;
        private static int i_sm_RpcSoftStopNotification = -435294673;
        private static int i_sm_RpcStartDisconnectionProcedure = 1124901489;
        private static int i_sm_CmdGameStarted = -40021267;
        #endregion

        public bool gameSyncTimeIsSent;
        public bool worldMessageIsSent;
        public bool subWorldMessageIsSent;
        public bool playerSpawnIsSent;
        public bool gameSpawnedIsSent;
        public bool allPlayersSpawned;
        public bool gameStartingIsSent;
        public bool gameStartedIsSent;

        public bool playerIsSpawned;

        public bool sessionIsSpawned;

        // public ChannelCombined ChannelCombined;
    }
}
