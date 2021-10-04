using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public class ShipData : Component
    {
        public float Speed = 300;
        public static int Level = 1;
        public static bool aDown = false;
        public static bool dDown = false;
        public static bool SpaceDown = false;
        public static int Score = 0;
        public ShipData(GameObject go, string name) : base(go, name)
        {

        }
    }
}
