using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public class NuclearLevel : Component
    {
        public static float level = 0;
        public static float speed = 0.1f;
        public static float reduceAmount = 0.05f;

        public NuclearLevel(GameObject go, string name) : base(go, name)
        {
        }
        
    }
}
