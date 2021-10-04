using Microsoft.Xna.Framework;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGame_Core.Scripts
{
    public static class GalaxyBlasterBehaviors
    {
        public static void BlasterControlls(float gt, Component[] c)
        {
            RigidBody rb = (RigidBody)c[0];
            ShipData m = (ShipData)c[1];
            Transform t = (Transform)c[2];

            Vector2 v = new Vector2();

            if (ShipData.aDown)
                v.X = -(m.Speed * gt);
            else if (ShipData.dDown)
                v.X = (m.Speed * gt);
            else
                v = new Vector2();
            

            rb.MoveVelocity = v;
        }
        public static void Shoot(float gt, Component[] c)
        {
            Transform t = (Transform)c[0];

            if(ShipData.SpaceDown)
            {
                ShipData.SpaceDown = false;
                CurrentWindow.sceneManager.CurrentScene.ToAdd.Add(new Bullet(t.Position + new Vector2(0,15)));
            }
        }
        public static void Bullet(float gt, Component[] c)
        {
            RigidBody rb = (RigidBody)c[0];
            Transform t = (Transform)c[1];

            rb.MoveVelocity = new Vector2(0, 300 * TimeManager.DeltaTime);

            if (t.Position.Y > 560)
                t.GameObject.Destroy();
        }

        public static void EnemyAction(float gt, Component[] c)
        {
            RigidBody rb = (RigidBody)c[0];
            Transform t = (Transform)c[1];

            rb.MoveVelocity = EnemyData.moveVelocity * TimeManager.DeltaTime;

            if ((t.Position.X < -900 && EnemyData.moveVelocity.X < 0) || (t.Position.X > -540 && EnemyData.moveVelocity.X > 0))
            {
                EnemyData.moveVelocity *= -1;
                foreach(Enemy e in EnemyData.Enemies)
                {
                    e.Transform.Place(e.Transform.Position - new Vector2(0, 40));
                }
            }
        }
    }
}
