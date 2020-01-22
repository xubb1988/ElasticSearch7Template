using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.Core
{
   public class ESBaseCondition
    {
        /// <summary>
        ///  elasticsearch Routing
        /// </summary>
        public string KeyRouting { get; set; }

        /// <summary>
        ///  分组时默认返回size值
        /// </summary>
        public int GroupDefautSize { get; set; }

        /// <summary>
        /// 默认的sum统计字段
        /// </summary>
        public string DefaultSumField { get; set; } = "a";

        /// <summary>
        /// 默认的时间统计字段
        /// </summary>
        public string DefaultTimeField { get; set; } = "createTime";

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }


        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }


    }
}
