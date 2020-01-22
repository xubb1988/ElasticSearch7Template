
using ElasticSearch7Template.Core;
using ElasticSearch7Template.Model;
using ElasticSearch7Template.Utility;
using ElasticSearch7Template.Utility.LoggerHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ElasticSearch7Template.Filters
{
    /// <summary>
    /// api请求异常日志类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class WebApiExceptionFilterAttribute : ExceptionFilterAttribute, IActionFilter
    {
      
        /// <summary>
        /// 控制器中的操作执行之前调用此方法
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items.Add("params", context.ActionArguments);
        }

        /// <summary>
        /// 控制器中的操作执行之后调用此方法
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // do nothing
        }
        /// <summary>
        /// 控制器中的操作异常调用此方法
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        public override Task OnExceptionAsync(ExceptionContext actionExecutedContext)
        {

            if (actionExecutedContext.Exception != null)
            {
                WriteErrorAsync(actionExecutedContext);
            }
            if (actionExecutedContext.Exception is NotImplementedException)
            {
                actionExecutedContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotImplemented;
            }
            else if (actionExecutedContext.Exception is TimeoutException)
            {
                actionExecutedContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
            }
            else
            {
                HttpResponseResultModel<string> result = new HttpResponseResultModel<string>();
                result.HttpStatusCode = HttpStatusCode.InternalServerError;
                result.IsSuccess = false;
                result.ErrorMessage = "出现异常,请稍后重试";
                result.ExceptionMessage = actionExecutedContext.Exception.ToString();
                actionExecutedContext.HttpContext.Response.StatusCode = (int)result.HttpStatusCode;
                actionExecutedContext.Result = new ObjectResult(result);
            }
            return base.OnExceptionAsync(actionExecutedContext);
        }

        /// <summary>
        /// 写异常日志
        /// </summary>
        /// <param name="actionExecutedContext"></param>
        /// <returns></returns>
        private async Task WriteErrorAsync(ExceptionContext exceptionContext)
        {
            WebApiExceptionLogModel logModel = new WebApiExceptionLogModel();
            //获取Action 参数
            logModel.ExecuteEndTime = DateTime.Now;
            logModel.ExecuteStartTime = DateTime.Now;

            var items = exceptionContext.HttpContext.Items;
            IDictionary<string, object> actionArguments = null;
            if (items.ContainsKey("params"))
            {
                actionArguments = (IDictionary<string, object>)items["params"];
            }
            logModel.ActionParams = new Dictionary<string, object>(actionArguments);
            logModel.HttpRequestHeaders = exceptionContext.HttpContext.Request.Headers;
            logModel.HttpRequestPath = exceptionContext.HttpContext.Request.Path;
            logModel.HttpMethod = exceptionContext.HttpContext.Request.Method;
            logModel.ActionName = ((ControllerActionDescriptor)exceptionContext.ActionDescriptor).ActionName;
            logModel.ControllerName = ((ControllerActionDescriptor)exceptionContext.ActionDescriptor).ControllerName;
            logModel.TotalSeconds = (logModel.ExecuteEndTime - logModel.ExecuteStartTime).TotalSeconds;
            logModel.ExceptionMessage = exceptionContext.Exception.ToString();
            logModel.IP = CommonHttpContext.Current.Connection.RemoteIpAddress.ToString();
            logModel.StatusCode = exceptionContext.HttpContext.Response.StatusCode;
            var json = JsonHelper.SerializeObject(logModel);
            MyLogger.Logger.Error(json+"\r\n");
            //MongoDbHelper<WebApiExceptionLogModel> helper = new MongoDbHelper<WebApiExceptionLogModel>();
            //helper.Insert(logModel);

        }
    }
}
