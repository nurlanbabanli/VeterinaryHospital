using Core.CrossCuttingConcerns.Logging.Serilog;
using Core.IoC;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Core.Interceptors;
using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Logging.Models;
using Newtonsoft.Json;

namespace Core.Aspect.Autofac.Logging
{
    public class LogAspects:MethodInterception
    {
        private readonly LoggerServiceBase _loggerServiceBase;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly bool _skipValueLog = false;

        public LogAspects(Type loggerService, bool skipValueLog=false)
        {
            _skipValueLog = skipValueLog;
            if (loggerService.BaseType!=typeof(LoggerServiceBase))
            {
                throw new ArgumentException("Log Aspect Error");
            }
            _loggerServiceBase = (LoggerServiceBase?)ServiceTool.ServiceProvider.GetService(loggerService);
            _httpContextAccessor=ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var result = GetLogDetail(invocation);
            _loggerServiceBase.Info(result);
        }

        private string GetLogDetail(IInvocation invocation)
        {
            var logParameters = new List<LogParameter>();
            for (int i = 0; i < invocation.Arguments.Length; i++)
            {
                logParameters.Add(new LogParameter
                {
                    Name=invocation.GetConcreteMethod().GetParameters()[i].Name,
                    Value=invocation.Arguments[i],
                    Type=invocation.Arguments[i].GetType().Name
                });
            }

            if (_skipValueLog)
            {
                foreach (var logParameter in logParameters)
                {
                    logParameter.Value=null;
                }
            }

            var logDetail = new LogDetail
            {
                MethodName=invocation.Method.Name,
                LogParameters=logParameters,
                FullName=invocation.Method.DeclaringType.FullName,
                User=(_httpContextAccessor.HttpContext==null || _httpContextAccessor.HttpContext.User.Identity.Name==null) ? "Error on to get user data from http context" :
                _httpContextAccessor.HttpContext.User.Identity.Name,
            };

            return JsonConvert.SerializeObject(logDetail,new JsonSerializerSettings() { ReferenceLoopHandling=ReferenceLoopHandling.Ignore});
        }
    }
}
