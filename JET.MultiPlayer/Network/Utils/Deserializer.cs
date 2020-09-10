using System;
using System.IO;
using EFT.InventoryLogic;
using UnityEngine;

namespace ServerLib.Network.Utils
{
    public static class Deserializer
    {
        public static GClass914 ReadInventoryCommand(byte[] bytes)
        {
            using var memoryStream = new MemoryStream(bytes);
            using var binaryReader = new BinaryReader(memoryStream);

            try
            {
                var operation = binaryReader.ReadPolymorph<GClass914>();
                return operation;
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
                return null;
            }
        }
    }
}
