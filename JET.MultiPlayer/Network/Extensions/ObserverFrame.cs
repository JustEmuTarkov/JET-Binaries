using UnityEngine;
using FramePacket = GStruct127;
using InteractionPacket = GStruct94;
using StatusPacket = GStruct126;
namespace ServerLib.Network.Extensions
{
    public static class ObserverFrame
    {
        public static bool EqualOrNotImportant(this FramePacket cur, FramePacket prev)
        {
            return cur.IsDisconnected == prev.IsDisconnected
                   && cur.DeathPacket.EqualsTo(prev.DeathPacket)
                   && !cur.PhraseCommandPacket.HasPhrase
                   && !cur.FirearmPacket.method_0()
                   && cur.HandsChangePacket.Equals(prev.HandsChangePacket)
                   && !cur.GrenadePacket.HasCommandsForHands()
                   && !cur.KnifePacket.HasCommandsForHands()
                   && cur.MovementInfo.EqualsTo(prev.MovementInfo)
                ;
        }

        public static bool EqualsTo(this InteractionPacket cur, InteractionPacket prev)
        {
            return cur.SoftSurface == prev.SoftSurface
                   && cur.Step == prev.Step
                   && cur.BlindFire == prev.BlindFire
                   && cur.CommandMask == prev.CommandMask
                   && cur.MovementDirection == prev.MovementDirection
                   && cur.EPlayerState == prev.EPlayerState
                   && Mathf.Approximately(cur.Tilt, prev.Tilt)
                   && Mathf.Approximately(cur.PoseLevel, prev.PoseLevel)
                   && Mathf.Approximately(cur.CharacterMovementSpeed, prev.CharacterMovementSpeed)
                   && V3Equal(cur.Rotation, prev.Rotation)
                   && V3Equal(cur.HeadRotation, prev.HeadRotation)
                   && V3Equal(cur.TransformPosition, prev.TransformPosition)
                   && cur.InteractWithDoorPacket.Equals(prev.InteractWithDoorPacket)
                   && cur.StationaryWeaponPacket.Equals(prev.StationaryWeaponPacket)
                   && cur.LootInteractionPacket.Equals(prev.LootInteractionPacket)
                   && cur.PlantItemPacket.Equals(prev.PlantItemPacket)
                ;
        }

        private static bool EqualsTo(this StatusPacket cur, StatusPacket prev)
        {
            return cur.IsAlive == prev.IsAlive
                   && cur.Level == prev.Level
                   && cur.Nickname == prev.Nickname
                   && cur.Side == prev.Side
                   && cur.Status == prev.Status
                   && cur.KillerName == prev.KillerName
                   && cur.WeaponName == prev.WeaponName
                   && cur.InventoryHashSum == prev.InventoryHashSum
                   && Mathf.Approximately((float) cur.Time, (float) prev.Time);
        }

        public static bool V3Equal(Vector3 a, Vector3 b)
        {
            return Vector3.SqrMagnitude(a - b) < 0.0001;
        }
    }
}
