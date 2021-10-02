using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MonoGame_Core.Scripts
{
    public class ChatForm : NoCloseForm
    {
        public ChatForm()
        {
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
            this.Controls.Add(browser);
        }
    }
}
