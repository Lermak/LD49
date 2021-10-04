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
            System.Drawing.Rectangle screenRectangle = f.RectangleToScreen(f.ClientRectangle);
            int titleHeight = screenRectangle.Top - f.Top;
            int sideWidth = screenRectangle.Left - f.Left;
            return new Vector2(pt.X + sideWidth, pt.Y + titleHeight);
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
        public float GlobalFade = 0;

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
        public static Window MainWindow;

        public static Window DigiPetWindow;

        public static Window ITHelp;
        public static bool killIT = false;

        public static Window UpdateWindow;
        public static bool KillUpdate = false;

        public static Window ReauthWindow;
        public static bool KillReauth = false;

        public static Window BadConnectionWindow;
        public static bool KillBadConnection = false;

        public static Window ResetKeysWindow;
        public static bool KillResetKeys = false;

        public static Window RoboTestchaWindw;
        public static bool KillRoboTestcha = false;

        public static Window GalaxyBlasterWindow;
        public static bool KillGalaxyBlaster = false;

        public static Window SecurityCheckWindow;

        public static List<Window> ToAdd = new List<Window>();

        private static ContentManager contentManager;
        public static void Initilize(ContentManager cm, Scene s)
        {
            contentManager = cm;
            Windows = new List<Window>();

            Windows.Add(new Window(null));

            CurrentWindow._window = Windows[0];
            Windows[0].sceneManager.Initilize(cm, s, new List<Camera>() { CameraManager.Cameras[0] });
            MainWindow = Windows[0];
        }

        public static Window AddWindow(Form f, string BlackboardConnection, Scene s, Vector2 size)
        {
            Window w = new Window(f);
            if (BlackboardConnection == "DigiPetWindow")
            {
                f.Text = "DigiPet";
                DigiPetWindow = w;
            }
            else if (BlackboardConnection == "ITHelp")
            {
                f.Text = "IT Help";
                ITHelp = w;
            }
            else if (BlackboardConnection == "UpdateWindow")
            {
                f.Text = "Software Update Required";
                UpdateWindow = w;
            }
            else if (BlackboardConnection == "ReauthWindow")
            {
                f.Text = "Re-Authorization";
                ReauthWindow = w;
            }
            else if (BlackboardConnection == "ResetKeysWindow")
            {
                f.Text = "ERROR 257: Data Keys Need Reset";
                ResetKeysWindow = w;
            }
            else if (BlackboardConnection == "RoboTestchaWindow")
            {
                f.Text = "ERROR 184: RoboTestcha Check Required";
                RoboTestchaWindw = w;
            }
            else if (BlackboardConnection == "BadConnectionWindow")
            {
                f.Text = "ERROR 312: Disconnected From Server";
                BadConnectionWindow = w;
            }
            else if (BlackboardConnection == "GalaxyBlasterWindow")
            {
                f.Text = "Galaxy Blaster";
                GalaxyBlasterWindow = w;
            }
            else if (BlackboardConnection == "SecurityCheckWindow")
            {
                f.Text = "Security Screening";
                SecurityCheckWindow = w;
            }

            ToAdd.Add(w);
            f.Size = new System.Drawing.Size((int)(size.X * GameManager.WidthScale), (int)(size.Y * GameManager.HeightScale));
            if(f is NoCloseForm)
            {
                Screen screen = Screen.FromHandle(GameManager.Instance.Window.Handle);
                Random r = new Random();
                int x = r.Next(0, (int)(screen.Bounds.Width - (size.X + 250) * GameManager.WidthScale));
                int y = r.Next(0, (int)(screen.Bounds.Height - (size.Y + 250) * GameManager.HeightScale));
                f.Location = new System.Drawing.Point(x, y);
                f.FormBorderStyle = FormBorderStyle.FixedSingle;
            }
            ToAdd[ToAdd.Count - 1].form.Show();

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
            //
            //RenderingManager.RenderTargets.Remove(w.renderTarget);
            //IEnumerable<Camera> cams = CameraManager.Cameras.Where(c => c.SwapChain != -1)
            //                                                .Where(c => RenderingManager.WindowTargets[c.SwapChain] == w.swapChainTarget);
            //foreach (Camera c in cams)
            //    CameraManager.Cameras.Remove(c);
            //RenderingManager.WindowTargets.Remove(w.swapChainTarget);
            w.form.Dispose();
            Windows.Remove(w);
            
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
            foreach(Window w in ToAdd)
            {
                Windows.Add(w);
            }
            ToAdd.Clear();
            if (killIT)
            {
                killIT = false;
                WindowManager.RemoveWindow(WindowManager.ITHelp);
                WindowManager.ITHelp = null;
            }
            if (KillUpdate)
            {
                KillUpdate = false;
                WindowManager.RemoveWindow(WindowManager.UpdateWindow);
                WindowManager.UpdateWindow = null;
            }
            if (KillReauth)
            {
                KillReauth = false;
                WindowManager.RemoveWindow(WindowManager.ReauthWindow);
                WindowManager.ReauthWindow = null;
            }
            if (KillBadConnection)
            {
                KillBadConnection = false;
                WindowManager.RemoveWindow(WindowManager.BadConnectionWindow);
                WindowManager.BadConnectionWindow = null;
            }
            if (KillResetKeys)
            {
                KillResetKeys = false;
                WindowManager.RemoveWindow(WindowManager.ResetKeysWindow);
                WindowManager.ResetKeysWindow = null;
            }
        }
    }
}
