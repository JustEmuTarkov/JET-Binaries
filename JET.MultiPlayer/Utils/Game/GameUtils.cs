using System;
using System.Threading.Tasks;
using Comfort.Common;
using EFT;
using JetBrains.Annotations;
using ServerLib.Utils.Reflection;
using UnityEngine;
using static ServerLib.Utils.Reflection.PrivateMethodAccessor;

namespace ServerLib.Utils.Game
{
    public static class GameUtils
    {
        [CanBeNull]
        public static ClientApplication GetClientApp()
        {
            var app = Singleton<ClientApplication>.Instance;
            if (app != null)
                return app;

            Console.WriteLine("ServerLib.Utils.Game.GameUtils.GetClientApp() app is null!!!");
            return null;
        }

        [CanBeNull]
        public static MainApplication GetMainApp()
        {
            var mainApp = GetClientApp() as MainApplication;
            if (mainApp != null)
                return mainApp;

            Console.WriteLine("ServerLib.Utils.Game.GameUtils.GetMainApp() mainApp is null!!!");
            return null;
        }

        public static bool GameReadyForStart()
        {
            return GetMainApp()?.gclass1149_0 != null;
        }

        public static GClass760.GClass762 StartLocalGame(string locationId, GStruct87 timeAndWeather)
        {
            var mainApp = GetMainApp();
            if (mainApp == null) return null;

            int num = UnityEngine.Random.Range(1, 6);
            var localLoot = GClass482.Load<TextAsset>("LocalLoot/" + locationId + num).text.ParseJsonTo<GClass760.GClass761>();
            var location = localLoot.Location.ParseJsonTo<GClass760.GClass762>();

            SetIsLocal();
            mainApp.method_29(location, timeAndWeather, "");

            return location;
        }


        private static void SetIsLocal()
        {
            var mainApp = GetMainApp();
            if (mainApp == null) return;

            var fieldInfo = PrivateValueAccessor.GetPrivateFieldInfo(mainApp.GetType(), "_localGame");
            if (fieldInfo != null) fieldInfo.SetValue(mainApp, true);
        }
    }
}
