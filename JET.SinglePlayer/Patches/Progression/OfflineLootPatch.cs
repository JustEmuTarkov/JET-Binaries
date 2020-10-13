﻿using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using JET.Common.Utils.App;
using JET.Common.Utils.HTTP;
using JET.Common.Utils.Patching;
using LocationInfo = GClass757.GClass759;
using System;

namespace JET.SinglePlayer.Patches.Progression
{
	public class OfflineLootPatch : AbstractPatch
	{
		public static PropertyInfo _property;

		static OfflineLootPatch()
		{
			// compile-time check
			_ = nameof(LocationInfo.BotLocationModifier);
		}

		public OfflineLootPatch()
		{
		}

		public override MethodInfo TargetMethod()
		{
			var localGameBaseType = PatcherConstants.LocalGameType.BaseType;

			_property = localGameBaseType.GetProperty("GClass759_0", BindingFlags.NonPublic | BindingFlags.Instance);
			return localGameBaseType.GetMethod("method_5", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
		}

		/// <summary>
		/// Loads loot from JET's server.
		/// Falls back to the client's local location loot if it fails.
		/// </summary>
		public static bool Prefix(ref Task<LocationInfo> __result, object __instance, string backendUrl)
		{
			if (__instance.GetType() != PatcherConstants.LocalGameType)
			{
				// online match
				Debug.Log("OfflineLootPatch > Online match?!");
				return true;
			}
			try
			{
				var location = (LocationInfo)_property.GetValue(__instance);
				var request = new Request("", backendUrl);
				var json = request.GetJson("/api/location/" + location.Id);
				// some magic here. do not change =)
				var locationLoot = Json.Deserialize<LocationInfo>(json);
				request.PostJson("/raid/map/name", Json.Serialize(new LocationName(location.Id)));
				if (locationLoot == null)
				{
					// failed to download loot
					Debug.Log("OfflineLootPatch > Failed to download loot, using fallback");
					return true;
				}
				Debug.Log("OfflineLootPatch > Successfully received loot from server");
				__result = Task.FromResult(locationLoot);

			}
			catch (Exception e1) { Debug.LogError("=========OfflineLootPatch Failed=========="); Debug.LogError(e1); Debug.LogError("==============================="); return true; }
			return false;
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