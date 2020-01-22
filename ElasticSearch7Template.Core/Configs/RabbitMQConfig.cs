using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.Core
{

    /// <summary>
    /// rabbitmq配置
    /// </summary>
    public class RabbitMQConfig
    {
        /// <summary>
        /// RabbitMQ host集合
        /// </summary>
        public static List<string> RabbitMQHostList { get; set; }

         

        /// <summary>
        /// RabbitMQ host
        /// </summary>
        public static string RabbitMQUserName { get; set; }

        /// <summary>
        /// RabbitMQ host
        /// </summary>
        public static string RabbitMQPassword { get; set; }

        /// <summary>
        /// RabbitMQ host
        /// </summary>
        public static string RabbitMQVirtualHost { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public static int RabbitMQPort { get; set; } = 5672;
        /// <summary>
        /// endpoint 地址
        /// </summary>

        public static string AmqpTcpEndpointUrl { get; set; }

        /// <summary>
        /// 是否自动重连
        /// </summary>
        public static bool AutomaticRecoveryEnabled { get; set; } = true;

        /// <summary>
        /// RabbitMQ最大连接连接数
        /// </summary>
        public static int MQMaxConnectionCount { get; set; } = 30;
    }
}
