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
        public static NoCloseForm chatWindow;
        public static NoCloseForm miniGame;
        public static NoCloseForm miniGameTwo;
        public GameManager()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
        }

        protected override void Initialize()
        {
            chatWindow = new NoCloseForm();
            chatWindow.Show();
            miniGame = new NoCloseForm();
            miniGame.Show();
            miniGameTwo = new NoCloseForm();
            miniGameTwo.Show();

            // TODO: Add your initialization logic here
            InputManager.Initilize();
            RenderingManager.Initilize(GraphicsDevice);
            SoundManager.Initilize();
            CollisionManager.Initilize();
            CoroutineManager.Initilize();
            CameraManager.Initilize();
            SceneManager.Initilize(Content, new TestScene());


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

            SceneManager.Update(TimeManager.DeltaTime);

            SoundManager.Update(TimeManager.DeltaTime);

            CameraManager.Update(TimeManager.DeltaTime);

            CollisionManager.Update(TimeManager.DeltaTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            RenderingManager.Draw(TimeManager.DeltaTime);

            base.Draw(gameTime);
        }
    }
}
