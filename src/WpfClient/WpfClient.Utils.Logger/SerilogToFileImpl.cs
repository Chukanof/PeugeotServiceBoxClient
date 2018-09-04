using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Utils;
using Serilog;
using ISerilogLogger = Serilog.ILogger;

namespace WpfClient.Utils.Logger
{
    public class SerilogToFileImpl:ILogger
    {
        private readonly ISerilogLogger _logger;
        public SerilogToFileImpl()
        {
           _logger = new LoggerConfiguration()
             .WriteTo.RollingFile($@"logs\\AppLogs.txt" )
             .CreateLogger();
        }
        public void Trace(string message, Exception ex = null)
        {
            _logger.Verbose(ex, message);
        }

        public void Debug(string message, Exception ex = null)
        {
        }

        public void Info(string message, Exception ex = null)
        {
            _logger.Information(ex, message);
        }

        public void Warn(string message, Exception ex = null)
        {
            _logger.Warning(ex, message);
        }

        public void Error(string message, Exception ex = null)
        {
            _logger.Error(ex, message);
        }

        public void Fatal(string message, Exception ex = null)
        {
            _logger.Fatal(ex, message);
        }
    }
}
