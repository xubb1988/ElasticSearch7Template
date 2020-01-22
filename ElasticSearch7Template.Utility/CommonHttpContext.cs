using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.Utility
{
    public static class CommonHttpContext
    {
        private static IHttpContextAccessor accessor;

       

        public static HttpContext Current => accessor.HttpContext;

        internal static void Configure(IHttpContextAccessor accessor)
        {
            CommonHttpContext.accessor = accessor;
        }
    }

    public static class StaticHttpContextExtensions
    {


        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            CommonHttpContext.Configure(httpContextAccessor);
            return app;
        }
    }


    public static class CommonServiceProvider
    {
        public static IServiceProvider ServiceProvider
        {
            get; set;
        }
    }
}
