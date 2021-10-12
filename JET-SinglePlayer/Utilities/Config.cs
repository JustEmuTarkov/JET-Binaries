using Comfort.Common;
using EFT;
#if B14687
using ISession = GInterface111; // GetPhpSessionId
using ClientConfig = GClass537; // BackendCacheDir or LoadApplicationConfig
#endif
#if B13074 || B13487
using ISession = GInterface106; // GetPhpSessionId
using ClientConfig = GClass350; // BackendCacheDir or LoadApplicationConfig
#endif
#if B11661 || B12102
using ISession = GInterface28; // GetPhpSessionId
using ClientConfig = GClass334; // BackendCacheDir or LoadApplicationConfig
#endif
#if B10988
using ISession = GInterface27;
using ClientConfig = GClass333;
#endif
#if B9767
using ISession = GInterface26;
using ClientConfig = GClass313;
#endif
#if B9018
using ISession = GInterface25;
using ClientConfig = GClass310;
#endif
#if DEBUG
using ISession = GInterface27;
using ClientConfig = GClass333;
#endif
namespace JET.Utilities
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
        public static bool WeaponDurabilityEnabled = false;
    }
}
