using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Comfort.Common;
using EFT;
using ServerLib.Network.Messages;
using ServerLib.Utils.Game;
using ServerLib.Utils.Reflection;
using ServerLib.Utils.Server;
using UnityEngine;
using BotSpawner = GClass875;

namespace ServerLib.Network.Bots
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class BotsMonitor : MonoBehaviour
    {
        private BotSpawner _botSpawner;
        public static readonly ConcurrentDictionary<int, BotOwner> AllBots = new ConcurrentDictionary<int, BotOwner>();
        public readonly List<EFT.Player> AllBotPlayers = new List<EFT.Player>();

        public void Start()
        {
            Singleton<BotsMonitor>.Create(this);
            _botSpawner = LocalGameUtils.GetBotSpawner();
            _botSpawner.OnBotCreated += OnBotCreated;
            _botSpawner.OnBotRemoved += OnBotRemoved;
            ReplaceBotCreator();
        }

        private void ReplaceBotCreator()
        {
            var botCreator = LocalGameUtils.GetBotCreator();
            if (botCreator == null) return;

            var botCreteFunc = PrivateValueAccessor.GetPrivateFieldInfo(botCreator.GetType(), "func_0");
            if (botCreteFunc.GetValue(botCreator) is Func<Profile, Task<EFT.Player>>)
            {
                botCreteFunc.SetValue(botCreator, new Func<Profile, Task<EFT.Player>>(CreateBot));
                return;
            }

            Console.WriteLine("ERROR!!! BotsMonitor ReplaceBotCreator() can't replace bot creator function!!!");
        }

        private async Task<EFT.Player> CreateBot(Profile profile)
        {
            EFT.Player player = await BotPlayer.Create(profile);
            player.Location = Singleton<ServerInstance>.Instance.mapSettings._Id;
            this.AllBotPlayers.Add(player);
            return player;
        }

        private void OnBotRemoved(BotOwner obj)
        {
            var emitter = obj.gameObject.GetComponent<BotStateEmitter>();
            emitter.SendBotState(0.02f);
            emitter.stop = true;
            DestroyImmediate(emitter);

            AllBots.TryRemove(emitter.channelId, out _);

            Console.WriteLine($"Bot Dead packet prev pos {emitter.prevPacket.MovementInfo.TransformPosition}");

            Console.WriteLine($"OnBotRemoved {obj.GetPlayer.Id}");
        }

        private static void OnBotCreated(BotOwner botOwner)
        {
            var emitter = botOwner.GetPlayer.GetOrAddComponent<BotStateEmitter>().Init(botOwner);

            AllBots.TryAdd(emitter.channelId, botOwner);

            processProfileBundles(botOwner);

            Console.WriteLine($"OnBotCreated {botOwner.GetPlayer.Id}");
        }

        private static void processProfileBundles(GInterface42 botOwner)
        {
            var prefabs = botOwner.Profile.Inventory.GetAllInventoryPrefabs();

            Singleton<ServerInstance>.Instance.AllPrefabs.AddRange(prefabs);

            var sessions = Singleton<ServerInstance>.Instance.GameSessions.Values;
            var bundleMessage = new SomeBundleMessage {Id = SomeBundleMessage.BundlesQueue, LootItems = prefabs};

            foreach (var session in sessions)
            {
                session.BundlesQueue.Enqueue(bundleMessage);
            }
        }
    }
}
