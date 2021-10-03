using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_Core.Scripts
{
    /// <summary>
    /// A wrapper class to help manage user input functions
    /// </summary>
    public class InputManager
    {
        WindowData parentWindow;

        const float DOUBLE_CLICK_DELAY = 1f;
        float timeSinceLastLeftClick = 0;
        Vector2 mousePos;
        bool firstClick = false;
        public bool IsDoubleClick = false;
        public enum MouseKeys { LeftButton, RightButton, MiddleButton }
        
        static KeyboardState currentKeyboardState;

        public Vector2 MousePos { get { return mousePos; } }

        KeyboardState previousKeyboardState;
        public KeyboardState PreviousKeyboardState { get { return previousKeyboardState; } }

        MouseState currentMouseState;
        MouseState previousMouseState;
        public MouseState PreviousMouseState { get { return previousMouseState; } }
        public bool IsKeyPressed(Keys k)
        {
            return currentKeyboardState.IsKeyDown(k);
        }

        /// <summary>
        /// A key is triggered if it was pressed this loop
        /// </summary>
        /// <param name="k">The key to check</param>
        /// <returns>true if the key was pressed this loop</returns>
        public bool IsKeyTriggered(Keys k)
        {
            return currentKeyboardState.IsKeyDown(k) && !previousKeyboardState.IsKeyDown(k);
        }

        public bool IsMouseDown(MouseKeys b)
        {
            if (b == MouseKeys.LeftButton)
            {
                return currentMouseState.LeftButton == ButtonState.Pressed;
            }
            else if (b == MouseKeys.RightButton)
            {
                return currentMouseState.RightButton == ButtonState.Pressed;
            }
            else if (b == MouseKeys.MiddleButton)
            {
                return currentMouseState.MiddleButton == ButtonState.Pressed;
            }

            return false;
        }
        /// <summary>
        /// A mouse button is triggered if it was pressed this loop
        /// </summary>
        /// <param name="k">The mouse button to check</param>
        /// <returns>true if the mouse button was pressed this loop</returns>
        public bool IsMouseTriggered(MouseKeys b)
        {
            if (b == MouseKeys.LeftButton)
            {
                return currentMouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton != ButtonState.Pressed;
            }
            else if (b == MouseKeys.RightButton)
            {
                return currentMouseState.RightButton == ButtonState.Pressed && previousMouseState.RightButton != ButtonState.Pressed;
            }
            else if (b == MouseKeys.MiddleButton)
            {
                return currentMouseState.MiddleButton == ButtonState.Pressed && previousMouseState.MiddleButton != ButtonState.Pressed;
            }

            return false;
        }

        public InputManager(WindowData parentWindow)
        {
            this.parentWindow = parentWindow;
            currentKeyboardState = new KeyboardState();
            currentMouseState = new MouseState();
        }

        /// <summary>
        /// Check if a double click has occurred, and change the double click flag to true if it has.
        /// </summary>
        /// <param name="gt">Game Time</param>
        private void checkDoubleClick(float gt)
        {
            if (IsDoubleClick)
                IsDoubleClick = false;

            if (!firstClick)
            {
                if (IsMouseTriggered(MouseKeys.LeftButton))
                {
                    firstClick = true;
                }
            }
            else
            {
                if (timeSinceLastLeftClick < DOUBLE_CLICK_DELAY)
                {
                    timeSinceLastLeftClick += gt;
                    if (IsMouseTriggered(MouseKeys.LeftButton))
                    {
                        timeSinceLastLeftClick = 0;
                        firstClick = false;
                        IsDoubleClick = true;
                    }
                }
                else
                {
                    timeSinceLastLeftClick = 0;
                    firstClick = false;
                }
            }
        }

        /// <summary>
        /// Get the current state of they keyboard, and move the current state to the previous
        /// </summary>
        /// <param name="gt">Game Time</param>
        public void Update(float gt)
        {
            Vector2 p = parentWindow.getRelativeCursorPos();
            mousePos = new Vector2(p.X, -p.Y) / RenderingManager.WindowScale - new Vector2(RenderingManager.WIDTH / 2, -RenderingManager.HEIGHT / 2);

            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            checkDoubleClick(gt);
        }
    }
}
