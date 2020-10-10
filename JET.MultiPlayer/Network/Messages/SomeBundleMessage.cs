using System;
using ComponentAce.Compression.Libs.zlib;
using EFT;
using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Network.Messages
{
    public class SomeBundleMessage : MessageBase
    {
        public override void Deserialize(NetworkReader reader)
        {
            Id = reader.ReadInt32();
            LootItems = SimpleZlib.Decompress(reader.ReadBytesAndSize()).ParseJsonTo<ResourceKey[]>();
            bytes = reader.ReadBytesAndSize();
            base.Deserialize(reader);
        }

        public override void Serialize(NetworkWriter writer)
        {
            writer.Write(Id);
            byte[] bundlesBytes = SimpleZlib.CompressToBytes(LootItems.ToJson(), 9);
            writer.WriteBytesAndSize(bundlesBytes, bundlesBytes.Length);
            writer.WriteBytesAndSize(bytes, bytes.Length);
            base.Serialize(writer);
        }

        public static SomeBundleMessage GetSomeBundleMessage(string base64 = null)
        {
            byte[] data;
            if (base64 == null)
            {
                data = Convert.FromBase64String(
                    "AAAAAFUBeNqtk91OAyEQhV+l2Wt1rY3G+irGTKYwXUj5k5+qa3x3obtra1K9oHvDHAJ8Mzk5PH82DqNonhoMgWJomTWRTGzfCJ01oU0BN4pARtKhLSvILe6OCpS18WaTDFfUXDWeSZ5hzdfVxeDyAKUhPw89OCXzhRM94+hn4FXjD0hteV6wwz6/PyjQ7h7EDkLkwJLfE4f1+3INq9tZ6M5dr5fQ635UpU3mPzpd1YFek3S67AUpTdnoIPrBnJ8jODmaoYN1gVlPINDwLhnYIiMIQpLifzT+50Wlp06GaNWi89JNm6J/uXuw1ikMUbKa6OUcjLrEoaTC4P5jNaThgsih1rbNF3xpMtSBiQ4eVpWGMIG+o6lOGS4lu1DJ9MQk7ckf1cnfqP0Mqe8VjWXCqdQBcnQxdyhOVLJzCthuLBMa7y4YNshOxLDwhH7QUOSE5j7pM+CXb+erKQw=");
            }
            else
            {
                data = Convert.FromBase64String(base64);
            }

            var reader = new NetworkReader(data);
            return reader.ReadMessage<SomeBundleMessage>();
        }

        internal int Id;
        internal ResourceKey[] LootItems;
        internal byte[] bytes;
        public const short MessageId = 188;
        public const int BundlesQueue = 5000;
    }
}
