using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using JET.Utilities.App;
using JET.Utilities.HTTP;
using JET.Utilities.Patching;
using JET.Utilities;
using LocationInfo = GClass759.GClass761;
using System;

namespace JET.Patches.Progression
{
	public class OfflineLootPatch : GenericPatch<OfflineLootPatch>
	{
		public static PropertyInfo _property;

		public OfflineLootPatch() : base(prefix: nameof(PatchPrefix))
		{
            // compile-time check
            _ = nameof(LocationInfo.BotLocationModifier);
        }

        protected override MethodBase GetTargetMethod()
        {
			var localGameBaseType = PatcherConstants.LocalGameType.BaseType;

			_property = localGameBaseType.GetProperty($"{nameof(LocationInfo)}_0", BindingFlags.NonPublic | BindingFlags.Instance);
			return localGameBaseType.GetMethod("method_5", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
		}

		/// <summary>
		/// Loads loot from JET's server.
		/// Falls back to the client's local location loot if it fails.
		/// </summary>
		public static bool PatchPrefix(ref Task<LocationInfo> __result, object __instance, string backendUrl)
		{
			if (__instance.GetType() != PatcherConstants.LocalGameType)
			{
				// online match
				return true;
			}

			var location = (LocationInfo)_property.GetValue(__instance);
			var request = new Request(Utilities.Config.BackEndSession.GetPhpSessionId(), backendUrl);
			var json = request.GetJson("/api/location/" + location.Id);

			// some magic here. do not change =)
			var locationLoot = json.ParseJsonTo<LocationInfo>();

			request.PostJson("/raid/map/name", Json.Serialize(new LocationName(location.Id)));

            if (locationLoot == null)
			{
				// failed to download loot
				Debug.Log("OfflineLootPatch > Failed to download loot, using fallback");
				return true;
			}

			Debug.Log("OfflineLootPatch > Successfully received loot from server");
			__result = Task.FromResult(locationLoot);

			// get weapon durability
			Config.WeaponDurabilityEnabled = GetDurabilityState();

			return false;
		}

		private static bool GetDurabilityState()
		{
			var json = new Request(null, Config.BackendUrl).GetJson("/singleplayer/settings/weapon/durability");

			if (string.IsNullOrWhiteSpace(json))
			{
				Debug.Log("JET.SinglePlayer: Received weapon durability state data is NULL, using fallback");
				return false;
			}

			Debug.Log("JET.SinglePlayer: Successfully received weapon durability state");
			return Convert.ToBoolean(json);
		}

		struct LocationName
		{
			public string Location { get; }

			public LocationName(string location)
			{
				Location = location;
			}
		}
	}
}