using EFT.Interactive;

namespace ServerLib.Utils.Player
{
    public static class SpawnUtils
    {
        public static PlayerSpawnPoint GetRandomSpawnPoint()
        {
            return PlayerSpawnPoint.GetFromScene().RandomElement();
        }
    }
}
