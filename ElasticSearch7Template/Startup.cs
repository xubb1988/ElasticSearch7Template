using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;
using MediatR;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Text.Json;
using ElasticSearch7Template.Core;
using ElasticSearch7Template.Filters;
using ElasticSearch7Template.Utility;
using ElasticSearch7Template.Utility.LoggerHelper;
using Microsoft.Extensions.Logging;

namespace ElasticSearch7Template
{
    /// <summary>
    /// 启动类
    /// </summary>
    public class Startup
    {

        private IServiceCollection services;
        private IDisposable callbackRegistration;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="env"></param>
        public Startup(IWebHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                  .SetBasePath(env.ContentRootPath)
                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                  .Build();
            callbackRegistration = Configuration.GetReloadToken().RegisterChangeCallback(ChangeCallBack, Configuration);
            MyLogger.AddMyLogger();
        }



        /// <summary>
        /// 更新下次appsettings重载
        /// </summary>
        /// <param name="obj"></param>
        private void ChangeCallBack(object obj)
        {
            callbackRegistration?.Dispose();
            GetJsonConfig();
            //重新注册callback，下次appsettings.josn更新后会自动调用
            callbackRegistration = Configuration.GetReloadToken().RegisterChangeCallback(ChangeCallBack, obj);
        }

        /// <summary>
        /// 获取配置文件
        /// </summary>
        private void GetJsonConfig()
        {

            AppsettingsConfig config = new AppsettingsConfig();
            AppsettingsConfig.ServiceApiHosts = new ServiceApiHosts();
            Configuration.Bind("WebConfig", config);

            ElasticSearchConfig elasticSearchConfig = new ElasticSearchConfig();
            ElasticSearchConfig.ClusterNodeUrlHosts = new string[0];
            Configuration.Bind("ElasticSearchConfig", elasticSearchConfig);

            var aipHostSection = Configuration.GetSection("ServiceApiHosts");
            var dic = new Dictionary<string, string>();
            if (aipHostSection != null)
            {
                foreach (var api in aipHostSection.GetChildren())
                {
                    dic.Add(api["Code"], api["Host"]);
                }
            }
            AppsettingsConfig.ServiceApiHosts = new ServiceApiHosts();
            AppsettingsConfig.ServiceApiHosts.ApiHostOptions = new Dictionary<string, string>();
            AppsettingsConfig.ServiceApiHosts.ApiHostOptions = dic;

        }

        public IConfiguration Configuration { get; }


        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {

            this.services = services;
            GetJsonConfig();
            services.AddMediatR(Assembly.GetEntryAssembly());
            //禁用默认ModelState行为
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            //全局配置Json序列化处理
            services.AddControllers(ops =>
            {
                ops.Filters.Add(new WebApiExceptionFilterAttribute());
                ops.Filters.Add(new WebApiTrackerAttribute());
              //  ops.Filters.Add(new IPFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddNewtonsoftJson(options =>
            {
                //设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
                options.SerializerSettings.ContractResolver = new CustomContractResolver();
            });
            services.AddSwaggerGen(options =>
            {
                //options.SwaggerDoc("v1", new OpenApiInfo { Title = "NetCore3WebApiTemplate API", Version = "v1" });
                options.SwaggerDoc("Basic", new OpenApiInfo { Title = "基础模块API", Version = "Basic" });
                options.SwaggerDoc("Index", new OpenApiInfo { Title = "索引文件管理API", Version = "Index" });
                options.SwaggerDoc("Template", new OpenApiInfo { Title = "索引模板管理API", Version = "Template" });
                options.DescribeAllEnumsAsStrings();
                var basePath = AppContext.BaseDirectory;
                options.IncludeXmlComments(Path.Combine(basePath, "ElasticSearch7Template.xml"));
                options.IncludeXmlComments(Path.Combine(basePath, "ElasticSearch7Template.Model.xml"));
                options.IncludeXmlComments(Path.Combine(basePath, "ElasticSearch7Template.Entity.xml"));
            });
            //注册服务
            services.AddHealthChecks();
            RegisterService.RegisterComponents(services);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseAuthorization();
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyHeader();
                builder.AllowAnyMethod();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            CommonServiceProvider.ServiceProvider = app.ApplicationServices.CreateScope().ServiceProvider;
            app.UseStaticHttpContext();
            app.UseStaticFiles();
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                // options.SwaggerEndpoint($"/swagger/v1/swagger.json", "NetCore3WebApiTemplate API V1");
                options.SwaggerEndpoint("/swagger/Basic/swagger.json", "基础模块");
                options.SwaggerEndpoint("/swagger/Index/swagger.json", "索引文件管理API");
                options.SwaggerEndpoint("/swagger/Template/swagger.json", "索引模板管理API");
            });
            app.UseHealthChecks("/health");
        }
    }
}
