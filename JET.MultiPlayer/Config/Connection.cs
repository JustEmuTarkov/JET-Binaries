using System.Collections.Generic;
using UnityEngine.Networking;
#pragma warning disable 618

namespace ServerLib.Config
{
    public class Connection
    {
        public static ConnectionConfig GetConfiguration()
        {
            var connectionConfig = GetConnectionConfig();

            for (int i = 0; i < 102; i++)
            {
                byte item = connectionConfig.AddChannel(QosType.Reliable);
                byte item2 = connectionConfig.AddChannel(QosType.Unreliable);

                connectionConfig.MakeChannelsSharedOrder(new List<byte>
                {
                    item,
                    item2
                });
            }

            return connectionConfig;
        }

        private static ConnectionConfig GetConnectionConfig()
        {
            return new ConnectionConfig
            {
                DisconnectTimeout = 4000U,
                NetworkDropThreshold = 25,
                OverflowDropThreshold = 80,
                AcksType = ConnectionAcksType.Acks128,
                Channels =
                {
                    new ChannelQOS(QosType.ReliableFragmented),
                    new ChannelQOS(QosType.ReliableFragmented),
                    new ChannelQOS(QosType.ReliableFragmented)
                }
            };
        }
    }
}
