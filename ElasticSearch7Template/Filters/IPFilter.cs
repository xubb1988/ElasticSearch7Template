using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ElasticSearch7Template.Filters
{
    /// <summary>
    /// 请求过滤器，以确保请求IP在信任列表中
    /// </summary>
    public class IPFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 控制器中的操作执行之前调用此方法
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            base.OnActionExecuting(actionContext);
        }
    }
}
