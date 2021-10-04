using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
using System.Windows.Forms;
using System.Linq;
using Microsoft.Xna.Framework.Audio;

namespace MonoGame_Core.Scripts
{
    public class ResetKeysScene : Scene
    {
        protected override void loadContent(List<Camera> c)
        {
            NuclearLevel.Locked = true;
            ResourceManager.SoundEffects["Unlock"] = Content.Load<SoundEffect>(@"Sound/unlock");
            ResourceManager.SoundEffects["Lockout"] = Content.Load<SoundEffect>(@"Sound/lock_out");
            SoundManager.PlaySoundEffect("Lockout");
            SoundManager.SoundEffects["Lockout"].Volume = .3f;

            WindowManager.ResetKeysWindow.form.AllowDrop = true;
            WindowManager.ResetKeysWindow.form.DragEnter += (object sender, System.Windows.Forms.DragEventArgs e) =>
            {
                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                    e.Effect = DragDropEffects.All;
                else
                    e.Effect = DragDropEffects.None;
            };
            WindowManager.ResetKeysWindow.form.DragDrop += (object sender, DragEventArgs e) =>
            {
                string[] files = e.Data.GetData(DataFormats.FileDrop) as string[]; // get all files droppeds  
                if (files != null && files.Any())
                {
                    string Text = files.First(); //select the first one  
                    string compair = Directory.GetCurrentDirectory() + "\\DataKeys.txt";
                    if (Text == compair)
                    {
                        WindowManager.KillResetKeys = true;
                        NuclearLevel.Locked = false;
                        SoundManager.PlaySoundEffect("Unlock");
                        SoundManager.SoundEffects["Unlock"].Volume = .5f;
                    }
                }
                
            };

            Globals.CreateFile("DataKeys", "");

            ResourceManager.Textures["MessageBox"] = Content.Load<Texture2D>(@"Images/SecurityCode/MessageBox");
            ResourceManager.Fonts["TestFont"] = Content.Load<SpriteFont>("Fonts/TestFont");

            GameObjects.Add(new WorldObject("MessageBox", "SecurityMessage", new Vector2(600, 200), new Vector2(-660, 440), 1));
            WorldObject obj = (WorldObject)GameObjects[GameObjects.Count - 1];
            ReauthData rad = (ReauthData)obj.ComponentHandler.AddComponent(new ReauthData(obj, "ReauthData", 60));
            FontRenderer fr = (FontRenderer)obj.ComponentHandler.AddComponent(new FontRenderer(obj,
                "Reset Data Keys\n" +
                "Click and drag DataKeys.txt here",
                "TestFont", obj.Transform, new Vector2(), new Vector2(600, 50), 0, Color.White));
        }
    }
}
