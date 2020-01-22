using System.Collections.Generic;

namespace ElasticSearch7Template.Core
{
    public class QueryPagedResponseModel<T>
    {
        /// <summary>
        ///  是否成功
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// 数据总数
        /// </summary>
        public long Total { get; set; }
        /// <summary>
        /// 当前分页数据
        /// </summary>
        public IList<T> Data { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
