using System.Collections.Generic;
using System.Linq;
using EFT.Interactive;
using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Network.Utils
{
    public static class Serializer
    {
        public static void SerializeWorldObjectsState(NetworkWriter writer)
        {
            var worldInteractiveObjects = LocationScene.GetAllObjects<WorldInteractiveObject>().ToArray();
            var list0 = new List<WorldInteractiveObject.GStruct197>(20);

            list0.Clear();

            foreach (WorldInteractiveObject worldInteractiveObject in worldInteractiveObjects)
            {
                if (worldInteractiveObject.DoorState != worldInteractiveObject.InitialDoorState &&
                    worldInteractiveObject.DoorState != EDoorState.Interacting ||
                    worldInteractiveObject is Door door && door.IsBroken)
                {
                    list0.Add(worldInteractiveObject.GetStatusInfo(true));
                }
            }

            writer.Write(list0.Count);

            for (var k = 0; k < list0.Count; k++)
            {
                writer.Write((short) list0[k].NetId);
                byte b = list0[k].State;

                if (list0[k].IsBroken)
                {
                    b |= 16;
                }

                writer.Write(b);
            }
        }
    }
}
