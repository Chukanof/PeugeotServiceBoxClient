using CefSharp;
using CefSharp.Wpf;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WpfClient.Infrastructure.mvvm;
using WpfClient.Utils;

namespace WpfClient.Shell.ViewModels
{
    internal class BrowserTabViewModel : ViewModelBase
    {
        public Action SuccefullLoaded;

        private readonly ILogger _logger;
        private readonly IPageScriptManager _pageScriptManager;
        private string address;
        private IWpfWebBrowser webBrowser;

        public BrowserTabViewModel(IPageScriptManager pageScriptManager, ILogger logger)
        {
            _pageScriptManager = pageScriptManager ?? throw new ArgumentNullException(nameof(pageScriptManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));


            PropertyChanged += OnPropertyChanged;

        }

        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }
        public IWpfWebBrowser WebBrowser
        {
            get { return webBrowser; }
            set { SetProperty(ref webBrowser, value); }
        }
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "WebBrowser":
                    if (WebBrowser != null)
                    {
                        WebBrowser.LoadError += OnWebBrowserLoadError;
                        WebBrowser.LoadingStateChanged += WebBrowser_LoadingStateChanged;

                        WebBrowser.FrameLoadEnd += (s, args) =>
                        {
                            var browser = s as ChromiumWebBrowser;
                            if (browser != null && !browser.IsDisposed)
                            {
                                browser.Dispatcher.BeginInvoke((Action)(() => browser.Focus()));
                            }
                        };
                    }

                    break;
            }
        }

        private void OnWebBrowserLoadError(object sender, LoadErrorEventArgs args)
        {
            // Don't display an error for downloaded files where the user aborted the download.
            if (args.ErrorCode == CefErrorCode.Aborted)
                return;

            var errorMessage = "<html><body><h2>Failed to load URL " + args.FailedUrl +
                  " with error " + args.ErrorText + " (" + args.ErrorCode +
                  ").</h2></body></html>";

            webBrowser.LoadHtml(errorMessage, args.FailedUrl);
        }

        private async void WebBrowser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (e.IsLoading) return;

            var uri = new Uri(Address);


            var script = await _pageScriptManager.ResolveScriptAsync(uri);

            if (string.IsNullOrEmpty(script))
                return;

            await WebBrowser.EvaluateScriptAsync(script).ContinueWith(t =>
            {
                if (uri.AbsolutePath == "/socle/")
                {
                    SuccefullLoaded();
                }
            });
        }
    }
}
