using Castle.DynamicProxy;
using Core.Extensions;
using Core.Interceptors;
using Core.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.BusinessAspects.Autofac
{
    public class SecuredOperation:MethodInterception
    {
        private string[] _roles;
        private IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// rules add example: admin,user
        /// </summary>
        /// <param name="roles"></param>
        public SecuredOperation(string roles)
        {
            _roles=roles.Split(',');
            _httpContextAccessor=ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }


        protected override void OnBefore(IInvocation invocation)
        {
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                throw new SecurityTokenValidationException();
            }
            else
            {
                var roleClaims = _httpContextAccessor.HttpContext.User.ClaimRoles();
                foreach (var role in _roles)
                {
                    if (roleClaims.Contains(role))
                    {
                        return;
                    }
                } 
            }

            throw new UnauthorizedAccessException("You are not allowed for this operation");
        }
    }
}
