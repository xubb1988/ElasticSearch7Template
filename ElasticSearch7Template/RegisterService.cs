using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System.Reflection;
using System.Collections.Generic;
using ElasticSearch7Template.Core;
using System.Linq;
using ElasticSearch7Template.DAL;
using ElasticSearch7Template.BLL.MediatR;
using ElasticSearch7Template.Utility;
using Microsoft.AspNetCore.Http;
using ElasticSearch7Template.BLL;

namespace ElasticSearch7Template
{
    public static class RegisterService
    {
        public static void RegisterComponents(IServiceCollection services)
        {
        
            services.AddHttpClient();
            services.AddSingleton<RequestToHttpHelper>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            RegisterBLL.RegisterComponents(services);
            RegisterDAL.RegisterComponents(services);
            RegisterMediatR.RegisterComponents(services);

            //自动注入IAutoInject
            services.Scan(x =>
            {
                var entryAssembly = Assembly.GetEntryAssembly();
                var referencedAssemblies = entryAssembly.GetReferencedAssemblies().Select(Assembly.Load);
                var assemblies = new List<Assembly> { entryAssembly }.Concat(referencedAssemblies);

                x.FromAssemblies(assemblies)
                    .AddClasses(classes => classes.AssignableTo(typeof(IAutoInject)))
                        .AsImplementedInterfaces()
                        .WithScopedLifetime()
                    //接口注册Scoped
                    .AddClasses(classes => classes.AssignableTo(typeof(IScopedAutoInject)))
                        .AsImplementedInterfaces()
                        .WithScopedLifetime()
                    //接口注册Singleton
                    .AddClasses(classes => classes.AssignableTo(typeof(ISingletonAutoInject)))
                          .AsImplementedInterfaces()
                          .WithSingletonLifetime()
                    //接口注册Transient
                    .AddClasses(classes => classes.AssignableTo(typeof(ITransientAutoInject)))
                          .AsImplementedInterfaces()
                          .WithTransientLifetime()
                    //具体类注册Scoped
                    .AddClasses(classes => classes.AssignableTo(typeof(ISelfScopedAutoInject)))
                          .AsSelf()
                          .WithScopedLifetime()
                    //具体类注册Singleton
                    .AddClasses(classes => classes.AssignableTo(typeof(ISelfSingletonAutoInject)))
                          .AsSelf()
                          .WithSingletonLifetime()
                    //具体类注册Transient
                    .AddClasses(classes => classes.AssignableTo(typeof(ISelfTransientAutoInject)))
                          .AsSelf()
                          .WithTransientLifetime();
            });
        }
    }
}