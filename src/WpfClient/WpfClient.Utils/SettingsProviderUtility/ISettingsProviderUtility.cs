using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Utils
{
    /// <summary>
    /// Настройки приложения
    /// </summary>
    public interface ISettingsProviderUtility
    {
        /// <summary>
        /// Url источника учетных записей
        /// </summary>
        string CredentialsSource { get; set; }

        /// <summary>
        /// MD5 хэш предыдущего состояния удаленного источника.
        /// </summary>
        string PreviousCredentialSourceHash { get; set; }

        /// <summary>
        /// Id последнего подключенного пользователя
        /// </summary>
        int LastCredentialsEntryId { get; set; }

    }
}
