using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace MonoGame_Core.Scripts
{
    public class UpdateRequiredScene : Scene
    {
        protected override void loadContent(List<Camera> c)
        {
            NuclearLevel.Locked = true;

            ResourceManager.Textures["CarretTexture"] = Content.Load<Texture2D>(@"Images/SecurityCode/Textbox");
            ResourceManager.Textures["MessageBox"] = Content.Load<Texture2D>(@"Images/SecurityCode/MessageBox");
            ResourceManager.Textures["UpdateNow"] = Content.Load<Texture2D>(@"Images/UpdateRequired/UpdateNow");
            ResourceManager.Textures["UpdateLater"] = Content.Load<Texture2D>(@"Images/UpdateRequired/UpdateLater");
            ResourceManager.Fonts["TestFont"] = Content.Load<SpriteFont>("Fonts/TestFont");
            
            GameObjects.Add(new WorldObject("MessageBox", "SecurityMessage", new Vector2(600, 200), new Vector2(-660, 440), 1));
            WorldObject obj = (WorldObject)GameObjects[GameObjects.Count - 1];
            obj.ComponentHandler.AddComponent(new FontRenderer(obj,
                "Update Required",
                "TestFont", obj.Transform, new Vector2(), new Vector2(600, 50), 0, Color.White));
            GameObjects.Add(new Button("UpdateNow", "UpdateNow", "UpdateButton", new Vector2(150, 50), new Vector2(-860, 380), 2, () => { }));
            WorldObject unBtn = (WorldObject)GameObjects[^1];
            unBtn.BehaviorHandler.AddBehavior("CheckUpdate", UpdateButton, new Component[] { });
            GameObjects.Add(new Button("UpdateLater", "UpdateLater", "LaterButton", new Vector2(150, 50), new Vector2(-460, 380), 2, () => {  }));

        }

        private static void UpdateButton(float gt, Component[] c)
        {
            if (CurrentWindow.inputManager.IsMouseTriggered(InputManager.MouseKeys.LeftButton))
            {
                if (!NuclearLevel.Updated)
                {
                    NuclearLevel.Updating = true;
                }
                WindowManager.KillUpdate = true;//WindowManager.RemoveWindow(CurrentWindow.windowData);
            }
        }
    }
}
