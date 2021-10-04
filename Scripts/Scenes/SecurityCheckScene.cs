using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace MonoGame_Core.Scripts
{
    public class SecurityCheckScene : Scene
    {
        string key = "";
        string message = "";
        bool error = false;
        static string[,] codes = new string[26, 10];

        protected override void loadContent(List<Camera> c)
        {
            if (!Globals.FirstLocoutComplete)
                Globals.FirstLockout = true;
            
            NuclearLevel.Locked = true;
            
            string s = "";
            var random = new Random();
            string fileName = @".\SecurityCodes.txt";

            try
            {
                if (!File.Exists(fileName))
                {
                    for (int y = 0; y < 26; ++y)
                        for (int x = 0; x < 10; ++x)
                        {
                            if (x == 9 && y == 0)
                                codes[y, x] = "M";
                            else if (x == 9 && y == 1)
                                codes[y, x] = "O";
                            else if (x == 9 && y == 2)
                                codes[y, x] = "R";
                            else if (x == 9 && y == 4)
                                codes[y, x] = "S";
                            else if (x == 9 && y == 5)
                                codes[y, x] = "E";
                            else if (x == 7 && y == 7)
                                codes[y, x] = "CO";
                            else if (x == 8 && y == 7)
                                codes[y, x] = "DE";
                            else if (x == 2 && y == 24)
                                codes[y, x] = "VO";
                            else if (x == 3 && y == 24)
                                codes[y, x] = "ID";
                            else
                            {
                                var chars = "0123456789";

                                for (int i = 0; i < 2; i++)
                                {
                                    codes[y, x] = "";
                                    codes[y, x] += chars[random.Next(chars.Length)];
                                    codes[y, x] += chars[random.Next(chars.Length)];
                                }
                            }
                        }

                    for (int x = 0; x < 10; ++x)
                        s += "\t" + (x + 1);
                    for (int y = 0; y < 26; ++y)
                    {
                        if (y == 8 || y == 14 || y == 3 || y == 21) continue;
                        s += GetCodeLine(y, codes);
                    }
                    s += GetCodeLine(21, codes);
                    s += GetCodeLine(14, codes);
                    s += GetCodeLine(8, codes);
                    s += GetCodeLine(3, codes);

                    using (StreamWriter sw = File.CreateText(fileName))
                    {
                        sw.Write(s);
                    }
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

            int yindex = random.Next(0, 26);
            char letter = (char)(yindex + 65);
            int xindex = random.Next(0, 10);
            message = "          Security Screening\n"
                + "Please check SecurityCodes.txt\n"
                + "Input code " + letter + (xindex + 1) + " and confirm.";
            key = codes[yindex, xindex];

            ResourceManager.SoundEffects["Unlock"] = Content.Load<SoundEffect>(@"Sound/unlock");
            ResourceManager.SoundEffects["Lockout"] = Content.Load<SoundEffect>(@"Sound/lock_out");
            SoundManager.PlaySoundEffect("Lockout");
            SoundManager.SoundEffects["Lockout"].Volume = .3f;
            ResourceManager.SoundEffects["Error"] = Content.Load<SoundEffect>(@"Sound/error");
            ResourceManager.Textures["Carret"] = Content.Load<Texture2D>(@"Images/Caret");
            ResourceManager.Textures["Textbox"] = Content.Load<Texture2D>(@"Images/SecurityCode/Textbox");
            ResourceManager.Textures["MessageBox"] = Content.Load<Texture2D>(@"Images/SecurityCode/MessageBox");
            ResourceManager.Fonts["TestFont"] = Content.Load<SpriteFont>("Fonts/TestFont");
            GameObjects.Add(new TextBox("Textbox", "TestFont", "TextInput", new Vector2(600, 40), new Vector2(0, 200), 1));
            GameObject tb = GameObjects[GameObjects.Count - 1];
            tb.BehaviorHandler.AddBehavior("LimitChar", RoboTestchaBenaviors.LimitCharCount, new Component[] { tb.ComponentHandler.GetComponent("textboxrenderer") });

            GameObjects.Add(new WorldObject("MessageBox", "SecurityMessage", new Vector2(600, 200), new Vector2(-660, 440), 1));
            WorldObject obj = (WorldObject)GameObjects[GameObjects.Count - 1];
            FontRenderer objfr = (FontRenderer)obj.ComponentHandler.AddComponent(new FontRenderer(obj, message, "TestFont", obj.Transform, new Vector2(), new Vector2(600, 50), 1, Color.White));
            obj.BehaviorHandler.AddBehavior("ErrorMessage", ShowError, new Component[] { objfr });

            ((TextBox)tb).OnEnterPressed += (TextBox tb) =>
            {
                if (tb.Text == key)
                {
                    error = false;
                    if (Globals.FirstLockout)
                    {
                        Globals.FirstLockout = false;
                        Globals.FirstLocoutComplete = true;
                    }
                    NuclearLevel.Locked = false;
                    SoundManager.PlaySoundEffect("Unlock");
                    SoundManager.SoundEffects["Unlock"].Volume = .5f;
                    WindowManager.KillSecurityCheckWindow = true;
                }
                else
                {
                    SoundManager.PlaySoundEffect("Error");
                    error = true;
                    tb.Text = "";
                }
            };

        }

        private static string GetCodeLine(int y, string[,] st)
        {
            string s = "";
            s += "\n";
            s += (char)(y + 65) + "\t";

            for (int x = 0; x < 10; ++x)
            {
                s += st[y, x];
                s += "\t";
            }

            return s;
        }
        void ShowError(float gt, Component[] c)
        {
            FontRenderer fr = (FontRenderer)c[0];

            if (error)
            {
                fr.Text = message + "\nError: Incorrect Code";
            }
            else
            {
                fr.Text = message;
            }
        }
    }
}
