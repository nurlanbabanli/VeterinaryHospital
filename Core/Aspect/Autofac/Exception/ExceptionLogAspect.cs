using Core.CrossCuttingConcerns.Logging.Serilog;
using Core.Interceptors;
using Core.IoC;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Core.CrossCuttingConcerns.Logging.Models;
using Castle.DynamicProxy;
using Newtonsoft.Json;

namespace Core.Aspect.Autofac.Exception
{
    public class ExceptionLogAspect:MethodInterception
    {
        private readonly LoggerServiceBase _loggerServiceBase;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly bool _skipValueLog = false;

        public ExceptionLogAspect(Type loggerService, bool skipValueLog=false)
        {
            if (loggerService.BaseType!=typeof(LoggerServiceBase))
            {
                throw new ArgumentException("ExceptionAspect Error");
            }

            _loggerServiceBase = Activator.CreateInstance(loggerService) as LoggerServiceBase;
            _httpContextAccessor=ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
            _skipValueLog= skipValueLog;
        }

        protected override void OnException(IInvocation invocation, System.Exception exception)
        {
            var logDetailWithException = GetLogDetail(invocation);
            if (exception is AggregateException)
                logDetailWithException.ExceptionMessage=
                    String.Join(Environment.NewLine, (exception as AggregateException).InnerExceptions.Select(x => x.Message));
            else
                logDetailWithException.ExceptionMessage=exception.Message;

            _loggerServiceBase.Error(JsonConvert.SerializeObject(logDetailWithException, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore }));
        }

        private LogDetailWithException GetLogDetail(IInvocation invocation)
        {
            var logParameters=new List<LogParameter>();
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

            var logDetailWithException = new LogDetailWithException()
            {
                TraceIdentifier=_httpContextAccessor.HttpContext.TraceIdentifier,
                MethodName=invocation.Method.Name,
                LogParameters=logParameters,
                User=(_httpContextAccessor.HttpContext==null || _httpContextAccessor.HttpContext.User.Identity.Name==null) ? "Error on to get user data from http context" :
                _httpContextAccessor.HttpContext.User.Identity.Name
            };

            return logDetailWithException;
        }
    }
}
