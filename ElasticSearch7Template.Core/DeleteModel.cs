using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPrimaryKeyType"></typeparam>
    public class DeleteSingleModel<TPrimaryKeyType> : BaseModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public TPrimaryKeyType Id { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TPrimaryKeyType"></typeparam>
    public class DeleteBatchModel<TPrimaryKeyType> : BaseModel
    {
        /// <summary>
        /// 主键id集合
        /// </summary>
        public IList<TPrimaryKeyType> IdList { get; set; }

    }
}
