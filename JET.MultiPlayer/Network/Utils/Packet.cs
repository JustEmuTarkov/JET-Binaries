using EFT;
using OperationType = GStruct103;
using FramePlayerInfo = GStruct127;
namespace ServerLib.Network.Utils
{
    public class Packet
    {
        public static GStruct90 Clone(GStruct90 packet)
        {
            return new GStruct90()
            {
                CancelApplyingItemPacket = packet.CancelApplyingItemPacket,
                DeltaTime = packet.DeltaTime,
                DevelopEnableServerHitDebuggingPacket = packet.DevelopEnableServerHitDebuggingPacket,
                DevelopHealPacket = packet.DevelopHealPacket,
                DevelopKillMePacket = packet.DevelopKillMePacket,
                DevelopPrintSkillsPacket = packet.DevelopPrintSkillsPacket,
                DevelopSetDamageCoeffPacket = packet.DevelopSetDamageCoeffPacket,
                DevelopTeleportPacket = packet.DevelopTeleportPacket,
                FirearmPacket = packet.FirearmPacket,
                FrameId = packet.FrameId,
                KnifePacket = packet.KnifePacket,
                GrenadePacket = packet.GrenadePacket,
                MovementInfoPacket = packet.MovementInfoPacket,
                HandsChangePacket = packet.HandsChangePacket,
                HandsTypePacket = packet.HandsTypePacket,
                InventoryCommandPackets = packet.InventoryCommandPackets,
                PhraseCommandPacket = packet.PhraseCommandPacket,
                StartSearchContentPacket = packet.StartSearchContentPacket,
                StopSearchContentPacket = packet.StopSearchContentPacket,
                RTT = packet.RTT
            };
        }

        public static bool HandsPacketHasImportantData(FramePlayerInfo framePlayerInfo)
        {
            EHandsTypePacket handsTypePacket = framePlayerInfo.HandsTypePacket;
            OperationType.EOperationType operationType = framePlayerInfo.HandsChangePacket.OperationType;

            if (handsTypePacket == EHandsTypePacket.Firearm && operationType == OperationType.EOperationType.CreateFirearm)
                return true;

            if (handsTypePacket == EHandsTypePacket.Grenade)
            {
                if (operationType == OperationType.EOperationType.CreateGrenade ||
                    operationType == OperationType.EOperationType.CreateQuickGrenadeThrow)
                {
                    return true;
                }
            }

            return handsTypePacket == EHandsTypePacket.Knife &&
                   (operationType == OperationType.EOperationType.CreateKnife ||
                    operationType == OperationType.EOperationType.CreateQuickKnifeKick);
        }
    }
}
