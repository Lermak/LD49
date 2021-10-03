using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public class NuclearLevel : Component
    {
        public static float level = 0;
        public static float speed = 0.006f;
        public static float reduceAmount = 0.04f;
        public static bool Locked = false;
        public static float ButtonHoldTime = 0f;
        public static bool started = false;
        public static Queue<char> MorseCode = new Queue<char>();
        public static bool Updating = false;
        public static bool Updated = false;

        public NuclearLevel(GameObject go, string name) : base(go, name)
        {
        }
        
    }
}
