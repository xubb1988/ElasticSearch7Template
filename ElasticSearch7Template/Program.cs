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
            // 配置 Serilog 
            Log.Logger = new LoggerConfiguration()
                // 最小的日志输出级别
                .MinimumLevel.Information()
                // 日志调用类命名空间如果以 Microsoft 开头，覆盖日志输出最小级别为 Information
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                // 配置日志输出到控制台
                .WriteTo.Console()
                // 配置日志输出到文件，文件输出到当前项目的 logs 目录下
                // 日记的生成周期为每天
                //.WriteTo.File(Path.Combine("logs/Information", @"log.txt"), LogEventLevel.Information, rollingInterval: RollingInterval.Day)
                //.WriteTo.File(Path.Combine("logs/Warning", @"log.txt"), LogEventLevel.Warning, rollingInterval: RollingInterval.Day)
                //.WriteTo.File(Path.Combine("logs/Debug", @"log.txt"), LogEventLevel.Debug, rollingInterval: RollingInterval.Day)
                //.WriteTo.File(Path.Combine("logs/Error", @"log.txt"), LogEventLevel.Error, rollingInterval: RollingInterval.Day)
                //.WriteTo.File(Path.Combine("logs/Fatal", @"log.txt"), LogEventLevel.Fatal, rollingInterval: RollingInterval.Day)
                .MinimumLevel.Debug() // 所有Sink的最小记录级别
.WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Debug).WriteTo.File(Path.Combine("logs/Debug", @"log.txt"), rollingInterval: RollingInterval.Day))
.WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Information).WriteTo.File(Path.Combine("logs/Information", @"log.txt"), rollingInterval: RollingInterval.Day))
.WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Warning).WriteTo.File(Path.Combine("logs/Warning", @"log.txt"), rollingInterval: RollingInterval.Day))
.WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Error).WriteTo.File(Path.Combine("logs/Error", @"log.txt"), rollingInterval: RollingInterval.Day))
.WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Fatal).WriteTo.File(Path.Combine("logs/Fatal", @"log.txt"), rollingInterval: RollingInterval.Day))
                // 创建 logger
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
