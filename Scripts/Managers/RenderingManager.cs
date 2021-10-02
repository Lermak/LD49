﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;
using System.Windows.Forms;

namespace MonoGame_Core.Scripts
{
    /// <summary>
    /// Handles the orginization and rendering of all spriteRenders to their sources
    /// </summary>
    public static class RenderingManager
    {
        /// <summary>
        /// Rendering order determines how items will be layered and orgized
        /// </summary>
        public enum RenderOrder { SideScrolling, TopDown, Isometric }
        /// <summary>
        /// Width of the target BackBuffer
        /// </summary>
        public const float WIDTH = 1920;
        /// <summary>
        /// Heigh of the target BackBuffer
        /// </summary>
        public const float HEIGHT = 1080;

        /// <summary>
        /// The global scale of the game after adjusting for the window size
        /// </summary>
        public static Vector2 GameScale { get { return WindowScale * BaseScale; } }
        /// <summary>
        /// The global scale of the game, before adjusting for window size
        /// </summary>
        public static Vector2 BaseScale = new Vector2(1, 1);
        /// <summary>
        /// The scale of the window compaired to the target size
        /// </summary>
        public static Vector2 WindowScale = new Vector2(1, 1);
        /// <summary>
        /// Global color value applied to produce a fade effect between scenes
        /// </summary>
        public static float GlobalFade = 255;
        /// <summary>
        /// Set the rendering order
        /// </summary>
        public static RenderOrder RenderingOrder = RenderOrder.SideScrolling;

        /// <summary>
        /// Render targets are what Cameras use to store image data
        /// </summary>
        public static List<RenderTarget2D> RenderTargets;
        public static List<SwapChainRenderTarget> WindowTargets;
        /// <summary>
        /// The list of all game sprites
        /// </summary>
        public static List<SpriteRenderer> Sprites;
        /// <summary>
        /// The batch to use for sending clustered sprite data to render targets
        /// **NOTE** consider using multiple to allow for multiple processes
        /// </summary>
        private static SpriteBatch spriteBatch;
        /// <summary>
        /// MonoGame's object for handling communication with the graphics card
        /// </summary>
        private static GraphicsDevice graphicsDevice;
        

        /// <summary>
        /// Setup the current state of the rendering manager.
        /// This includes creating any render targets that cameras will need
        /// </summary>
        /// <param name="gd">Game Time</param>
        public static void Initilize(GraphicsDevice gd)
        {
            graphicsDevice = gd;
            spriteBatch = new SpriteBatch(graphicsDevice);
            Sprites = new List<SpriteRenderer>();
            RenderTargets = new List<RenderTarget2D>();
            WindowTargets = new List<SwapChainRenderTarget>();

            RenderTargets.Add(new RenderTarget2D(graphicsDevice,
                (int)1920,
                (int)1080,
                false,
                graphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24,
                0,
                RenderTargetUsage.PlatformContents));

            WindowTargets.Add(new SwapChainRenderTarget(graphicsDevice,
                GameManager.chatWindow.Handle,
                GameManager.chatWindow.Width,
                GameManager.chatWindow.Height,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24Stencil8,
                0,
                RenderTargetUsage.PlatformContents,
                PresentInterval.Default));

            WindowTargets.Add(new SwapChainRenderTarget(graphicsDevice,
                GameManager.chatWindow.Handle,
                GameManager.chatWindow.Width,
                GameManager.chatWindow.Height,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24Stencil8,
                0,
                RenderTargetUsage.PlatformContents,
                PresentInterval.Default));
        }

        /// <summary>
        /// Remove all items from the list of sprites
        /// reset the spriteBatch
        /// </summary>
        public static void Clear()
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
            Sprites = new List<SpriteRenderer>();
        }

        /// <summary>
        /// Update the window scale to reperesent the current window size
        /// Set the target, then sort all items by their camera
        /// Render all items to their designated targets
        /// Draw all cameras
        /// </summary>
        /// <param name="gt"></param>
        public static void Draw(float gt)
        {
            var x = graphicsDevice.GetRenderTargets();
            WindowScale = new Vector2(graphicsDevice.Viewport.Width / WIDTH, graphicsDevice.Viewport.Height / HEIGHT);

            string prevShader = "";
            int Target = -1;

            SetTarget(Target);

            graphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            IEnumerable<Camera> cameras = CameraManager.Cameras.OrderByDescending(s => s.Target);
           
            foreach (Camera c in cameras)
            {
                if(c.Target == -1) continue;

                foreach (SpriteRenderer sr in Sprites)
                {
                    if (sr.Visible && sr.Cameras.Contains(c))
                    {
                        if (sr.Shader != prevShader)
                        {
                            if (c.Target == Target)
                            {
                                spriteBatch.End();
                                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                            }

                            prevShader = sr.Shader;
                        }

                        if (c.Target != Target)
                        {
                            spriteBatch.End();

                            SetTarget(c.Target);
                            graphicsDevice.Clear(Color.Transparent);

                            Target = c.Target;

                            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                        }

                        if (sr.Shader != "")
                        {
                            foreach (EffectTechnique t in SceneManager.CurrentScene.Effects[sr.Shader].Techniques)
                            {
                                foreach (EffectPass p in t.Passes)
                                {
                                    p.Apply();
                                    sr.Draw(spriteBatch, c);
                                }
                            }
                        }
                        else
                        {
                            sr.Draw(spriteBatch, c);
                        }
                    }
                }
            }
            spriteBatch.End();

            IEnumerable<Camera> camerasBySwapChain = CameraManager.Cameras.OrderByDescending(s => s.SwapChain);
            int currentSwapChain = -1;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            foreach (Camera c in cameras)
            {
                if (c.SwapChain == -1) continue;

                if(currentSwapChain != c.SwapChain)
                {
                    spriteBatch.End();
                    currentSwapChain = c.SwapChain;
                    graphicsDevice.SetRenderTarget(RenderingManager.WindowTargets[currentSwapChain]);

                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    c.Draw(spriteBatch);
                }
            }
            spriteBatch.End();
            SetTarget(-1);

            foreach (var chain in RenderingManager.WindowTargets)
            {
                chain.Present();
            }

            SetTarget(-1);
            graphicsDevice.Clear(Color.Transparent);

            Target = -1;
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            
            foreach (Camera c in cameras)
            {
                if (c.Target != -1) continue;

                foreach (SpriteRenderer sr in Sprites)
                {
                    if (sr.Visible && sr.Cameras.Contains(c))
                    {
                        if (sr.Shader != prevShader)
                        {
                            prevShader = sr.Shader;
                            spriteBatch.End();
                            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                        }

                        if (sr.Shader != "")
                        {
                            foreach (EffectTechnique t in SceneManager.CurrentScene.Effects[sr.Shader].Techniques)
                            {
                                foreach (EffectPass p in t.Passes)
                                {
                                    p.Apply();
                                    sr.Draw(spriteBatch, c);
                                }
                            }
                        }
                        else
                        {
                            sr.Draw(spriteBatch, c);
                        }
                    }
                }
            }
            spriteBatch.End();
            graphicsDevice.Present();
        }

        /// <summary>
        /// Sort the sprite list based on the current sort type
        /// </summary>
        public static void Sort()
        {
            IEnumerable<Camera> cameras = CameraManager.Cameras.OrderByDescending(s => s.Target);

            IEnumerable<SpriteRenderer> s = Sprites;

            foreach (Camera c in cameras)
            {
                if (RenderingOrder == RenderOrder.SideScrolling)
                    s = Sprites.OrderBy(s => s.Shader)
                                .ThenBy(s => s.Transform.Layer)
                                .ThenBy(s => s.OrderInLayer)
                                .Where(s => s.Cameras.Contains(c))
                                .Where(s => Vector2.Distance(s.Transform.Position, c.Transform.Position) <= s.Transform.Radius + c.Transform.Radius);
                else if (RenderingOrder == RenderOrder.TopDown)
                    s = Sprites.OrderBy(s => s.Shader)
                                .ThenBy(s => s.Transform.Layer)
                                .ThenBy(s => s.Transform.Position.Y)
                                .ThenBy(s => s.OrderInLayer)
                                .Where(s => s.Cameras.Contains(c))
                                .Where(s => Vector2.Distance(s.Transform.Position, c.Transform.Position) <= s.Transform.Radius + c.Transform.Radius);

                else if (RenderingOrder == RenderOrder.Isometric)
                {

                }
            }

            Sprites = s.ToList();
        }

        /// <summary>
        /// Changes the current Render Target
        /// </summary>
        /// <param name="Target">new target id</param>
        public static void SetTarget(int Target)
        {
            if (Target < 0 || Target >= RenderTargets.Count)
                graphicsDevice.SetRenderTarget(null);
            else
                graphicsDevice.SetRenderTarget(RenderTargets[Target]);
        }
        public static void SetWindow(int Target)
        {
            if (Target < 0 || Target >= WindowTargets.Count)
                graphicsDevice.SetRenderTarget(null);
            else
                graphicsDevice.SetRenderTarget(WindowTargets[Target]);
        }
    }
}
