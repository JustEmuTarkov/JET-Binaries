using UnityEngine;
using UnityEngine.Networking;

#pragma warning disable 618

namespace ServerLib.Network
{
    public class AbstractSession : NetworkBehaviour
    {
        internal static NetworkHash128 AuthorityHash => NetworkHash128.Parse("d");

        public NetworkConnection connection;
        public byte chanelId;
        public byte chanelIndex;
        public string profileId = "empty";

        public EMemberCategory MemberCategory
        {
            get => EMemberCategory.Developer;
            set => _memberCategory = value;
        }

        protected static T Instantiate<T>(Transform parent, string name, string profileId) where T : AbstractSession
        {
            T t = new GameObject
            {
                name = name,
                transform =
                {
                    parent = parent
                }
            }.AddComponent<T>();

            t.profileId = profileId;
            return t;
        }

        public virtual void ProfileResourcesLoadProgress(int id, float progress)
        {
        }

        protected virtual void OnDestroy()
        {
        }

        private void method_0()
        {
        }

        public override bool OnSerialize(NetworkWriter writer, bool forceAll)
        {
            return false;
        }

        public override void OnDeserialize(NetworkReader reader, bool initialState)
        {
        }

        internal const short Short0 = 147;
        internal const short Short1 = 188;
        internal const short Short2 = 189;
        internal const short Short3 = 190;
        internal const short Short4 = 151;
        internal const short Short5 = 152;
        internal const short Short6 = 153;
        internal const short Short7 = 154;
        internal const short Short8 = 155;
        internal const short Short9 = 156;
        internal const short Short10 = 157;
        internal const short Short11 = 158;
        internal const short Short12 = 160;
        internal const short Short13 = 165;
        internal const short Short14 = 167;
        internal const short Short15 = 168;
        private EMemberCategory _memberCategory;
    }
}
