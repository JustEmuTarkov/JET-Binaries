using Comfort.Common;
using EFT;


/* START REFERENCE ######################################################################
using ISession = GInterface111; // GetPhpSessionId

 * 	// Token: 0x06006C36 RID: 27702
	[CanBeNull]
	string GetPhpSessionId();
 

using ClientConfig = GClass558; // BackendCacheDir or LoadApplicationConfig

 * 	public static string BackendCacheDir
	{
		get
		{
			return Application.dataPath + "/../cache/";
		}
	}
 *
 * public static bool LoadApplicationConfig(GClass728 @default = null)
	{
		if (GClass536.Config != null)
		{
			return false;
		}
		if (!File.Exists(GClass536.ConfigFilePath))
		{
			GClass536.Config = @default;
			return false;
		}
		bool result;
		try
		{
			GClass536.Config = File.ReadAllText(GClass536.ConfigFilePath).ParseJsonTo(Array.Empty<JsonConverter>());
			result = true;
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			GClass536.Config = @default;
			result = false;
		}
		return result;
	}
 
END REFERENCE #############################################################
*/


#if B16338
using ISession = GInterface113;
// GetPhpSessionId
using ClientConfig = GClass570; 
// BackendCacheDir or LoadApplicationConfig in dnSpy
#endif
#if B16029
using ISession = GInterface111; // GetPhpSessionId
using ClientConfig = GClass558; // BackendCacheDir or LoadApplicationConfig
#endif
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
        private static ISession _backEndSession;
        public static ISession BackEndSession { 
            get 
            {
                if (_backEndSession == null)
                {
                    _backEndSession = Singleton<ClientApplication>.Instance.GetClientBackEndSession();
                }

                return _backEndSession;
            }
        } 
        public static string BackendUrl => ClientConfig.Config.BackendUrl;
        public static bool WeaponDurabilityEnabled = false;
    }
}
