using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace ElasticSearch7Template.Utility.LoggerHelper
{
    public static class MyLogger
    {
        public static ILogger Logger;
        public static void AddMyLogger()
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

                .MinimumLevel.Debug() // 所有Sink的最小记录级别
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Debug).WriteTo.File(Path.Combine("logs/Debug", @"log.txt"), rollingInterval: RollingInterval.Day))
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Information).WriteTo.File(Path.Combine("logs/Information", @"log.txt"), rollingInterval: RollingInterval.Day))
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Warning).WriteTo.File(Path.Combine("logs/Warning", @"log.txt"), rollingInterval: RollingInterval.Day))
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Error).WriteTo.File(Path.Combine("logs/Error", @"log.txt"), rollingInterval: RollingInterval.Day))
                .WriteTo.Logger(lg => lg.Filter.ByIncludingOnly(p => p.Level == LogEventLevel.Fatal).WriteTo.File(Path.Combine("logs/Fatal", @"log.txt"), rollingInterval: RollingInterval.Day))
                // 创建 logger
                .CreateLogger();
            Logger = Log.Logger;
        }
    }
}
