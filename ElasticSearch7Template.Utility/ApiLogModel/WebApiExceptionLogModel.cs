namespace ElasticSearch7Template.Utility
{
    /// <summary>
    /// webapi异常日志类
    /// </summary>
    public class WebApiExceptionLogModel: WebApiLogModel
    {
        /// <summary>
        /// 异常信息
        /// </summary>
        public string ExceptionMessage { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public int StatusCode { get; set; }
    }
}
