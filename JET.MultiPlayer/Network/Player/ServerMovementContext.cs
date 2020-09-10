using System;
using EFT;
using UnityEngine;

namespace ServerLib.Network.Player
{
    public class ServerMovementContext : GClass1001
    {
        public byte CommandMask { get; private set; }

        public static new ServerMovementContext Create(EFT.Player player, Func<GInterface6> animatorGetter,
            Func<GInterface19> characterControllerGetter, LayerMask groundMask)
        {
            return smethod_0<ServerMovementContext>(player, animatorGetter, characterControllerGetter, groundMask);
        }

        public override void PlayerAnimatorEnableInert(bool enabled)
        {
            this.CommandMask = (enabled
                ? NetworkPlayer.EMovementCommand.MoveStart.AddToMask(this.CommandMask)
                : NetworkPlayer.EMovementCommand.MoveEnd.AddToMask(this.CommandMask));
            base.PlayerAnimatorEnableInert(enabled);
        }

        public override void PlayerAnimatorEnableSprint(bool enabled)
        {
            this.CommandMask = (enabled
                ? NetworkPlayer.EMovementCommand.SprintStart.AddToMask(this.CommandMask)
                : NetworkPlayer.EMovementCommand.SprintEnd.AddToMask(this.CommandMask));
            base.PlayerAnimatorEnableSprint(enabled);
        }

        public override void PlayerAnimatorEnableJump(bool enabled)
        {
            this.CommandMask = (enabled
                ? NetworkPlayer.EMovementCommand.JumpStart.AddToMask(this.CommandMask)
                : NetworkPlayer.EMovementCommand.JumpEnd.AddToMask(this.CommandMask));
            base.PlayerAnimatorEnableJump(enabled);
        }

        public override void PlayerAnimatorEnableProne(bool enabled)
        {
            this.CommandMask = (enabled
                ? NetworkPlayer.EMovementCommand.StartProne.AddToMask(this.CommandMask)
                : NetworkPlayer.EMovementCommand.StopProne.AddToMask(this.CommandMask));
            base.PlayerAnimatorEnableProne(enabled);
        }

        public void ClearCommandMask()
        {
            this.CommandMask = 0;
        }
    }
}
