using Core.CrossCuttingConcerns.Logging.Serilog;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tools.LocalLogger
{
    public static class LocalLogHandler
    {
        //static LoggerServiceBase _loggerService;
        public static void Log(string message, LogLevel logLevel)
        {
            //if (_loggerService == null)
            //{
               var _loggerService=new MssqlLogger();
            _loggerService.Error(message);
            //}

            //switch (logLevel)
            //{
            //    case LogLevel.Error: LogError(message); break;
            //    case LogLevel.Warning:LogWarning(message); break;
            //    case LogLevel.Info: LogInfo(message); break;
            //}
        }

        //private static void LogError(string message)
        //{
        //    _loggerService.Error(message);
        //}

        //private static void LogWarning(string message)
        //{
        //    _loggerService.Warn(message);
        //}

        //private static void LogInfo(string message)
        //{
        //    _loggerService.Info(message);
        //}

    }

    public enum LogLevel
    {
        Error=1,
        Warning=2,
        Info=3,
    }
}
