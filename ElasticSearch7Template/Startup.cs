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
    /// ������
    /// </summary>
    public class Startup
    {

        private IServiceCollection services;
        private IDisposable callbackRegistration;
        /// <summary>
        /// ���캯��
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
        /// �����´�appsettings����
        /// </summary>
        /// <param name="obj"></param>
        private void ChangeCallBack(object obj)
        {
            callbackRegistration?.Dispose();
            GetJsonConfig();
            //����ע��callback���´�appsettings.josn���º���Զ�����
            callbackRegistration = Configuration.GetReloadToken().RegisterChangeCallback(ChangeCallBack, obj);
        }

        /// <summary>
        /// ��ȡ�����ļ�
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
            //����Ĭ��ModelState��Ϊ
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            //ȫ������Json���л�����
            services.AddControllers(ops =>
            {
                ops.Filters.Add(new WebApiExceptionFilterAttribute());
                ops.Filters.Add(new WebApiTrackerAttribute());
              //  ops.Filters.Add(new IPFilter());
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddNewtonsoftJson(options =>
            {
                //����ʱ���ʽ
                options.SerializerSettings.DateFormatString = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
                options.SerializerSettings.ContractResolver = new CustomContractResolver();
            });
            services.AddSwaggerGen(options =>
            {
                //options.SwaggerDoc("v1", new OpenApiInfo { Title = "NetCore3WebApiTemplate API", Version = "v1" });
                options.SwaggerDoc("Basic", new OpenApiInfo { Title = "����ģ��API", Version = "Basic" });
                options.SwaggerDoc("Index", new OpenApiInfo { Title = "�����ļ�����API", Version = "Index" });
                options.SwaggerDoc("Template", new OpenApiInfo { Title = "����ģ�����API", Version = "Template" });
                options.DescribeAllEnumsAsStrings();
                var basePath = AppContext.BaseDirectory;
                options.IncludeXmlComments(Path.Combine(basePath, "ElasticSearch7Template.xml"));
                options.IncludeXmlComments(Path.Combine(basePath, "ElasticSearch7Template.Model.xml"));
                options.IncludeXmlComments(Path.Combine(basePath, "ElasticSearch7Template.Entity.xml"));
            });
            //ע�����
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
                options.SwaggerEndpoint("/swagger/Basic/swagger.json", "����ģ��");
                options.SwaggerEndpoint("/swagger/Index/swagger.json", "�����ļ�����API");
                options.SwaggerEndpoint("/swagger/Template/swagger.json", "����ģ�����API");
            });
            app.UseHealthChecks("/health");
        }
    }
}
