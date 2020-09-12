using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Comfort.Common;
using EFT;
using EFT.AssetsManager;
using ServerLib.Config;
using ServerLib.Network.Bots;
using ServerLib.Network.Player;
using ServerLib.Network.World;
using ServerLib.Utils;
using ServerLib.Utils.Game;
using UnityEngine;
using UnityEngine.Networking;
using NetworkGameSession = ServerLib.Network.NetworkGameSession;
using LocationSettings = GClass735.GClass737;
using WeatherNodes = GClass1210;

// warning CS0618: 'NetworkManager' is obsolete: 'The high level API classes are deprecated and will be removed in the future.'
#pragma warning disable 618

namespace ServerLib
{
    public class ServerInstance : NetworkManager, GInterface48
    {
        const int ShortPort = 5000;
        const int MaxConnections = 20;
        public const int MaxPlayersOnMap = 200;
        public const int MaxChannels = MaxPlayersOnMap * 2;
        private bool _worldSpawned;
        private bool _botMonitorSpawned;
        private bool _gameStarted;
        public static int NextChanelId = 5;

        public LocationSettings mapSettings;
        public WeatherNodes[] weatherNodes;

        public readonly List<ResourceKey> AllPrefabs = new List<ResourceKey>();

        // Dictionary<int = connectionID, ..>
        public readonly ConcurrentDictionary<int, NetworkGameSession> GameSessions =
            new ConcurrentDictionary<int, NetworkGameSession>();

        public readonly ConcurrentDictionary<int, ServerPlayer> NetworkClients =
            new ConcurrentDictionary<int, ServerPlayer>();

        public void Start()
        {
            Singleton<ServerInstance>.Create(this);

            weatherNodes = WeatherNodes.GetRandomTestWeatherNodes();
            EftDebugConsole.ShowConsole();
            Console.WriteLine("ServerInstance.Start()");
        }

        //temporary variables
        private int _serverFrameIndex;

        public void FixedUpdate()
        {
            ulong localIndex = LocalIndex;
            LocalIndex = localIndex + 1UL;
            LocalTime += Time.deltaTime;
            EftDebugConsole.WriteAt(ref _serverFrameIndex, $"ServerInstance FixedUpdate local index {LocalIndex}");

            if (!_gameStarted && GameUtils.GameReadyForStart())
            {
                _gameStarted = true;
                // EnableLogs();

                // develop: "develop",
                // Woods: "Woods",
                // factory4_day: "factory4_day",
                // factory4_night: "factory4_night",
                // bigMap: "bigmap",
                // Shoreline: "Shoreline",
                // Interchange: "Interchange",
                // RezervBase: "RezervBase"

                var mapId = "factory4_day"; // it need to be loaded from file or something
                mapSettings = GameUtils.StartLocalGame(mapId,
                    new GStruct77(true, true)
                );
            }

            if (!_botMonitorSpawned)
            {
                var localGame = Singleton<AbstractGame>.Instance as LocalGame;
                if (localGame != null && localGame.BotsController != null)
                {
                    var botSpawner = LocalGameUtils.GetBotSpawner();
                    if (botSpawner != null)
                    {
                        gameObject.AddComponent<BotsMonitor>();
                        _botMonitorSpawned = true;
                    }
                }
            }

            if (_worldSpawned)
                return;

            var gameWorld = Singleton<GameWorld>.Instance;
            if (gameWorld == null || gameWorld.AllLoot.Count <= 0)
                return;

            _worldSpawned = true;

            if (!NetworkServer.active)
            {
                StartLocalServer();
                World.smethod_0<ServerWorld>(null, null);
                // Server started LevelBounds Center: (152.0, 0.0, -10.0), Extents: (600.0, 200.0, 270.0)
                // rezerv Center: (152.0, -71.7, -10.0), Extents: (700.0, 163.6, 350.0)
            }

            var game = LocalGameUtils.Get();
            if (game == null)
                return;

            var levelPhysicsSettings = GClass387.GetAllComponentsOfType<LevelPhysicsSettings>(false);
            Console.WriteLine($"LevelPhysicsSettings count: {levelPhysicsSettings.Count}");
            GClass806.SetupPositionQuantizer(levelPhysicsSettings.ToArray()[0].GetGlobalBounds());

            game.PlayerOwner.Player.GClass1544_0.SetExamined(true); // !!!!
            game.BotsController.DestroyInfo(game.PlayerOwner.Player);
            Singleton<GameWorld>.Instance.UnregisterPlayer(game.PlayerOwner.Player);
        }

        public static void EnableLogs()
        {
            AbstractLogger.IsLogsEnabled = true;
            AbstractLogger.UnityDebugLogsEnabled = true;
            Debug.unityLogger.logEnabled = true;
            Application.SetStackTraceLogType(LogType.Assert, StackTraceLogType.Full);
            Application.SetStackTraceLogType(LogType.Error, StackTraceLogType.Full);
            Application.SetStackTraceLogType(LogType.Exception, StackTraceLogType.Full);
            Application.SetStackTraceLogType(LogType.Warning, StackTraceLogType.Full);
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.Full);
        }

        private void StartLocalServer()
        {
            networkPort = ShortPort;

            if (!StartServer(Connection.GetConfiguration(), MaxConnections))
            {
                Debug.Log("No server created, error on the configuration definition");
                return;
            }

            Debug.Log(" ServerManager Event StartLocalServer()");
            Handlers.RegisterServerHandlers();
        }

        public override void OnServerReady(NetworkConnection conn)
        {
            Console.WriteLine(" OnSceneReady called on client ");
            Handlers.OnSceneReady(conn);
            base.OnServerReady(conn);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            DeletePlayer(conn.connectionId);
            base.OnServerDisconnect(conn);
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            DeletePlayer(conn.connectionId);
            base.OnClientDisconnect(conn);
        }

        private void DeletePlayer(int connectionId)
        {
            if (NetworkClients.TryRemove(connectionId, out var player))
            {
                player.Dispose();
                AssetPoolObject.ReturnToPool(player.gameObject, true);

                if (GameSessions.TryRemove(player.channelIndex, out var session))
                {
                    session.connection.Close("Disconnected", false);
                }
            }
        }

        public static int GetNextChannelId()
        {
            var id = ++NextChanelId;
            NextChanelId = (id % 2 == 0) ? ++id : id;

            Console.WriteLine($"Created New Channel ID:{id}");
            return id;
        }

        void OnApplicationQuit()
        {
            NetworkServer.Shutdown();
        }

        public ulong LocalIndex { get; private set; }
        public double LocalTime { get; private set; }
    }
}
