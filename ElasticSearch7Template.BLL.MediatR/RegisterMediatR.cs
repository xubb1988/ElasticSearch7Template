using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.BLL.MediatR
{
    public static class RegisterMediatR
    {
        public static void RegisterComponents(IServiceCollection services)
        {
            //生成代码开始位置勿删
            services.AddScoped<MediatRProxyService>();
        }
    }
}
