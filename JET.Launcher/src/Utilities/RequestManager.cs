using JET.Utilities.HTTP;
using JET.Utilities.App;
using JET.Launcher.Structures;

namespace JET.Launcher.Utilities
{
	internal class RequestManager
    {
		internal static bool ServerConnectedProperly = false;
		internal static bool OngoingRequest = false;
		internal static void Busy() { OngoingRequest = true; }
		internal static void Free() { OngoingRequest = false; }

		private static readonly Request request = new Request(null, "https://127.0.0.1:443", false);
		internal static string GetBackendUrl() => request.RemoteEndPoint;
		internal static void ChangeBackendUrl(string remoteEndPoint) => request.RemoteEndPoint = remoteEndPoint;
		internal static void ChangeSession(string session) => request.Session = session;
		internal static string Connect() => request.GetJson(Global.URL.ServerConnect);
		internal static string Login(RequestData.Login data) => request.PostJson(Global.URL.ProfileLogin, Json.Serialize(data));
		internal static string Register(RequestData.Register data) => request.PostJson(Global.URL.ProfileRegister, Json.Serialize(data));
		internal static string RemoveProfile(RequestData.Login data) => request.PostJson(Global.URL.ProfileRemove, Json.Serialize(data));
		internal static string WipeProfile(RequestData.Register data) => request.PostJson(Global.URL.ProfileChangeWipe, Json.Serialize(data));
		internal static string GetAccount(RequestData.Login data) => request.PostJson(Global.URL.ProfileGet, Json.Serialize(data));
		internal static string ChangeEmail(RequestData.Change data) => request.PostJson(Global.URL.ProfileChangeEmail, Json.Serialize(data));
		internal static string ChangePassword(RequestData.Change data) => request.PostJson(Global.URL.ProfileChangePassword, Json.Serialize(data));
	}
}
