﻿using Microsoft.Xna.Framework;
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
        private GraphicsDeviceManager _graphics;
        private static bool quit;

        public GameManager()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ResourceManager.Initilize();
            InputManager.Initilize();
            RenderingManager.Initilize(GraphicsDevice);
            SoundManager.Initilize();
            //CollisionManager.Initilize();
            CoroutineManager.Initilize();
            CameraManager.Initilize();
            WindowManager.Initilize(Content, new TestScene()); 
            WindowManager.AddWindow(new NoCloseForm(), new TestScene(), new Vector2(1920,1080) / 4);
            //WindowManager.AddWindow(new NoCloseForm(), new TestScene(), new Vector2(1920, 1080) / 4);
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

            InputManager.Update(TimeManager.DeltaTime);

            CoroutineManager.Update(TimeManager.DeltaTime);
            
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
