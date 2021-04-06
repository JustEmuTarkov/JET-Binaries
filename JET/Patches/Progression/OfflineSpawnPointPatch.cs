using System.Reflection;
using UnityEngine;
using EFT;
using JET.Utilities.Patching;
using System.Linq;
using System;
using System.Collections.Generic;
#if B11661
using EFT.Game.Spawning;
using ISpawnPoints = GInterface222; // DestroySpawnPoint or CreateSpawnPoint as ginterface
#endif
#if B10988
using EFT.Game.Spawning;
using ISpawnPoints = GInterface217;
#endif
#if B9767
using EFT.Game.Spawning;
using ISpawnPoints = GInterface208;
#endif
#if B9018
using EFT.Interactive;
#endif
#if DEBUG
using EFT.Game.Spawning;
using ISpawnPoints = GInterface217;
#endif

namespace JET.Patches.Progression
{
    class OfflineSpawnPointPatch : GenericPatch<OfflineSpawnPointPatch>
    {
        public OfflineSpawnPointPatch() : base(prefix: nameof(PatchPrefix)) { }

        protected override MethodBase GetTargetMethod()
        {
            var targetType = PatcherConstants.TargetAssembly.GetTypes().First(IsTargetType);
            return targetType.GetMethods(PatcherConstants.DefaultBindingFlags).First(m => m.Name.Contains("SelectSpawnPoint"));
        }

        private static bool IsTargetType(Type type)
        {
            var methods = type.GetMethods(PatcherConstants.DefaultBindingFlags);

            if (!methods.Any(x => x.Name.IndexOf("CheckFarthestFromOtherPlayers", StringComparison.OrdinalIgnoreCase) != -1))
                return false;

            return !type.IsInterface;
        }


#if B9018
        public static bool PatchPrefix(SpawnArea.SpawnAreaSettings[] ___spawnAreaSettings_0, EPlayerSide side, out Vector3 position, out Quaternion rotation, string spawnPointFilter = null, string infiltrationZone = null)
        {
            var spawnAreaSettingHelper = new SpawnAreaSettingHelper(side, spawnPointFilter, infiltrationZone);
            var spawnAreaSettings = ___spawnAreaSettings_0.Where(spawnAreaSettingHelper.isSpawnAreaSetting).RandomElement();

            if (spawnAreaSettings == null)
            {
                Debug.LogError("No spawn points for " + side + " found! Spawn points count: " + ___spawnAreaSettings_0.Length);
                position = Vector3.zero;
                rotation = Quaternion.identity;
                return false;
            }

            position = spawnAreaSettings.Position;
            rotation = Quaternion.Euler(0f, spawnAreaSettings.Orientation, 0f);

            return false;
        }
        public class SpawnAreaSettingHelper
        {
            private readonly EPlayerSide side;
            private readonly string spawnPointFilter;
            private readonly string infiltrationZone;

            public SpawnAreaSettingHelper(EPlayerSide side, string spawnPointFilter, string infiltrationZone)
            {
                this.side = side;
                this.spawnPointFilter = spawnPointFilter;
                this.infiltrationZone = infiltrationZone;
            }

            public bool isSpawnAreaSetting(SpawnArea.SpawnAreaSettings x)
            {
                return x.Sides.Contains(side)
                    && (string.IsNullOrWhiteSpace(infiltrationZone) || x.InfiltrationZone == infiltrationZone)
                    && (string.IsNullOrWhiteSpace(spawnPointFilter) || spawnPointFilter.Contains(x.Id));
            }
        }
#else
        public static bool PatchPrefix(ref ISpawnPoint __result, ISpawnPoints ___ginterface217_0, ESpawnCategory category, EPlayerSide side, string infiltration)
        {
            var spawnPoints = ___ginterface217_0.ToList();
            var unfilteredSpawnPoints = spawnPoints.ToList();
            var infils = spawnPoints.Select(sp => sp.Infiltration).Distinct();
            Debug.LogError($"PatchPrefix SelectSpawnPoint Infiltrations: {spawnPoints.Count} | {String.Join(", ", infils)}");

           // Debug.LogError($"Filter by Infiltration: {infiltration}");
            spawnPoints = spawnPoints.Where(sp => sp != null && sp.Infiltration != null && (String.IsNullOrEmpty(infiltration) || sp.Infiltration.Equals(infiltration))).ToList();
            if (spawnPoints.Count == 0)
            {
                __result = GetFallBackSpawnPoint(unfilteredSpawnPoints, category, side, infiltration);
                return false;
            }

           // Debug.LogError($"Filter by Categories: {category}");
            spawnPoints = spawnPoints.Where(sp => sp.Categories.Contain(category)).ToList();
            if (spawnPoints.Count == 0)
            {
                __result = GetFallBackSpawnPoint(unfilteredSpawnPoints, category, side, infiltration);
                return false;
            }

           // Debug.LogError($"Filter by Side: {side}");
            spawnPoints = spawnPoints.Where(sp => sp.Sides.Contain(side)).ToList();
            if (spawnPoints.Count == 0)
            {
                __result = GetFallBackSpawnPoint(unfilteredSpawnPoints, category, side, infiltration);
                return false;
            }

            __result = spawnPoints.RandomElement();
           // Debug.LogError($"PatchPrefix SelectSpawnPoint: {__result.Id}");
            return false;
        }

        private static ISpawnPoint GetFallBackSpawnPoint(List<ISpawnPoint> spawnPoints, ESpawnCategory category, EPlayerSide side, string infiltration)
        {
            var spawn = spawnPoints.Where(sp => sp.Categories.Contain(ESpawnCategory.Player)).RandomElement();
            Debug.LogError($"PatchPrefix SelectSpawnPoint [Id: {spawn.Id}]: Couldn't find any spawn points for:  {category}  |  {side}  |  {infiltration}");
            //Debug.LogError($"PatchPrefix SelectSpawnPoint: {spawn.Id}");
            return spawn;
        }
#endif
    }
}
