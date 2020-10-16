﻿using UnityEngine;
using Comfort.Common;
using EFT;

namespace JET.SinglePlayer.Utils.Reflection
{
    internal static class ClientAppUtils
    {
        public static ClientApplication GetClientApp()
        {
            return Singleton<ClientApplication>.Instance;
        }

        public static MainApplication GetMainApp()
        {
            return GetClientApp() as MainApplication;
        }
    }
}