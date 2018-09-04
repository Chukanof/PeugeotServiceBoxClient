
using CefSharp;
using CefSharp.Wpf;
using DryIoc;
using Jot.Storage;
using Prism.DryIoc;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfClient.Infrastructure.DataAccess;
using WpfClient.Shell.CefInfrastructure;
using WpfClient.Shell.CefInfrastructure.Handlers;
using WpfClient.Shell.ViewModels;
using WpfClient.Utils;
using WpfClient.Utils.CredentialsManager;
using WpfClient.Utils.Logger;
using WpfClient.Utils.PageScriptManager;
using WpfClient.Utils.SettingsProviderUtility;

namespace WpfClient.Shell
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    internal partial class App : PrismApplication
    {
        private readonly ILogger _logger;

        public App()
        {
            Application.Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
            Application.Current.Exit += Current_Exit;
            _logger = new SerilogToFileImpl();
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            Cef.Shutdown();
        }

        protected override Window CreateShell()
        {
            var shellWindow = Container.Resolve<ShellWindow>();
            return shellWindow;
        }

        protected override void InitializeShell(Window shell)
        {
            base.InitializeShell(shell);

            var shellWindow = shell;

            Application.Current.MainWindow = shellWindow;
            Application.Current.MainWindow.Show();
            shellWindow.WindowState = WindowState.Maximized;
            shellWindow.Activate();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var container = containerRegistry.GetContainer();

            var stateTracker = new Jot.StateTracker() { StoreFactory = new JsonFileStoreFactory($@"{Environment.CurrentDirectory}\AppSettings") };

            containerRegistry.Register<ShellWindow>();
            containerRegistry.Register<ShellViewModel>();
            containerRegistry.Register<BrowserTabViewModel>();

            containerRegistry.Register<ICredentialsManager, GitGistCredentialsManager>();
            containerRegistry.RegisterSingleton<ISettingsProviderUtility, SettingsProviderUtility>();
            containerRegistry.RegisterSingleton<ILogger, SerilogToFileImpl>();
            containerRegistry.Register<IPageScriptManager, PageScriptManagerImpl>();

            container.Register<SettingsDbContext>(setup: Setup.With(allowDisposableTransient: true));
            container.Register(typeof(IUnitOfWork<>), typeof(EfUnitOfWorkGeneric<>), setup: Setup.With(allowDisposableTransient: true));
            containerRegistry.Register(typeof(IRepository<>), typeof(EfRepository<>));
            


            container.RegisterInitializer<ISettingsProviderUtility>((service, resolver) =>
            {
                stateTracker.Configure(service)
                    .AddProperty<ISettingsProviderUtility>(p => p.CredentialsSource, "https://gist.githubusercontent.com/Chukanof/29ec504da91162289b726a372666a564/raw/")
                    .AddProperties<ISettingsProviderUtility>(p => p.LastCredentialsEntryId, p => p.PreviousCredentialSourceHash)
                    .Apply();
            });
        }

        private void Current_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            _logger.Warn(e.Exception.Message, e.Exception);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            const bool multiThreadedMessageLoop = true;

            IBrowserProcessHandler browserProcessHandler = new BrowserProcessHandler();


            var settings = new CefSettings();
            settings.MultiThreadedMessageLoop = multiThreadedMessageLoop;
            settings.ExternalMessagePump = !multiThreadedMessageLoop;

            CefHelper.Init(settings, browserProcessHandler: browserProcessHandler);

            base.OnStartup(e);
        }

        
    }

}
