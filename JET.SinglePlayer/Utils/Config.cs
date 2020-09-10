using Comfort.Common;
using EFT;
using ISession = GInterface23;
using ClientConfig = GClass266;

namespace JET.SinglePlayer.Utils
{
    public static class Config
    {
        static Config()
        {
            _ = nameof(ISession.GetPhpSessionId);
            _ = nameof(ClientConfig.BackendUrl);
        }

        public static ISession BackEndSession => Singleton<ClientApplication>.Instance.GetClientBackEndSession();
        public static string BackendUrl => ClientConfig.Config.BackendUrl;
    }
}
