using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace JET
{
    internal class CustomMods
    {
        internal static string GetGameDirectory => AppDomain.CurrentDomain.BaseDirectory;
        public static readonly List<Assembly> LoadedAssemblyMods = new List<Assembly>();

        internal static void Load() {
            var pathToCustomMods = Path.Combine(GetGameDirectory, "ClientMods");
            var files = Directory.GetFiles(pathToCustomMods, "*.launch").ToList();
            foreach (var file in files)
            {
                var launchingCommands = File.ReadLines(file).ToArray();
                var assemblyPathing = launchingCommands[1].Split('.').ToList();
                //Debug.Log($"{launchingCommands[0]} {launchingCommands[1]}");
                if (assemblyPathing.Count < 2) {
                    Debug.LogError($"Failed with count {assemblyPathing.Count}, string be: {launchingCommands[1]}");
                    continue;
                }
                try
                {
                    var loadedAssembly = Assembly.LoadFile(Path.Combine(GetGameDirectory, "ClientMods", launchingCommands[0]));

                    var classHandle = loadedAssembly.GetExportedTypes().Single(type => type.Name == assemblyPathing[assemblyPathing.Count - 2]);

                    classHandle.GetMethods(BindingFlags.Public | BindingFlags.Static).Single(method => {
                            if(method.Name == assemblyPathing[assemblyPathing.Count - 1])
                                Debug.Log(method.Name + " " + method.GetParameters().Length);
                            return method.Name == assemblyPathing[assemblyPathing.Count - 1] && method.GetParameters().Length == 0;
                        })
                        .Invoke(classHandle, new object[] { });

                    LoadedAssemblyMods.Add(loadedAssembly);
                }
                catch (Exception errorOccured)
                {
                    Debug.Log($"Error Loading: {file}");
                    Debug.Log($"Error: {errorOccured.Message}");
                    Debug.Log(errorOccured.StackTrace);
                    Debug.Log("-  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -");
                    continue;
                }
                Debug.Log($"Loaded: {launchingCommands[0]} [{launchingCommands[1]}]");
            }
        }
    }
}
