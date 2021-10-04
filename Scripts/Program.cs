using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.WinForms;
using System;
using System.IO;
using System.Reflection;

namespace MonoGame_Core.Scripts
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            var settings = new CefSettings();
            settings.CachePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/cef_cache";
            settings.LogSeverity = LogSeverity.Disable;

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = "localfolder",
                DomainName = "cefsharp",
                SchemeHandlerFactory = new FolderSchemeHandlerFactory(
                    rootFolder: Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/Content/Web/Chat",
                    hostName: "cefsharp",
                    defaultPage: "index.html" // will default to index.html
                )
            });
            Cef.Initialize(settings);

            using (var game = new GameManager())
                game.Run();
        }
    }
}
