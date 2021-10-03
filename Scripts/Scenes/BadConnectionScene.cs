using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace MonoGame_Core.Scripts
{
    public class BadConnectionScene : Scene
    {
        protected override void loadContent(List<Camera> c)
        {
            NuclearLevel.Locked = true;

            ResourceManager.Textures["MessageBox"] = Content.Load<Texture2D>(@"Images/BadConnection/MessageBox");
            ResourceManager.Textures["BadConnectionBG"] = Content.Load<Texture2D>(@"Images/BadConnection/BadConnectionBG");
            ResourceManager.Textures["BadServer"] = Content.Load<Texture2D>(@"Images/BadConnection/BadServer");
            ResourceManager.Textures["GoodServer"] = Content.Load<Texture2D>(@"Images/BadConnection/GoodServer");
            ResourceManager.Fonts["TestFont"] = Content.Load<SpriteFont>("Fonts/TestFont");
            GameObjects.Add(new WorldObject("BadConnectionBG", "BG", new Vector2(600, 600), new Vector2(-660, 240), 0));
            GameObjects.Add(new WorldObject("MessageBox", "SecurityMessage", new Vector2(600, 100), new Vector2(-660, 490), 1));
            WorldObject obj = (WorldObject)GameObjects[GameObjects.Count - 1];
            ReauthData rad = (ReauthData)obj.ComponentHandler.AddComponent(new ReauthData(obj, "ReauthData", 60));
            FontRenderer fr = (FontRenderer)obj.ComponentHandler.AddComponent(new FontRenderer(obj,
                "Server Disconnected: Select new server",
                "TestFont", obj.Transform, new Vector2(), new Vector2(600, 50), 0, Color.White));

            Random r = new Random();
            int[] goodServers = new int[] { r.Next(0, 16), r.Next(0, 16), r.Next(0, 16) };
            for (int y = 0; y < 4; ++y)
            {
                for (int x = 0; x < 4; ++x)
                {
                    bool isGood = false;
                    for (int i = 0; i < goodServers.Length; ++i)
                    {
                        if (y * 4 + x == goodServers[i])
                        {
                            isGood = true;
                            break;
                        }
                    }
                    if (isGood)
                    {
                        GameObjects.Add(new WorldObject("GoodServer", "Server" + y * 4 + x + 1, new Vector2(100, 75), new Vector2(x * 150 - 885, 350 - y * 100), 1));
                        WorldObject wo = (WorldObject)GameObjects[^1];
                        wo.BehaviorHandler.AddBehavior("GoodServer", GoodServer, new Component[] { wo.Transform, fr });
                        wo.ComponentHandler.AddComponent(new FontRenderer(obj,
                            (y * 4 + x + 1).ToString(),
                            "TestFont", wo.Transform, new Vector2(-15,0), new Vector2(100, 80), 0, Color.White));

                    }
                    else
                    {
                        GameObjects.Add(new WorldObject("BadServer", "Server" + y * 4 + x + 1, new Vector2(100, 75), new Vector2(x * 150 - 885, 350 - y * 100), 1));
                        WorldObject wo = (WorldObject)GameObjects[^1];
                        wo.BehaviorHandler.AddBehavior("BadServer", BadServer, new Component[] { wo.Transform, fr });
                        wo.ComponentHandler.AddComponent(new FontRenderer(obj,
                            (y * 4 + x + 1).ToString(),
                            "TestFont", wo.Transform, new Vector2(-15, 0), new Vector2(100, 80), 0, Color.White));
                    }
                }
            }
        }
        static void BadServer(float gt, Component[] c)
        {
            Transform t = (Transform)c[0];
            FontRenderer fr = (FontRenderer)c[1];
            Vector2 v = CurrentWindow.inputManager.MousePos;
            if (CurrentWindow.inputManager.IsMouseDown(InputManager.MouseKeys.LeftButton) &&
                t.ContainsPoint(v))
            {
                fr.Text = "Unable to connect";
            }
        }

        static void GoodServer(float gt, Component[] c)
        {
            Transform t = (Transform)c[0];
            FontRenderer fr = (FontRenderer)c[1];
            Vector2 v = CurrentWindow.inputManager.MousePos;
            if (CurrentWindow.inputManager.IsMouseDown(InputManager.MouseKeys.LeftButton) &&
                t.ContainsPoint(v))
            {
                CurrentWindow.coroutineManager.AddCoroutine(Coroutines.ConnectToServer(fr), "Connect", 0, true);
            }
        }

    }
}
