using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Text;
using System.Windows.Input;
using System.Collections.Generic;
using MonoGame_Core.Scripts;

namespace EventInput
{

    public class KeyboardLayout
    {
        const uint KLF_ACTIVATE = 1; //activate the layout
        const int KL_NAMELENGTH = 9; // length of the keyboard buffer
        const string LANG_EN_US = "00000409";
        const string LANG_HE_IL = "0001101A";

        [DllImport("user32.dll")]
        private static extern long LoadKeyboardLayout(
              string pwszKLID,  // input locale identifier
              uint Flags       // input locale identifier options
              );

        [DllImport("user32.dll")]
        private static extern long GetKeyboardLayoutName(
              System.Text.StringBuilder pwszKLID  //[out] string that receives the name of the locale identifier
              );

        public static string getName()
        {
            System.Text.StringBuilder name = new System.Text.StringBuilder(KL_NAMELENGTH);
            GetKeyboardLayoutName(name);
            return name.ToString();
        }
    }

    public class CharacterEventArgs : EventArgs
    {
        private readonly char character;
        private readonly int lParam;

        public CharacterEventArgs(char character, int lParam)
        {
            this.character = character;
            this.lParam = lParam;
        }

        public char Character
        {
            get { return character; }
        }

        public int Param
        {
            get { return lParam; }
        }

        public int RepeatCount
        {
            get { return lParam & 0xffff; }
        }

        public bool ExtendedKey
        {
            get { return (lParam & (1 << 24)) > 0; }
        }

        public bool AltPressed
        {
            get { return (lParam & (1 << 29)) > 0; }
        }

        public bool PreviousState
        {
            get { return (lParam & (1 << 30)) > 0; }
        }

        public bool TransitionState
        {
            get { return (lParam & (1 << 31)) > 0; }
        }
    }

    public class KeyEventArgs : EventArgs
    {
        private Keys keyCode;

        public KeyEventArgs(Keys keyCode)
        {
            this.keyCode = keyCode;
        }

        public Keys KeyCode
        {
            get { return keyCode; }
        }
    }

    public delegate void CharEnteredHandler(object sender, CharacterEventArgs e);
    public delegate void KeyEventHandler(object sender, KeyEventArgs e);

    public static class EventInput
    {
        public delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        public class WindowData
        {
            public IntPtr prevWndProc;
            public WndProc hookProcDelegate;
            public IntPtr hIMC;

            /// <summary>
            /// Event raised when a character has been entered.
            /// </summary>
            public event CharEnteredHandler CharEntered;

            /// <summary>
            /// Event raised when a key has been pressed down. May fire multiple times due to keyboard repeat.
            /// </summary>
            public event KeyEventHandler KeyDown;

            /// <summary>
            /// Event raised when a key has been released.
            /// </summary>
            public event KeyEventHandler KeyUp;

            public void OnKeyUp(KeyEventArgs args)
            {
                if(KeyUp != null)
                {
                    KeyUp(null, args);
                }
            }

            public void OnKeyDown(KeyEventArgs args)
            {
                if (KeyDown != null)
                {
                    KeyDown(null, args);
                }
            }

            public void OnCharEntered(CharacterEventArgs args)
            {
                if (CharEntered != null)
                {
                    CharEntered(null, args);
                }
            }
        }

        static Dictionary<IntPtr, WindowData> WindowHooks = new Dictionary<IntPtr, WindowData>();

        //various Win32 constants that we need
        const int GWL_WNDPROC = -4;
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        const int WM_CHAR = 0x102;
        const int WM_IME_SETCONTEXT = 0x0281;
        const int WM_INPUTLANGCHANGE = 0x51;
        const int WM_GETDLGCODE = 0x87;
        const int WM_IME_COMPOSITION = 0x10f;
        const int DLGC_WANTALLKEYS = 4;
        const int WM_WM_ACTIVATEAPP = 0x001C;

        //Win32 functions that we're using
        [DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr ImmGetContext(IntPtr hWnd);

        [DllImport("Imm32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr ImmAssociateContext(IntPtr hWnd, IntPtr hIMC);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW")]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr lpdwProcessId);


        /// <summary>
        /// Initialize the TextInput with the given GameWindow.
        /// </summary>
        /// <param name="window">The XNA window to which text input should be linked.</param>
        public static WindowData AddWindow(IntPtr WindowHandle)
        {
            if (WindowHooks.ContainsKey(WindowHandle))
                return WindowHooks[WindowHandle];

            WindowData d = new WindowData();
            d.hookProcDelegate = new WndProc(HookProc);
            var ptr = Marshal.GetFunctionPointerForDelegate(d.hookProcDelegate);
            d.prevWndProc = (IntPtr)SetWindowLongPtr64(WindowHandle, GWL_WNDPROC, ptr);

            d.hIMC = ImmGetContext(WindowHandle);

            WindowHooks.Add(WindowHandle, d);

            return d;
        }

        static IntPtr HookProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            WindowData data = WindowHooks[hWnd];
            IntPtr returnCode = CallWindowProc(data.prevWndProc, hWnd, msg, wParam, lParam);

            switch (msg)
            {
                case WM_WM_ACTIVATEAPP:
                    if(wParam.ToInt32() == 1 && hWnd == GameManager.Instance.Window.Handle)
                    {
                        if (WindowManager.ShouldDoActivations())
                        {
                            WindowManager.MaybeFocusMainWindow = true;
                        }
                    }
                    break;

                case WM_GETDLGCODE:
                    returnCode = (IntPtr)(returnCode.ToInt32() | DLGC_WANTALLKEYS);
                    break;

                case WM_KEYDOWN:
                    data.OnKeyDown(new KeyEventArgs((Keys)wParam));
                    break;

                case WM_KEYUP:
                    data.OnKeyUp(new KeyEventArgs((Keys)wParam));
                    break;

                case WM_CHAR:
                    data.OnCharEntered(new CharacterEventArgs((char)wParam, lParam.ToInt32()));
                    break;

                case WM_IME_SETCONTEXT:
                    if (wParam.ToInt32() == 1)
                        ImmAssociateContext(hWnd, data.hIMC);
                    break;

                case WM_INPUTLANGCHANGE:
                    ImmAssociateContext(hWnd, data.hIMC);
                    returnCode = (IntPtr)1;
                    break;
            }

            return returnCode;
        }
    }
}