using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace MonoGame_Core.Scripts
{
    public class ReauthScene : Scene
    {
        protected override void loadContent(List<Camera> c)
        {
            NuclearLevel.Locked = true;

            ResourceManager.Textures["MessageBox"] = Content.Load<Texture2D>(@"Images/SecurityCode/MessageBox");
            ResourceManager.Fonts["TestFont"] = Content.Load<SpriteFont>("Fonts/TestFont");

            GameObjects.Add(new WorldObject("MessageBox", "SecurityMessage", new Vector2(600, 200), new Vector2(-660, 440), 1));
            WorldObject obj = (WorldObject)GameObjects[GameObjects.Count - 1];
            ReauthData rad = (ReauthData)obj.ComponentHandler.AddComponent(new ReauthData(obj, "ReauthData", 60));
            FontRenderer fr = (FontRenderer)obj.ComponentHandler.AddComponent(new FontRenderer(obj,
                "Account Reauthorization Required\nContact Administrator",
                "TestFont", obj.Transform, new Vector2(), new Vector2(600, 50), 0, Color.White));
            obj.BehaviorHandler.AddBehavior("ReauthDelay", ReauthTimer, new Component[] { rad, fr });

        }

        static void ReauthTimer(float gt, Component[] c)
        {
            ReauthData rd = (ReauthData)c[0];
            FontRenderer fr = (FontRenderer)c[1];
            rd.TimeRemaining -= gt;
            if(rd.TimeRemaining <= 0)
            {
                NuclearLevel.Locked = true;
                fr.Text = "You must reauthorize your account";
            }
            else
            {
                fr.Text = "Account Reauthorization Required\nContact Administrator ("+(int)rd.TimeRemaining+")";
            }

            if(ReauthData.ReAuthorized)
            {
                WindowManager.KillReauth = true;
            }
        }
    }
}
