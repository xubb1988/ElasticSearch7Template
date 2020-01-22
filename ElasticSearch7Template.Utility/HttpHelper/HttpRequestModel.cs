using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.Utility
{
    public class HttpRequestModel
    {

        /// <summary>
        /// Host地址 带http://或者https://
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 请求url路径 如/api/get
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public List<KeyValuePair<string, string>> KeyValuePairData { get; set; }

        /// <summary>
        /// ContentType类型
        /// </summary>
        public string ContentType { get; set; } = "application/json";

        /// <summary>
        /// 授权码
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 授权码类型
        /// </summary>
        public string TokenType { get; set; } = "bearer";

        /// <summary>
        /// 请求类型
        /// </summary>
        public bool IsJsonRequest { get; set; } = true;
    }
}
