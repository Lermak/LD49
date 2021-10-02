using Microsoft.Xna.Framework;
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
        public static GraphicsDevice GraphicsDevice { get { return graphicsDevice; } }

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

        class RenderState
        {
            public string prevShader = "";
            public SpriteBatch batch;
        };

        static void RenderCamera(Camera c, RenderState state)
        {
            IEnumerable<SpriteRenderer> visibleContainedSpritesInRange = Sprites.Where(s => s.Visible && s.Cameras.Contains(c) && Vector2.Distance(s.Transform.Position, c.Transform.Position) <= s.Transform.Radius + c.Transform.Radius);
            IEnumerable<SpriteRenderer> sortedSprites = visibleContainedSpritesInRange;

            if (RenderingOrder == RenderOrder.SideScrolling)
                sortedSprites = visibleContainedSpritesInRange.OrderBy(s => s.Shader)
                            .ThenBy(s => s.Transform.Layer)
                            .ThenBy(s => s.OrderInLayer);
            else if (RenderingOrder == RenderOrder.TopDown)
                sortedSprites = visibleContainedSpritesInRange.OrderBy(s => s.Shader)
                            .ThenBy(s => s.Transform.Layer)
                            .ThenBy(s => s.Transform.Position.Y)
                            .ThenBy(s => s.OrderInLayer);

            foreach (SpriteRenderer sr in sortedSprites)
            {
                if (sr.Shader != state.prevShader)
                {
                    state.batch.End();
                    state.batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                    state.prevShader = sr.Shader;
                }

                if (sr.Shader != "")
                {
                    foreach (EffectTechnique t in ResourceManager.Effects[sr.Shader].Techniques)
                    {
                        foreach (EffectPass p in t.Passes)
                        {
                            p.Apply();
                            sr.Draw(state.batch, c);
                        }
                    }
                }
                else
                {
                    sr.Draw(state.batch, c);
                }
            }
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
            WindowScale = new Vector2(1, 1);//new Vector2(graphicsDevice.Viewport.Width / WIDTH, graphicsDevice.Viewport.Height / HEIGHT);

            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(Color.Transparent);

            Dictionary<int, List<Camera>> rtBucket = new Dictionary<int, List<Camera>>();

            //Create buckets for all the cameras that render to a render target
            foreach (Camera c in CameraManager.Cameras)
            {
                if (c.Target == -1) continue;
                if(!rtBucket.ContainsKey(c.Target))
                {
                    rtBucket.Add(c.Target, new List<Camera>());
                }

                rtBucket[c.Target].Add(c);
            }

            //Render all the cameras to their targets
            foreach(var p in rtBucket)
            {
                RenderState renderState = new RenderState();
                renderState.batch = spriteBatch;

                graphicsDevice.SetRenderTarget(RenderingManager.RenderTargets[p.Key]);
                graphicsDevice.Clear(Color.Transparent);

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                foreach (Camera c in p.Value)
                {
                    RenderCamera(c, renderState);
                }

                spriteBatch.End();
            }

            graphicsDevice.SetRenderTarget(null);

            //Next, get all the cameras that render to a swap chain
            Dictionary<int, List<Camera>> scBucket = new Dictionary<int, List<Camera>>();
            foreach (Camera c in CameraManager.Cameras)
            {
                if (c.SwapChain == -1) continue;
                if (!scBucket.ContainsKey(c.SwapChain))
                {
                    scBucket.Add(c.SwapChain, new List<Camera>());
                }

                scBucket[c.SwapChain].Add(c);
            }

            //Render all the cameras to their targets
            foreach (var p in scBucket)
            {
                graphicsDevice.SetRenderTarget(RenderingManager.WindowTargets[p.Key]);
                graphicsDevice.Clear(Color.Black);

                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

                foreach (Camera c in p.Value)
                {
                    c.Draw(spriteBatch);
                }

                spriteBatch.End();
            }

            graphicsDevice.SetRenderTarget(null);

            //Present all the swap chains
            foreach (var chain in RenderingManager.WindowTargets)
            {
                chain.Present();
            }


            //Finally, we draw all of the things to the main window
            foreach (Camera c in CameraManager.Cameras)
            {
                if (c.Target != -1) continue;

                RenderState renderState = new RenderState();
                renderState.batch = spriteBatch;
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                RenderCamera(c, renderState);
                spriteBatch.End();
            }

            graphicsDevice.Present();
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
