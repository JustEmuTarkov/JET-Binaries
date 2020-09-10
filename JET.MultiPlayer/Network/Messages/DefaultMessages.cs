using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Network.Messages
{
    public static class DefaultMessages
    {
        public static void OnNotReadyMessage(NetworkMessage networkMessage)
        {
            Debug.Log("OnNotReadyMessage received, profile id is: " + networkMessage.conn.connectionId);
        }
    }
}
