using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Infrastructure.DataAccess;
using WpfClient.Infrastructure.Helpers;
using WpfClient.Infrastructure.Threading;
using WpfClient.Utils.SettingsProviderUtility;

namespace WpfClient.Utils.CredentialsManager
{
    public class GitGistCredentialsManager : ICredentialsManager
    {
        private readonly ILogger _logger;
        private readonly ISettingsProviderUtility _settingsProvider;
        private readonly IUnitOfWork<SettingsDbContext> _uow;

        public AsyncLazy<CredentialEntry> CurrentCredentials { get; private set; }
        public GitGistCredentialsManager(ILogger logger, ISettingsProviderUtility settingsProvider,
           IUnitOfWork<SettingsDbContext> uow)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingsProvider = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));

            CurrentCredentials = new AsyncLazy<CredentialEntry>(() => GetCurrentCredentialsAsync());
        }

        public async Task<bool> CheckIsUpToDateCredentialsAsync()
        {
            var result = false;

            if (string.IsNullOrEmpty(_settingsProvider.CredentialsSource))
                return result;

            try
            {

                var rawStr = await GetCredentialsRawAsync(_settingsProvider.CredentialsSource);

                if (string.IsNullOrEmpty(rawStr))
                {
                    _logger.Warn("проверьте корректность предоставленной по ссылке информации");
                    return result;
                }

                var isNoChanges = HasNoChanges(rawStr);

                if (isNoChanges)
                    result = true;

            }
            catch (Exception ex)
            {
                _logger.Error("Ошибка во время проверки актуальности учетных записей", ex);
                return result;
            }

            return result;
        }

        private async Task<CredentialEntry> GetCurrentCredentialsAsync()
        {
            CredentialEntry result = null;

            var credentialsRepo = _uow.GetRepository<CredentialEntry>();
            var lastId = _settingsProvider.LastCredentialsEntryId;

            result = await credentialsRepo
                    .Query()
                    .FirstOrDefaultAsync(x => x.Id > lastId);

            if (result == null)
            {
                result = await credentialsRepo
                    .Query()
                    .FirstOrDefaultAsync();
            }

            if (result != null)
            {
                _settingsProvider.LastCredentialsEntryId = result.Id;
            }

            return result;
        }

        public async Task<bool> SyncSourceAndLocalStoragesAsync()
        {
            var result = false;
            try
            {
                var rawStr = await GetCredentialsRawAsync(_settingsProvider.CredentialsSource);

                List<CredentialEntry> entries = await ParseEntriesAsync(rawStr);

                if (entries == null)
                {
                    _logger.Warn("Проверьте корректность источника учетных записей");
                    return false;
                }
                var credentialsRepository = _uow.GetRepository<CredentialEntry>();

                var sourceCredsLogins = entries.Select(e => e.Login).ToList();

                var containedEntries = await credentialsRepository
                     .Query()
                     .Where(x => sourceCredsLogins.Contains(x.Login))
                     .ToListAsync();

                foreach (var entry in containedEntries)
                {
                    var sourceEntry = entries.First(x => x.Login == entry.Login);

                    if (entry.Password != sourceEntry.Password)
                        entry.Password = sourceEntry.Password;
                }

                var notContained = entries
                    .Where(x => !containedEntries.Any(e => e.Login == x.Login));

                foreach (var entry in notContained)
                {
                    credentialsRepository.Add(entry);
                }

                await _uow.SaveChangesAsync();
                _settingsProvider.PreviousCredentialSourceHash = HashHelper.CalculateMD5Hash(rawStr);
                result = true;
            }
            catch (Exception ex)
            {
                _logger.Warn("Ошибка синхронизации источника учетных записей и локального хранилища", ex);
            }

            return result;
        }

        private async Task<string> GetCredentialsRawAsync(string uri)
        {
            string result = null;

            var req = HttpWebRequest.Create(uri);
            var resp = await req.GetResponseAsync();

            using (var sr = new StreamReader(resp.GetResponseStream()))
            {
                var allCredentials = await sr.ReadToEndAsync();

                result = allCredentials;
            }

            return result;
        }

        private bool HasNoChanges(string rawStr)
        {
            var hash = HashHelper.CalculateMD5Hash(rawStr);

            return hash == _settingsProvider.PreviousCredentialSourceHash;
        }

        private Task<List<CredentialEntry>> ParseEntriesAsync(string rawStr)
        {
            return Task.Run<List<CredentialEntry>>(() =>
           {
               List<CredentialEntry> result = null;

               var lines = rawStr.Split(new[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

               foreach (var item in lines)
               {
                   if (string.IsNullOrWhiteSpace(item))
                       continue;

                   var splittedCred = item.Split(';');


                   if (splittedCred.Length != 2)
                       continue;

                   var login = splittedCred[0];
                   var password = splittedCred[1];

                   if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                       continue;

                   if (result == null)
                       result = new List<CredentialEntry>();


                   var newCred = new CredentialEntry { Login = login, Password = password };
                   result.Add(newCred);
               }

               return result;
           });
        }
    }
}
