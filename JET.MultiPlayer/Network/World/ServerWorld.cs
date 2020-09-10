using System;
using System.Collections.Concurrent;
using System.Linq;
using Comfort.Common;
using EFT;
using EFT.Interactive;
using EFT.InventoryLogic;
using ServerLib.Network.Utils;
using UnityEngine;
using WorldPacket = GStruct68;

#pragma warning disable 618

namespace ServerLib.Network.World
{
    internal class ServerWorld : ClientWorld
    {
        private void LateUpdate()
        {
            SynchronizeWorld();
        }

        private void SynchronizeWorld()
        {
            // send world state to all clients
            _packet.GrenadeSyncPackets.AddRange(gameWorld_0.GrenadesCriticalStates);
            if (_interactingDoors.Count > 0)
            {
                AddInteractiveObjectsStatusPacket();
            }

            if (!_packet.HasData)
                return;

            var sessions = Singleton<ServerInstance>.Instance.GameSessions;
            foreach (var session in sessions.Values)
            {
                session.SendWorldMessage(ref _packet, _previousPacket);
                _previousPacket.Dispose();
                _previousPacket = _packet.Clone();
                _packet = GStruct67.Create();
            }

            if (gameWorld_0.GrenadesCriticalStates.Count > 0) gameWorld_0.GrenadesCriticalStates.Clear();
        }

        public void AddInteractiveObjectsStatusPacket()
        {
            var segment = this.SerializeInteractiveObjectsStatus();
            base.AddInteractiveObjectsStatusPacket(segment);
        }

        public void ChangeLampState(int netId, Turnable.EState state)
        {
            var lampControllers = LocationScene.GetAllObjects<LampController>().ToArray();
            var lampController = lampControllers.FirstOrDefault(l => l.NetId == netId);
            if (lampController == null) return;

            lampController.LampState = state;
            AddLampChangeStatePacket(netId, state);
        }

        public WorldInteractiveObject[] GetInteractiveStatusInfos()
        {
            return worldInteractiveObject_0;
        }

        private void Awake()
        {
            // run only once on start
            Singleton<ServerWorld>.Create(this);
            var newWorldData = new ServerGameWorld(Singleton<GInterface43>.Instance);
            Singleton<GInterface43>.TryRelease(Singleton<GInterface43>.Instance);
            Singleton<GInterface43>.Create(newWorldData);

            Singleton<GClass301>.Instance.OnGrenadeExplosive += delegate(Vector3 position, EFT.Player player,
                bool smoke,
                float radius, float time)
            {
                Console.WriteLine("Singleton<GClass301>.Instance.OnGrenadeExplosive");
            };

            base.RegisterNetworkInteractionObjects();

            InitializeVariables();
            RegisterDelegates();
            Console.WriteLine("ServerWorld has been created!!!!!!");
        }

        private void RegisterDelegates()
        {
            foreach (var worldInteractiveObject in worldInteractiveObject_0)
            {
                worldInteractiveObject.OnDoorStateChanged += OnDoorStateChanged;
            }
        }

        private readonly ConcurrentDictionary<int, WorldInteractiveObject> _interactingDoors =
            new ConcurrentDictionary<int, WorldInteractiveObject>();

        private void OnDoorStateChanged(WorldInteractiveObject obj, EDoorState prevstate, EDoorState nextstate)
        {
            if (!_interactingDoors.ContainsKey(obj.NetId))
            {
                _interactingDoors.TryAdd(obj.NetId, obj);
            }
            else
            {
                if (_interactingDoors.TryRemove(obj.NetId, out var door))
                {
                }
            }

            Console.WriteLine($"OnDoorStateChanged prev state {prevstate} next state {nextstate}");
        }

        private void InitializeVariables()
        {
            this.list_0.Clear();
            foreach (var worldInteractiveObject in this.worldInteractiveObject_0)
            {
                this.list_0.Add(worldInteractiveObject.GetStatusInfo(true));
            }

            Singleton<GameWorld>.Instance.RegisterBorderZones();

            _packet = GStruct67.Create();
            _previousPacket = GStruct67.Create();
        }

        class ServerGameWorld : GInterface43
        {
            private readonly GInterface43 _gameWorld;

            public ServerGameWorld(GInterface43 instance)
            {
                _gameWorld = instance;
            }

            public GInterface184 CreateBallisticCalculator(int seed) => _gameWorld.CreateBallisticCalculator(seed);

            public void RemoveBallisticCalculator(Item weapon) => _gameWorld.RemoveBallisticCalculator(weapon);

            public void RegisterGrenade(Throwable grenade)
            {
                grenade.DestroyEvent += DestroyEvent;
                _gameWorld.RegisterGrenade(grenade);
            }

            private static void DestroyEvent(Throwable grenade)
            {
                GStruct74 netPacket = grenade.GetNetPacket();
                Singleton<GameWorld>.Instance.GrenadesCriticalStates.Add(netPacket);
            }

            public LootItem ThrowItem(Item item, EFT.Player player) => _gameWorld.ThrowItem(item, player);

            public LootItem SetupItem(Item item, EFT.Player player, Vector3 position, Quaternion rotation) =>
                _gameWorld.SetupItem(item, player, position, rotation);

            public void DestroyLoot(string id) => _gameWorld.DestroyLoot(id);

            public GClass330 GrenadeFactory => _gameWorld.GrenadeFactory;

            event Action<GInterface7> GInterface43.OnLootItemDestroyed
            {
                add => this._gameWorld.OnLootItemDestroyed += value;
                remove => this._gameWorld.OnLootItemDestroyed -= value;
            }
        }
    }
}
