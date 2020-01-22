using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseModel
    {
        /// <summary>
        ///  索引名称
        /// </summary>
        public string IndexName { get; set; }

        /// <summary>
        ///  路由
        /// </summary>
        public string Routing { get; set; }

    }
}
