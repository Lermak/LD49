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
            int num = random.Next(0, 10);
            message = "Please input code \n" + letter + (num + 1) + " from SecurityCodes.txt";
            key = codes[yindex, num];

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

            ResourceManager.Textures["CarretTexture"] = Content.Load<Texture2D>(@"Images/Base");
            ResourceManager.Textures["Test"] = Content.Load<Texture2D>(@"Images/Test");
            ResourceManager.Fonts["TestFont"] = Content.Load<SpriteFont>("Fonts/TestFont");
            GameObjects.Add(new TextBox("Test", "TestFont", "TextInput", new Vector2(300, 40), new Vector2(0, 200), 1));
            GameObject tb = GameObjects[GameObjects.Count - 1];
            tb.BehaviorHandler.AddBehavior("LimitChar", RoboTestchaBenaviors.LimitCharCount, new Component[] { tb.ComponentHandler.GetComponent("textboxrenderer") });
            ResourceManager.Textures["DigiPetBG"] = Content.Load<Texture2D>("Images/DigiPet/DigiPetBackground");            
            //GameObjects.Add(new WorldObject("DigiPetBG", "Background", new Vector2(400, 600), new Vector2(-760, 240), 0));
            GameObjects.Add(new WorldObject("Test", "SecurityMessage", new Vector2(660, 50), new Vector2(-660, 440), 1));
            WorldObject obj = (WorldObject)GameObjects[GameObjects.Count - 1];
            obj.ComponentHandler.AddComponent(new FontRenderer(obj, message, "TestFont", obj.Transform, new Vector2(), new Vector2(100, 50), 0, Color.White));
            
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

        TextBoxEvent confirmSub(TextBox tb)
        {
            if (tb.Text == "aaa")
                NuclearLevel.Locked = false;

            return null;
        }
    }
}
