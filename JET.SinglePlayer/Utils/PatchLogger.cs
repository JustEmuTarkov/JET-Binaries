using System.Reflection;
using UnityEngine;

namespace JET.SinglePlayer.Utils
{
    public class PatchLogger
    {
        public static void LogTranspileSearchError(MethodBase transpileMethod)
        {
            Debug.Log(GetErrorHeader(transpileMethod) + "Could not find reference code.");
        }

        public static void LogPatchErrorWithMessage(MethodBase method, string message)
        {
            Debug.Log(GetErrorHeader(method) + message);
        }

        private static string GetErrorHeader(MethodBase method)
        {
            return "Patch " + method.DeclaringType.Name + "failed: ";
        }
    }
}
