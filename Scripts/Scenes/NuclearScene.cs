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
            ResourceManager.Fonts["TestFont"] = Content.Load<SpriteFont>("Fonts/TestFont");

            ResourceManager.Textures["UprightChalk"] = Content.Load<Texture2D>("Images/Nuclear/chalk_held");
            ResourceManager.Textures["SideChalk"] = Content.Load<Texture2D>("Images/Nuclear/chalk_laying");
            ResourceManager.Textures["ChalkBox"] = Content.Load<Texture2D>("Images/Nuclear/chalk_box");
            ResourceManager.Textures["Dust"] = Content.Load<Texture2D>("Images/Nuclear/chalk_smudge");

            ResourceManager.Textures["EvilBackground"] = Content.Load<Texture2D>("Images/Nuclear/background_evil");
            ResourceManager.Textures["SoulsTitle"] = Content.Load<Texture2D>("Images/Nuclear/souls_text");
            ResourceManager.Textures["EvilButton"] = Content.Load<Texture2D>("Images/Nuclear/evil_button");
            ResourceManager.Textures["EvilButtonPress"] = Content.Load<Texture2D>("Images/Nuclear/evil_button_press");

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
            ResourceManager.SoundEffects["Boot"] = Content.Load<SoundEffect>("Sound/machine_starting");
            ResourceManager.SoundEffects["MysterySound"] = Content.Load<SoundEffect>("Sound/arabian_harp");
            ResourceManager.SoundEffects["Shutdown"] = Content.Load<SoundEffect>("Sound/machine_stopping");
            ResourceManager.SoundEffects["Explosion"] = Content.Load<SoundEffect>("Sound/explosion");
            ResourceManager.Songs["OminousMusic"] = Content.Load<Song>("Music/ominous_piano");
            ResourceManager.Songs["EndTimes"] = Content.Load<Song>("Music/final_ritual_loop");
            ResourceManager.Songs["EndTimesOpening"] = Content.Load<Song>("Music/final_ritual_opening");

            Vector2 dialSize = new Vector2(200, 200);
            GameObjects = new List<GameObject>();


            GameObjects.Add(new GameObject("EndTimesDetection"));
            GameObject endTimes = GameObjects[^1];
            endTimes.BehaviorHandler.AddBehavior("DetectChalk", Behaviors.SpawnChalk, new Component[] { });


            GameObjects.Add(new WorldObject("BG", "Background", new Vector2(250, 250), screenCenter, 0));
            WorldObject background = (WorldObject)GameObjects[^1];
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
            WorldObject text = (WorldObject)GameObjects[^1];
            Vector2 buttonCenterPos = screenCenter + new Vector2(0, -32);

            WorldObject dialBackObj = new WorldObject("DialBackDark", "DialBG", dialSize, buttonCenterPos, 1);
            dialBackObj.BehaviorHandler.AddBehavior("NuclearDeath", Behaviors.NuclearDeath, new Component[] { dialBackObj.SpriteRenderer });
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
                    if (!ChalkData.Held)
                    {
                        Random r = new Random();
                        SoundManager.PlaySoundEffect(Globals.ClickSounds[r.Next(0, 2)]);
                    
                        if (!Globals.ButtonNotCool)
                        {
                            NuclearLevel.buttonHit = true;

                            NuclearLevel.ButtonHitStopTime = 1.0f + (float)r.NextDouble() * 2.0f;

                            NuclearLevel.level -= NuclearLevel.reduceAmount;
                            if (NuclearLevel.level < 0.0f)
                                NuclearLevel.level = 0.0f;
                        }

                        if (Globals.ExpectFinalButtonPush)
                        {
                            Globals.FinalButtonPush = true;
                        }
                    }
                }
            });
            cooldownButton.BehaviorHandler.AddBehavior("clickSwapAnim", Behaviors.ButtonSwapImagesOnClick, new Component[] {
                    cooldownButton.Transform, 
                    cooldownButton.ComponentHandler.GetComponent("ButtonData"),
                    cooldownButton.ComponentHandler.GetComponent("AnimationData")
            });
            //cooldownButton.BehaviorHandler.Behaviors.Remove(cooldownButton.BehaviorHandler.GetBehavior("Hover"));
            GameObjects.Add(cooldownButton);


            endTimes.BehaviorHandler.AddBehavior("ChangeOverlay", Behaviors.ChangeOverlay, new Component[] { background.SpriteRenderer, text.SpriteRenderer, cooldownButton.ComponentHandler.GetComponent("ButtonData") });


            base.loadContent(c);
        }
    }
}
