using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using EFT;
using UnityEngine;

namespace JET.Modding
{
    internal class CustomMods
    {
        internal static string GetGameDirectory => AppDomain.CurrentDomain.BaseDirectory;
        internal static readonly List<JetMod> ModInstances = new List<JetMod>();

        internal static void Load()
        {
            var pathToCustomMods = Path.Combine(GetGameDirectory, "ClientMods");
            var mods = Directory.GetFiles(pathToCustomMods, "*.dll").ToList();

            var availableMods = new Dictionary<Type, ModSettings>();


            foreach (var file in mods)
            {
                var fullPath = Path.Combine(pathToCustomMods, file);

                // Read the file before loading so the dll file doesn't stay locked.
                var bytes = File.ReadAllBytes(fullPath);
                var assembly = Assembly.Load(bytes);
                var types = assembly.GetTypes().Where(x => x.IsClass && x.BaseType == typeof(JetMod)).ToArray();
                if (types.Length > 1)
                {
                    Debug.LogError($"Failed to load {Path.GetFileName(file)}. You may only have one class that inherits from {nameof(JetMod)}.");
                    continue;
                }

                var type = types.FirstOrDefault();
                // DLL file is not a mod and is likely a dependency. Keep it loaded and continue.
                if (type == default)
                    continue;

                availableMods.Add(type, new ModSettings(type));
            }

            var noDependencies = availableMods.Where(x => !x.Value.DependsOn.Any() && !x.Value.SoftDependsOn.Any());
            var remaining = availableMods.Where(x => !noDependencies.Contains(x)).ToArray();

            // Load mods without dependencies first for less recursion (hopefully)
            foreach (var (type, settings) in noDependencies)
            {
                if (!LoadMod(settings, out var mod)) continue;
                ModInstances.Add(mod);
                Debug.Log($"Mod {type.FullName} loaded successfully");
            }

            // Load mods only if dependencies and soft dependencies are loaded
            while (remaining.Length > 0)
            {
                var modsLoaded = 0;
                var newRemaining = remaining.ToList();
                foreach (var (type, settings) in remaining)
                {
                    if (!settings.DependsOn.Select(x => ModInstances.Any(y => y.GetType() == x)).All(x => x) ||
                        !settings.SoftDependsOn.Select(x => ModInstances.Any(y => y.GetType() == x)).All(x => x))
                        continue;
                    if (!LoadMod(settings, out var mod)) continue;
                    ModInstances.Add(mod);
                    newRemaining.RemoveFirst(x => x.Key == type);
                    modsLoaded++;
                    Debug.Log($"Mod {type.FullName} loaded successfully");
                }

                remaining = newRemaining.ToArray();
                if (modsLoaded == 0)
                    break;
            }

            // Load mods if dependencies are loaded, ignoring soft dependencies
            while (remaining.Length > 0)
            {
                var modsLoaded = 0;
                var newRemaining = remaining.ToList();
                foreach (var (type, settings) in remaining)
                {
                    if (!settings.DependsOn.Select(x => ModInstances.Any(y => y.GetType() == x)).All(x => x))
                        continue;
                    if (!LoadMod(settings, out var mod)) continue;
                    ModInstances.Add(mod);
                    newRemaining.RemoveFirst(x => x.Key == type);
                    modsLoaded++;
                    Debug.Log($"Mod {type.FullName} loaded successfully");
                }

                remaining = newRemaining.ToArray();
                if (modsLoaded == 0)
                    break;
            }

            // Remaining mods have required dependencies that don't exist
            foreach (var (type, settings) in remaining)
            {
                var missing = settings.DependsOn.Where(x => ModInstances.All(y => y.GetType() != x));
                var missingList = missing.Select(x => x.FullName);
                Debug.LogError($"Mod {type.FullName} is missing required dependencies and will not be loaded. Missing: {string.Join(", ", missingList)}");
            }

            #region Maoci's version

            //foreach (var file in mods)
            //{
            //    var launchingCommands = File.ReadLines(file).ToArray();
            //    var assemblyPathing = launchingCommands[1].Split('.').ToList();
            //    //Debug.Log($"{launchingCommands[0]} {launchingCommands[1]}");
            //    if (assemblyPathing.Count < 2)
            //    {
            //        Debug.LogError($"Failed with count {assemblyPathing.Count}, string be: {launchingCommands[1]}");
            //        continue;
            //    }
            //    try
            //    {
            //        var loadedAssembly = Assembly.LoadFile(Path.Combine(GetGameDirectory, "ClientMods", launchingCommands[0]));

            //        var classHandle = loadedAssembly.GetExportedTypes().Single(type => type.Name == assemblyPathing[assemblyPathing.Count - 2]);

            //        classHandle.GetMethods(BindingFlags.Public | BindingFlags.Static).Single(method =>
            //        {
            //            if (method.Name == assemblyPathing[assemblyPathing.Count - 1])
            //                Debug.Log(method.Name + " " + method.GetParameters().Length);
            //            return method.Name == assemblyPathing[assemblyPathing.Count - 1] && method.GetParameters().Length == 0;
            //        })
            //            .Invoke(classHandle, new object[] { });

            //        LoadedAssemblyMods.Add(loadedAssembly);
            //    }
            //    catch (Exception errorOccured)
            //    {
            //        Debug.Log($"Error Loading: {file}");
            //        Debug.Log($"Error: {errorOccured.Message}");
            //        Debug.Log(errorOccured.StackTrace);
            //        Debug.Log("-  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -");
            //        continue;
            //    }
            //    Debug.Log($"Loaded: {launchingCommands[0]} [{launchingCommands[1]}]");
            //}

            #endregion
        }

        private static bool LoadMod(ModSettings settings, out JetMod mod)
        {
            mod = null;
            if (settings.ModType.GetConstructors().All(x => x.GetParameters().Length != 0))
            {
                Debug.LogError($"Mod {settings.ModType} does not contain a constructor that takes 0 arguments. Please add one.");
                return false;
            }
            try
            {
                var instance = Activator.CreateInstance(settings.ModType) as JetMod;
                var dependInstances = ModInstances
                    .Where(x => settings.DependsOn.Contains(x.GetType()) || settings.DependsOn.Contains(x.GetType()))
                    .ToDictionary(x => x.GetType(), x => x);

                try
                {
                    instance?.Initialize(dependInstances, Application.version);
                    if (instance == null)
                    {
                        Debug.LogError($"Failed to load mod {settings.ModType}. Instance is null.");
                        return false;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"An error occurred while loading mod {settings.ModType.FullName}: {e}");
                    // Don't return as the mod could still be (mostly) working
                }

                mod = instance;
                return true;

            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load mod {settings.ModType.FullName}: {e}");
                return false;
            }
        }
    }
}
