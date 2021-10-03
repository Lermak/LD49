using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public class NuclearLevel : Component
    {
        public static float level = 0;
        public static float speed = 0.05f;
        public static float reduceAmount = 0.02f;
        public static bool Locked = false;
        public static float ButtonHoldTime = 0f;
        public NuclearLevel(GameObject go, string name) : base(go, name)
        {
        }
        
    }
}
