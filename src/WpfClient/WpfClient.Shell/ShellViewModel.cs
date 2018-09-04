using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Infrastructure.mvvm;
using WpfClient.Shell.ViewModels;
using WpfClient.Utils;

namespace WpfClient.Shell
{
    internal class ShellViewModel : ViewModelBase
    {
        private readonly ISettingsProviderUtility _settingsProvider;
        private readonly ICredentialsManager _credentialsMngr;
        private readonly ILogger _logger;

        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
        private bool isLoading;

        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }

        private bool isReloading;

        public bool IsReloading
        {
            get { return isReloading; }
            set { SetProperty(ref isReloading, value); }
        }

        public ObservableCollection<BrowserTabViewModel> BrowserTabs { get; set; }

        public ShellViewModel(
            ISettingsProviderUtility settingsProvider,
            ICredentialsManager credentialsMngr,
            ILogger logger)
        {
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
            _credentialsMngr = credentialsMngr ?? throw new ArgumentNullException(nameof(credentialsMngr));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));


            BrowserTabs = new ObservableCollection<BrowserTabViewModel>();


            Title = "Peugeot Dealer Online";
            IsLoading = true;
        }

        internal Task UpdateCredentials()
        {
            return _credentialsMngr.SyncSourceAndLocalStoragesAsync();
        }
    }
}
