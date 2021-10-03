﻿using CefSharp;
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
    public class CallbackObjectForJs
    {
        public string readFile(string path)
        {
            path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/" + path;

            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }

            return "";
        }
    }

    public class ChatForm : NoCloseForm
    {
        public ChatForm()
        {
            //Size = new System.Drawing.Size(0,0);
            Size = new System.Drawing.Size((int)(700 * GameManager.WidthScale), (int)(800 * GameManager.HeightScale));

            //Create a new instance in code or add via the designer
            //Set the ChromiumWebBrowser.Address property to your Url if you use the designer.
            var browser = new ChromiumWebBrowser("localfolder://cefsharp/");

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
    }
}
