using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.Utility.EnumClass
{
    public enum ConsumeActionEnum
    {
        /// <summary>
        /// 消费成功
        /// </summary>
        ACCEPT,
        /// <summary>
        /// 消费失败，可以放回队列重新消费
        /// </summary>
        RETRY,
        /// <summary>
        ///  消费失败，直接丢弃 ,有死信队列就会进入死信队列
        /// </summary>
        REJECT, 
    }
}
