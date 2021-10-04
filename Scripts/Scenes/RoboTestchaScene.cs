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
        protected override void loadContent(List<Camera> c)
        {
            NuclearLevel.Locked = true;
            ResourceManager.SoundEffects["Unlock"] = Content.Load<SoundEffect>(@"Sound/unlock");
            ResourceManager.SoundEffects["Lockout"] = Content.Load<SoundEffect>(@"Sound/lock_out");
            SoundManager.PlaySoundEffect("Lockout");
            SoundManager.SoundEffects["Lockout"].Volume = .3f;

            ResourceManager.Textures["CarretTexture"] = Content.Load<Texture2D>(@"Images/Test");
            ResourceManager.Textures["Test"] = Content.Load<Texture2D>(@"Images/Test");
            ResourceManager.Fonts["TestFont"] = Content.Load<SpriteFont>("Fonts/TestFont");
            GameObjects.Add(new TextBox("Test", "TestFont", "TextInput", new Vector2(300, 40), new Vector2(0, 200), 1));
            GameObject tb = GameObjects[GameObjects.Count - 1];
            tb.BehaviorHandler.AddBehavior("LimitChar", RoboTestchaBenaviors.LimitCharCount, new Component[] { tb.ComponentHandler.GetComponent("textboxrenderer") });
            ResourceManager.Textures["DigiPetBG"] = Content.Load<Texture2D>("Images/DigiPet/DigiPetBackground");
            GameObjects.Add(new WorldObject("DigiPetBG", "Background", new Vector2(400, 600), new Vector2(-760, 240), 0));
            
            ((TextBox)tb).OnEnterPressed += (TextBox tb) =>        
                {
                    if (tb.Text == "aaa")
                    {
                        NuclearLevel.Locked = false;
                        SoundManager.PlaySoundEffect("Unlock");
                        SoundManager.SoundEffects["Unlock"].Volume = .5f;
                    }
                    WindowManager.RemoveWindow(CurrentWindow.windowData);
                };

        }
    }
}
