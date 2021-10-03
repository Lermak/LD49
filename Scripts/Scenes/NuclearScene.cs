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
            ResourceManager.Textures["DialArrow"] = Content.Load<Texture2D>("Images/Nuclear/dial_arrow");
            ResourceManager.Textures["DialBack"] = Content.Load<Texture2D>("Images/Nuclear/dial_back");
            ResourceManager.Textures["DialBorder"] = Content.Load<Texture2D>("Images/Nuclear/dial_border");
            //Scene background
            ResourceManager.Textures["BG"] = Content.Load<Texture2D>("Images/Background");

            Vector2 dialSize = new Vector2(200, 200);
            GameObjects = new List<GameObject>();

            WorldObject dialBackObj = new WorldObject("DialBack", "DialBG", dialSize, new Vector2(-360, 140), 1);
            NuclearDial dialArrowObj = new NuclearDial("DialArrow", "NuclearDial", dialSize, new Vector2(-360, 140), 2);
            WorldObject dialBorderObj = new WorldObject("DialBorder", "DialBorder", dialSize, new Vector2(-360, 140), 3);
            dialArrowObj.Transform.AttachToTransform(dialBackObj.Transform);
            GameObjects.Add(dialBackObj);
            GameObjects.Add(dialArrowObj);
            GameObjects.Add(dialBorderObj);

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
