using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Utils.PageScriptManager;

namespace WpfClient.Utils
{
    /// <summary>
    /// Управляем скриптами подключаемым к страницам
    /// </summary>
    public interface IPageScriptManager
    {
        /// <summary>
        /// контейнер хранящий соотношение URL и скрипта
        /// </summary>
        Dictionary<string, ExtScript> Container { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourcePath"></param>
        void InitializeContainer(string sourcePath);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        Task<string> ResolveScriptAsync(Uri uri);
    }
}
