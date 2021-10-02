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

        protected override void loadContent(List<Camera> c)
        {      
            SoundManager.Songs["Melody"] = Content.Load<Song>("Music/TestSong");
            //SoundManager.PlaySong("Melody");

            SoundManager.SoundEffects["TestHit"] = Content.Load<SoundEffect>("Sound/TestHit").CreateInstance();

            ResourceManager.Textures = new Dictionary<string, Texture2D>();
            ResourceManager.Textures["Test"] = Content.Load<Texture2D>("Images/Test");
            ResourceManager.Textures["Base"] = Content.Load<Texture2D>("Images/Base");

            ResourceManager.Fonts["TestFont"] = Content.Load<SpriteFont>("Fonts/TestFont");

            GameObjects = new List<GameObject>();

            GameObjects.Add(new Button("Test", "Base", "PlayButton", new Vector2(40, 40), new Vector2(500, 100), 1, () => { CurrentWindow.sceneManager.ChangeScene(new TestScene()); }));
            GameObjects.Add(new Button("Test", "Base", "NuclearButton", new Vector2(40, 40), new Vector2(500, 40), 1, () => { CurrentWindow.sceneManager.ChangeScene(new NuclearScene()); }));
            GameObjects.Add(new Button("Test", "Base", "QuitButton", new Vector2(40, 40), new Vector2(500, -20), 1, () => { GameManager.Quit(); }));
            
            base.loadContent(c);
        }
    }
}
