using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace MonoGame_Core.Scripts
{
    public class RoboTestchaScene : Scene
    {
        string message = "Please prove you are human.\nEnter the code below.";
        bool error = false;
        string code = "";
        protected override void loadContent(List<Camera> c)
        {
            NuclearLevel.Locked = true;
            ResourceManager.SoundEffects["Unlock"] = Content.Load<SoundEffect>(@"Sound/unlock");
            ResourceManager.SoundEffects["Lockout"] = Content.Load<SoundEffect>(@"Sound/lock_out");
            SoundManager.PlaySoundEffect("Lockout");
            SoundManager.SoundEffects["Lockout"].Volume = .3f;

            ResourceManager.Textures["Capture1"] = Content.Load<Texture2D>(@"Images/RoboTestcha/capture1");
            ResourceManager.Textures["Capture2"] = Content.Load<Texture2D>(@"Images/RoboTestcha/capture2");
            ResourceManager.Textures["Capture3"] = Content.Load<Texture2D>(@"Images/RoboTestcha/capture3");
            ResourceManager.Textures["Capture4"] = Content.Load<Texture2D>(@"Images/RoboTestcha/capture4");


            ResourceManager.Textures["Carret"] = Content.Load<Texture2D>(@"Images/Caret");
            ResourceManager.Textures["Textbox"] = Content.Load<Texture2D>(@"Images/SecurityCode/Textbox");
            ResourceManager.Fonts["TestFont"] = Content.Load<SpriteFont>("Fonts/TestFont");
            GameObjects.Add(new TextBox("Textbox", "TestFont", "TextInput", new Vector2(600, 40), new Vector2(0, 200), 1));
            GameObject tb = GameObjects[GameObjects.Count - 1];
            tb.BehaviorHandler.AddBehavior("LimitChar", RoboTestchaBenaviors.LimitCharCount, new Component[] { tb.ComponentHandler.GetComponent("textboxrenderer") });
            ResourceManager.Textures["MessageBox"] = Content.Load<Texture2D>(@"Images/SecurityCode/MessageBox");
            GameObjects.Add(new WorldObject("MessageBox", "SecurityMessage", new Vector2(600, 200), new Vector2(-660, 440), 0));
            WorldObject obj = (WorldObject)GameObjects[GameObjects.Count - 1];
            FontRenderer objfr = (FontRenderer)obj.ComponentHandler.AddComponent(new FontRenderer(obj, message, "TestFont", obj.Transform, new Vector2(), new Vector2(600, 100), 1, Color.White));
            obj.BehaviorHandler.AddBehavior("ErrorMessage", ShowError, new Component[] { objfr });

            Random r = new Random();
            string img = "";
            int i = r.Next(0, 4);
            switch(i)
            {
                case 0:
                    img = "Capture1";
                    code = "nxErB";
                    break;
                case 1:
                    img = "Capture2";
                    code = "FjMw";
                    break;
                case 2:
                    img = "Capture3";
                    code = "VOID";
                    break;
                case 3:
                    img = "Capture4";
                    code = "dPtR";
                    break;
            }

            GameObjects.Add(new WorldObject(img, "Capture", new Vector2(100, 50), new Vector2(-660, 370), 1));

            ((TextBox)tb).OnEnterPressed += (TextBox tb) =>        
                {
                    if (tb.Text == code)
                    {
                        NuclearLevel.Locked = false;
                        SoundManager.PlaySoundEffect("Unlock");
                        SoundManager.SoundEffects["Unlock"].Volume = .5f;
                        WindowManager.RemoveWindow(CurrentWindow.windowData);
                    }
                    else
                    {
                        SoundManager.PlaySoundEffect("Error");
                        error = true;
                        tb.Text = "";
                    }
                };

        }
        void ShowError(float gt, Component[] c)
        {
            FontRenderer fr = (FontRenderer)c[0];

            if (error)
            {
                fr.Text = "\nError: Incorrect Code";
            }
            else
            {
                fr.Text = message;
            }
        }
    }
}
