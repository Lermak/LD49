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
            size = new Vector2(800, 600);
            //CollisionManager.Initilize();

            CameraManager.Cameras[0].SetMinPos(Size / 2 * -1);
            CameraManager.Cameras[0].SetMaxPos(Size / 2);

            ResourceManager.Textures = new Dictionary<string, Texture2D>();
            //NuclearButton!
            ResourceManager.Textures["CoolantButton"] = Content.Load<Texture2D>("Images/Nuclear/button");
            ResourceManager.Textures["CoolantButtonHover"] = Content.Load<Texture2D>("Images/Nuclear/button_hover");
            ResourceManager.Textures["CoolantButtonPress"] = Content.Load<Texture2D>("Images/Nuclear/button_press");
            //Dial
            ResourceManager.Textures["nd"] = Content.Load<Texture2D>("Images/Nuclear/NuclearDial");
            ResourceManager.Textures["dialBG"] = Content.Load<Texture2D>("Images/Nuclear/DialBG");
            //Scene background
            ResourceManager.Textures["BG"] = Content.Load<Texture2D>("Images/Background");

            int dialSize = 75;
            GameObjects = new List<GameObject>();
            GameObjects.Add(new WorldObject("dialBG", "DialBG", new Vector2(dialSize, dialSize), new Vector2(-360, 140), 1));
            GameObjects.Add(new NuclearDial("nd", "NuclearDial", new Vector2(dialSize, dialSize), new Vector2(-360, 140)));
            ((WorldObject)GameObjects[^1]).Transform.AttachToTransform(((WorldObject)GameObjects[^2]).Transform);

            GameObjects.Add(new WorldObject("BG", "Background", new Vector2(1920, 1080), new Vector2(), 0));
            ((WorldObject)GameObjects[GameObjects.Count - 1]).SpriteRenderer.Transform.Layer = 0;

            Vector2 cooldownButtonSize = new Vector2(100, 100);
            Button cooldownButton = new Button("CoolantButton", "CoolantButtonHover", "NuclearButton", cooldownButtonSize, new Vector2(-560, 140), 1, () =>
            {
                if (!NuclearLevel.Locked)
                {
                    NuclearLevel.level -= NuclearLevel.reduceAmount;
                    if (NuclearLevel.level < 0.0f)
                        NuclearLevel.level = 0.0f;
                }
            });
            cooldownButton.Transform.SetScale(1, 1);
            GameObjects.Add(cooldownButton);

            base.loadContent(c);
        }
    }
}
