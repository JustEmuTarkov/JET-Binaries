using System;
using System.Collections.Generic;
using System.Linq;
using Comfort.Common;
using ComponentAce.Compression.Libs.zlib;
using EFT;
using EFT.Interactive;
using EFT.Weather;
using Newtonsoft.Json;
using ServerLib.Network.World;
using ServerLib.Utils.Game;
using UnityEngine;
using UnityEngine.Networking;
using DateTimeClass = GClass878;
using WeatherClass = GClass1210;
#pragma warning disable 618

namespace ServerLib.Network.Messages
{
    public class AuthResponseMessage : MessageBase
    {
        public override void Deserialize(NetworkReader reader)
        {
            Byte0 = reader.ReadByte();
            GameDateTime = DateTimeClass.Deserialize(reader);

            Prefabs = SimpleZlib
                .Decompress(reader.ReadBytesAndSize())
                .ParseJsonTo<ResourceKey[]>(Array.Empty<JsonConverter>());

            Weather = SimpleZlib
                .Decompress(reader.ReadBytesAndSize())
                .ParseJsonTo<WeatherClass[]>();

            IsCanRestart = reader.ReadBoolean();
            MemberCategory = (EMemberCategory) reader.ReadInt32();
            FixedDeltaTime = reader.ReadSingle();

            Interactables = SimpleZlib
                .Decompress(reader.ReadBytesAndSize())
                .ParseJsonTo<Dictionary<string, int>>();

            SessionId = reader.ReadBytesAndSize();
            var min = reader.ReadVector3();
            var max = reader.ReadVector3();

            LevelBounds = new Bounds {min = min, max = max};
            SaberServerAntiCheatPort = reader.ReadUInt16();

            //NetLogsLevel = (ENetLogsLevel) reader.ReadByte();
            NetLogsLevel = ENetLogsLevel.Normal;
            base.Deserialize(reader);
        }

        public static AuthResponseMessage GetAuthResponseMessage()
        {
            string base64 =
                "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";

            byte[] data = Convert.FromBase64String(base64);
            var reader = new NetworkReader(data);
            return reader.ReadMessage<AuthResponseMessage>();
        }

        public override void Serialize(NetworkWriter writer)
        {
            var gameWorld = Singleton<GameWorld>.Instance;
            var game = LocalGameUtils.Get();

            writer.Write(Byte0);
            game.GameDateTime.Serialize(writer, false); //GameDateTime.Serialize(writer, true);

            var prefabs = gameWorld.GetAllLootPrefabs().ToList();
            prefabs.Add(new ResourceKey
            {
                path = "assets/content/items/quest/item_quest_letter/item_quest_letter.bundle", rcid = ""
            });

            writer.WriteBytesFull(SimpleZlib.CompressToBytes(prefabs.ToJson(), 9));

            var weather = Singleton<ServerInstance>.Instance.weatherNodes;
            writer.WriteBytesFull(SimpleZlib.CompressToBytes(weather.ToJson(), 9));

            writer.Write(IsCanRestart);
            writer.Write((int) MemberCategory);

            // MainApplication
            // Token: 0x04005617 RID: 22039
            // [SerializeField]
            // float _fixedDeltaTime = 0.0166666675f;
            writer.Write(0.0166666675f);

            var allInteractiveObjects = LocationScene.GetAllObjects<WorldInteractiveObject>().ToArray();
            var networkInteractiveObjects = allInteractiveObjects.ToDictionary(
                interactiveObject => interactiveObject.Id,
                interactiveObject => interactiveObject.NetId
            );

            writer.WriteBytesFull(SimpleZlib.CompressToBytes(networkInteractiveObjects.ToJson(), 9));
            writer.WriteBytesFull(SessionId);
            writer.Write(LevelBounds.min);
            writer.Write(LevelBounds.max);
            writer.Write(SaberServerAntiCheatPort);
            //writer.Write((byte) NetLogsLevel);
            writer.Write((byte) ENetLogsLevel.Normal);
            base.Serialize(writer);
        }

        /// <summary>Uses case not found</summary>
        public Dictionary<string, int> Interactables;

        public ushort SaberServerAntiCheatPort;
        public ENetLogsLevel NetLogsLevel;
        public const short MessageId = 147;

        internal byte Byte0;
        internal DateTimeClass GameDateTime;
        internal ResourceKey[] Prefabs;
        internal WeatherClass[] Weather;
        internal bool IsCanRestart;
        internal EMemberCategory MemberCategory;
        internal float FixedDeltaTime;
        internal byte[] SessionId;
        internal Bounds LevelBounds;
    }
}
