using Autofac;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Business.Abstract;
using Business.Concrete;
using Core.Interceptors;
using DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Tools.Security.JWT;
using DataAccess.Concrete.Postgre.EntityFramework;
using AutoMapper.Contrib.Autofac.DependencyInjection;

namespace Business.DependencyResolvers
{
    public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<PostgreEfUserDal>().As<IUserDal>().SingleInstance();
            builder.RegisterType<UserManager>().As<IUserService>().SingleInstance();

            builder.RegisterType<AuthManager>().As<IAuthService>().SingleInstance();

            builder.RegisterAutoMapper(false,AppDomain.CurrentDomain.GetAssemblies());

            builder.RegisterType<JwtHelper>().As<ITokenHelper>().SingleInstance();

            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces().EnableInterfaceInterceptors(new ProxyGenerationOptions()
            {
                Selector = new AspectInterceptorSelector()
            }).SingleInstance();
        }
    }
}
