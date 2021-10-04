using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Audio;
using MonoGame_Core.Scripts.Managers;

namespace MonoGame_Core.Scripts
{
    public class GameManager : Game
    {
        public static GameManager Instance;
        public static float WidthScale = 1;
        public static float HeightScale = 1;
        public static PlotManager plotManager;
        public static ChatForm chatWindow;

        private GraphicsDeviceManager _graphics;
        private static bool quit;

        public GameManager()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Instance = this;

            Screen screen = Screen.FromHandle(Window.Handle);
            WidthScale = (screen.Bounds.Width / 1920.0f);
            HeightScale = (screen.Bounds.Height / 1080.0f);
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = (int)(250 * WidthScale);
            _graphics.PreferredBackBufferHeight = (int)(250 * HeightScale);

            _graphics.ApplyChanges();
            // TODO: Add your initialization logic here
            ResourceManager.Initilize();
            RenderingManager.Initilize(GraphicsDevice);
            SoundManager.Initilize();
            //CollisionManager.Initilize();
            CameraManager.Initilize();

            ResourceManager.SoundEffects["MessagePop"] = Content.Load<SoundEffect>("Sound/Relaque/message_pop");
            ResourceManager.SoundEffects["MessageNotification"] = Content.Load<SoundEffect>("Sound/Relaque/message_notification");
            chatWindow = new ChatForm();
            chatWindow.Show();

            WindowManager.Initilize(Content, new NuclearScene()); 
            WindowManager.AddWindow(new NoCloseForm(), "DigiPetWindow", new DigiPetScene(), new Vector2(480,330));
            WindowManager.AddWindow(new NoCloseForm(), "ResetKeysWindow", new ResetKeysScene(), new Vector2(600, 200));
            //WindowManager.AddWindow(new NoCloseForm(), new AskITScene(), new Vector2(600, 200));
            //WindowManager.AddWindow(new NoCloseForm(), new UpdateRequiredScene(), new Vector2(600, 200));
            //WindowManager.UpdateWindow = WindowManager.ToAdd[^1];//SceneManager.Initilize(Content, new TestScene());

            plotManager = new PlotManager();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
        }
        public static void Quit()
        {
            quit = true;
        }

        protected override void Update(GameTime gameTime)
        {
            if (quit)
                Exit();

            plotManager.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            // TODO: Add your update logic here
            TimeManager.Update(gameTime);
            
            WindowManager.Update(TimeManager.DeltaTime);

            //SceneManager.Update(TimeManager.DeltaTime);

            SoundManager.Update(TimeManager.DeltaTime);

            CameraManager.Update(TimeManager.DeltaTime);

            //CollisionManager.Update(TimeManager.DeltaTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            RenderingManager.Draw(TimeManager.DeltaTime);

            base.Draw(gameTime);
        }
    }
}
