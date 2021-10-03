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
            
            // Textures
            ResourceManager.Textures = new Dictionary<string, Texture2D>();
            ResourceManager.Textures["CoolantButton"] = Content.Load<Texture2D>("Images/Nuclear/button");
            ResourceManager.Textures["CoolantButtonHover"] = Content.Load<Texture2D>("Images/Nuclear/button_hover");
            ResourceManager.Textures["CoolantButtonPress"] = Content.Load<Texture2D>("Images/Nuclear/button_press");
            ResourceManager.Textures["DialArrow"] = Content.Load<Texture2D>("Images/Nuclear/dial_arrow");
            ResourceManager.Textures["DialBack"] = Content.Load<Texture2D>("Images/Nuclear/dial_back");
            ResourceManager.Textures["DialBorder"] = Content.Load<Texture2D>("Images/Nuclear/dial_border");
            ResourceManager.Textures["HeatText"] = Content.Load<Texture2D>("Images/Nuclear/heat_text");
            ResourceManager.Textures["BG"] = Content.Load<Texture2D>("Images/Nuclear/background");
            ResourceManager.SoundEffects["alert"] = Content.Load<SoundEffect>("Sound/alert");

            Vector2 dialSize = new Vector2(200, 200);
            GameObjects = new List<GameObject>();

            GameObjects.Add(new WorldObject("BG", "Background", new Vector2(250, 250), screenCenter, 0));

            GameObjects.Add(new WorldObject("HeatText", "HeatText", new Vector2(96, 32), screenCenter + new Vector2(0, 86), 1));

            Vector2 buttonCenterPos = screenCenter + new Vector2(0, -32);

            WorldObject dialBackObj = new WorldObject("DialBack", "DialBG", dialSize, buttonCenterPos, 1);
            NuclearDial dialArrowObj = new NuclearDial("DialArrow", "NuclearDial", dialSize, buttonCenterPos, 2);
            WorldObject dialBorderObj = new WorldObject("DialBorder", "DialBorder", dialSize, buttonCenterPos, 3);
            dialArrowObj.Transform.AttachToTransform(dialBackObj.Transform);
            GameObjects.Add(dialBackObj);
            GameObjects.Add(dialArrowObj);
            GameObjects.Add(dialBorderObj);

            Vector2 cooldownButtonSize = new Vector2(100, 100);
            Button cooldownButton = new Button("CoolantButton", "CoolantButtonPress", "NuclearButton", cooldownButtonSize, buttonCenterPos, 4, () =>
            {
                if (!NuclearLevel.started)
                    NuclearLevel.started = true;
                if (!NuclearLevel.Locked)
                {
                    NuclearLevel.level -= NuclearLevel.reduceAmount;
                    if (NuclearLevel.level < 0.0f)
                        NuclearLevel.level = 0.0f;
                }
            });
            cooldownButton.BehaviorHandler.AddBehavior("clickSwapAnim", Behaviors.ButtonSwapImagesOnClick, new Component[] {
                    cooldownButton.Transform, 
                    cooldownButton.ComponentHandler.GetComponent("ButtonData"),
                    cooldownButton.ComponentHandler.GetComponent("AnimationData")
            });
            cooldownButton.BehaviorHandler.Behaviors.Remove(cooldownButton.BehaviorHandler.GetBehavior("Hover"));
            GameObjects.Add(cooldownButton);
             
            base.loadContent(c);
        }
    }
}
