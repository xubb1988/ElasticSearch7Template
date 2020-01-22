using ElasticSearch7Template.Core;
using ElasticSearch7Template.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net;

namespace ElasticSearch7Template.Filters
{
    /// <summary>
    /// 验证数据的格式，按照自定义的值类型返回
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true)]
    public class ModelValidationAttribute : ActionFilterAttribute
    {
        /// <summary>
        ///  控制器中的操作执行之前调用此方法
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            var modelState = actionContext.ModelState;
            if (!modelState.IsValid)
            {
                string error = string.Empty;
                foreach (var key in modelState.Keys)
                {
                    var state = modelState[key];
                    if (state.Errors.Any())
                    {
                        var errorInfo = state.Errors.First();
                        string ex = (errorInfo.Exception == null ? "" : errorInfo.Exception.ToString());
                        error = string.IsNullOrEmpty(errorInfo.ErrorMessage) ? ex : errorInfo.ErrorMessage;
                        break;
                    }
                }
                HttpResponseResultModel<string> response = new HttpResponseResultModel<string>() {
                    IsSuccess = false, ErrorMessage = error, HttpStatusCode = HttpStatusCode.BadRequest };

                actionContext.Result = new JsonResult(response);
            }
        }
    }
}
