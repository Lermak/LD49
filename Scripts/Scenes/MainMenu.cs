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


namespace MonoGame_Core.Scripts
{
    public class MainMenu : Scene
    {
        public MainMenu() : base()
        {

        }

        protected override void loadContent()
        {      
            SoundManager.Songs["Melody"] = Content.Load<Song>("Music/TestSong");
            SoundManager.PlaySong("Melody");

            SoundManager.SoundEffects["TestHit"] = Content.Load<SoundEffect>("Sound/TestHit").CreateInstance();

            Textures = new Dictionary<string, Texture2D>();
            Textures["Test"] = Content.Load<Texture2D>("Images/Test");
            Textures["Base"] = Content.Load<Texture2D>("Images/Base");

            Fonts["TestFont"] = Content.Load<SpriteFont>("Fonts/TestFont");

            GameObjects = new List<GameObject>();

            GameObjects.Add(new Button("Test", "Base", "PlayButton", new Vector2(40, 40), new Vector2(500, 100), 1, Behaviors.LoadLevelOnClick));
            GameObjects.Add(new Button("Test", "Base", "QuitButton", new Vector2(40, 40), new Vector2(500, 40), 1, Behaviors.QuitOnClick));
            
            base.loadContent();
        }
    }
}
