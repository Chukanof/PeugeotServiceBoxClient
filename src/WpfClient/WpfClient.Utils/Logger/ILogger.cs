using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Utils
{
    public interface ILogger
    {
        void Trace(string message, Exception ex = null);

        void Debug(string message, Exception ex = null);

        void Info(string message, Exception ex = null);

        void Warn(string message, Exception ex = null);

        void Error(string message, Exception ex = null);

        void Fatal(string message, Exception ex = null);
    }
}
