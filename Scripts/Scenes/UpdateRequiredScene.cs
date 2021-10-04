using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace MonoGame_Core.Scripts
{
    public class UpdateRequiredScene : Scene
    {
        static float countdown = 120;
        protected override void loadContent(List<Camera> c)
        {
            ResourceManager.SoundEffects["Unlock"] = Content.Load<SoundEffect>(@"Sound/unlock");
            ResourceManager.SoundEffects["Lockout"] = Content.Load<SoundEffect>(@"Sound/lock_out");
            ResourceManager.Textures["CarretTexture"] = Content.Load<Texture2D>(@"Images/SecurityCode/Textbox");
            ResourceManager.Textures["MessageBox"] = Content.Load<Texture2D>(@"Images/SecurityCode/MessageBox");
            ResourceManager.Textures["UpdateNow"] = Content.Load<Texture2D>(@"Images/UpdateRequired/UpdateNow");
            ResourceManager.Textures["UpdateLater"] = Content.Load<Texture2D>(@"Images/UpdateRequired/UpdateLater");
            ResourceManager.Fonts["TestFont"] = Content.Load<SpriteFont>("Fonts/TestFont");
            ResourceManager.SoundEffects["Click1"] = Content.Load<SoundEffect>("Sound/click1");
            ResourceManager.SoundEffects["Click2"] = Content.Load<SoundEffect>("Sound/click2");
            ResourceManager.SoundEffects["Click3"] = Content.Load<SoundEffect>("Sound/click3");

            GameObjects.Add(new WorldObject("MessageBox", "SecurityMessage", new Vector2(600, 200), new Vector2(-660, 440), 1));
            WorldObject obj = (WorldObject)GameObjects[GameObjects.Count - 1];
            FontRenderer fr = (FontRenderer)obj.ComponentHandler.AddComponent(new FontRenderer(obj,
                "Update Required",
                "TestFont", obj.Transform, new Vector2(), new Vector2(600, 50), 0, Color.White));
            obj.BehaviorHandler.AddBehavior("Countdown", UpdateTimer, new Component[] { fr });

            GameObjects.Add(new Button("UpdateNow", "UpdateNow", "UpdateButton", new Vector2(150, 50), new Vector2(-860, 375), 2, () => { }));
            WorldObject unBtn = (WorldObject)GameObjects[^1];
            unBtn.BehaviorHandler.AddBehavior("CheckUpdate", UpdateButton, new Component[] { unBtn.Transform });
            GameObjects.Add(new Button("UpdateLater", "UpdateLater", "LaterButton", new Vector2(150, 50), new Vector2(-460, 375), 2, () => {  }));
            WorldObject ulBtn = (WorldObject)GameObjects[^1];
            ulBtn.BehaviorHandler.AddBehavior("UpdateLater", LaterButton, new Component[] { ulBtn.Transform });
        }

        private static void UpdateButton(float gt, Component[] c)
        {
            Transform t = (Transform)c[0];
            Vector2 v = CurrentWindow.inputManager.MousePos;
            if (CurrentWindow.inputManager.IsMouseDown(InputManager.MouseKeys.LeftButton) &&
                t.ContainsPoint(v))
            {
                Random r = new Random();
                SoundManager.PlaySoundEffect(Globals.ClickSounds[r.Next(0, 3)]);

                if (!NuclearLevel.Updated)
                {
                    NuclearLevel.NeedsUpdate = true;
                }
                WindowManager.KillUpdate = true;//WindowManager.RemoveWindow(CurrentWindow.windowData);
            }
        }

        public static void LaterButton(float gt, Component[] c)
        {
            Transform t = (Transform)c[0];
            Vector2 v = CurrentWindow.inputManager.MousePos;
            if (CurrentWindow.inputManager.IsMouseDown(InputManager.MouseKeys.LeftButton) &&
                t.ContainsPoint(v))
            {
                Random r = new Random();
                SoundManager.PlaySoundEffect(Globals.ClickSounds[r.Next(0, 3)]);

                WindowManager.MainWindow.coroutineManager.AddCoroutine(Coroutines.UpdateLater(), "UpdateLater", 0, true);
                WindowManager.KillUpdate = true;//WindowManager.RemoveWindow(CurrentWindow.windowData);
            }
        }

        static void UpdateTimer(float gt, Component[] c)
        {
            FontRenderer fr = (FontRenderer)c[0];
            countdown -= gt;
            if (countdown <= 0)
            {
                if (!NuclearLevel.Updated)
                {
                    NuclearLevel.NeedsUpdate = true;
                }
                WindowManager.KillUpdate = true;
            }
            else
            {
                fr.Text = "              Update Required\n" + (int)countdown + " seconds before update begins.";
            }
        }
    }
}
