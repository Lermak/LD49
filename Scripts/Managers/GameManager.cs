using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Windows.Forms;

namespace MonoGame_Core.Scripts
{
    public class GameManager : Game
    {
        public static GameManager Instance;

        private GraphicsDeviceManager _graphics;
        private static bool quit;

        public GameManager()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            Instance = this;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.PreferredBackBufferWidth = 800;

            _graphics.ApplyChanges();
            // TODO: Add your initialization logic here
            ResourceManager.Initilize();
            RenderingManager.Initilize(GraphicsDevice);
            SoundManager.Initilize();
            //CollisionManager.Initilize();
            CameraManager.Initilize();

            var c = new ChatForm();
            c.Show();

            WindowManager.Initilize(Content, new NuclearScene()); 
            WindowManager.AddWindow(new NoCloseForm(), new DigiPetScene(), new Vector2(400,600));
            WindowManager.AddWindow(new NoCloseForm(), new SecurityCheckScene(), new Vector2(600, 300));
            //SceneManager.Initilize(Content, new TestScene());

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
