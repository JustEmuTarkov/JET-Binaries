using ServerLib.Network.Handlers;
using ServerLib.Network.Messages;
using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Config
{
    public static class Handlers
    {
        public static void RegisterServerHandlers()
        {
            if (!NetworkServer.active)
                return;

            NetworkServer.RegisterHandler(AuthRequestMessage.MessageId, AuthHandler.OnAuthMessage);
            NetworkServer.RegisterHandler(LoadingBundlesMessage.MessageId, BundleLoadHandler.OnReportProgressLoading);
            NetworkServer.RegisterHandler(170, InteractiveHandler.OnInteractivePacketHandler);
        }

        public static void OnSceneReady(NetworkConnection conn)
        {
            SceneReadyHandler.OnSceneReady(conn);
        }
    }
}
