using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Content;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace MonoGame_Core.Scripts
{
    public interface WindowData
    {
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }

        public Vector2 getRelativeCursorPos();
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

        public Vector2 getRelativeCursorPos()
        {
            var cursorPt = Cursor.Position;
            var pt = f.PointToClient(cursorPt);
            return new Vector2(pt.X, pt.Y);
        }
    }

    public class WindowDataMain : WindowData
    {
        public int X => GameManager.Instance.Window.ClientBounds.X;

        public int Y => GameManager.Instance.Window.ClientBounds.Y;

        public int Width => GameManager.Instance.Window.ClientBounds.Width;

        public int Height => GameManager.Instance.Window.ClientBounds.Height;

        public Vector2 getRelativeCursorPos()
        {
            Point pt = Mouse.GetState().Position;
            return new Vector2(pt.X, pt.Y);
        }
    }

    public class Window
    {
        public Form form;

        public RenderTarget2D renderTarget;
        public SwapChainRenderTarget swapChainTarget;

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
                GameManager.Instance.Window.Title = "Remote Work";
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

        public static Window windowData { set { _window = value; }  get { return _window; } }
        public static InputManager inputManager { get { return windowData.inputManager; } }
        public static SceneManager sceneManager { get { return windowData.sceneManager; } }
        public static KeyboardDispatcher keyboardDispatcher { get { return windowData.keyboardDispatcher; } }
        public static CoroutineManager coroutineManager { get { return windowData.coroutineManager; } }
        public static float GlobalFade { get { return windowData.GlobalFade; } set { windowData.GlobalFade = value; } }
    }

    public static class WindowManager
    {
        public static List<Window> Windows;
        public static Window ITHelp;
        public static bool killIT = false;
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
            f.Size = new System.Drawing.Size((int)(size.X * GameManager.WidthScale), (int)(size.Y * GameManager.HeightScale));
            Windows[Windows.Count - 1].form.Show();

            w.swapChainTarget = new SwapChainRenderTarget(RenderingManager.GraphicsDevice,
                f.Handle,
                (int)size.X,
                (int)size.Y,
                false,
                SurfaceFormat.Color,
                DepthFormat.Depth24Stencil8,
                0,
                RenderTargetUsage.PlatformContents,
                PresentInterval.Default);

            w.renderTarget = new RenderTarget2D(RenderingManager.GraphicsDevice,
                (int)RenderingManager.WIDTH,
                (int)RenderingManager.HEIGHT,
                false,
                RenderingManager.GraphicsDevice.PresentationParameters.BackBufferFormat,
                DepthFormat.Depth24,
                0,
                RenderTargetUsage.PlatformContents);

            RenderingManager.WindowTargets.Add(w.swapChainTarget);
            RenderingManager.RenderTargets.Add(w.renderTarget);

            CameraManager.AddCamera(new Camera("Camera" + CameraManager.Cameras.Count,
                RenderingManager.RenderTargets.Count - 1,
                0,
                RenderingManager.WIDTH,
                RenderingManager.HEIGHT,
                new Vector2(RenderingManager.WIDTH,
                RenderingManager.HEIGHT),
                new Vector2(RenderingManager.WIDTH, RenderingManager.HEIGHT) * -1,
                new Vector2(RenderingManager.WIDTH, RenderingManager.HEIGHT)));

            w.form.ResizeEnd += (object sender, EventArgs e) =>
            {
                var size = w.form.Size;
                int idx = RenderingManager.WindowTargets.FindIndex(0, (sc) => { return sc == w.swapChainTarget; });
                w.swapChainTarget = new SwapChainRenderTarget(RenderingManager.GraphicsDevice,
                   f.Handle,
                   (int)size.Width,
                   (int)size.Height,
                   false,
                   SurfaceFormat.Color,
                   DepthFormat.Depth24Stencil8,
                   0,
                   RenderTargetUsage.PlatformContents,
                   PresentInterval.Default);

                RenderingManager.WindowTargets[idx] = w.swapChainTarget;
            };

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
            if (killIT)
            {
                killIT = false;
                WindowManager.RemoveWindow(WindowManager.ITHelp);
            }
        }
    }
}
