using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace MonoGame_Core.Scripts
{
    public class AskITScene : Scene
    {
        protected override void loadContent(List<Camera> c)
        {
            NuclearLevel.Locked = true;

            ResourceManager.Textures["CarretTexture"] = Content.Load<Texture2D>(@"Images/SecurityCode/Textbox");
            ResourceManager.Textures["MessageBox"] = Content.Load<Texture2D>(@"Images/SecurityCode/MessageBox");
            ResourceManager.Fonts["TestFont"] = Content.Load<SpriteFont>("Fonts/TestFont");

            GameObjects.Add(new WorldObject("MessageBox", "SecurityMessage", new Vector2(600, 200), new Vector2(-660, 440), 1));
            WorldObject obj = (WorldObject)GameObjects[GameObjects.Count - 1];
            obj.ComponentHandler.AddComponent(new FontRenderer(obj, 
                "Error: This should NEVER\nhappen, contact Tim", 
                "TestFont", obj.Transform, new Vector2(), new Vector2(600, 50), 0, Color.White));
            obj.BehaviorHandler.AddBehavior("UnlockButton", (float gt, Component[] c) => { 
                if (NuclearLevel.ButtonHoldTime >= 5) { 
                    NuclearLevel.Locked = false;
                    if (WindowManager.ITHelp != null)
                    {
                        WindowManager.killIT = true;
                    }
                } }, new Component[] { });
        }
    }
}
