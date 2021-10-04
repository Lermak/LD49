using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public class Enemy : WorldObject
    {
        public Enemy(Vector2 pos) : base("Enemy", "Enemy", new Vector2(40,40), pos, 2)
        {
            EnemyData.Enemies.Add(this);
            behaviorHandler.AddBehavior("EnemyAction", GalaxyBlasterBehaviors.EnemyAction, new Component[] { RigidBody, Transform });
        }
    }
}
