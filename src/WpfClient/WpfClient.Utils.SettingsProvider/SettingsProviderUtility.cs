using Jot;
using Jot.DefaultInitializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfClient.Utils.SettingsProviderUtility
{
    public class SettingsProviderUtility : ISettingsProviderUtility
    {
        private readonly ICredentialsManager _credentialSource;
        private readonly ILogger _logger;

        public SettingsProviderUtility(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public string CredentialsSource { get; set; }
        public int LastCredentialsEntryId { get; set; }
        public string PreviousCredentialSourceHash { get; set; }
    }
}
