using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comfort.Common;
using EFT;
using EFT.HealthSystem;
using EFT.Interactive;
using EFT.InventoryLogic;
using EFT.NetworkPackets;
using ServerLib.Utils.Game;
using UnityEngine;
using UnityEngine.Serialization;

namespace ServerLib.Network.Bots
{
    public class BotPlayer : NetworkPlayer
    {
        public GClass1036 movementContext;
        public override byte ChannelIndex { get; }
        public GStruct105 lootInteractionPacket;
        public List<GStruct116> inventoryCommands;
        public GStruct134 handsChangePacket;
        public GStruct105 interactWithDoorPacket;
        public GStruct190? syncHealthPacket;
        public PhraseCommandPacket phraseCommandPacket;
        public GStruct94 lastMovementContext;

        public override AbstractHandsController HandsController
        {
            get
            {
                return this._handsController;
            }
            protected set
            {
                this._handsController = value;
                var animationType = this.GetWeaponAnimationType(this._handsController);
                this.MovementContext.PlayerAnimatorSetWeaponId(animationType);
            }
        }

        public static async Task<BotPlayer> Create(Profile profile)
        {
            int playerId = ServerInstance.GetNextChannelId();
            EUpdateMode armsUpdateMode = EUpdateMode.Auto;
            if (GClass310.Config.UseHandsFastAnimator)
            {
                armsUpdateMode = EUpdateMode.Manual;
            }

            var player = smethod_2<BotPlayer>(
                GClass829.PLAYER_BUNDLE_NAME, playerId,
                Vector3.zero, "Player",
                Singleton<ServerInstance>.Instance,
                LocalGameUtils.Get().UpdateQueue,
                armsUpdateMode, EUpdateMode.Auto, GClass310.Config.CharacterController.BotPlayerMode,
                () => 1f,
                () => 1f
            );

            await player.Init(Quaternion.identity, "Player", EPointOfView.ThirdPerson, profile,
                () => new BotInventoryController(player, profile),
                () => new BotHeathController(profile.Health, player, true),
                new GClass1121(),
                null, false
            );

            foreach (GClass1585 magazine in player.Inventory.NonQuestItems.OfType<GClass1477>())
            {
                player._inventoryController.StrictCheckMagazine(
                    magazine, true, player.Profile.MagDrillsMastering, false, false
                );
            }

            player._handsController = EmptyHandsController.smethod_5<ClientEmptyHandsController>(player);
            player._handsController.Spawn(1f, () => { Console.WriteLine("BotPlayer has been spawned"); });

            player.AIData = new GClass204(null, player);
            player.PlacingBeacon = false;
            player.AggressorFound = false;
            player._animators[0].enabled = true;

            player.HealthController.DiedEvent += player.OnDiedEvent;

            return player;
        }

        private void OnDiedEvent(EDamageType obj)
        {
            this.HealthController.DiedEvent -= OnDiedEvent;

            lastMovementContext = new GStruct94(0,
                MovementContext.TransformPosition, MovementContext.Rotation,
                movementContext.CommandMask, MovementContext.CurrentState.Name, CurrentAnimatorStateIndex,
                GClass986.ConvertToMovementDirection(MovementContext.MovementDirection),
                MovementContext.PoseLevel,
                MovementContext.ClampSpeed(MovementContext.CharacterMovementSpeed),
                MovementContext.Tilt,
                MovementContext.Step, MovementContext.BlindFire, interactWithDoorPacket,
                lootInteractionPacket, new GStruct96(), new GStruct97(),
                MovementContext.SoftSurface, HeadRotation, Physical.SerializationStruct
            );
        }

        public void AddInventoryCommand(GStruct116 command)
        {
            inventoryCommands ??= new List<GStruct116>();
            inventoryCommands.Add(command);
        }

        protected override void OnPhraseTold(EPhraseTrigger phrase, TaggedClip clip, TagBank bank, GClass1093 speaker)
        {
            this.phraseCommandPacket = new PhraseCommandPacket
            {
                HasPhrase = true, PhraseCommand = phrase, PhraseId = clip.NetId
            };
        }

        protected override void TriggerPhraseCommand(EPhraseTrigger phrase, int netPhraseId)
        {
            this.phraseCommandPacket = new PhraseCommandPacket
            {
                HasPhrase = true, PhraseCommand = phrase, PhraseId = netPhraseId
            };
        }

        public override void TryInteractionCallback(LootableContainer container)
        {
            if (container == null) return;

            base.TryInteractionCallback(container);

            this.interactWithDoorPacket.Id = container.Id;
            this.interactWithDoorPacket.EInteractionType = EInteractionType.Close;
            this.interactWithDoorPacket.Execute = EInteractionStage.Ignore;
        }

        public override GClass395 CreatePhysical()
        {
            return new GClass396();
        }

        internal override void vmethod_1(WorldInteractiveObject door, GClass1673 interactionResult, Action callback)
        {
            this.interactWithDoorPacket.HasInteraction = true;
            this.interactWithDoorPacket.Id = door.Id;
            this.interactWithDoorPacket.EInteractionType = interactionResult.InteractionType;
            this.interactWithDoorPacket.Execute = EInteractionStage.Start;
            this.interactWithDoorPacket.ItemId = ((interactionResult is GClass1674 result)
                ? result.Key.Item.Id
                : string.Empty);
            base.vmethod_1(door, interactionResult, callback);
        }

        internal override void vmethod_2(WorldInteractiveObject door, GClass1673 interactionResult)
        {
            base.vmethod_2(door, interactionResult);
            this.interactWithDoorPacket.HasInteraction = true;
            this.interactWithDoorPacket.Id = door.Id;
            this.interactWithDoorPacket.EInteractionType = interactionResult.InteractionType;
            this.interactWithDoorPacket.Execute = EInteractionStage.Execute;
            this.interactWithDoorPacket.ItemId = ((interactionResult is GClass1674 result)
                ? result.Key.Item.Id
                : string.Empty);
            base.UpdateInteractionCast();
        }

        protected override void Proceed(Item item, Callback<GInterface80> callback, bool scheduled = true)
        {
            QuickUseItemController Factory() =>
                QuickUseItemController.smethod_5<QuickUseItemController>(this, item);

            new Process<QuickUseItemController, GInterface80>(this, Factory, item, true,
                    AbstractProcess.Completion.Sync, AbstractProcess.Confirmation.Succeed, false)
                .method_0((err) =>
                    {
                        this.handsChangePacket.OperationType = GStruct103.EOperationType.CreateQuickUseItem;
                        this.handsChangePacket.CallbackId = 0;
                        this.handsChangePacket.ItemId = item.Id;
                    }, callback, scheduled
                );
        }

        protected override void Proceed(KnifeComponent knife, Callback<GInterface79> callback, bool scheduled = true)
        {
            ClientQuickKnifeKickController Factory() =>
                QuickKnifeKickController.smethod_8<ClientQuickKnifeKickController>(this, knife);

            new Process<ClientQuickKnifeKickController, GInterface79>(this, Factory, knife.Item).method_0(
                (err) =>
                {
                    this.handsChangePacket.OperationType = GStruct103.EOperationType.CreateQuickKnifeKick;
                    this.handsChangePacket.CallbackId = 0;
                    this.handsChangePacket.ItemId = knife.Item.Id;
                }, callback, scheduled
            );
        }

        protected override void Proceed(GClass1525 throwWeap, Callback<GInterface78> callback, bool scheduled = true)
        {
            ClientQuickGrenadeThrowController Factory() =>
                QuickGrenadeThrowController.smethod_8<ClientQuickGrenadeThrowController>(this, throwWeap);

            new Process<ClientQuickGrenadeThrowController, GInterface78>(this, Factory, throwWeap).method_0(
                (err) =>
                {
                    this.handsChangePacket.OperationType = GStruct103.EOperationType.CreateQuickGrenadeThrow;
                    this.handsChangePacket.CallbackId = 0;
                    this.handsChangePacket.ItemId = throwWeap.Id;
                }, callback, scheduled
            );
        }

        protected override void Proceed(KnifeComponent knife, Callback<GInterface75> callback, bool scheduled = true)
        {
            ClientKnifeController Factory() => KnifeController.smethod_8<ClientKnifeController>(this, knife);

            new Process<ClientKnifeController, GInterface75>(this, Factory, knife.Item).method_0(
                (err) =>
                {
                    this.handsChangePacket.OperationType = GStruct103.EOperationType.CreateKnife;
                    this.handsChangePacket.CallbackId = 0;
                    this.handsChangePacket.ItemId = knife.Item.Id;
                }, callback, scheduled
            );
        }

        protected override void Proceed(GClass1507 foodDrink, float amount, Callback<GInterface76> callback,
            int animationVariant, bool scheduled = true)
        {
            MedsController Factory() => MedsController.smethod_5<ClientMedsController>(
                this, foodDrink, EBodyPart.Head, amount, animationVariant
            );

            new Process<MedsController, GInterface76>(this, Factory, foodDrink).method_0(
                (err) =>
                {
                    this.handsChangePacket.OperationType = GStruct103.EOperationType.CreateMeds;
                    this.handsChangePacket.CallbackId = 0;
                    this.handsChangePacket.ItemId = foodDrink.Id;
                    this.handsChangePacket.MedsBodyPart = EBodyPart.Head;
                    this.handsChangePacket.MedsAmount = amount;
                    this.handsChangePacket.AnimationVariant = animationVariant;
                }, callback, scheduled
            );
        }

        protected override void Proceed(GClass1516 meds, EBodyPart bodyPart, Callback<GInterface76> callback,
            int animationVariant, bool scheduled = true)
        {
            MedsController Factory() => MedsController.smethod_5<ClientMedsController>(
                this, meds, bodyPart, 1f, animationVariant
            );

            new Process<MedsController, GInterface76>(this, Factory, meds).method_0(
                (err) =>
                {
                    this.handsChangePacket.OperationType = GStruct103.EOperationType.CreateMeds;
                    this.handsChangePacket.CallbackId = 0;
                    this.handsChangePacket.ItemId = meds.Id;
                    this.handsChangePacket.MedsBodyPart = bodyPart;
                    this.handsChangePacket.MedsAmount = 1f;
                    this.handsChangePacket.AnimationVariant = animationVariant;
                }, callback, scheduled
            );
        }

        protected override void Proceed(GClass1525 throwWeap, Callback<GInterface74> callback, bool scheduled = true)
        {
            BotGrenadeController Factory() => BotGrenadeController.Create(this, throwWeap);
            new Process<GrenadeController, GInterface74>(this, Factory, throwWeap).method_0(
                (err) =>
                {
                    this.handsChangePacket.OperationType = GStruct103.EOperationType.CreateGrenade;
                    this.handsChangePacket.CallbackId = 0;
                    this.handsChangePacket.ItemId = throwWeap.Id;
                }, callback, scheduled
            );
        }

        protected override void Proceed(Weapon weapon, Callback<GInterface73> callback, bool scheduled = true)
        {
            BotFireArmsController Factory() => BotFireArmsController.Create(this, weapon);

            new Process<FirearmController, GInterface73>(this, Factory, weapon).method_0(
                err =>
                {
                    this.handsChangePacket.OperationType = GStruct103.EOperationType.CreateFirearm;
                    this.handsChangePacket.CallbackId = 0;
                    this.handsChangePacket.ItemId = weapon.Id;
                },
                callback, scheduled
            );
        }

        protected override void Proceed(bool withNetwork, Callback<GInterface71> callback, bool scheduled = true)
        {
            ClientEmptyHandsController Factory() => EmptyHandsController.smethod_5<ClientEmptyHandsController>(this);

            new Process<EmptyHandsController, GInterface71>(this, Factory, null).method_0(x =>
            {
                this.handsChangePacket.OperationType = GStruct103.EOperationType.CreateEmptyHands;
                this.handsChangePacket.CallbackId = 0;
            }, callback, scheduled);
        }

        public override void Interact(GInterface166 loot, Callback callback)
        {
            base.Interact(loot, callback);
            this.lootInteractionPacket.Interact = true;
            this.lootInteractionPacket.LootId = loot.ID;
            this.lootInteractionPacket.CallbackId = 0;
        }

        protected override void DropCurrentController(Action callback, bool fastDrop)
        {
            this.handsChangePacket.OperationType =
                (fastDrop ? GStruct103.EOperationType.FastDrop : GStruct103.EOperationType.Drop);
            base.DropCurrentController(callback, fastDrop);
        }

        protected override void CreateMovementContext()
        {
            LayerMask movementMask = EFTHardSettings.Instance.MOVEMENT_MASK;
            this.movementContext = GClass1000.Create(
                this, base.GetBodyAnimatorCommon,
                base.GetCharacterControllerCommon, movementMask
            );
            base.MovementContext = this.movementContext;
        }

        public float LastDelta => LastDeltaTime;
    }
}
