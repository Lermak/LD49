using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace MonoGame_Core.Scripts
{
    public class SecurityCheckScene : Scene
    {
        string key = "";
        string message = "";
        protected override void loadContent(List<Camera> c)
        {
            string[,] codes = new string[26, 10];
            NuclearLevel.Locked = true;
            string s = "";
            var random = new Random();
            for (int y = 0; y < 26; ++y)
                for (int x = 0; x < 10; ++x)
                {                  
                    var chars = "0123456789";


                    for (int i = 0; i < 2; i++)
                    {
                        codes[y, x] = "";
                        codes[y, x] += chars[random.Next(chars.Length)];
                        codes[y, x] += chars[random.Next(chars.Length)];
                    }
                }

            
            int yindex = random.Next(0, 26);
            char letter = (char)(yindex+65);
            int xindex = random.Next(0, 10);
            message = "Please input code \n" + letter + (xindex + 1) + " from SecurityCodes.txt";
            key = codes[yindex, xindex];

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

            string fileName = @".\SecurityFile.txt";

            try
            {
                // Check if file already exists. If yes, delete it.     
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                // Create a new file     
                using (StreamWriter sw = File.CreateText(fileName))
                {
                    sw.Write(s);
                }
            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }

            ResourceManager.Textures["CarretTexture"] = Content.Load<Texture2D>(@"Images/SecurityCode/Textbox");
            ResourceManager.Textures["Textbox"] = Content.Load<Texture2D>(@"Images/SecurityCode/Textbox");
            ResourceManager.Textures["MessageBox"] = Content.Load<Texture2D>(@"Images/SecurityCode/MessageBox");
            ResourceManager.Fonts["TestFont"] = Content.Load<SpriteFont>("Fonts/TestFont");
            GameObjects.Add(new TextBox("Textbox", "TestFont", "TextInput", new Vector2(600, 40), new Vector2(0, 200), 1));
            GameObject tb = GameObjects[GameObjects.Count - 1];
            tb.BehaviorHandler.AddBehavior("LimitChar", RoboTestchaBenaviors.LimitCharCount, new Component[] { tb.ComponentHandler.GetComponent("textboxrenderer") });

            GameObjects.Add(new WorldObject("MessageBox", "SecurityMessage", new Vector2(600, 200), new Vector2(-660, 440), 1));
            WorldObject obj = (WorldObject)GameObjects[GameObjects.Count - 1];
            obj.ComponentHandler.AddComponent(new FontRenderer(obj, message, "TestFont", obj.Transform, new Vector2(), new Vector2(600, 50), 0, Color.White));
            
            ((TextBox)tb).OnEnterPressed += (TextBox tb) =>
            {
                if (tb.Text == key)
                {
                    NuclearLevel.Locked = false;
                    WindowManager.RemoveWindow(CurrentWindow.windowData);
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
                s += st[y,x];
                s += "\t";
            }

            return s;
        }
    }
}
