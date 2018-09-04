using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Infrastructure.Threading;
using WpfClient.Utils.CredentialsManager;

namespace WpfClient.Utils
{
    /// <summary>
    /// Управление учетными записями
    /// </summary>
    public interface ICredentialsManager
    {
        /// <summary>
        /// Проверить наличие изменений с момента последнего обновления
        /// </summary>
        Task<bool> CheckIsUpToDateCredentialsAsync();

        /// <summary>
        /// Синхронизировать учетные записи из удаленного источника с локальным хранилищем
        /// </summary>
        /// <returns></returns>
        Task<bool> SyncSourceAndLocalStoragesAsync();

        /// <summary>
        /// Получить учетные данные пользователя для подключения в этот раз
        /// </summary>
        /// <returns></returns>
        AsyncLazy<CredentialEntry> CurrentCredentials { get; }
    }
}
