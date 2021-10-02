using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace MonoGame_Core.Scripts
{

    public class Window
    {
        public Form form;
        public KeyboardDispatcher keyboardDispatcher;
        public InputManager inputManager;
    };

    public static class CurrentWindow {
        public static Window _window;

        public static Window windowData { set { _window = value; } private get { return _window; } }
        public static InputManager inputManager { get { return windowData.inputManager; } }
    }

    public static class WindowManager
    {

        public static List<Window> Windows;

        public static void Initilize()
        {
            Windows = new List<Window>();
        }

        public static Window AddWindow(Vector2 size)
        {
            Form f = new NoCloseForm();
            Window w = new Window();
            w.form = f;
            w.keyboardDispatcher = new KeyboardDispatcher(f.Handle);
            w.inputManager = new InputManager(w);
            Windows.Add(w);

            f.Size = new System.Drawing.Size((int)size.X, (int)size.Y);
            f.Show();
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

            return w;
        }

        public static void RemoveWindow(Window w)
        {
            Windows.Remove(w);
            w.form.Dispose();
        }

        public static void Update(float gt)
        {
            foreach(Window w in WindowManager.Windows)
            {
                CurrentWindow._window = w;
                
                //Do update for scenes and such
            }
        }
    }
}
