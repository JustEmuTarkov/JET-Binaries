using System.Reflection;
using JET.Common.Utils.HTTP;
using JET.Common.Utils.App;

namespace JET.SinglePlayer.Utils
{
    public static class Offline
    {
        [ObfuscationAttribute(Exclude = true)]
        public class OfflineMode
        {
            public bool Offline = true;
        }
        public static bool IsEnabled() {
            try
            {
                var request = new Request(null, Utils.Config.BackendUrl);
                var json = request.GetJson("/mode/offline");
                OfflineMode __offlineClass = Json.Deserialize<OfflineMode>(json);
                return __offlineClass.Offline;
            }
            catch
            { // if somehow this fails load offline anyway
                return true;
            }
        }
    }
}
