using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public class ChalkData : Component
    {
        public bool FirstPickup = false;
        public bool Draw = false;
        public static bool Held = false;
        public Vector2 LastDrawPos;
        public ChalkData(GameObject go, string name) : base(go, name)
        {

        }
    }
}
