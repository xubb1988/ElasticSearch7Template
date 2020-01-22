using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.Core
{
   public class QueryModel<TPrimaryKeyType>: BaseModel
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public TPrimaryKeyType Id { get; set; }
        
    }
}
