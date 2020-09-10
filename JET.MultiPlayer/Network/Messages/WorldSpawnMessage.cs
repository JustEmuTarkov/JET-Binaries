using Comfort.Common;
using EFT;
using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Network.Messages
{
    public class WorldSpawnMessage : MessageBase
    {
        public override void Serialize(NetworkWriter writer)
        {
            var gameWorld = Singleton<GameWorld>.Instance;
            gameWorld.world_0.OnSerialize(writer);
        }

        public const short MessageId = 151;
    }
}
