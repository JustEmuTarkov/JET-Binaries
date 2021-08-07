using JET.Utilities.Patching;
using System.Linq;
using System.Reflection;

namespace JET.Utilities
{
    public class Game
    {
        private static string GameVersion = "";
        public static string GetVersion 
        {
            get
            {
                if (GameVersion == "")
                {
                    var list = PatcherConstants.TargetAssembly.GetTypes()
                        .Where(type =>
                            type.Name.StartsWith("Class") &&
                            type.GetField("string_0", BindingFlags.NonPublic | BindingFlags.Static) != null &&
                            type.GetMethods().Length == 4 &&
                            type.GetProperties().Length == 0 &&
                            type.GetMethods(BindingFlags.NonPublic | BindingFlags.Static).Length == 0 &&
                            type.GetProperties(BindingFlags.NonPublic | BindingFlags.Static).Length == 0)
                        .ToList();
                    if (list.Count > 0)
                    {
                        GameVersion = list[0].GetField("string_0", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null).ToString();
                    }
                }
                return GameVersion;
            }
        }
        
    }
}
