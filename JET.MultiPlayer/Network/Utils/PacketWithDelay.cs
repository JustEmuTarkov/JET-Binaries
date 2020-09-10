namespace ServerLib.Network.Utils
{
    public class PacketWithDelay<T>
    {
        public float TimeDelay = 0.5f;
        public T Packet;

        public PacketWithDelay(T packet, float time)
        {
            Packet = packet;
            TimeDelay = time;
        }
    }
}
