using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.Web;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using System.Windows.Forms;

namespace MonoGame_Core.Scripts
{
    public class CustomMenuHandler : CefSharp.IContextMenuHandler
    {
        public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
        {
            model.Clear();
        }

        public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
        {

            return false;
        }

        public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
        {

        }

        public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
        {
            return false;
        }
    }
    public class CallbackObjectForJs
    {
        public void ready()
        {
            GameManager.chatWindow.ready = true;
        }

        public string readFile(string path)
        {
            path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/" + path;

            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }

            return "";
        }

        public void playSound(string sound)
        {
            SoundManager.PlaySoundEffect(sound);
        }

        public void sendEvent(string ev)
        {
            GameManager.plotManager.SendEvent(ev);
        }
    }

    public class ChatForm : NoCloseForm
    {
        ChromiumWebBrowser browser;
        public bool ready = false;

        public ChatForm()
        {
            Text = "Relaque";

            //Size = new System.Drawing.Size(0,0);
            Size = new System.Drawing.Size((int)(700 * GameManager.WidthScale), (int)(800 * GameManager.HeightScale));

            //Create a new instance in code or add via the designer
            //Set the ChromiumWebBrowser.Address property to your Url if you use the designer.
            browser = new ChromiumWebBrowser("localfolder://cefsharp/");
            browser.MenuHandler = new CustomMenuHandler();

            browser.ConsoleMessage += (sender, args) =>
            {
                Debug.WriteLine(string.Format("Webview {0}({1}): {2}",
                                                args.Source,
                                                args.Line,
                                                args.Message));
            };

            browser.JavascriptObjectRepository.ResolveObject += (sender, e) =>
            {
                var repo = e.ObjectRepository;
                if (e.ObjectName == "game")
                {
                    repo.Register("game", new CallbackObjectForJs());
                }
            };

            this.Controls.Add(browser);

            this.Visible = false;
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //this.ShowInTaskbar = false;
            browser.FrameLoadEnd += (object owner, FrameLoadEndEventArgs args) =>
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        //Size = new System.Drawing.Size((int)(700 * GameManager.WidthScale), (int)(800 * GameManager.HeightScale));
                        //this.ShowInTaskbar = true;
                        //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;

                        this.Visible = true;
                        this.WindowState = FormWindowState.Normal;
                        this.ShowInTaskbar = true;
                    }));
                }
            };
        }

        public void runChat(string person, string chat, bool forceWatch)
        {
            browser.ExecuteScriptAsync("runCustomChat", new object[] { person, chat, forceWatch });
        }

        public void setGlobal(string var, object value)
        {
            browser.ExecuteScriptAsync("setGlobalValue", new object[] { var, value });
        }

        public void sendEvent(string ev)
        {
            browser.ExecuteScriptAsync("recieveGameEvent", new object[] { ev });
        }
    }
}
