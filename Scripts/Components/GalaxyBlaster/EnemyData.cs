using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public class EnemyData : Component
    {
        public static Vector2 moveVelocity = new Vector2(0, 100);
        public static List<Enemy> Enemies = new List<Enemy>();
        public EnemyData(GameObject go, string name) : base(go, name)
        {

        }
    }
}
