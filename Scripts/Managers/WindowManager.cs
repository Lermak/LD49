using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MonoGame_Core.Scripts
{
    public static class WindowManager
    {
        public static List<Form> Windows;

        public static void Initilize()
        {
            Windows = new List<Form>();
        }

        public static void AddWindow(Form f, Vector2 size)
        {
            Windows.Add(f);
            f.Size = new System.Drawing.Size((int)size.X, (int)size.Y);
            Windows[Windows.Count - 1].Show();
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
            

        }

        public static void RemoveWindow(Form f)
        {
            Windows.Remove(f);
            f.Dispose();
        }

        public static void Update(float gt)
        {

        }
    }
}
