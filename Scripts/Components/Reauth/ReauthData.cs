using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public class ReauthData : Component
    {
        public float TimeRemaining;
        public static bool ReAuthorized = false;
        public ReauthData(GameObject go, string name, float time) : base(go, name)
        {
            TimeRemaining = time;
        }
    }
}
