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
            size = new Vector2(250, 250);
            //CollisionManager.Initilize();
            Vector2 screenCenter = new Vector2(-835, 415);


            CameraManager.Cameras[0].SetMinPos(size/2 * -1);
            CameraManager.Cameras[0].SetMaxPos(size/2);
            
            // Textures
            ResourceManager.Textures = new Dictionary<string, Texture2D>();
            ResourceManager.Textures["CoolantButton"] = Content.Load<Texture2D>("Images/Nuclear/button");
            ResourceManager.Textures["CoolantButtonHover"] = Content.Load<Texture2D>("Images/Nuclear/button_hover");
            ResourceManager.Textures["CoolantButtonPress"] = Content.Load<Texture2D>("Images/Nuclear/button_press");
            ResourceManager.Textures["DialArrow"] = Content.Load<Texture2D>("Images/Nuclear/dial_arrow");
            ResourceManager.Textures["DialBackDark"] = Content.Load<Texture2D>("Images/Nuclear/dial_back_darkened");
            ResourceManager.Textures["DialBack"] = Content.Load<Texture2D>("Images/Nuclear/dial_back");
            ResourceManager.Textures["DialBorder"] = Content.Load<Texture2D>("Images/Nuclear/dial_border");
            ResourceManager.Textures["HeatText"] = Content.Load<Texture2D>("Images/Nuclear/heat_text");
            ResourceManager.Textures["BG"] = Content.Load<Texture2D>("Images/Nuclear/background");
            ResourceManager.Textures["UpdateOverlay"] = Content.Load<Texture2D>("Images/Nuclear/UpdatingOverlay");
            ResourceManager.Textures["UpdateSpinner"] = Content.Load<Texture2D>("Images/Nuclear/update_arrows");
            ResourceManager.Textures["lock"] = Content.Load<Texture2D>("Images/Nuclear/lock_white");
            ResourceManager.SoundEffects["alert"] = Content.Load<SoundEffect>("Sound/alert");
            ResourceManager.SoundEffects["Click1"] = Content.Load<SoundEffect>("Sound/click1");
            ResourceManager.SoundEffects["Click2"] = Content.Load<SoundEffect>("Sound/click2");
            ResourceManager.SoundEffects["Click3"] = Content.Load<SoundEffect>("Sound/click3");
            ResourceManager.SoundEffects["Boot"] = Content.Load<SoundEffect>("Sound/machine_starting");
            ResourceManager.SoundEffects["MysterySound"] = Content.Load<SoundEffect>("Sound/arabian_harp");

            Vector2 dialSize = new Vector2(200, 200);
            GameObjects = new List<GameObject>();

            GameObjects.Add(new WorldObject("BG", "Background", new Vector2(250, 250), screenCenter, 0));
            
            GameObjects.Add(new WorldObject("UpdateOverlay", "UpdateOverlay", new Vector2(250, 250), screenCenter, 10));
            WorldObject overlay = (WorldObject)GameObjects[^1];
            overlay.SpriteRenderer.Visible = false;
            AnimationData oad = (AnimationData)overlay.ComponentHandler.AddComponent(new AnimationData(overlay, "AnimationData", overlay.SpriteRenderer, 2));
            overlay.BehaviorHandler.AddBehavior("Overlay", Behaviors.UpdateNuclear, new Component[] { oad });

            WorldObject updateSpinner = new WorldObject("UpdateSpinner", "UpdateSpinner", new Vector2(80f, 80f), screenCenter, 10);
            updateSpinner.SpriteRenderer.Visible = false;
            updateSpinner.BehaviorHandler.AddBehavior("UpdateSpinner", Behaviors.UpdateSpinner, new Component[] { updateSpinner.SpriteRenderer });
            GameObjects.Add(updateSpinner);



            GameObjects.Add(new WorldObject("HeatText", "HeatText", new Vector2(96, 32), screenCenter + new Vector2(0, 86), 1));

            Vector2 buttonCenterPos = screenCenter + new Vector2(0, -32);

            WorldObject dialBackObj = new WorldObject("DialBackDark", "DialBG", dialSize, buttonCenterPos, 1);
            NuclearDial dialArrowObj = new NuclearDial("DialArrow", "NuclearDial", dialSize, buttonCenterPos, 2);
            WorldObject dialBorderObj = new WorldObject("DialBorder", "DialBorder", dialSize, buttonCenterPos, 3);
            dialArrowObj.Transform.AttachToTransform(dialBackObj.Transform);
            GameObjects.Add(dialBackObj);
            GameObjects.Add(dialArrowObj);
            GameObjects.Add(dialBorderObj);

            Vector2 lockoutPos = buttonCenterPos + new Vector2(0f, 5f);
            WorldObject visualLockout = new WorldObject("lock", "Lockout", new Vector2(100f, 100f), lockoutPos, 5);
            visualLockout.Transform.SetScale(0.6f, 0.6f);
            visualLockout.BehaviorHandler.AddBehavior("lockoutUpdate", Behaviors.LockoutUpdate, new Component[] { visualLockout.SpriteRenderer });
            GameObjects.Add(visualLockout);

            Vector2 cooldownButtonSize = new Vector2(100, 100);
            Button cooldownButton = new Button("CoolantButton", "CoolantButtonPress", "NuclearButton", cooldownButtonSize, buttonCenterPos, 4, () =>
            {
                if (!NuclearLevel.started)
                {
                    CurrentWindow.coroutineManager.AddCoroutine(Coroutines.BootUp(dialBackObj.SpriteRenderer), "BootUp", 0, true);
                    
                }
                if (!NuclearLevel.Locked)
                {
                    Random r = new Random();
                    SoundManager.PlaySoundEffect(Globals.ClickSounds[r.Next(0, 3)]);

                    NuclearLevel.buttonHit = true;

                    NuclearLevel.ButtonHitStopTime = 1.0f + (float)r.NextDouble() * 2.0f;

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
            //cooldownButton.BehaviorHandler.Behaviors.Remove(cooldownButton.BehaviorHandler.GetBehavior("Hover"));
            GameObjects.Add(cooldownButton);
             
            base.loadContent(c);
        }
    }
}
