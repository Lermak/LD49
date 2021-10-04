using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public class Fly : WorldObject
    {
        public Fly(Vector2 start, List<Vector2> positions) : base("Fly", "Fly", new Vector2(30,20), start, 3)
        {
            FlyData fd = (FlyData)componentHandler.AddComponent(new FlyData(this, "FlyData", positions));
            behaviorHandler.AddBehavior("Move", DigiPetBehaviors.Fly, new Component[] { fd, Transform });
        }
    }
}
