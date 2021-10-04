using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;


namespace MonoGame_Core.Scripts
{
    public class Credits : Scene
    {
        static bool restartReady = false;
        protected override void loadContent(List<Camera> c)
        {
            GameObjects = new List<GameObject>();

            ResourceManager.Textures["Credits"] = Content.Load<Texture2D>(@"Images/Background");
            GameObjects.Add(new WorldObject("Credits", "Credits", new Vector2(800, 600), new Vector2(-560, 0), 1));
            WorldObject go = (WorldObject)GameObjects[^1];
            go.BehaviorHandler.AddBehavior("Scroll", Scroll, new Component[] { go.RigidBody, go.Transform });
            go.BehaviorHandler.AddBehavior("Restart", Restart, new Component[] { });
            base.loadContent(c);
        }

        public static void Scroll(float gt, Component[] c)
        {
            RigidBody rb = (RigidBody)c[0];
            Transform t = (Transform)c[1];
            if (!restartReady)
            {              
                rb.MoveVelocity = new Vector2(0, 100) * gt;
            }
            if(t.Position.Y > 240)
            {
                rb.MoveVelocity = new Vector2(0,0);
                restartReady = true;
            }
            
        }

        public static void Restart(float gt, Component[] c)
        {
            if(restartReady)
            {
                GameManager.Quit();
            }
        }
    }
}
