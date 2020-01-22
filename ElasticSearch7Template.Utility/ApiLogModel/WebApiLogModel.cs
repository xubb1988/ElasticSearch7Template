using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.Utility
{
    /// <summary>
    /// webapi 日志类
    /// </summary>
    public class WebApiLogModel
    {
        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 执行开始时间
        /// </summary>
        public DateTime ExecuteStartTime { get; set; }

        /// <summary>
        /// 执行结束时间
        /// </summary>
        public DateTime ExecuteEndTime { get; set; }

        /// <summary>
        /// 总耗时(秒)
        /// </summary>

        public double TotalSeconds { get; set; }

        /// <summary>
        /// 请求的Action 参数
        /// </summary>
        public Dictionary<string, object> ActionParams { get; set; }

        /// <summary>
        /// Http请求头
        /// </summary>
        public IHeaderDictionary HttpRequestHeaders { get; set; }

        /// <summary>
        /// 请求的路径
        /// </summary>
        public string HttpRequestPath { get; set; }

        /// <summary>
        /// 请求方法类型(POST,GET等)
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>
        /// 请求的IP地址
        /// </summary>
        public string IP { get; set; }
    }
}
