using CefSharp;
using CefSharp.SchemeHandler;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Shell.CefInfrastructure.Handlers
{
    public class BrowserProcessHandler : IBrowserProcessHandler
    {
        /// <summary>
        /// The maximum number of milliseconds we're willing to wait between calls to OnScheduleMessagePumpWork().
        /// </summary>
        protected const int MaxTimerDelay = 1000 / 30;  // 30fps

        void IBrowserProcessHandler.OnContextInitialized()
        {
            //The Global CookieManager has been initialized, you can now set cookies
            var cookieManager = Cef.GetGlobalCookieManager();
            //cookieManager.SetStoragePath("cookies", true);
            cookieManager.DeleteCookies("http://public.servicebox.peugeot.com/");

          var sc=  cookieManager.SetCookie("http://public.servicebox.peugeot.com/", new Cookie
            {
                Name= "CodeLanguePaysOI",
                Value= "ru_RU",
                Domain= "",
                Path="/",
                Expires=DateTime.Now.AddYears(1)
            });

            cookieManager.VisitAllCookiesAsync().ContinueWith(t =>
            {
                if (t.Status == TaskStatus.RanToCompletion)
                {
                    var cookies = t.Result;

                    foreach (var cookie in cookies)
                    {
                        Debug.WriteLine("CookieName:" + cookie.Name);
                    }
                }
                else
                {
                    Debug.WriteLine("No Cookies found");
                }
            });
            //}

            //The Request Context has been initialized, you can now set preferences, like proxy server settings
            //Dispose of context when finished - preferable not to keep a reference if possible.
            using (var context = Cef.GetGlobalRequestContext())
            {
                string errorMessage;
                //You can set most preferences using a `.` notation rather than having to create a complex set of dictionaries.
                //The default is true, you can change to false to disable
                context.SetPreference("webkit.webprefs.plugins_enabled", true, out errorMessage);

                //It's possible to register a scheme handler for the default http and https schemes
                //In this example we register the FolderSchemeHandlerFactory for https://cefsharp.example
                //Best to include the domain name, so only requests for that domain are forwarded to your scheme handler
                //It is possible to intercept all requests for a scheme, including the built in http/https ones, be very careful doing this!
                //var folderSchemeHandlerExample = new FolderSchemeHandlerFactory(rootFolder: @"..\..\..\..\CefSharp.Example\Resources",
                //                                                        hostName: "cefsharp.example", //Optional param no hostname checking if null
                //                                                        defaultPage: "home.html"); //Optional param will default to index.html

                //context.RegisterSchemeHandlerFactory("https", "cefsharp.example", folderSchemeHandlerExample);
            }
        }

        void IBrowserProcessHandler.OnScheduleMessagePumpWork(long delay)
        {
            //If the delay is greater than the Maximum then use MaxTimerDelay
            //instead - we do this to achieve a minimum number of FPS
            if (delay > MaxTimerDelay)
            {
                delay = MaxTimerDelay;
            }
            OnScheduleMessagePumpWork((int)delay);
        }

        protected virtual void OnScheduleMessagePumpWork(int delay)
        {
            //TODO: Schedule work on the UI thread - call Cef.DoMessageLoopWork
        }

        public virtual void Dispose()
        {
        }
    }
}
