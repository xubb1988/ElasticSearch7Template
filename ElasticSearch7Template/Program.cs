using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace ElasticSearch7Template
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // ���� Serilog 
            Log.Logger = new LoggerConfiguration()
                // ��С����־�������
                .MinimumLevel.Information()
                // ��־�����������ռ������ Microsoft ��ͷ��������־�����С����Ϊ Information
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                // ������־���������̨
                .WriteTo.Console()
                // ������־������ļ����ļ��������ǰ��Ŀ�� logs Ŀ¼��
                // �ռǵ���������Ϊÿ��
                //.WriteTo.File(Path.Combine("logs/Information", @"log.txt"), LogEventLevel.Information, rollingInterval: RollingInterval.Day)
                //.WriteTo.File(Path.Combine("logs/Warning", @"log.txt"), LogEventLevel.Warning, rollingInterval: RollingInterval.Day)
                //.WriteTo.File(Path.Combine("logs/Debug", @"log.txt"), LogEventLevel.Debug, rollingInterval: RollingInterval.Day)
                //.WriteTo.File(Path.Combine("logs/Error", @"log.txt"), LogEventLevel.Error, rollingInterval: RollingInterval.Day)
                //.WriteTo.File(Path.Combine("logs/Fatal", @"log.txt"), LogEventLevel.Fatal, rollingInterval: RollingInterval.Day)
                .MinimumLevel.Debug() // ����Sink����С��¼����
.WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Debug).WriteTo.File(Path.Combine("logs/Debug", @"log.txt"), rollingInterval: RollingInterval.Day))
.WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Information).WriteTo.File(Path.Combine("logs/Information", @"log.txt"), rollingInterval: RollingInterval.Day))
.WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Warning).WriteTo.File(Path.Combine("logs/Warning", @"log.txt"), rollingInterval: RollingInterval.Day))
.WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Error).WriteTo.File(Path.Combine("logs/Error", @"log.txt"), rollingInterval: RollingInterval.Day))
.WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Fatal).WriteTo.File(Path.Combine("logs/Fatal", @"log.txt"), rollingInterval: RollingInterval.Day))
                // ���� logger
                .CreateLogger();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseSerilog();
                });
    }
}
