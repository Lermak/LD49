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

            if (ShipData.aDown && t.Position.X > -920)
                v.X = -(m.Speed * gt);
            else if (ShipData.dDown && t.Position.X < -400)
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
            List<Enemy> remove = new List<Enemy>();
            foreach (Enemy e in EnemyData.Enemies)
            {
                if (Vector2.Distance(t.Position, e.Transform.Position) <= 30)
                {
                    remove.Add(e);
                    e.Destroy();
                    t.GameObject.Destroy();
                    ShipData.Score += (int)(ShipData.Level * ShipData.Level/2) + 5;
                }
            }
            foreach(Enemy e in remove)
            {
                EnemyData.Enemies.Remove(e);
            }
        }

        public static void EnemyAction(float gt, Component[] c)
        {
            RigidBody rb = (RigidBody)c[0];
            Transform t = (Transform)c[1];


            if ((t.Position.X < -940 && EnemyData.moveVelocity.X < 0) || (t.Position.X > -380 && EnemyData.moveVelocity.X > 0))
            {
                EnemyData.moveVelocity *= -1;
                foreach(Enemy e in EnemyData.Enemies)
                {
                    e.Transform.Place(e.Transform.Position +- new Vector2(0, 10));
                }
            }
            rb.MoveVelocity = EnemyData.moveVelocity * TimeManager.DeltaTime * ShipData.Level * 24/EnemyData.Enemies.Count/4;
            if(t.Position.Y < 210)
            {
                SoundManager.PlaySoundEffect("Error");
                foreach (Enemy e in EnemyData.Enemies)
                    e.Destroy();
                ((GalaxyBlasterScene)WindowManager.GalaxyBlasterWindow.sceneManager.CurrentScene).SpawnHorde();
                ShipData.Level = 1;
                ShipData.Score = 0;
            }
        }

        public static void NextLevel(float gt, Component[] c)
        {
            if (EnemyData.Enemies.Count == 0)
            {
                ((GalaxyBlasterScene)WindowManager.GalaxyBlasterWindow.sceneManager.CurrentScene).SpawnHorde();
                ShipData.Level++;
            }
        }

        public static void ShowScore(float gt, Component[] c)
        {
            FontRenderer fr = (FontRenderer)c[0];
            fr.Text = "Score: " + ShipData.Score;
        }
    }
}
