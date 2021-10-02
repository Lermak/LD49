using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;

namespace MonoGame_Core.Scripts
{

    public interface WindowData
    {
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }
    }

    public class WindowDataForm : WindowData
    {
        public Form f;
        public WindowDataForm(Form form)
        {
            this.f = form;
        }

        public int X => f.DesktopLocation.X;

        public int Y => f.DesktopLocation.Y;

        public int Width => f.Width;

        public int Height => f.Height;
    }

    public class WindowDataMain : WindowData
    {
        public int X => GameManager.Instance.Window.ClientBounds.X;

        public int Y => GameManager.Instance.Window.ClientBounds.Y;

        public int Width => GameManager.Instance.Window.ClientBounds.Width;

        public int Height => GameManager.Instance.Window.ClientBounds.Height;
    }

    public class Window
    {
        public Form form;
        public WindowData data;
        public KeyboardDispatcher keyboardDispatcher;
        public InputManager inputManager;
        public SceneManager sceneManager;
        public CoroutineManager coroutineManager;

        /// <summary>
        /// Global color value applied to produce a fade effect between scenes
        /// </summary>
        public float GlobalFade = 255;

        public Window(Form f)
        {
            //Main Window
            if(f == null)
            {
                data = new WindowDataMain();
                keyboardDispatcher = new KeyboardDispatcher(GameManager.Instance.Window.Handle);
            }
            //All other windows
            else
            {
               form = f;
               data = new WindowDataForm(form);
               keyboardDispatcher = new KeyboardDispatcher(f.Handle);
            }

            inputManager = new InputManager(data);
            sceneManager = new SceneManager();
            coroutineManager = new CoroutineManager();
        }
    };

    public static class CurrentWindow {
        public static Window _window;

        public static Window windowData { set { _window = value; } private get { return _window; } }
        public static InputManager inputManager { get { return windowData.inputManager; } }
        public static SceneManager sceneManager { get { return windowData.sceneManager; } }
        public static KeyboardDispatcher keyboardDispatcher { get { return windowData.keyboardDispatcher; } }
        public static CoroutineManager coroutineManager { get { return windowData.coroutineManager; } }
        public static float GlobalFade { get { return windowData.GlobalFade; } set { windowData.GlobalFade = value; } }
    }

        //public static Window AddWindow(Vector2 size)
        //{
        //    Form f = new NoCloseForm();
        //    Window w = new Window();
        //    w.form = f;
        //    w.keyboardDispatcher = new KeyboardDispatcher(f.Handle);
        //    w.inputManager = new InputManager(w);
        //    Windows.Add(w);
//
        //    f.Size = new System.Drawing.Size((int)size.X, (int)size.Y);
        //    f.Show();
    public static class WindowManager
    {
        public static List<Window> Windows;
        private static ContentManager contentManager;
        public static void Initilize(ContentManager cm, Scene s)
        {
            contentManager = cm;
            Windows = new List<Window>();

            Windows.Add(new Window(null));

            CurrentWindow._window = Windows[0];
            Windows[0].sceneManager.Initilize(cm, s, new List<Camera>() { CameraManager.Cameras[0] });
        }

        public static Window AddWindow(Form f, Scene s, Vector2 size)
        {
            Window w = new Window(f);
            Windows.Add(w);
            f.Size = new System.Drawing.Size((int)size.X, (int)size.Y);
            Windows[Windows.Count - 1].form.Show();

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

            CurrentWindow._window = w;
            w.sceneManager.Initilize(contentManager, s, new List<Camera>() { c });

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
                w.inputManager.Update(gt);
                w.coroutineManager.Update(gt);
                w.sceneManager.Update(gt);
            }
        }
    }
}
