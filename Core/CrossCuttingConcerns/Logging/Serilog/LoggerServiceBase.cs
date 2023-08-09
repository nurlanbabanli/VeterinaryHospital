using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Logging.Serilog
{
    public class LoggerServiceBase
    {
        public ILogger Logger;

        public void Info(string message)=>Logger.Information(message);
        public void Warn(string message)=>Logger.Warning(message);
        public void Error(string message)=>Logger.Error(message);
    }
}
