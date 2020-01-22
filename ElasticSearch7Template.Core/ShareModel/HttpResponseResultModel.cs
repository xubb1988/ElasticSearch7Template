using System.Net;

namespace ElasticSearch7Template.Core
{
    public class HttpResponseResultModel<T>
    {
        /// <summary>
        /// http码
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public T BackResult { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public string ExceptionMessage { get; set; }
    }
}
