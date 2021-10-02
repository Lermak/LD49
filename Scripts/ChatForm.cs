using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.Web;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MonoGame_Core.Scripts
{
    public class CallbackObjectForJs
    {
        public string readFile(string path)
        {
            if(File.Exists(path))
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
            Size = new System.Drawing.Size(600, 800);

            var settings = new CefSettings();

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = "localfolder",
                DomainName = "cefsharp",
                SchemeHandlerFactory = new FolderSchemeHandlerFactory(
                    rootFolder: Directory.GetCurrentDirectory() + "/Content/Web/Chat",
                    hostName: "cefsharp",
                    defaultPage: "index.html" // will default to index.html
                )
            });
            Cef.Initialize(settings);

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
        }
    }
}
