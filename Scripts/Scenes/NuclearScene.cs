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
            size = new Vector2(1920, 1080);
            //CollisionManager.Initilize();
            Vector2 screenCenter = new Vector2(-560, 240);


            CameraManager.Cameras[0].SetMinPos(size/2 * -1);
            CameraManager.Cameras[0].SetMaxPos(size/2);
            

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
            GameObjects.Add(new WorldObject("dialBG", "DialBG", new Vector2(dialSize, dialSize), screenCenter, 1));
            GameObjects.Add(new NuclearDial("nd", "NuclearDial", new Vector2(dialSize, dialSize),screenCenter));
            ((WorldObject)GameObjects[GameObjects.Count - 1]).Transform.AttachToTransform(((WorldObject)GameObjects[GameObjects.Count - 2]).Transform);

            GameObjects.Add(new WorldObject("BG", "Background", new Vector2(1920, 1080),screenCenter, 0));
            ((WorldObject)GameObjects[GameObjects.Count - 1]).SpriteRenderer.Transform.Layer = 0;

            Vector2 cooldownButtonSize = new Vector2(100, 100);
            //Wonky, yell at Rhen later
            Vector2 ButtonPosition = screenCenter;
            Button cooldownButton = new Button("CoolantButton", "CoolantButtonHover", "NuclearButton", cooldownButtonSize, ButtonPosition, 1, () =>
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
