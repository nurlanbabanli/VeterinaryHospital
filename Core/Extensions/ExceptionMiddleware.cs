using Core.CrossCuttingConcerns.Logging.Serilog;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.IoC;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Net;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace Core.Extensions
{
    public class ExceptionMiddleware
    {
        private RequestDelegate _requestDelegate;
        private readonly LoggerServiceBase _loggerServiceBase;

        public ExceptionMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate=requestDelegate;
            _loggerServiceBase=(LoggerServiceBase)ServiceTool.ServiceProvider.GetService(typeof(MssqlLogger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            _loggerServiceBase.Error("ErrorIdentifier="+httpContext.TraceIdentifier+";"+exception.ToString());

            httpContext.Response.ContentType="application/json";
            httpContext.Response.StatusCode=(int)HttpStatusCode.InternalServerError;

            string message = "Internal Server Error";

            IEnumerable<FluentValidation.Results.ValidationFailure> errors;

            if (exception.GetType()==typeof(FluentValidation.ValidationException))
            {
                message=exception.Message;
                errors=((FluentValidation.ValidationException)exception).Errors;
                httpContext.Response.StatusCode=400;

                return httpContext.Response.WriteAsync(new ValidationErrorDetails()
                {
                    StatusCode=400,
                    Message=message+"  ErrorIdentifier="+httpContext.TraceIdentifier,
                    Errors=errors
                }.ToString());
            }
            else if (exception.GetType()==typeof(UnauthorizedAccessException))
            {
                message = exception.Message;
                httpContext.Response.StatusCode=400;
                return httpContext.Response.WriteAsync(new ErrorDetails
                {
                    StatusCode=httpContext.Response.StatusCode,
                    Message=message+"  ErrorIdentifier="+httpContext.TraceIdentifier
                }.ToString());
            }
            else if (exception.GetType()==typeof(SecurityTokenValidationException))
            {
                message=exception.Message;
                httpContext.Response.StatusCode=StatusCodes.Status401Unauthorized;
                return httpContext.Response.WriteAsync(new ErrorDetails
                {
                    StatusCode=httpContext.Response.StatusCode,
                    Message="TokenValidationException" + "   ErrorIdentifier=" + httpContext.TraceIdentifier
                }.ToString());
            }

            return httpContext.Response.WriteAsync(new ErrorDetails
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = message + "   ErrorIdentifier=" + httpContext.TraceIdentifier
            }.ToString());
        }
    }
}
