using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch7Template.Utility.QuartZHelper
{
    public class QuartZHelper
    {
        private static IScheduler scheduler;
        public QuartZHelper() {

        }

        public static async Task InitAsync<T>(IServiceProvider serviceProvider,int intervalInMilliSeconds) where T : IJob
        {
            #region quartZ 定时任务
            var properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "RemoteServerSchedulerClient";

            // set thread pool info
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";

            // set remoting expoter
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "556";
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            properties["quartz.scheduler.exporter.channelType"] = "tcp";

            var schedulerFactory = new StdSchedulerFactory(properties);
            scheduler = await schedulerFactory.GetScheduler();
            scheduler.JobFactory = new CustomerJobFactory(serviceProvider);//定时调度器
            var map = new JobDataMap();
            map.Put("msg", "Some message!");
            string className = typeof(T).ToString();
            var job = JobBuilder.Create<T>()
                .WithIdentity(className, className)
                .UsingJobData(map)
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"{className}Trigger", className)
                .ForJob(job)
                .StartNow()
                .WithSimpleSchedule(t => t.WithInterval(new TimeSpan(0,0,0,0, intervalInMilliSeconds)).RepeatForever())
                .Build();

            // schedule the job
            await scheduler.ScheduleJob(job, trigger);


            #endregion
        }

        public static async Task Start() {
            await scheduler.Start();
        }
    }
}
