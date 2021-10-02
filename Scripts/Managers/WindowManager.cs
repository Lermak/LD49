using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;
using System.Linq;

namespace MonoGame_Core.Scripts
{
    public class Window
    {
        public Form Viewport;
        public SceneManager SceneManager;

        public Window(Form f, SceneManager sm)
        {
            Viewport = f;
            SceneManager = sm;
        }
    }

    public static class WindowManager
    {
        public static List<Window> Windows;
        private static ContentManager contentManager;
        public static void Initilize(ContentManager cm, Scene s)
        {
            contentManager = cm;
            Windows = new List<Window>();
            Windows.Add(new Window(null, new SceneManager()));
            Windows[0].SceneManager.Initilize(cm, s, new List<Camera>() { CameraManager.Cameras[0] });
        }

        public static void AddWindow(Form f, Scene s, Vector2 size)
        {
            SceneManager sm = new SceneManager();
            
            Windows.Add(new Window(f, sm));
            f.Size = new System.Drawing.Size((int)size.X, (int)size.Y);
            Windows[Windows.Count - 1].Viewport.Show();
            RenderingManager.WindowTargets.Add(new SwapChainRenderTarget(RenderingManager.GraphicsDevice,
                f.Handle,
                (int)size.X,
                (int)size.Y,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24Stencil8,
                0,
                RenderTargetUsage.PlatformContents,
                PresentInterval.Default));

            RenderingManager.RenderTargets.Add(new RenderTarget2D(RenderingManager.GraphicsDevice,
                (int)1920,
                (int)1080,
                false,
                RenderingManager.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24,
                0,
                RenderTargetUsage.PlatformContents));

            CameraManager.AddCamera(new Camera("Camera" + CameraManager.Cameras.Count,
                RenderingManager.RenderTargets.Count - 1,
                0,
                RenderingManager.WIDTH,
                RenderingManager.HEIGHT,
                new Vector2(RenderingManager.WIDTH,
                RenderingManager.HEIGHT),
                new Vector2(RenderingManager.WIDTH, RenderingManager.HEIGHT) * -1,
                new Vector2(RenderingManager.WIDTH, RenderingManager.HEIGHT)));

            Camera c = CameraManager.Cameras[CameraManager.Cameras.Count - 1];
            c.SwapChain = RenderingManager.WindowTargets.Count-1;

            sm.Initilize(contentManager, s, new List<Camera>() { c });
        }

        public static void RemoveWindow(Form f)
        {
            Window x = Windows.Where(s => s.Viewport == f).First();
            Windows.Remove(x);
        }

        public static void Update(float gt)
        {
            for(int i = 0; i < Windows.Count; ++i)//foreach(Window w in Windows)
            {
                Windows[i].SceneManager.Update(gt);
            }
        }
    }
}
