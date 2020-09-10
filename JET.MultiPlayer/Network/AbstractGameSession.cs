using System;
using EFT;
using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Network
{
    // Token: 0x02000AC3 RID: 2755
    public class AbstractGameSession : AbstractSession
    {
        public static T Create<T>(Transform parent, string name, string profileId) where T : AbstractGameSession
        {
            var t = Instantiate<T>(parent, name, profileId);
            t.GetOrAddComponent<NetworkIdentity>().localPlayerAuthority = true;
            return t;
        }

        // Token: 0x06003E45 RID: 15941 RVA: 0x000B7B8C File Offset: 0x000B5D8C
        public override float GetNetworkSendInterval()
        {
            return 0f;
        }

        // Token: 0x06003E46 RID: 15942 RVA: 0x000C6FDC File Offset: 0x000C51DC
        public override int GetNetworkChannel()
        {
            return 0;
        }

        // Token: 0x06003E47 RID: 15943 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void CmdSpawn()
        {
            Console.WriteLine("Call command CmdSpawn from client.");

            sessionIsSpawned = true;
        }

        // Token: 0x06003E48 RID: 15944 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void CmdReSpawn()
        {
            Console.WriteLine("Call command from client.");
        }

        // Token: 0x06003E49 RID: 15945 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void CmdStartGame()
        {
            Console.WriteLine("Call command CmdStartGame from client.");
        }

        // Token: 0x06003E4A RID: 15946 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void CmdStartGameAfterTeleport()
        {
            Console.WriteLine("Call command from client.");
        }

        // Token: 0x06003E4B RID: 15947 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void CmdRestartGameInitiate()
        {
            Console.WriteLine("Call command from client.");
        }

        // Token: 0x06003E4C RID: 15948 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void CmdRestartGame()
        {
            Console.WriteLine("Call command from client.");
        }

        // Token: 0x06003E4D RID: 15949 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void CmdStopGame()
        {
            Console.WriteLine("Call CmdStopGame command from client.");
            RpcSoftStopNotification(5);
            RpcGameStopped();
        }

        // Token: 0x06003E4E RID: 15950 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void CmdSyncGameTime()
        {
            Console.WriteLine("Call command CmdSyncGameTime from client.");
            RpcSyncGameTime(DateTime.UtcNow.ToBinary());
            gameSyncTimeIsSent = true;
        }

        // Token: 0x06003E4F RID: 15951 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void CmdDevelopRequestBot()
        {
            Console.WriteLine("Call command from client.");
        }

        // Token: 0x06003E50 RID: 15952 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void CmdDevelopmentSpawnBotRequest(EPlayerSide side)
        {
            Console.WriteLine("Call command from client.");
        }

        // Token: 0x06003E51 RID: 15953 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void CmdDevelopmentSpawnBotOnServer(EPlayerSide side)
        {
            Console.WriteLine("Call command from client.");
        }

        // Token: 0x06003E52 RID: 15954 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void CmdDevelopmentSpawnBotOnClient(EPlayerSide side, int instanceId)
        {
            Console.WriteLine("Call command from client.");
        }

        // Token: 0x06003E53 RID: 15955 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void CmdDisconnectAcceptedOnClient()
        {
            Console.WriteLine("Call command from client.");
        }

        // Token: 0x06003E54 RID: 15956 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void CmdSpawnConfirm(int playerId)
        {
        }

        // Token: 0x06003E55 RID: 15957 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void vmethod_14()
        {
            Console.WriteLine("Call command RpcGameSpawned from client.");
        }

        // Token: 0x06003E56 RID: 15958 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void vmethod_15(ushort activitiesCounter, ushort minCounter, int seconds)
        {
            Console.WriteLine("Call RpcGameMatching command from client.");
        }

        // Token: 0x06003E57 RID: 15959 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void vmethod_16(int seconds)
        {
            Console.WriteLine("Call command from client.");
        }

        // Token: 0x06003E58 RID: 15960 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void vmethod_17(Vector3 position, int exfiltrationId, string spawnAreaName, string entryPoint)
        {
            Console.WriteLine("Call command from client.");
        }

        // Token: 0x06003E59 RID: 15961 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void vmethod_18(float pastTime, int escapeSeconds)
        {
            Console.WriteLine("Call command RpcGameStarted from client.");
        }

        // Token: 0x06003E5A RID: 15962 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void vmethod_19()
        {
            Console.WriteLine("Call command from client.");
        }

        // Token: 0x06003E5B RID: 15963 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void vmethod_20()
        {
            Console.WriteLine("Call command from client.");
        }

        // Token: 0x06003E5C RID: 15964 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void vmethod_21(ExitStatus exitStatus, int playTime)
        {
            Console.WriteLine("Call command from client.");
        }

        // Token: 0x06003E5D RID: 15965 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void vmethod_22()
        {
            Console.WriteLine("Call command from client.");
        }

        // Token: 0x06003E5E RID: 15966 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void vmethod_23(long time)
        {
            Console.WriteLine("Call command from client.");
        }

        // Token: 0x06003E5F RID: 15967 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void vmethod_24(byte[] data)
        {
            Console.WriteLine("Call command from client.");
        }

        // Token: 0x06003E60 RID: 15968 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void vmethod_25(EPlayerSide side, int instanceId)
        {
            Console.WriteLine("Call command from client.");
        }

        // Token: 0x06003E61 RID: 15969 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void vmethod_26(int escapeSeconds)
        {
            Console.WriteLine("Call command from client.");
        }

        // Token: 0x06003E62 RID: 15970 RVA: 0x000A20B4 File Offset: 0x000A02B4
        protected virtual void vmethod_27(int disconnectionCode, string additionalInfo, string technicalMessage)
        {
            Console.WriteLine("Call command from client.");
            RpcStartDisconnectionProcedure(disconnectionCode, additionalInfo, technicalMessage);
        }

        // Token: 0x06003E64 RID: 15972 RVA: 0x000A20B4 File Offset: 0x000A02B4
        private void method_1()
        {
            Console.WriteLine("Call command from client.");
        }

        // Token: 0x06003E65 RID: 15973 RVA: 0x000C6FEB File Offset: 0x000C51EB
        protected static void CmdSpawn(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdSpawn called on client.");
                return;
            }

            ((AbstractGameSession) obj).CmdSpawn();
        }

        // Token: 0x06003E66 RID: 15974 RVA: 0x000C700A File Offset: 0x000C520A
        protected static void CmdRespawn(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdRespawn called on client.");
                return;
            }

            ((AbstractGameSession) obj).CmdReSpawn();
        }

        // Token: 0x06003E67 RID: 15975 RVA: 0x000C7029 File Offset: 0x000C5229
        protected static void CmdStartGame(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdStartGame called on client.");
                return;
            }

            ((AbstractGameSession) obj).CmdStartGame();
        }

        // Token: 0x06003E68 RID: 15976 RVA: 0x000C7048 File Offset: 0x000C5248
        protected static void CmdStartGameAfterTeleport(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdStartGameAfterTeleport called on client.");
                return;
            }

            ((AbstractGameSession) obj).CmdStartGameAfterTeleport();
        }

        protected static void CmdRestartGameInitiate(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdRestartGameInitiate called on client.");
                return;
            }

            ((AbstractGameSession) obj).CmdRestartGameInitiate();
        }

        protected static void CmdRestartGame(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdRestartGame called on client.");
                return;
            }

            ((AbstractGameSession) obj).CmdRestartGame();
        }

        protected static void CmdStopGame(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdStopGame called on client.");
                return;
            }

            ((AbstractGameSession) obj).CmdStopGame();
        }

        protected static void CmdSyncGameTime(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdSyncGameTime called on client.");
                return;
            }

            ((AbstractGameSession) obj).CmdSyncGameTime();
        }

        protected static void CmdDevelopRequestBot(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdDevelopRequestBot called on client.");
                return;
            }

            ((AbstractGameSession) obj).CmdDevelopRequestBot();
        }

        protected static void CmdDevelopmentSpawnBotRequest(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdDevelopmentSpawnBotRequest called on client.");
                return;
            }

            ((AbstractGameSession) obj).CmdDevelopmentSpawnBotRequest((EPlayerSide) reader.ReadInt32());
        }

        protected static void CmdDevelopmentSpawnBotOnServer(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdDevelopmentSpawnBotOnServer called on client.");
                return;
            }

            ((AbstractGameSession) obj).CmdDevelopmentSpawnBotOnServer((EPlayerSide) reader.ReadInt32());
        }

        protected static void CmdDevelopmentSpawnBotOnClient(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdDevelopmentSpawnBotOnClient called on client.");
                return;
            }

            ((AbstractGameSession) obj).CmdDevelopmentSpawnBotOnClient((EPlayerSide) reader.ReadInt32(),
                (int) reader.ReadPackedUInt32());
        }

        protected static void CmdDisconnectAcceptedOnClient(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdDisconnectAcceptedOnClient called on client.");
                return;
            }

            ((AbstractGameSession) obj).CmdDisconnectAcceptedOnClient();
        }

        protected static void CmdSpawnConfirm(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdSpawnConfirm called on client.");
                return;
            }

            ((AbstractGameSession) obj).CmdSpawnConfirm((int) reader.ReadPackedUInt32());
        }

        public void CallCmdSpawn()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdSpawn called on server.");
                return;
            }

            if (isServer)
            {
                CmdSpawn();
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_0);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendCommandInternal(networkWriter, 0, "CmdSpawn");
        }

        public void CallCmdReSpawn()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdRespawn called on server.");
                return;
            }

            if (isServer)
            {
                CmdReSpawn();
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_1);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendCommandInternal(networkWriter, 0, "CmdRespawn");
        }

        public void CallCmdStartGame()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdStartGame called on server.");
                return;
            }

            if (isServer)
            {
                CmdStartGame();
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_2);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendCommandInternal(networkWriter, 0, "CmdStartGame");
        }

        public void CallCmdStartGameAfterTeleport()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdStartGameAfterTeleport called on server.");
                return;
            }

            if (isServer)
            {
                CmdStartGameAfterTeleport();
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_3);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendCommandInternal(networkWriter, 0, "CmdStartGameAfterTeleport");
        }

        public void CallCmdRestartGameInitiate()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdRestartGameInitiate called on server.");
                return;
            }

            if (isServer)
            {
                CmdRestartGameInitiate();
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_4);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendCommandInternal(networkWriter, 0, "CmdRestartGameInitiate");
        }

        public void CallCmdRestartGame()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdRestartGame called on server.");
                return;
            }

            if (isServer)
            {
                CmdRestartGame();
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_5);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendCommandInternal(networkWriter, 0, "CmdRestartGame");
        }

        public void CallCmdStopGame()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdStopGame called on server.");
                return;
            }

            if (isServer)
            {
                CmdStopGame();
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_6);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendCommandInternal(networkWriter, 0, "CmdStopGame");
        }

        public void CallCmdSyncGameTime()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdSyncGameTime called on server.");
                return;
            }

            if (isServer)
            {
                CmdSyncGameTime();
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_7);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendCommandInternal(networkWriter, 0, "CmdSyncGameTime");
        }

        public void CallCmdDevelopRequestBot()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdDevelopRequestBot called on server.");
                return;
            }

            if (isServer)
            {
                CmdDevelopRequestBot();
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_8);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendCommandInternal(networkWriter, 0, "CmdDevelopRequestBot");
        }

        public void CallCmdDevelopmentSpawnBotRequest(EPlayerSide side)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdDevelopmentSpawnBotRequest called on server.");
                return;
            }

            if (isServer)
            {
                CmdDevelopmentSpawnBotRequest(side);
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_9);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.Write((int) side);
            SendCommandInternal(networkWriter, 0, "CmdDevelopmentSpawnBotRequest");
        }

        public void CallCmdDevelopmentSpawnBotOnServer(EPlayerSide side)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdDevelopmentSpawnBotOnServer called on server.");
                return;
            }

            if (isServer)
            {
                CmdDevelopmentSpawnBotOnServer(side);
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_10);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.Write((int) side);
            SendCommandInternal(networkWriter, 0, "CmdDevelopmentSpawnBotOnServer");
        }

        public void CallCmdDevelopmentSpawnBotOnClient(EPlayerSide side, int instanceId)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdDevelopmentSpawnBotOnClient called on server.");
                return;
            }

            if (isServer)
            {
                CmdDevelopmentSpawnBotOnClient(side, instanceId);
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_11);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.Write((int) side);
            networkWriter.WritePackedUInt32((uint) instanceId);
            SendCommandInternal(networkWriter, 0, "CmdDevelopmentSpawnBotOnClient");
        }

        public void CallCmdDisconnectAcceptedOnClient()
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdDisconnectAcceptedOnClient called on server.");
                return;
            }

            if (isServer)
            {
                CmdDisconnectAcceptedOnClient();
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_12);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendCommandInternal(networkWriter, 0, "CmdDisconnectAcceptedOnClient");
        }

        public void CallCmdSpawnConfirm(int playerId)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("Command function CmdSpawnConfirm called on server.");
                return;
            }

            if (isServer)
            {
                CmdSpawnConfirm(playerId);
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 5);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_13);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.WritePackedUInt32((uint) playerId);
            SendCommandInternal(networkWriter, 0, "CmdSpawnConfirm");
        }

        protected static void smethod_16(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcGameSpawned called on server.");
                return;
            }

            ((AbstractGameSession) obj).vmethod_14();
        }

        protected static void smethod_17(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcGameMatching called on server.");
                return;
            }

            ((AbstractGameSession) obj).vmethod_15((ushort) reader.ReadPackedUInt32(),
                (ushort) reader.ReadPackedUInt32(), (int) reader.ReadPackedUInt32());
        }

        protected static void smethod_18(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcGameStarting called on server.");
                return;
            }

            ((AbstractGameSession) obj).vmethod_16((int) reader.ReadPackedUInt32());
        }

        protected static void smethod_19(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcGameStartingWithTeleport called on server.");
                return;
            }

            ((AbstractGameSession) obj).vmethod_17(reader.ReadVector3(), (int) reader.ReadPackedUInt32(),
                reader.ReadString(), reader.ReadString());
        }

        protected static void smethod_20(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcGameStarted called on server.");
                return;
            }

            ((AbstractGameSession) obj).vmethod_18(reader.ReadSingle(), (int) reader.ReadPackedUInt32());
        }

        protected static void smethod_21(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcGameRestarting called on server.");
                return;
            }

            ((AbstractGameSession) obj).vmethod_19();
        }

        protected static void smethod_22(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcGameRestarted called on server.");
                return;
            }

            ((AbstractGameSession) obj).vmethod_20();
        }

        protected static void smethod_23(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcGameStopping called on server.");
                return;
            }

            ((AbstractGameSession) obj).vmethod_21((ExitStatus) reader.ReadInt32(), (int) reader.ReadPackedUInt32());
        }

        protected static void smethod_24(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcGameStopped called on server.");
                return;
            }

            ((AbstractGameSession) obj).vmethod_22();
        }

        protected static void smethod_25(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcSyncGameTime called on server.");
                return;
            }

            ((AbstractGameSession) obj).vmethod_23((long) reader.ReadPackedUInt64());
        }

        protected static void smethod_26(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcDevelopSendBotData called on server.");
                return;
            }

            ((AbstractGameSession) obj).vmethod_24(reader.ReadBytesAndSize());
        }

        protected static void smethod_27(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcDevelopmentSpawnBotResponse called on server.");
                return;
            }

            ((AbstractGameSession) obj).vmethod_25((EPlayerSide) reader.ReadInt32(), (int) reader.ReadPackedUInt32());
        }

        protected static void smethod_28(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcSoftStopNotification called on server.");
                return;
            }

            ((AbstractGameSession) obj).vmethod_26((int) reader.ReadPackedUInt32());
        }

        protected static void smethod_29(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkClient.active)
            {
                Console.WriteLine("RPC RpcStartDisconnectionProcedure called on server.");
                return;
            }

            ((AbstractGameSession) obj).vmethod_27((int) reader.ReadPackedUInt32(), reader.ReadString(),
                reader.ReadString());
        }

        public void RpcGameSpawned()
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcGameSpawned called on client.");
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_14);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);

            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcGameSpawned");
        }

        public void RpcGameMatching(ushort activitiesCounter, ushort minCounter, int seconds)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcGameMatching called on client.");
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_15);
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
                Console.WriteLine("RPC Function RpcGameStarting called on client.");
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_16);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.WritePackedUInt32((uint) seconds);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcGameStarting");
        }

        public void RpcGameStartingWithTeleport(Vector3 position, int exfiltrationId, string spawnAreaName,
            string entryPoint)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcGameStartingWithTeleport called on client.");
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_17);
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
                Console.WriteLine("RPC Function RpcGameStarted called on client.");
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_18);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.Write(pastTime);
            networkWriter.WritePackedUInt32((uint) escapeSeconds);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcGameStarted");
        }

        public void RpcGameRestarting()
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcGameRestarting called on client.");
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_19);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcGameRestarting");
        }

        public void RpcGameRestarted()
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcGameRestarted called on client.");
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_20);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcGameRestarted");
        }

        public void RpcGameStopping(ExitStatus exitStatus, int playTime)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcGameStopping called on client.");
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_21);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.Write((int) exitStatus);
            networkWriter.WritePackedUInt32((uint) playTime);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcGameStopping");
        }

        public void RpcGameStopped()
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcGameStopped called on client.");
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_22);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcGameStopped");
        }

        public void RpcSyncGameTime(long time)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcSyncGameTime called on client.");
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_23);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.WritePackedUInt64((ulong) time);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcSyncGameTime");
        }

        public void RpcDevelopSendBotData(byte[] data)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcDevelopSendBotData called on client.");
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_24);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.WriteBytesFull(data);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcDevelopSendBotData");
        }

        public void RpcDevelopmentSpawnBotResponse(EPlayerSide side, int instanceId)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcDevelopmentSpawnBotResponse called on client.");
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_25);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.Write((int) side);
            networkWriter.WritePackedUInt32((uint) instanceId);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcDevelopmentSpawnBotResponse");
        }

        // Token: 0x06003E9B RID: 16027 RVA: 0x0021EF98 File Offset: 0x0021D198
        public void RpcSoftStopNotification(int escapeSeconds)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcSoftStopNotification called on client.");
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_26);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.WritePackedUInt32((uint) escapeSeconds);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcSoftStopNotification");
        }

        // Token: 0x06003E9C RID: 16028 RVA: 0x0021EFFC File Offset: 0x0021D1FC
        public void RpcStartDisconnectionProcedure(int disconnectionCode, string additionalInfo,
            string technicalMessage)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("RPC Function RpcStartDisconnectionProcedure called on client.");
                return;
            }

            var networkWriter = new NetworkWriter();
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 0);
            networkWriter.Write((byte) 2);
            networkWriter.Write((byte) 0);
            networkWriter.WritePackedUInt32((uint) int_27);
            networkWriter.Write(GetComponent<NetworkIdentity>().netId);
            networkWriter.WritePackedUInt32((uint) disconnectionCode);
            networkWriter.Write(additionalInfo);
            networkWriter.Write(technicalMessage);
            SendTargetRPCInternal(connection, networkWriter, chanelId, "RpcStartDisconnectionProcedure");
        }

        protected static void smethod_8(NetworkBehaviour obj, NetworkReader reader)
        {
            if (!NetworkServer.active)
            {
                Console.WriteLine("Command CmdGameStarted called on client.");
                return;
            }

            Console.WriteLine("Command CmdGameStarted called from client.");
        }

        // Token: 0x06003E9D RID: 16029 RVA: 0x0021F070 File Offset: 0x0021D270
        static AbstractGameSession()
        {
            RegisterCommandDelegate(typeof(AbstractGameSession), int_0, CmdSpawn);
            int_1 = 740792038;
            RegisterCommandDelegate(typeof(AbstractGameSession), int_1, CmdRespawn);
            int_2 = -1220356686; //FB B2 D5 42 B7‬ ‭1081037111991‬
            RegisterCommandDelegate(typeof(AbstractGameSession), int_2, CmdStartGame);
            int_3 = 1792897173;
            RegisterCommandDelegate(typeof(AbstractGameSession), int_3, CmdStartGameAfterTeleport);
            int_4 = 273195288;
            RegisterCommandDelegate(typeof(AbstractGameSession), int_4, CmdRestartGameInitiate);
            int_5 = -1501005473; // FB 5F 79 88 A6‬ ‭1079638591654‬
            RegisterCommandDelegate(typeof(AbstractGameSession), int_5, CmdRestartGame);
            int_6 = -750099178; // FB 16 65 4A D3 ‬‭1078412528339‬
            RegisterCommandDelegate(typeof(AbstractGameSession), int_6, CmdStopGame);
            int_7 = 463608476;
            RegisterCommandDelegate(typeof(AbstractGameSession), int_7, CmdSyncGameTime);
            int_8 = -1035840717; // FB 33 53 42 C2 ‬‭1078897885890‬
            RegisterCommandDelegate(typeof(AbstractGameSession), int_8, CmdDevelopRequestBot);
            int_9 = -1581543574; // FB 6A 8F BB A1 ‬‭1079824595873‬
            RegisterCommandDelegate(typeof(AbstractGameSession), int_9, CmdDevelopmentSpawnBotRequest);
            int_10 = 102630535;
            RegisterCommandDelegate(typeof(AbstractGameSession), int_10, CmdDevelopmentSpawnBotOnServer);
            int_11 = -349255409; // FB 0F C9 2E EB‬ ‭1078301634283‬
            RegisterCommandDelegate(typeof(AbstractGameSession), int_11, CmdDevelopmentSpawnBotOnClient);
            int_12 = -1733636721; // FB 8F CD AA 98 ‬‭1080449411736‬
            RegisterCommandDelegate(typeof(AbstractGameSession), int_12, CmdDisconnectAcceptedOnClient);
            int_13 = -1317447737; // FB C7 57 79 B1‬ ‭1081381190065‬
            RegisterCommandDelegate(typeof(AbstractGameSession), int_13, CmdSpawnConfirm);
            int_14 = -1952818640; // FB 30 5A 9A 8B ‬‭1078848035467‬
            RegisterRpcDelegate(typeof(AbstractGameSession), int_14, smethod_16);
            int_15 = 2117859815;
            RegisterRpcDelegate(typeof(AbstractGameSession), int_15, smethod_17);
            int_16 = -1157222870; // FB 2A 2E 06 BB ‭1078744450747‬
            RegisterRpcDelegate(typeof(AbstractGameSession), int_16, smethod_18);
            int_17 = 1572370779;
            RegisterRpcDelegate(typeof(AbstractGameSession), int_17, smethod_19);
            int_18 = -1838445225; // FB 57 8D 6B 92 ‬‭1079505677202‬
            RegisterRpcDelegate(typeof(AbstractGameSession), int_18, smethod_20);
            int_19 = 94275293;
            RegisterRpcDelegate(typeof(AbstractGameSession), int_19, smethod_21);
            int_20 = -1243884988; // FB 44 D2 DB B5 ‬‭1079191460789‬
            RegisterRpcDelegate(typeof(AbstractGameSession), int_20, smethod_22);
            int_21 = -758380962; // FB 5E 06 CC D2‬ ‭1079614295250‬
            RegisterRpcDelegate(typeof(AbstractGameSession), int_21, smethod_23);
            int_22 = -1825579357; // FB A3 DE 2F 93 ‬‭1080786038675‬
            RegisterRpcDelegate(typeof(AbstractGameSession), int_22, smethod_24);
            int_23 = 547040626;
            RegisterRpcDelegate(typeof(AbstractGameSession), int_23, smethod_25);
            int_24 = 1152897188;
            RegisterRpcDelegate(typeof(AbstractGameSession), int_24, smethod_26);
            int_25 = -1269941968; // FB 30 39 4E B4‬ ‭1078845853364‬
            RegisterRpcDelegate(typeof(AbstractGameSession), int_25, smethod_27);
            int_26 = -435294673; // FB 2F EE 0D E6‬ ‭1078840921574‬
            RegisterRpcDelegate(typeof(AbstractGameSession), int_26, smethod_28);
            int_27 = 1124901489;
            RegisterRpcDelegate(typeof(AbstractGameSession), int_27, smethod_29);
            int_28 = -40021267;
            RegisterCommandDelegate(typeof(AbstractGameSession), int_28, smethod_8);
            NetworkCRC.RegisterBehaviour("AbstractGameSession", 0);
        }

        public override bool OnSerialize(NetworkWriter writer, bool forceAll)
        {
            var flag = base.OnSerialize(writer, forceAll);
            return false || flag;
        }

        private static int int_0 = -1723132743;
        private static int int_1;
        private static int int_2;
        private static int int_3;
        private static int int_4;
        private static int int_5;
        private static int int_6;
        private static int int_7;
        private static int int_8;
        private static int int_9;
        private static int int_10;
        private static int int_11;
        private static int int_12;
        private static int int_13;
        private static int int_14;
        private static int int_15;
        private static int int_16;
        private static int int_17;
        private static int int_18;
        private static int int_19;
        private static int int_20;
        private static int int_21;
        private static int int_22;
        private static int int_23;
        private static int int_24;
        private static int int_25;
        private static int int_26;
        private static int int_27;
        private static int int_28;

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
