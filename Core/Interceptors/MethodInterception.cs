using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interceptors
{
    public abstract class MethodInterception:MethodInterceptionBaseAttribute
    {
        protected virtual void OnBefore(IInvocation invocation) { }
        protected virtual void OnAfter(IInvocation invocation) { }
        protected virtual void OnException(IInvocation invocation, Exception exception) { }
        protected virtual void OnSuccess(IInvocation invocation) { }

        public override async void Intercept(IInvocation invocation)
        {
            bool isSuccess = true;
            OnBefore(invocation);
            try
            {
                invocation.Proceed();
                var invocationResult = invocation.ReturnValue;
                if(invocationResult is Task)
                {
                    var invocationResultTask= invocationResult as Task;
                    await invocationResultTask;
                }
            }
            catch (Exception exception)
            {
                isSuccess = false;
                OnException(invocation, exception);
                throw;
            }
            finally
            {
                if(isSuccess)
                {
                    OnSuccess(invocation);
                }
            }
            OnAfter(invocation);
        }
    }
}
