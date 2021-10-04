using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public class Player : WorldObject
    {
        public Player() : base("PeaShooter", "Player", new Vector2(40, 40), new Vector2(-720, 240), 2)
        {
            ShipData sd = (ShipData)componentHandler.AddComponent(new ShipData(this, "ShipData"));
            behaviorHandler.AddBehavior("Controlls", GalaxyBlasterBehaviors.BlasterControlls, new Component[] { RigidBody, sd, Transform });
            behaviorHandler.AddBehavior("Shoot", GalaxyBlasterBehaviors.Shoot, new Component[] { Transform });
        }
    }
}
