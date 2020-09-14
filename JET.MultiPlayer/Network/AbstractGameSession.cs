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
        internal static T Create<T>(Transform parent, string name, string profileId) where T : AbstractGameSession
        {
            T t = Instantiate<T>(parent, name, profileId);
            t.GetComponent<NetworkIdentity>().localPlayerAuthority = true;
            return t;
        }

        public override float GetNetworkSendInterval()
        {
            return 0f;
        }

        public override int GetNetworkChannel()
        {
            return 0; // always return 0 // (int)Class783.Byte_0;
                      // make sure its avaliable cause that class is internal static
        }

        #region vm Calls [command] [clientrpc]
        [Command]
        protected virtual void vmCmdSpawn()
        {
            Console.WriteLine("Call command <CmdSpawn> from client.");
            sessionIsSpawned = true;
        }

        [Command]
        protected virtual void vmCmdRespawn()
        {
            Console.WriteLine("Call command <CmdRespawn> from client.");
        }

        [Command]
        protected virtual void vmCmdStartGame()
        {
            Console.WriteLine("Call command <CmdStartGame> from client.");
        }

        [Command]
        protected virtual void vmCmdStartGameAfterTeleport()
        {
            Console.WriteLine("Call command <CmdStartGameAfterTeleport> from client.");
        }

        [Command]
        protected virtual void vmCmdRestartGameInitiate()
        {
            Console.WriteLine("Call command <CmdRestartGameInitiate> from client.");
        }

        [Command]
        protected virtual void vmCmdRestartGame()
        {
            Console.WriteLine("Call command <CmdRestartGame> from client.");
        }

        [Command]
        protected virtual void vmCmdGameStarted()
        {
            Console.WriteLine("Call command <CmdGameStarted> from client.");
        }

        [Command]
        protected virtual void vmCmdStopGame()
        {
            Console.WriteLine("Call command <CmdStopGame> from client.");
            CallRpcSoftStopNotification(5);
            CallRpcGameStopped(ExitStatus.Left, 0);
        }

        [Command]
        protected virtual void vmCmdSyncGameTime()
        {
            Console.WriteLine("Call command <CmdSyncGameTime> from client.");
            CallRpcSyncGameTime(DateTime.UtcNow.ToBinary());
            gameSyncTimeIsSent = true;
        }

        [Command]
        protected virtual void vmCmdDevelopRequestBot()
        {
            Console.WriteLine("Call command <CmdDevelopRequestBot> from client.");
        }

        [Command]
        protected virtual void vmCmdDevelopmentSpawnBotRequest(EPlayerSide side)
        {
            Console.WriteLine("Call command <CmdDevelopmentSpawnBotRequest> from client.");
        }

        [Command]
        protected virtual void vmCmdDevelopmentSpawnBotOnServer(EPlayerSide side)
        {
            Console.WriteLine("Call command <CmdDevelopmentSpawnBotOnServer> from client.");
        }

        [Command]
        protected virtual void vmCmdDevelopmentSpawnBotOnClient(EPlayerSide side, int instanceId)
        {
            Console.WriteLine("Call command <CmdDevelopmentSpawnBotOnClient> from client.");
        }

        [Command]
        protected virtual void vmCmdDisconnectAcceptedOnClient()
        {
            Console.WriteLine("Call command <CmdDisconnectAcceptedOnClient> from client.");
        }

        [Command]
        protected virtual void vmCmdSpawnConfirm(int playerId)
        {
            Console.WriteLine("Call command <CmdSpawnConfirm> from client.");
        }

        [ClientRpc]
        protected virtual void vmRpcGameSpawned()
        {
            Console.WriteLine("Call command <RpcGameSpawned> from client.");
        }

        [ClientRpc]
        protected virtual void vmRpcGameMatching(ushort activitiesCounter, ushort minCounter, int seconds)
        {
            Console.WriteLine("Call command <RpcGameMatching> from client.");
        }

        [ClientRpc]
        protected virtual void vmRpcGameStarting(int seconds)
        {
            Console.WriteLine("Call command <RpcGameStarting> from client.");
        }

        [ClientRpc]
        protected virtual void vmRpcGameStartingWithTeleport(Vector3 position, int exfiltrationId, string spawnAreaName, string entryPoint)
        {
            Console.WriteLine("Call command <RpcGameStartingWithTeleport> from client.");
        }

        [ClientRpc]
        protected virtual void vmRpcGameStarted(float pastTime, int escapeSeconds)
        {
            Console.WriteLine("Call command <RpcGameStarted> from client.");
        }

        [ClientRpc]
        protected virtual void vmRpcGameRestarting()
        {
            Console.WriteLine("Call command <RpcGameRestarting> from client.");
        }

        [ClientRpc]
        protected virtual void vmRpcGameRestarted()
        {
            Console.WriteLine("Call command <RpcGameRestarted> from client.");
        }

        [ClientRpc]
        protected virtual void vmRpcGameStopping()
        {
            Console.WriteLine("Call command <RpcGameStopping> from client.");
        }

        [ClientRpc]
        protected virtual void vmRpcGameStopped(ExitStatus exitStatus, int playTime)
        {
            Console.WriteLine("Call command <RpcGameStopped> from client.");
        }

        [ClientRpc]
        protected virtual void vmRpcSyncGameTime(long time)
        {
            Console.WriteLine("Call command <RpcSyncGameTime> from client.");
        }

        [ClientRpc]
        protected virtual void vmRpcDevelopSendBotData(byte[] data)
        {
            Console.WriteLine("Call command <RpcDevelopSendBotData> from client.");
        }

        [ClientRpc]
        protected virtual void vmRpcDevelopmentSpawnBotResponse(EPlayerSide side, int instanceId)
        {
            Console.WriteLine("Call command <RpcDevelopmentSpawnBotResponse> from client.");
        }

        [ClientRpc]
        protected virtual void vmRpcSoftStopNotification(int escapeSeconds)
        {
            Console.WriteLine("Call command <RpcSoftStopNotification> from client.");
        }

        [ClientRpc]
        protected virtual void vmRpcStartDisconnectionProcedure(int disconnectionCode, string additionalInfo, string technicalMessage)
        {
            Console.WriteLine("Call command <RpcStartDisconnectionProcedure> from client.");
            CallRpcStartDisconnectionProcedure(disconnectionCode, additionalInfo, technicalMessage);
        }

        #endregion

        private void method_1()
        {
            Console.WriteLine("Call command <method_1> from client.");
        }

        #region sm Calls
        protected static void smCmdSpawn(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdSpawn called on client.");
                return;
            }
            ((AbstractGameSession) obj).vmCmdSpawn();
        }

        protected static void smCmdRespawn(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdRespawn called on client.");
                return;
            }
            ((AbstractGameSession) obj).vmCmdRespawn();
        }

        protected static void smCmdStartGame(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdStartGame called on client.");
                return;
            }
            ((AbstractGameSession) obj).vmCmdStartGame();
        }

        protected static void smCmdStartGameAfterTeleport(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdStartGameAfterTeleport called on client.");
                return;
            }
            ((AbstractGameSession) obj).vmCmdStartGameAfterTeleport();
        }

        protected static void smCmdRestartGameInitiate(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdRestartGameInitiate called on client.");
                return;
            }
            ((AbstractGameSession) obj).vmCmdRestartGameInitiate();
        }

        protected static void smCmdRestartGame(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdRestartGame called on client.");
                return;
            }
            ((AbstractGameSession) obj).vmCmdRestartGame();
        }

        protected static void smCmdGameStarted(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdGameStarted called on client.");
                return;
            }
            ((AbstractGameSession) obj).vmCmdGameStarted();
        }

        protected static void smCmdStopGame(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdStopGame called on client.");
                return;
            }
            ((AbstractGameSession) obj).vmCmdStopGame();
        }

        protected static void smCmdSyncGameTime(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdSyncGameTime called on client.");
                return;
            }
            ((AbstractGameSession) obj).vmCmdSyncGameTime();
        }

        protected static void smCmdDevelopRequestBot(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdDevelopRequestBot called on client.");
                return;
            }
            ((AbstractGameSession) obj).vmCmdDevelopRequestBot();
        }

        protected static void smCmdDevelopmentSpawnBotRequest(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdDevelopmentSpawnBotRequest called on client.");
                return;
            }
            ((AbstractGameSession) obj).vmCmdDevelopmentSpawnBotRequest((EPlayerSide) reader.ReadInt32());
        }

        protected static void smCmdDevelopmentSpawnBotOnServer(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdDevelopmentSpawnBotOnServer called on client.");
                return;
            }
            ((AbstractGameSession) obj).vmCmdDevelopmentSpawnBotOnServer((EPlayerSide) reader.ReadInt32());
        }

        protected static void smCmdDevelopmentSpawnBotOnClient(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdDevelopmentSpawnBotOnClient called on client.");
                return;
            }
            ((AbstractGameSession) obj).vmCmdDevelopmentSpawnBotOnClient((EPlayerSide) reader.ReadInt32(), (int) reader.ReadPackedUInt32());
        }

        protected static void smCmdDisconnectAcceptedOnClient(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdDisconnectAcceptedOnClient called on client.");
                return;
            }
            ((AbstractGameSession) obj).vmCmdDisconnectAcceptedOnClient();
        }

        protected static void smCmdSpawnConfirm(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdSpawnConfirm called on client.");
                return;
            }
            ((AbstractGameSession) obj).vmCmdSpawnConfirm((int) reader.ReadPackedUInt32());
        }

        protected static void smRpcGameSpawned(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcGameSpawned called on server.");
                return;
            }
            ((AbstractGameSession) obj).vmRpcGameSpawned();
        }

        protected static void smRpcGameMatching(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcGameMatching called on server.");
                return;
            }
            ((AbstractGameSession) obj).vmRpcGameMatching((ushort) reader.ReadPackedUInt32(), (ushort) reader.ReadPackedUInt32(), (int) reader.ReadPackedUInt32());
        }

        protected static void smRpcGameStarting(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcGameStarting called on server.");
                return;
            }
            ((AbstractGameSession) obj).vmRpcGameStarting((int) reader.ReadPackedUInt32());
        }

        protected static void smRpcGameStartingWithTeleport(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcGameStartingWithTeleport called on server.");
                return;
            }
            ((AbstractGameSession) obj).vmRpcGameStartingWithTeleport(reader.ReadVector3(), (int) reader.ReadPackedUInt32(), reader.ReadString(), reader.ReadString());
        }

        protected static void smRpcGameStarted(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcGameStarted called on server.");
                return;
            }
            ((AbstractGameSession) obj).vmRpcGameStarted(reader.ReadSingle(), (int) reader.ReadPackedUInt32());
        }

        protected static void smRpcGameRestarting(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcGameRestarting called on server.");
                return;
            }
            ((AbstractGameSession) obj).vmRpcGameRestarting();
        }

        protected static void smRpcGameRestarted(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcGameRestarted called on server.");
                return;
            }
            ((AbstractGameSession) obj).vmRpcGameRestarted();
        }

        protected static void smRpcGameStopping(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcGameStopping called on server.");
                return;
            }
            ((AbstractGameSession) obj).vmRpcGameStopping();
        }

        protected static void smRpcGameStopped(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcGameStopped called on server.");
                return;
            }
            ((AbstractGameSession) obj).vmRpcGameStopped((ExitStatus) reader.ReadInt32(), (int) reader.ReadPackedUInt32());
        }

        protected static void smRpcSyncGameTime(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcSyncGameTime called on server.");
                return;
            }
            ((AbstractGameSession) obj).vmRpcSyncGameTime((long) reader.ReadPackedUInt64());
        }

        protected static void smRpcDevelopSendBotData(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcDevelopSendBotData called on server.");
                return;
            }
            ((AbstractGameSession) obj).vmRpcDevelopSendBotData(reader.ReadBytesAndSize());
        }

        protected static void smRpcDevelopmentSpawnBotResponse(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcDevelopmentSpawnBotResponse called on server.");
                return;
            }
            ((AbstractGameSession) obj).vmRpcDevelopmentSpawnBotResponse((EPlayerSide) reader.ReadInt32(), (int) reader.ReadPackedUInt32());
        }

        protected static void smRpcSoftStopNotification(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcSoftStopNotification called on server.");
                return;
            }
            ((AbstractGameSession) obj).vmRpcSoftStopNotification((int) reader.ReadPackedUInt32());
        }

        protected static void smRpcStartDisconnectionProcedure(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcStartDisconnectionProcedure called on server.");
                return;
            }
            ((AbstractGameSession) obj).vmRpcStartDisconnectionProcedure((int) reader.ReadPackedUInt32(), reader.ReadString(), reader.ReadString());
        }
        #endregion

        #region Call Cmd & Rpc
        public void CallCmdSpawn()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdSpawn called on server.");
                return;
            }
            if (base.isServer)
            {
                this.vmCmdSpawn();
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(5);
            networkWriter.WritePackedUInt32((uint) iCmdSpawn);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            base.SendCommandInternal(networkWriter, 0, "CmdSpawn");
        }
        public void CallCmdRespawn()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdRespawn called on server.");
                return;
            }
            if (base.isServer)
            {
                this.vmCmdRespawn();
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(5);
            networkWriter.WritePackedUInt32((uint) iCmdRespawn);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            base.SendCommandInternal(networkWriter, 0, "CmdRespawn");
        }
        public void CallCmdStartGame()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdStartGame called on server.");
                return;
            }
            if (base.isServer)
            {
                this.vmCmdStartGame();
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(5);
            networkWriter.WritePackedUInt32((uint) iCmdStartGame);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            base.SendCommandInternal(networkWriter, 0, "CmdStartGame");
        }
        public void CallCmdStartGameAfterTeleport()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdStartGameAfterTeleport called on server.");
                return;
            }
            if (base.isServer)
            {
                this.vmCmdStartGameAfterTeleport();
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(5);
            networkWriter.WritePackedUInt32((uint) iCmdStartGameAfterTeleport);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            base.SendCommandInternal(networkWriter, 0, "CmdStartGameAfterTeleport");
        }
        public void CallCmdRestartGameInitiate()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdRestartGameInitiate called on server.");
                return;
            }
            if (base.isServer)
            {
                this.vmCmdRestartGameInitiate();
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(5);
            networkWriter.WritePackedUInt32((uint) iCmdRestartGameInitiate);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            base.SendCommandInternal(networkWriter, 0, "CmdRestartGameInitiate");
        }
        public void CallCmdRestartGame()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdRestartGame called on server.");
                return;
            }
            if (base.isServer)
            {
                this.vmCmdRestartGame();
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(5);
            networkWriter.WritePackedUInt32((uint) iCmdRestartGame);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            base.SendCommandInternal(networkWriter, 0, "CmdRestartGame");
        }
        public void CallCmdGameStarted()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdGameStarted called on server.");
                return;
            }
            if (base.isServer)
            {
                this.vmCmdGameStarted();
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(5);
            networkWriter.WritePackedUInt32((uint) iCmdGameStarted);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            base.SendCommandInternal(networkWriter, 0, "CmdGameStarted");
        }
        public void CallCmdStopGame()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdStopGame called on server.");
                return;
            }
            if (base.isServer)
            {
                this.vmCmdStopGame();
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(5);
            networkWriter.WritePackedUInt32((uint) iCmdStopGame);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            base.SendCommandInternal(networkWriter, 0, "CmdStopGame");
        }
        public void CallCmdSyncGameTime()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdSyncGameTime called on server.");
                return;
            }
            if (base.isServer)
            {
                this.vmCmdSyncGameTime();
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(5);
            networkWriter.WritePackedUInt32((uint) iCmdSyncGameTime);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            base.SendCommandInternal(networkWriter, 0, "CmdSyncGameTime");
        }
        public void CallCmdDevelopRequestBot()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdDevelopRequestBot called on server.");
                return;
            }
            if (base.isServer)
            {
                this.vmCmdDevelopRequestBot();
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(5);
            networkWriter.WritePackedUInt32((uint) iCmdDevelopRequestBot);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            base.SendCommandInternal(networkWriter, 0, "CmdDevelopRequestBot");
        }
        public void CallCmdDevelopmentSpawnBotRequest(EPlayerSide side)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdDevelopmentSpawnBotRequest called on server.");
                return;
            }
            if (base.isServer)
            {
                this.vmCmdDevelopmentSpawnBotRequest(side);
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(5);
            networkWriter.WritePackedUInt32((uint) iCmdDevelopmentSpawnBotRequest);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            networkWriter.Write((int) side);
            base.SendCommandInternal(networkWriter, 0, "CmdDevelopmentSpawnBotRequest");
        }
        public void CallCmdDevelopmentSpawnBotOnServer(EPlayerSide side)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdDevelopmentSpawnBotOnServer called on server.");
                return;
            }
            if (base.isServer)
            {
                this.vmCmdDevelopmentSpawnBotOnServer(side);
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(5);
            networkWriter.WritePackedUInt32((uint) iCmdDevelopmentSpawnBotOnServer);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            networkWriter.Write((int) side);
            base.SendCommandInternal(networkWriter, 0, "CmdDevelopmentSpawnBotOnServer");
        }
        public void CallCmdDevelopmentSpawnBotOnClient(EPlayerSide side, int instanceId)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdDevelopmentSpawnBotOnClient called on server.");
                return;
            }
            if (base.isServer)
            {
                this.vmCmdDevelopmentSpawnBotOnClient(side, instanceId);
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(5);
            networkWriter.WritePackedUInt32((uint) iCmdDevelopmentSpawnBotOnClient);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            networkWriter.Write((int) side);
            networkWriter.WritePackedUInt32((uint) instanceId);
            base.SendCommandInternal(networkWriter, 0, "CmdDevelopmentSpawnBotOnClient");
        }
        public void CallCmdDisconnectAcceptedOnClient()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdDisconnectAcceptedOnClient called on server.");
                return;
            }
            if (base.isServer)
            {
                this.vmCmdDisconnectAcceptedOnClient();
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(5);
            networkWriter.WritePackedUInt32((uint) iCmdDisconnectAcceptedOnClient);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            base.SendCommandInternal(networkWriter, 0, "CmdDisconnectAcceptedOnClient");
        }
        public void CallCmdSpawnConfirm(int playerId)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdSpawnConfirm called on server.");
                return;
            }
            if (base.isServer)
            {
                this.vmCmdSpawnConfirm(playerId);
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(5);
            networkWriter.WritePackedUInt32((uint) iCmdSpawnConfirm);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            networkWriter.WritePackedUInt32((uint) playerId);
            base.SendCommandInternal(networkWriter, 0, "CmdSpawnConfirm");
        }
        public void CallRpcGameSpawned()
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcGameSpawned called on client.");
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(2);
            networkWriter.WritePackedUInt32((uint) iRpcGameSpawned);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            this.SendRPCInternal(networkWriter, 0, "RpcGameSpawned");
        }
        public void CallRpcGameMatching(ushort activitiesCounter, ushort minCounter, int seconds)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcGameMatching called on client.");
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(2);
            networkWriter.WritePackedUInt32((uint) iRpcGameMatching);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            networkWriter.WritePackedUInt32((uint) activitiesCounter);
            networkWriter.WritePackedUInt32((uint) minCounter);
            networkWriter.WritePackedUInt32((uint) seconds);
            this.SendRPCInternal(networkWriter, 0, "RpcGameMatching");
        }
        public void CallRpcGameStarting(int seconds)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcGameStarting called on client.");
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(2);
            networkWriter.WritePackedUInt32((uint) iRpcGameStarting);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            networkWriter.WritePackedUInt32((uint) seconds);
            this.SendRPCInternal(networkWriter, 0, "RpcGameStarting");
        }
        public void CallRpcGameStartingWithTeleport(Vector3 position, int exfiltrationId, string spawnAreaName, string entryPoint)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcGameStartingWithTeleport called on client.");
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(2);
            networkWriter.WritePackedUInt32((uint) iRpcGameStartingWithTeleport);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            networkWriter.Write(position);
            networkWriter.WritePackedUInt32((uint) exfiltrationId);
            networkWriter.Write(spawnAreaName);
            networkWriter.Write(entryPoint);
            this.SendRPCInternal(networkWriter, 0, "RpcGameStartingWithTeleport");
        }
        public void CallRpcGameStarted(float pastTime, int escapeSeconds)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcGameStarted called on client.");
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(2);
            networkWriter.WritePackedUInt32((uint) iRpcGameStarted);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            networkWriter.Write(pastTime);
            networkWriter.WritePackedUInt32((uint) escapeSeconds);
            this.SendRPCInternal(networkWriter, 0, "RpcGameStarted");
        }
        public void CallRpcGameRestarting()
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcGameRestarting called on client.");
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(2);
            networkWriter.WritePackedUInt32((uint) iRpcGameRestarting);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            this.SendRPCInternal(networkWriter, 0, "RpcGameRestarting");
        }
        public void CallRpcGameRestarted()
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcGameRestarted called on client.");
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(2);
            networkWriter.WritePackedUInt32((uint) iRpcGameRestarted);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            this.SendRPCInternal(networkWriter, 0, "RpcGameRestarted");
        }
        public void CallRpcGameStopping()
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcGameStopping called on client.");
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(2);
            networkWriter.WritePackedUInt32((uint) iRpcGameStoppin);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            this.SendRPCInternal(networkWriter, 0, "RpcGameStopping");
        }
        public void CallRpcGameStopped(ExitStatus exitStatus, int playTime)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcGameStopped called on client.");
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(2);
            networkWriter.WritePackedUInt32((uint) iRpcGameStopped);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            networkWriter.Write((int) exitStatus);
            networkWriter.WritePackedUInt32((uint) playTime);
            this.SendRPCInternal(networkWriter, 0, "RpcGameStopped");
        }
        public void CallRpcSyncGameTime(long time)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcSyncGameTime called on client.");
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(2);
            networkWriter.WritePackedUInt32((uint) iRpcSyncGameTime);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            networkWriter.WritePackedUInt64((ulong) time);
            this.SendRPCInternal(networkWriter, 0, "RpcSyncGameTime");
        }
        public void CallRpcDevelopSendBotData(byte[] data)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcDevelopSendBotData called on client.");
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(2);
            networkWriter.WritePackedUInt32((uint) iRpcDevelopSendBotData);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            networkWriter.WriteBytesFull(data);
            this.SendRPCInternal(networkWriter, 0, "RpcDevelopSendBotData");
        }
        public void CallRpcDevelopmentSpawnBotResponse(EPlayerSide side, int instanceId)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcDevelopmentSpawnBotResponse called on client.");
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(2);
            networkWriter.WritePackedUInt32((uint) iRpcDevelopmentSpawnBotResponse);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            networkWriter.Write((int) side);
            networkWriter.WritePackedUInt32((uint) instanceId);
            this.SendRPCInternal(networkWriter, 0, "RpcDevelopmentSpawnBotResponse");
        }
        public void CallRpcSoftStopNotification(int escapeSeconds)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcSoftStopNotification called on client.");
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(2);
            networkWriter.WritePackedUInt32((uint) iRpcSoftStopNotification);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            networkWriter.WritePackedUInt32((uint) escapeSeconds);
            this.SendRPCInternal(networkWriter, 0, "RpcSoftStopNotification");
        }
        public void CallRpcStartDisconnectionProcedure(int disconnectionCode, string additionalInfo, string technicalMessage)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcStartDisconnectionProcedure called on client.");
                return;
            }
            NetworkWriter networkWriter = new NetworkWriter();
            networkWriter.Write(0);
            networkWriter.Write(2);
            networkWriter.WritePackedUInt32((uint) iRpcStartDisconnectionProcedure);
            networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
            networkWriter.WritePackedUInt32((uint) disconnectionCode);
            networkWriter.Write(additionalInfo);
            networkWriter.Write(technicalMessage);
            this.SendRPCInternal(networkWriter, 0, "RpcStartDisconnectionProcedure");
        }
        #endregion

        #region Inicialization
        static AbstractGameSession()
        {
            RegisterCommandDelegate(typeof(AbstractGameSession), iCmdSpawn, smCmdSpawn);
            iCmdRespawn = 740792038;
            RegisterCommandDelegate(typeof(AbstractGameSession), iCmdRespawn, smCmdRespawn);
            iCmdStartGame = -1220356686;
            RegisterCommandDelegate(typeof(AbstractGameSession), iCmdStartGame, smCmdStartGame);
            iCmdStartGameAfterTeleport = 1792897173;
            RegisterCommandDelegate(typeof(AbstractGameSession), iCmdStartGameAfterTeleport, smCmdStartGameAfterTeleport);
            iCmdRestartGameInitiate = 273195288;
            RegisterCommandDelegate(typeof(AbstractGameSession), iCmdRestartGameInitiate, smCmdRestartGameInitiate);
            iCmdRestartGame = -1501005473;
            RegisterCommandDelegate(typeof(AbstractGameSession), iCmdRestartGame, smCmdRestartGame);
            iCmdGameStarted = -40021267;
            RegisterCommandDelegate(typeof(AbstractGameSession), iCmdGameStarted, smCmdGameStarted);
            iCmdStopGame = -750099178;
            RegisterCommandDelegate(typeof(AbstractGameSession), iCmdStopGame, smCmdStopGame);
            iCmdSyncGameTime = 463608476;
            RegisterCommandDelegate(typeof(AbstractGameSession), iCmdSyncGameTime, smCmdSyncGameTime);
            iCmdDevelopRequestBot = -1035840717;
            RegisterCommandDelegate(typeof(AbstractGameSession), iCmdDevelopRequestBot, smCmdDevelopRequestBot);
            iCmdDevelopmentSpawnBotRequest = -1581543574;
            RegisterCommandDelegate(typeof(AbstractGameSession), iCmdDevelopmentSpawnBotRequest, smCmdDevelopmentSpawnBotRequest);
            iCmdDevelopmentSpawnBotOnServer = 102630535;
            RegisterCommandDelegate(typeof(AbstractGameSession), iCmdDevelopmentSpawnBotOnServer, smCmdDevelopmentSpawnBotOnServer);
            iCmdDevelopmentSpawnBotOnClient = -349255409;
            RegisterCommandDelegate(typeof(AbstractGameSession), iCmdDevelopmentSpawnBotOnClient, smCmdDevelopmentSpawnBotOnClient);
            iCmdDisconnectAcceptedOnClient = -1733636721;
            RegisterCommandDelegate(typeof(AbstractGameSession), iCmdDisconnectAcceptedOnClient, smCmdDisconnectAcceptedOnClient);
            iCmdSpawnConfirm = -1317447737;
            RegisterCommandDelegate(typeof(AbstractGameSession), iCmdSpawnConfirm, smCmdSpawnConfirm);
            iRpcGameSpawned = -1952818640;
            RegisterRpcDelegate(typeof(AbstractGameSession), iRpcGameSpawned, smRpcGameSpawned);
            iRpcGameMatching = 2117859815;
            RegisterRpcDelegate(typeof(AbstractGameSession), iRpcGameMatching, smRpcGameMatching);
            iRpcGameStarting = -1157222870;
            RegisterRpcDelegate(typeof(AbstractGameSession), iRpcGameStarting, smRpcGameStarting);
            iRpcGameStartingWithTeleport = 1572370779;
            RegisterRpcDelegate(typeof(AbstractGameSession), iRpcGameStartingWithTeleport, smRpcGameStartingWithTeleport);
            iRpcGameStarted = -1838445225;
            RegisterRpcDelegate(typeof(AbstractGameSession), iRpcGameStarted, smRpcGameStarted);
            iRpcGameRestarting = 94275293;
            RegisterRpcDelegate(typeof(AbstractGameSession), iRpcGameRestarting, smRpcGameRestarting);
            iRpcGameRestarted = -1243884988;
            RegisterRpcDelegate(typeof(AbstractGameSession), iRpcGameRestarted, smRpcGameRestarted);
            iRpcGameStoppin = -758380962;
            RegisterRpcDelegate(typeof(AbstractGameSession), iRpcGameStoppin, smRpcGameStopping);
            iRpcGameStopped = -1825579357;
            RegisterRpcDelegate(typeof(AbstractGameSession), iRpcGameStopped, smRpcGameStopped);
            iRpcSyncGameTime = 547040626;
            RegisterRpcDelegate(typeof(AbstractGameSession), iRpcSyncGameTime, smRpcSyncGameTime);
            iRpcDevelopSendBotData = 1152897188;
            RegisterRpcDelegate(typeof(AbstractGameSession), iRpcDevelopSendBotData, smRpcDevelopSendBotData);
            iRpcDevelopmentSpawnBotResponse = -1269941968;
            RegisterRpcDelegate(typeof(AbstractGameSession), iRpcDevelopmentSpawnBotResponse, smRpcDevelopmentSpawnBotResponse);
            iRpcSoftStopNotification = -435294673;
            RegisterRpcDelegate(typeof(AbstractGameSession), iRpcSoftStopNotification, smRpcSoftStopNotification);
            iRpcStartDisconnectionProcedure = 1124901489;
            RegisterRpcDelegate(typeof(AbstractGameSession), iRpcStartDisconnectionProcedure, smRpcStartDisconnectionProcedure);
            NetworkCRC.RegisterBehaviour("AbstractGameSession", 0);
        }
        #endregion
        #region Serialize Deserialize
        public override bool OnSerialize(NetworkWriter writer, bool forceAll)
        {
            bool flag = base.OnSerialize(writer, forceAll);
            return false || flag;
        }

        public override void OnDeserialize(NetworkReader reader, bool initialState)
        {
            base.OnDeserialize(reader, initialState);
        }
        #endregion
        #region Variables
        private static int iCmdSpawn = -1723132743;

        private static int iCmdRespawn;

        private static int iCmdStartGame;

        private static int iCmdStartGameAfterTeleport;

        private static int iCmdRestartGameInitiate;

        private static int iCmdRestartGame;

        private static int iCmdGameStarted;

        private static int iCmdStopGame;

        private static int iCmdSyncGameTime;

        private static int iCmdDevelopRequestBot;

        private static int iCmdDevelopmentSpawnBotRequest;

        private static int iCmdDevelopmentSpawnBotOnServer;

        private static int iCmdDevelopmentSpawnBotOnClient;

        private static int iCmdDisconnectAcceptedOnClient;

        private static int iCmdSpawnConfirm;

        private static int iRpcGameSpawned;

        private static int iRpcGameMatching;

        private static int iRpcGameStarting;

        private static int iRpcGameStartingWithTeleport;

        private static int iRpcGameStarted;

        private static int iRpcGameRestarting;

        private static int iRpcGameRestarted;

        private static int iRpcGameStoppin;

        private static int iRpcGameStopped;

        private static int iRpcSyncGameTime;

        private static int iRpcDevelopSendBotData;

        private static int iRpcDevelopmentSpawnBotResponse;

        private static int iRpcSoftStopNotification;

        private static int iRpcStartDisconnectionProcedure;

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
		#endregion
        #region Classes
        internal sealed class Class577 : MessageBase
        {
            public override void Deserialize(NetworkReader reader)
            {
                this.string_0 = reader.ReadString();
                this.bool_0 = reader.ReadBoolean();
                base.Deserialize(reader);
            }

            public override void Serialize(NetworkWriter writer)
            {
                writer.Write(this.string_0);
                writer.Write(this.bool_0);
                base.Serialize(writer);
            }

            internal string string_0;

            internal bool bool_0;
        }

        internal class Class578 : MessageBase
        {
            public override void Deserialize(NetworkReader reader)
            {
                this.TracerId = reader.ReadByte();
                byte b = reader.ReadByte();
                if (b > 0)
                {
                    this.Context = new string[(int) b];
                    for (int i = 0; i < (int) b; i++)
                    {
                        this.Context[i] = reader.ReadString();
                    }
                }
            }

            public override void Serialize(NetworkWriter writer)
            {
                writer.Write(this.TracerId);
                string[] context = this.Context;
                int num = (context != null) ? context.Length : 0;
                writer.Write(num);
                for (int i = 0; i < num; i++)
                {
                    writer.Write(this.Context[i] ?? "");
                }
            }

            public byte TracerId;

            public string[] Context;
        }

        internal sealed class Class579 : AbstractGameSession.Class578
        {
            public override void Deserialize(NetworkReader reader)
            {
                this.Message = reader.ReadString();
                base.Deserialize(reader);
            }

            public override void Serialize(NetworkWriter writer)
            {
                writer.Write(this.Message);
                base.Serialize(writer);
            }

            public string Message;
        }

        internal sealed class Class580 : AbstractGameSession.Class578
        {
            public override void Deserialize(NetworkReader reader)
            {
                this.Code = (ETraceCode) reader.ReadByte();
                base.Deserialize(reader);
            }

            public override void Serialize(NetworkWriter writer)
            {
                writer.Write((byte) this.Code);
                base.Serialize(writer);
            }

            public ETraceCode Code;
        }

        internal sealed class Class581 : MessageBase
        {
            public override void Deserialize(NetworkReader reader)
            {
                this.byte_0 = reader.ReadByte();
                this.gclass878_0 = GClass878.Deserialize(reader);
                this.byte_1 = reader.ReadBytesAndSize();
                this.byte_2 = reader.ReadBytesAndSize();
                this.bool_0 = reader.ReadBoolean();
                this.ememberCategory_0 = (EMemberCategory) reader.ReadInt32();
                this.float_0 = reader.ReadSingle();
                this.byte_3 = reader.ReadBytesAndSize();
                this.byte_4 = reader.ReadBytesAndSize();
                Vector3 min = reader.ReadVector3();
                Vector3 max = reader.ReadVector3();
                this.bounds_0 = new Bounds
                {
                    min = min,
                    max = max
                };
                this.ushort_0 = reader.ReadUInt16();
                this.enetLogsLevel_0 = (ENetLogsLevel) reader.ReadByte();
                base.Deserialize(reader);
            }

            public override void Serialize(NetworkWriter writer)
            {
                writer.Write(this.byte_0);
                this.gclass878_0.Serialize(writer, true);
                writer.WriteBytesFull(this.byte_1);
                writer.WriteBytesFull(this.byte_2);
                writer.Write(this.bool_0);
                writer.Write((int) this.ememberCategory_0);
                writer.Write(this.float_0);
                writer.WriteBytesFull(this.byte_3);
                writer.WriteBytesFull(this.byte_4);
                writer.Write(this.bounds_0.min);
                writer.Write(this.bounds_0.max);
                writer.Write(this.ushort_0);
                writer.Write((byte) this.enetLogsLevel_0);
                base.Serialize(writer);
            }

            internal byte byte_0;

            internal GClass878 gclass878_0;

            internal byte[] byte_1;

            internal byte[] byte_2;

            internal bool bool_0;

            internal EMemberCategory ememberCategory_0;

            internal float float_0;

            internal byte[] byte_3;

            internal byte[] byte_4;

            internal Bounds bounds_0;

            internal ushort ushort_0;

            internal ENetLogsLevel enetLogsLevel_0;
        }

        internal sealed class Class582 : MessageBase
        {
            public override void Deserialize(NetworkReader reader)
            {
                this.float_0 = reader.ReadSingle();
                base.Deserialize(reader);
            }

            public override void Serialize(NetworkWriter writer)
            {
                writer.Write(this.float_0);
                base.Serialize(writer);
            }

            internal float float_0;
        }

        internal sealed class Class583 : MessageBase
        {
            public override void Serialize(NetworkWriter writer)
            {
                writer.WriteBytesFull(this.byte_0);
                base.Serialize(writer);
            }

            public override void Deserialize(NetworkReader reader)
            {
                this.byte_0 = reader.ReadBytesAndSize();
                base.Deserialize(reader);
            }

            internal byte[] byte_0;
        }

        internal sealed class Class584 : MessageBase
        {
            public override void Deserialize(NetworkReader reader)
            {
                this.int_0 = reader.ReadInt32();
                this.byte_0 = reader.ReadBytesAndSize();
                base.Deserialize(reader);
            }

            public override void Serialize(NetworkWriter writer)
            {
                writer.Write(this.int_0);
                writer.WriteBytesAndSize(this.byte_0, this.byte_0.Length);
                base.Serialize(writer);
            }

            internal int int_0;

            internal byte[] byte_0;
        }

        internal sealed class Class585 : MessageBase
        {
            public override void Deserialize(NetworkReader reader)
            {
                this.string_0 = reader.ReadString();
                this.int_0 = reader.ReadInt32();
                this.float_0 = reader.ReadSingle();
                base.Deserialize(reader);
            }

            public override void Serialize(NetworkWriter writer)
            {
                writer.Write(this.string_0);
                writer.Write(this.int_0);
                writer.Write(this.float_0);
                base.Serialize(writer);
            }

            internal string string_0;

            internal int int_0;

            internal float float_0;
        }
        #endregion
    }
}
