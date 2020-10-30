using System.IO;
using ServerLib.Network.Player;

namespace ServerLib.Network.Packets
{
    public static class Extensions
    {
        public static GStruct55<GClass1700> ReadInventoryCommand(this GStruct117 command, ServerPlayer player)
        {
            GClass1700 operation;
            using (var memoryStream = new MemoryStream(command.CommandBytes))
            {
                using var reader = new BinaryReader(memoryStream);
                operation = player.ToInventoryOperation(reader.ReadPolymorph<GClass914>()).Value;
            }

            return operation;
        }
    }
}
