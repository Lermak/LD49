using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace MonoGame_Core.Scripts
{
    public class ChatWindowScene : Scene
    {
        public ChatWindowScene() : base()
        {

        }

        protected override void loadContent(List<Camera> c)
        {
            ResourceManager.Textures["Test"] = Content.Load<Texture2D>("Images/Test");
            ResourceManager.Textures["CarretTexture"] = Content.Load<Texture2D>("Images/Test");
            ResourceManager.Fonts["TestFont"] = Content.Load<SpriteFont>("Fonts/TestFont");

            GameObjects.Add(new TextBox("Test", "TestFont", "PlayButton", new Vector2(400, 40), new Vector2(100, 100), 1));

            base.loadContent(c);
        }
    }
}
