
using System.Reflection;
using UnityEngine;
using EFT;
using JET.Utilities.Patching;
using System.Linq;
using System;
//using EFT.Game.Spawning;
using System.Collections.Generic;
using EFT.Interactive;

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

    }
}
