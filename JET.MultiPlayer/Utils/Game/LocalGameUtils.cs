using System;
using Comfort.Common;
using EFT;
using ServerLib.Utils.Reflection;
using BotSpawner = GClass875;
using BotCreator = GClass294;

namespace ServerLib.Utils.Game
{
    static class LocalGameUtils
    {
        public static LocalGame Get()
        {
            var game = Singleton<AbstractGame>.Instance as LocalGame;
            if (game != null) return game;

            Console.WriteLine("ERROR!!! ServerLib.Utils.Game.LocalGameUtils game is null");
            return null;
        }

        public static BotSpawner GetBotSpawner()
        {
            var game = Get();
            if (game == null)
            {
                Console.WriteLine("ERROR!!! ServerLib.Utils.Game.GetBotSpawner game is null");
                return null;
            }

            // public class GClass864 : GInterface0
            // Token: 0x0600426E RID: 17006 RVA: 0x0022A704 File Offset: 0x00228904
            // public void Init(GInterface11 botGame, GInterface14 botCreator, ...)
            var botSpawner = PrivateValueAccessor.GetPrivateFieldValue(
                game.BotsController.GetType(), "gclass875_0", game.BotsController
            ) as BotSpawner;
            botSpawner.Stop(); // stop spawning bots
            return botSpawner;
        }

        public static BotCreator GetBotCreator()
        {
            var spawner = GetBotSpawner();
            if (spawner == null)
            {
                Console.WriteLine("ERROR!!! ServerLib.Utils.Game.GetBotCreator spawner is null");
                return null;
            }

            BotCreator botCreator = null;
            var fieldInfo = PrivateValueAccessor.GetPrivateFieldInfo(spawner.GetType(), "ginterface14_0");
            if (fieldInfo != null) botCreator = fieldInfo.GetValue(spawner) as BotCreator;
            return botCreator;
        }
    }
}
