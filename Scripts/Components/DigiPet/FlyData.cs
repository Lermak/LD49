using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public class FlyData : Component
    {
        public List<Vector2> Positions;
        public int PositionIndex = 0;
        public float TimeSinceLastMove = 0;
        public FlyData(GameObject go, string name, List<Vector2> positions) : base(go, name)
        {
            Positions = positions;
        }
    }
}
