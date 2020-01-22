using System.Collections.Generic;

namespace ElasticSearch7.xTemplate.Core
{
    /// <summary>
    /// 服务器配置类
    /// </summary>
    public class AppsettingsConfig
    {
        /// <summary>
        /// 默认数据库连接
        /// </summary>
        public static string DefaultConnectionString { get; set; } = "";


        /// <summary>
        /// 生产主键机器码
        /// </summary>
        public static int? MachineId { get; set; } = 1;

        /// <summary>
        /// 生产主键数据中心id
        /// </summary>
        public static int? DataCenterId { get; set; } = 1;

        /// <summary>
        /// 省库链接地址
        /// </summary>
        public static string MainDBConnectionString { get; set; }

        /// <summary>
        /// mongodb数据库地址
        /// </summary>
        public static string MongoDbUrl { get; set; }

        /// <summary>
        /// mongodb数据库名
        /// </summary>
        public static string MongoDbName { get; set; }
        /// <summary>
        /// RabbitMQ 配置
        /// </summary>
        public static RabbitMQWebConfig RabbitMQConfig { get; set; }

        /// <summary>
        /// ApiService 配置
        /// </summary>
        public static ServiceApiHosts ServiceApiHosts { get; set; }

        public static ElasticSearchConfig ElasticSearchConfig { get; set; }
    }

    /// <summary>
    /// RabbitMQ 配置
    /// </summary>

    public class RabbitMQWebConfig
    {
        /// <summary>
        /// RabbitMQ host
        /// </summary>
        public string RabbitMQHost { get; set; }

        /// <summary>
        /// RabbitMQ host
        /// </summary>
        public string RabbitMQUserName { get; set; }

        /// <summary>
        /// RabbitMQ host
        /// </summary>
        public string RabbitMQPassword { get; set; }

        /// <summary>
        /// RabbitMQ host
        /// </summary>
        public string RabbitMQVirtualHost { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int RabbitMQPort { get; set; } = 5672;
        /// <summary>
        /// endpoint 地址
        /// </summary>

        public string AmqpTcpEndpointUrl { get; set; }

        /// <summary>
        /// 是否自动重连
        /// </summary>
        public bool AutomaticRecoveryEnabled { get; set; } = true;

        /// <summary>
        /// RabbitMQ最大连接连接数
        /// </summary>
        public int? MQMaxConnectionCount { get; set; } = 30;
    }

    public class ServiceApiHosts
    {
        public Dictionary<string, string> ApiHostOptions { get; set; }

    }



    public class ElasticSearchConfig
    {
        /// <summary>
        /// 集群节点
        /// </summary>
        public string[] ClusterNodeUrlHosts { get; set; }

        /// <summary>
        /// 是否开启es 调试状态
        /// </summary>
        public bool? IsOpenDebugger { get; set; } = false;
    }
}
