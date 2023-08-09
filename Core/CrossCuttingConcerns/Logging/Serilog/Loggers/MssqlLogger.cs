using Core.IoC;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Core.CrossCuttingConcerns.Logging.Models;
using Serilog;
using System.Net.NetworkInformation;

namespace Core.CrossCuttingConcerns.Logging.Serilog.Loggers
{
    public class MssqlLogger : LoggerServiceBase
    {
        public MssqlLogger()
        {
            var configuration = ServiceTool.ServiceProvider.GetService<IConfiguration>();
            var logConfiguration = configuration.GetSection("SeriLogConfigurations:PostgreConfiguration").Get<PostgreConfiguration>()
                ?? throw new Exception("Serilog configuration error");

            //var sinkOptions=new MSSqlServerSinkOptions { TableName ="Logs", AutoCreateSqlTable = true };
            //var sinkOptions-new Pos
            var seriLogConfiguration = new LoggerConfiguration().WriteTo.PostgreSQL(connectionString: logConfiguration.ConnectionString, "Logs", needAutoCreateTable: true).CreateLogger();

            Logger=seriLogConfiguration;
        }
    }
}
