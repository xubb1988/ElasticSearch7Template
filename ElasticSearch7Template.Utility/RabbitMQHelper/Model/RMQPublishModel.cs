using ElasticSearch7Template.Utility.EnumClass;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.Utility.RabbitMQHelper
{
    public class RMQPublishModel<T>
    {

        /// <summary>
        /// 消息
        /// </summary>
        public T Message { get; set; }

        /// <summary>
        /// 交换器名称
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// 路由键值
        /// </summary>
        public string RoutingKey { get; set; } = "";

        /// <summary>
        /// 是否需要死信队列
        /// </summary>
        public bool IsNeedDLX { get; set; } = true;
        /// <summary>
        /// 
        /// </summary>
        public ushort SendMessageCount { get; set; } = 200;

        /// <summary>
        /// 交换器类型
        /// </summary>
        public ExchangeTypeEnum ExchangeType { get; set; } = ExchangeTypeEnum.Fanout;

    }
}
