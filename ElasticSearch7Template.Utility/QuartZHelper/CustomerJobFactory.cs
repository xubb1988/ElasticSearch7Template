using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace ElasticSearch7Template.Utility
{
    public class CustomerJobFactory : IJobFactory
    {
        private readonly IServiceProvider serviceProvider;
        public CustomerJobFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider; 
        }
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            
            var job= serviceProvider.GetService(bundle.JobDetail.JobType) as IJob;
            return job;
            
        }

        public void ReturnJob(IJob job)
        {
            
        }
    }
}
