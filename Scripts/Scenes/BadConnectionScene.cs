using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using Microsoft.Xna.Framework.Audio;

namespace MonoGame_Core.Scripts
{
    public class BadConnectionScene : Scene
    {
        protected override void loadContent(List<Camera> c)
        {
            NuclearLevel.Locked = true;
            ResourceManager.SoundEffects["Unlock"] = Content.Load<SoundEffect>(@"Sound/unlock");
            ResourceManager.SoundEffects["Lockout"] = Content.Load<SoundEffect>(@"Sound/lock_out");
            SoundManager.PlaySoundEffect("Lockout");
            SoundManager.SoundEffects["Lockout"].Volume = .3f;
            ResourceManager.SoundEffects["Error"] = Content.Load<SoundEffect>(@"Sound/error");

            ResourceManager.SoundEffects["Click1"] = Content.Load<SoundEffect>("Sound/click1");
            ResourceManager.SoundEffects["Click2"] = Content.Load<SoundEffect>("Sound/click2");
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
                "                     Error Code: 312\n" +
                "Disconnected from Server.\n" +
                "Please select new server with better connection.\n",
                "TestFont", obj.Transform, new Vector2(0,-20), new Vector2(600, 50), 0, Color.White));
            fr.TextScale = .75f;
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
                SoundManager.PlaySoundEffect("Error");

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
                Random r = new Random();
                SoundManager.PlaySoundEffect(Globals.ClickSounds[r.Next(0, 2)]);

                CurrentWindow.coroutineManager.AddCoroutine(Coroutines.ConnectToServer(fr), "Connect", 0, true);
            }
        }

    }
}
