using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.Core
{
    /// <summary>
    /// ElasticSearch 配置
    /// </summary>
    public class ElasticSearchConfig
    {
        /// <summary>
        /// 集群节点
        /// </summary>
        public static string[] ClusterNodeUrlHosts { get; set; }

        /// <summary>
        /// 是否开启es 调试状态
        /// </summary>
        public static bool? IsOpenDebugger { get; set; } = false;
    }
}
