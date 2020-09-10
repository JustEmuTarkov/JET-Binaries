using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comfort.Common;
using EFT;
using EFT.Ballistics;
using EFT.HealthSystem;
using EFT.InventoryLogic;
using EFT.NetworkPackets;
using ServerLib.Network.Messages;
using ServerLib.Utils.Server;
using UnityEngine;
using AssetsPool = GClass1082;

namespace ServerLib.Network.Player
{
    public class ServerPlayer : ObservedPlayer
    {
        public GStruct128 currentPacket;
        public GClass1544 InventoryController => base._inventoryController;
        private Queue<uint> _confirmQueue = new Queue<uint>();

        public static ServerPlayer Create(int playerId, Vector3 position, GInterface48 frameIndexer)
        {
            var player = smethod_2<ServerPlayer>(
                GClass801.PLAYER_BUNDLE_NAME,
                playerId, position,
                "Observed_",
                frameIndexer,
                EUpdateQueue.Update, EUpdateMode.Manual, EUpdateMode.Auto,
                GClass266.Config.CharacterController.ObservedPlayerMode,
                () => 1f, () => 1f
            );

            player.EnabledAnimators = 0;
            if (player._triggerColliderSearcher != null)
            {
                player._triggerColliderSearcher.OnEnter += player.method_99;
                player._triggerColliderSearcher.OnExit += player.method_100;
            }

            player._armsUpdateQueue = EUpdateQueue.FixedUpdate;
            player.ginterface90_0 = new GClass1128(player);
            player.method_82();

            method_4(player);

            return player;
        }

        static void method_4(EFT.Player observedPlayer)
        {
            foreach (var player in Singleton<GameWorld>.Instance.RegisteredPlayers)
            {
                Collider collider = observedPlayer.CharacterControllerCommon.GetCollider();
                if (player == null)
                {
                    Console.WriteLine($"method_4 Error while IgnoreCollision observedPlayer2 is null!!!!!!!!!!!!!!!!!");
                    continue;
                }

                Collider collider2 = player.CharacterControllerCommon.GetCollider();
                GClass306.IgnoreCollision(collider, collider2, true);
            }
        }

        public async Task Init(PlayerSpawnMessage spawnMessage)
        {
            var profile = spawnMessage.Profile.Clone();
            profile.Inventory = spawnMessage.Inventory;
            gclass656_0 = new GClass656(1);

            await Singleton<AssetsPool>.Instance.LoadBundlesAndCreatePools(
                AssetsPool.PoolsCategory.Raid, AssetsPool.AssemblyType.Local,
                profile.GetAllPrefabPaths().ToArray(), // hope its good one ... profile.GetAllPrefabPaths().ToArray(),
                GClass593.General
            );

            await Init(spawnMessage.Rotation, "Player", EPointOfView.ThirdPerson, profile,
                () => new ServerInventoryController(this, profile),
                () => new ServerHealthController(profile.Health, this, true),
                new GClass1075(),
                null
            );

            this.method_95(); // need patch to public

            if (GClass266.Config.UseSpiritPlayer)
            {
                this.Spirit.ConnectToPlayerEvents();
            }

            SpawnController(() => EmptyHandsController.smethod_5<ObservedEmptyHandsController>(this), null);

            if (base.NeverInited)
            {
                method_98(); // need patch to public
            }

            Location = Singleton<ServerInstance>.Instance.mapSettings._Id;
            _healthController.DiedEvent += base.OnDead;
        }

        public void ProcessStartSearchContent(string itemId, Callback callback)
        {
            if (this.FindItemById(itemId).OrElse((Item) null) is GClass1486 item)
            {
                this._inventoryController.SearchContents(item, callback);
            }
            else
                Console.WriteLine("ERROR!!! ProcessStartSearchingAction FindItemById result is null");
        }

        protected override void ProcessStopSearching(string itemId)
        {
            Console.WriteLine($"ProcessStopSearching item {itemId}");
            InventoryController.StopSearching(itemId);

            currentPacket.CommonPacket ??= new GClass1057();
            currentPacket.CommonPacket.AddStopSearchingPacket(itemId);
            currentPacket.CommonPacket.AddStopSearchingActionPacket(itemId);
        }

        public void EnqueueConfirmCallBack(uint callbackId)
        {
            _confirmQueue.Enqueue(callbackId);
        }

        private void ProceedCallBackFromQueue(string error, int errorCode)
        {
            Console.WriteLine("ERROR!!!!! ServerPlayer.ProceedCallBackFromQueue()");
            if (_confirmQueue.Count <= 0)
            {
                Console.WriteLine(
                    "ERROR!!!!! ServerPlayer.ProceedCallBackFromQueue() _confirmQueue count is less that 0 "
                );
                return;
            }

            var callbackId = _confirmQueue.Dequeue();

            currentPacket.AddClientConfirmCallbackPacket(
                callbackId, error, _inventoryController.Inventory.CreateInventoryHashSum(), (byte) errorCode
            );
        }

        public override void ApplyDamageInfo(GStruct202 damageInfo, EBodyPart bodyPartType, EDamageType damageType,
            float absorbed, EHeadSegment? headSegment = null)
        {
            currentPacket.AddHitInfo(
                new GStruct131 {BodyPart = bodyPartType, DamageType = damageType, Damage = damageInfo.Damage}
            );
            currentPacket.AddDetailedHitInfo(
                (int) Math.Floor(damageInfo.Damage), (int) Math.Floor(absorbed), 1, bodyPartType,
                MaterialType.None
            );
            base.ApplyDamageInfo(damageInfo, bodyPartType, damageType, absorbed, headSegment);
        }

        protected override void OnArmorPointsChanged(RepairableComponent armor, bool children = false)
        {
            currentPacket.UpdateArmor(new GStruct130 {Id = armor.Item.Id, Durability = armor.Durability});
            base.OnArmorPointsChanged(armor, children);
        }

        public override GClass995 ApplyShot(GStruct202 damageInfo, EBodyPart bodyPartType, GStruct201 shotId)
        {
            Console.WriteLine($"ServerPlayer shotApply");
            return base.ApplyShot(damageInfo, bodyPartType, shotId);
        }

        protected override void CreateMovementContext()
        {
            base.CreateMovementContext();
            this.AIData = new GClass204(null, this);
        }

        public override void ManualUpdate(float deltaTime, int loop = 1)
        {
            this.LastDeltaTime = deltaTime;

            if (!(this._healthController is GClass1269))
            {
                this._healthController?.ManualUpdate(deltaTime);
            }

            base.ManualUpdate(deltaTime, loop);
        }

        public void ProcessPacket(GStruct90 clientPacket, float prevTime)
        {
            clientPacket.FirearmPacket.ShotsForApprovement = new OneAndList<ShotInfo>();
            clientPacket.FirearmPacket.FiredShotInfos = null;

            var handsPacket = clientPacket.HandsChangePacket;
            var itemId = handsPacket.ItemId;
            switch (handsPacket.OperationType)
            {
                case GStruct103.EOperationType.None:
                    break;
                case GStruct103.EOperationType.Drop:
                    DropCurrentController(() => { }, false);
                    break;
                case GStruct103.EOperationType.FastDrop:
                    DropCurrentController(() => { }, true);
                    break;
                case GStruct103.EOperationType.CreateEmptyHands:
                    EnqueueConfirmCallBack(handsPacket.CallbackId);
                    SpawnController(
                        () => EmptyHandsController.smethod_5<ObservedEmptyHandsController>(this),
                        null
                    );
                    break;
                case GStruct103.EOperationType.CreateFirearm:
                    if (this.HandsController == null || this.HandsController.Item.Id != itemId)
                    {
                        var stationaryWeapon = MovementContext.StationaryWeapon;
                        var fireArmItem = (!(stationaryWeapon != null) || stationaryWeapon.Item.Id != itemId)
                            ? this.GClass1544_0.Inventory.Equipment.FindItem(itemId)
                            : MovementContext.StationaryWeapon.Item;

                        if (fireArmItem == null) break;

                        EnqueueConfirmCallBack(handsPacket.CallbackId);
                        this.SpawnController(
                            () => FirearmController.smethod_5<ObservedFirearmController>(this,
                                (Weapon) fireArmItem),
                            fireArmItem
                        );

                        return;
                    }

                    break;
                case GStruct103.EOperationType.CreateGrenade:
                    if (this.GClass1544_0.Inventory.Equipment.FindItem(itemId) is GClass1525 grenadeItem)
                    {
                        EnqueueConfirmCallBack(handsPacket.CallbackId);
                        SpawnController(
                            () => GrenadeController.smethod_8<ObservedGrenadeController>(this, grenadeItem),
                            grenadeItem
                        );
                    }

                    break;
                case GStruct103.EOperationType.CreateMeds:
                    var medItem = FindItemById(handsPacket.ItemId);
                    if (medItem.Error == null)
                    {
                        EnqueueConfirmCallBack(handsPacket.CallbackId);
                        SpawnController(
                            () => MedsController.smethod_5<ObservedMedsController>(
                                this, medItem.Value, handsPacket.MedsBodyPart,
                                handsPacket.MedsAmount, handsPacket.AnimationVariant
                            ),
                            medItem.Value
                        );
                    }

                    break;
                case GStruct103.EOperationType.CreateKnife:
                    var knifeItem = InventoryController.Inventory.Equipment
                        .FindItem<KnifeComponent>(handsPacket.ItemId);
                    if (knifeItem != null)
                    {
                        EnqueueConfirmCallBack(handsPacket.CallbackId);
                        SpawnController(
                            () => KnifeController.smethod_8<ObservedKnifeController>(this, knifeItem),
                            knifeItem.Item
                        );
                    }

                    break;
                case GStruct103.EOperationType.CreateQuickGrenadeThrow:
                    if (FindInEquipment(handsPacket.ItemId) is GClass1525 quickGrenadeItem)
                    {
                        EnqueueConfirmCallBack(handsPacket.CallbackId);
                        SpawnController(
                            () => QuickGrenadeThrowController.smethod_8<ObservedQuickGrenadeThrowController>(
                                this, quickGrenadeItem
                            ),
                            quickGrenadeItem
                        );
                    }

                    break;
                case GStruct103.EOperationType.CreateQuickKnifeKick:
                    var quickKnifeItem =
                        InventoryController.Inventory.Equipment.FindItem<KnifeComponent>(handsPacket.ItemId);
                    if (quickKnifeItem != null)
                    {
                        EnqueueConfirmCallBack(handsPacket.CallbackId);
                        SpawnController(
                            () => QuickKnifeKickController.smethod_8<ObservedQuickKnifeKickController>(
                                this,
                                quickKnifeItem
                            ),
                            quickKnifeItem.Item
                        );
                    }

                    break;
                case GStruct103.EOperationType.CreateQuickUseItem:
                    var quickUseItem = FindInEquipment(handsPacket.ItemId);
                    if (quickUseItem != null)
                    {
                        EnqueueConfirmCallBack(handsPacket.CallbackId);
                        SpawnController(
                            () => QuickUseItemController.smethod_5<QuickUseItemController>(this, quickUseItem),
                            quickUseItem
                        );
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            clientPacket.HandsChangePacket.OperationType = GStruct103.EOperationType.None;

            var frame = new GStruct127(
                clientPacket.FrameId,
                new GStruct126 {IsAlive = true},
                clientPacket.MovementInfoPacket,
                clientPacket.HandsChangePacket,
                clientPacket.HandsTypePacket,
                clientPacket.FirearmPacket,
                clientPacket.GrenadePacket,
                clientPacket.KnifePacket,
                new List<GStruct116>(),
                clientPacket.PhraseCommandPacket,
                null, null, null, null
            ) {FrameIndex = clientPacket.FrameId, Time = prevTime + clientPacket.DeltaTime};

            this.method844(frame, clientPacket.DeltaTime);
        }
        /*// Token: 0x060061E5 RID: 25061
        private void method_84(GStruct56<GStruct127, GClass656, GStruct35> interpolationSpan)
        {
            GStruct127 gstruct = interpolationSpan.Interpolate();
            float deltaTime = interpolationSpan.GetDeltaTime();
            this.method_844(gstruct, deltaTime);
        }*/
        /*// Token: 0x0600B769 RID: 46953
		public void method_844(GStruct127 gstruct, float deltaTime)
		{
			if (gstruct.FrameIndex > 0UL)
			{
				this.LastServerFrameId = gstruct.FrameIndex;
			}
			bool flag;
			if (flag = ObservedPlayer.smethod_5(gstruct))
			{
				try
				{
					this.method_91(gstruct.HandsChangePacket);
				}
				catch (Exception ex)
				{
					GClass1120.TraceError(ETraceCode.ObserverProcessHandsChangePacketException, new string[]
					{
						base.ProfileId,
						ex.ToString()
					});
					Debug.LogException(ex);
				}
			}
			if (gstruct.HandsTypePacket != EHandsTypePacket.None)
			{
				try
				{
					ObservedPlayer.GInterface89 ginterface = this.HandsController as ObservedPlayer.GInterface89;
					if (ginterface == null)
					{
						throw new InvalidOperationException(string.Concat(new object[]
						{
							"Profile.Info.Nickname:",
							base.Profile.Info.Nickname,
							" framePlayerInfo.HandsTypePacket:",
							gstruct.HandsTypePacket,
							" ",
							this.HandsController
						}));
					}
					ginterface.ProcessPlayerPacket(gstruct);
				}
				catch (Exception ex2)
				{
					GClass1120.TraceError(ETraceCode.ObserverProcessPlayerPacketException, new string[]
					{
						base.ProfileId,
						ex2.ToString()
					});
					Debug.LogException(ex2);
				}
			}
			try
			{
				if (this._handsController != null)
				{
					this._handsController.ManualUpdate(deltaTime);
				}
				if ((this.EnabledAnimators & Player.EAnimatorMask.Arms) != (Player.EAnimatorMask)0)
				{
					base.ArmsAnimatorCommon.Update(deltaTime);
				}
				this._armsupdated = true;
				this._armsTime = deltaTime;
				if (this._handsController != null)
				{
					this._handsController.EmitEvents();
				}
			}
			catch (Exception ex3)
			{
				GClass1120.TraceError(ETraceCode.ObserverProcessInterpolationSpanException, new string[]
				{
					base.ProfileId,
					ex3.ToString()
				});
				Debug.LogException(ex3);
			}
			if (!flag)
			{
				try
				{
					this.method_91(gstruct.HandsChangePacket);
				}
				catch (Exception ex4)
				{
					GClass1120.TraceError(ETraceCode.ObserverProcessHandsChangePacketException, new string[]
					{
						base.ProfileId,
						ex4.ToString()
					});
					Debug.LogException(ex4);
				}
			}
			if (gstruct.InventoryCommandPackets != null)
			{
				for (int i = 0; i < gstruct.InventoryCommandPackets.Count; i++)
				{
					GStruct116 gstruct2 = gstruct.InventoryCommandPackets[i];
					try
					{
						GStruct116.ETag tag = gstruct2.Tag;
						if (tag != GStruct116.ETag.Command)
						{
							if (tag != GStruct116.ETag.Status)
							{
								throw new ArgumentOutOfRangeException();
							}
							GStruct132 statusPacket = gstruct2.StatusPacket;
							this.method_104((uint)statusPacket.Id, statusPacket.Status, statusPacket.Error, 0, false);
						}
						else
						{
							this.method_127(gstruct2.CommandPacket.CommandBytes);
						}
					}
					catch (Exception ex5)
					{
						GClass1120.TraceError(ETraceCode.ObserverInventoryCommandPacketException, new string[]
						{
							base.ProfileId,
							ex5.ToString()
						});
						Debug.LogException(ex5);
					}
				}
			}
			if (gstruct.PhraseCommandPacket.HasPhrase)
			{
				try
				{
					this.Speaker.PlayDirect(gstruct.PhraseCommandPacket.PhraseCommand, gstruct.PhraseCommandPacket.PhraseId);
				}
				catch (Exception ex6)
				{
					GClass1120.TraceError(ETraceCode.ObserverSpeakerPlayDirectException, new string[]
					{
						base.ProfileId,
						ex6.ToString()
					});
					Debug.LogException(ex6);
				}
			}
			if (gstruct.CommonPacket != null)
			{
				base.ProcessCommonPacket(gstruct.CommonPacket);
			}
			for (GStruct165? gstruct3 = gstruct.SyncHealthPacket; gstruct3 != null; gstruct3 = gstruct3.Value.GetNested<GStruct165>())
			{
				base.NetworkHealthController.HandleSyncPacket(gstruct3.Value);
			}
			if (!gstruct.DeathPacket.IsAlive && !this.bool_29)
			{
				this.bool_29 = true;
				this.method_85(gstruct.DeathPacket);
			}
			if (gstruct.DeathInventorySyncPacket != null)
			{
				try
				{
					this.method_89(gstruct.DeathInventorySyncPacket.GetValueOrDefault().Bytes);
				}
				catch (Exception ex7)
				{
					GClass1120.TraceError(ETraceCode.ObserverProcessDeathInventorySyncException, new string[]
					{
						base.ProfileId,
						ex7.ToString()
					});
					Debug.LogException(ex7);
				}
			}
			try
			{
				this.gclass1001_0.ProcessFrame(gstruct, deltaTime);
			}
			catch (Exception ex8)
			{
				GClass1120.TraceError(ETraceCode.ObserverProcessFrameException, new string[]
				{
					base.ProfileId,
					ex8.ToString()
				});
				Debug.LogException(ex8);
			}
			bool isDisconnected;
			if ((isDisconnected = gstruct.IsDisconnected) != this.bool_28)
			{
				this.bool_28 = isDisconnected;
				if (this.bool_28)
				{
					this.method_106();
					return;
				}
				this.method_107();
			}
		}*/
        private Item FindInEquipment(string itemId)
        {
            return this.GClass1544_0.Inventory.Equipment.FindItem(itemId);
        }

        void SpawnController(Func<AbstractHandsController> controllerFactory, Item item)
        {
            var setInHandsOperation = ((item != null) ? base.method_62(item) : null);
            setInHandsOperation?.Confirm();

            if (this.HandsController != null)
            {
                AbstractHandsController handsController = this.HandsController;
                this.HandsController.FastForwardCurrentState();
                if (this.HandsController != handsController && this.HandsController != null)
                {
                    this.HandsController.FastForwardCurrentState();
                }

                this.HandsController.Destroy();
                this.HandsController = null;
            }

            base.SpawnController(controllerFactory(), () =>
            {
                setInHandsOperation?.Dispose();
                ProceedCallBackFromQueue(String.Empty, 0);
            });
        }

        public byte channelIndex;
        public override byte ChannelIndex => channelIndex;
        public NetworkGameSession Session;
    }
}
