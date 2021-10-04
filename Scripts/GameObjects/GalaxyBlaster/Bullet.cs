using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public class Bullet : WorldObject
    {
        public Bullet(Vector2 spawn) : base("Bullet", "Bullet", new Vector2(5,15), spawn, 1)
        {
            behaviorHandler.AddBehavior("Bullet", GalaxyBlasterBehaviors.Bullet, new Component[] { RigidBody, Transform });
        }
    }
}
