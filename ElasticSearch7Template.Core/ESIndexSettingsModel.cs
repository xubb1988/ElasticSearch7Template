using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.Core
{
    public class ESIndexSettingsModel
    {
        /// <summary>
        /// 主分片数（建议小于等于节点数）
        /// </summary>
        public int NumberOfShards { get; set; } = 3;

        /// <summary>
        /// 副本数
        /// </summary>
        public int NumberOfReplicas { get; set; } = 1;


        /// <summary>
        /// 刷新间隔（秒） 默认30s
        /// </summary>
        public int RefreshInterval { get; set; } = 30;
    }
}
