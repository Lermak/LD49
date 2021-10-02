using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_Core.Scripts
{
    public class NuclearScene : Scene
    {
        public NuclearScene() : base()
        {

        }

        protected override void loadContent(List<Camera> c)
        {
            size = new Vector2(2100, 1080);
            //CollisionManager.Initilize();

            CameraManager.Cameras[0].SetMinPos(Size / 2 * -1);
            CameraManager.Cameras[0].SetMaxPos(Size / 2);

            SoundManager.Songs["Melody"] = Content.Load<Song>("Music/TestSong");

            SoundManager.SoundEffects["TestHit"] = Content.Load<SoundEffect>("Sound/TestHit").CreateInstance();

            ResourceManager.Textures = new Dictionary<string, Texture2D>();
            //Button things
            ResourceManager.Textures["Test"] = Content.Load<Texture2D>("Images/Test");
            ResourceManager.Textures["Base"] = Content.Load<Texture2D>("Images/Base");
            //NuclearButton!
            ResourceManager.Textures["CoolantButtonUp"] = Content.Load<Texture2D>("Images/CoolantButton");
            ResourceManager.Textures["CoolantButtonDown"] = Content.Load<Texture2D>("Images/CoolantButtonPressed");
            //Dial
            ResourceManager.Textures["nd"] = Content.Load<Texture2D>("Images/NuclearDial");
            ResourceManager.Textures["dialBG"] = Content.Load<Texture2D>("Images/DialBG");
            //Scene background
            ResourceManager.Textures["BG"] = Content.Load<Texture2D>("Images/Background");

            ResourceManager.Fonts["TestFont"] = Content.Load<SpriteFont>("Fonts/TestFont");


            int dialSize = 512;
            GameObjects = new List<GameObject>();
            GameObjects.Add(new NuclearDial("nd", "NuclearDial", new Vector2(dialSize, dialSize), new Vector2(400, -200)));
            GameObjects.Add(new WorldObject("dialBG", "DialBG", new Vector2(dialSize, dialSize), new Vector2(400, -200), 1));

            GameObjects.Add(new WorldObject("BG", "Background", new Vector2(1920, 1080), new Vector2(), 0));
            ((WorldObject)GameObjects[GameObjects.Count - 1]).SpriteRenderer.Transform.Layer = 0;

            GameObjects.Add(new Button("CoolantButtonUp", "CoolantButtonDown", "NuclearButton", new Vector2(512, 512), new Vector2(-400, -200), 1, () => {
                NuclearLevel.level -= NuclearLevel.reduceAmount;
                if (NuclearLevel.level < 0.0f)
                    NuclearLevel.level = 0.0f;
            }));

            base.loadContent(c);
        }
    }
}
