using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using EFT;
using EFT.Game.Spawning;
using JET.Utilities.Patching;

namespace JET.Patches.Progression
{
    class OfflineSpawnPointPatch : GenericPatch<OfflineSpawnPointPatch>
    {
        public OfflineSpawnPointPatch() : base(prefix: nameof(PatchPrefix)) { }

        protected override MethodBase GetTargetMethod()
        {
            return PatcherConstants.TargetAssembly.GetTypes().First(IsTargetType).GetMethods(PatcherConstants.DefaultBindingFlags).First(m => m.Name.Contains("SelectSpawnPoint"));
        }

        private static bool IsTargetType(Type type)
        {
            if (!type.GetMethods(PatcherConstants.DefaultBindingFlags).Any(x => x.Name.IndexOf("CheckFarthestFromOtherPlayers", StringComparison.OrdinalIgnoreCase) != -1))
                return false;

            return !type.IsInterface;
        }

        public static bool PatchPrefix(ref ISpawnPoint __result, GInterface208 ___ginterface208_0, ESpawnCategory category, EPlayerSide side, string infiltration)
        {
            var spawnPoints = Enumerable.ToList(___ginterface208_0);

            spawnPoints = spawnPoints.Where(sp => sp.Sides.Contain(side) && sp.Categories.Contain(category)).ToList();
            var infils = spawnPoints.Select(sp => sp.Infiltration).Distinct();

            Debug.Log($"PatchPrefix SelectSpawnPoint: {spawnPoints.Count} | {String.Join(", ", infils)}");

            __result = spawnPoints.Where(sp => sp.Infiltration.Equals(infiltration)).RandomElement();
            Debug.Log($"Selected Spawn Point: {__result.Id}");
            return false;
        }
    }
}
