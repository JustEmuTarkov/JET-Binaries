using JET.Utilities.App;
using JET.Utilities.HTTP;
using System.Reflection;

namespace JET.Utilities
{
    public static class Offline
    {
        [ObfuscationAttribute(Exclude = true)]
        public class OfflineMode
        {
            public bool Offline = true;
        }
        public static bool IsEnabled()
        {
            try
            {
                var request = new Request(null, Utilities.Config.BackendUrl);
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
