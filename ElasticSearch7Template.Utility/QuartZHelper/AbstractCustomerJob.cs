using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace ElasticSearch7Template.Utility.QuartZHelper
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractCustomerJob : IJob
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual async Task CustomerExecute() {

        }

        public virtual async Task Execute(IJobExecutionContext context)
        {
            await CustomerExecute();
        }
    }
}
