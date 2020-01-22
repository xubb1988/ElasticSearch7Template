using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.Core
{
    /// <summary>
    /// MongoDB配置
    /// </summary>
    public class MongoDBConfig
    {
        /// <summary>
        /// mongodb数据库地址
        /// </summary>
        public static string MongoDbUrl { get; set; }

        /// <summary>
        /// mongodb数据库名
        /// </summary>
        public static string MongoDbName { get; set; }
    }
}
