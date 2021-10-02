using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoGame_Core.Scripts
{
    public class SecurityCheckScene : Scene
    {
        string key = "";
        protected override void loadContent(List<Camera> c)
        {
            NuclearLevel.Locked = true;



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
                if (tb.Text == key)
                    NuclearLevel.Locked = false;
                WindowManager.RemoveWindow(CurrentWindow.windowData);
            };

        }

        TextBoxEvent confirmSub(TextBox tb)
        {
            if (tb.Text == "aaa")
                NuclearLevel.Locked = false;

            return null;
        }
    }
}
