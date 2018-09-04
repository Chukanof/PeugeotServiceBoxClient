using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfClient.Shell.CefInfrastructure.Handlers;
using WpfClient.Shell.ViewModels;
using WpfClient.Utils;

namespace WpfClient.Shell
{
    /// <summary>
    /// Interaction logic for ShellWindow.xaml
    /// </summary>
    internal partial class ShellWindow
    {
        private readonly ShellViewModel _dataContext;
        private readonly IContainer _container;
        private readonly ICredentialsManager _credentialsManager;
        private readonly ILogger _logger;

        public ShellWindow(
            ShellViewModel vm,
            IContainer container,
            ICredentialsManager credentialsManager,
            ILogger logger
            )
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            _credentialsManager = credentialsManager ?? throw new ArgumentNullException(nameof(credentialsManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));


            InitializeComponent();

            this.DataContext = vm;
            this._dataContext = vm;

            this.Loaded += ShellWindow_Loaded;
        }



        private async void ShellWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _dataContext.UpdateCredentials();

            AddTab();
        }

        private void AddTab()
        {
            var newTab = _container.Resolve<BrowserTabViewModel>();
            newTab.Address = "http://public.servicebox.peugeot.com/pages/index.jsp";
            newTab.SuccefullLoaded = () =>
            {
                _dataContext.IsLoading = false;
                newTab.WebBrowser.LoadingStateChanged += WebBrowser_LoadingStateChanged;
                
            };

            _dataContext.BrowserTabs.Add(newTab);

        }

        private async void WebBrowser_LoadingStateChanged(object sender, CefSharp.LoadingStateChangedEventArgs e)
        {
            await Task.Delay(100);
            _dataContext.IsReloading = e.IsLoading;
        }
    }
}
