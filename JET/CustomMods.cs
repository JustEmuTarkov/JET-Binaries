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
    class CustomMods
    {
        internal static string GetGameDirectory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }
        private static List<Assembly> LoadedAssemblyMods = new List<Assembly>();

        internal static void Load() {
            string PathToCustomMods = Path.Combine(GetGameDirectory, "ClientMods");
            List<string> files = Directory.GetFiles(PathToCustomMods, "*.launch").ToList();
            for (int i = 0; i < files.Count; i++)
            {
                string[] launchingCommands = File.ReadLines(files[i]).ToArray();
                List<string> AssemblyPathing = launchingCommands[1].Split('.').ToList();
                //Debug.Log($"{launchingCommands[0]} {launchingCommands[1]}");
                if (AssemblyPathing.Count < 2) {
                    Debug.LogError($"Failed with count {AssemblyPathing.Count}, string be: {launchingCommands[1]}");
                    continue;
                }
                try
                {
                    Assembly LoadedAssembly = Assembly.LoadFile(Path.Combine(GetGameDirectory, "ClientMods", launchingCommands[0]));

                    Type ClassHandle = LoadedAssembly.GetExportedTypes().Single(type => type.Name == AssemblyPathing[AssemblyPathing.Count - 2]);

                    ClassHandle.GetMethods(BindingFlags.Public | BindingFlags.Static).Single(method => {
                        if(method.Name == AssemblyPathing[AssemblyPathing.Count - 1])
                            Debug.Log(method.Name + " " + method.GetParameters().Length);
                        return method.Name == AssemblyPathing[AssemblyPathing.Count - 1] && method.GetParameters().Length == 0;
                        })
                        .Invoke(ClassHandle, new object[] { });

                    LoadedAssemblyMods.Add(LoadedAssembly);
                }
                catch (Exception errorOccured)
                {
                    Debug.Log($"Error Loading: {files[i]}");
                    Debug.Log($"Error: {errorOccured.Message}");
                    Debug.Log(errorOccured.StackTrace);
                    Debug.Log("-  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -");
                    continue;
                }
                Debug.Log($"Loaded: {launchingCommands[0]} [{launchingCommands[1]}]");
            }
        }

       // private void LogError(string data) {

            //File.AppendAllText(Path.Combine(GetGameDirectory, "ClientMods.log"), data + "\n");
       // }
    }
}
