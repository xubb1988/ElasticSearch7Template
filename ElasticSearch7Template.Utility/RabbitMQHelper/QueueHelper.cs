using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.Utility.RabbitMQHelper
{
    public class QueueHelper
    {


        /// <summary>
        /// 定义一个队列
        /// </summary>
        /// <param name="subscribeModel"></param>
        /// <returns></returns>
        public bool DeclareQueueOnServer(RMQSubscribeModel subscribeModel)
        {
            var connection = RabbitMQBaseHelper.CreateMQConnectionInPoolNew();
            using (var channel = connection.CreateModel())//建立通讯信道
            {
                SubscribeHelper.DefineQueue(connection, subscribeModel);
                RabbitMQBaseHelper.ResetMQConnectionToFree(connection);
            }

          
            return true;
        }
        public bool DeleteQueueOnServer(string queueName)
        {
            var connection = RabbitMQBaseHelper.CreateMQConnectionInPoolNew();
            using (var channel = connection.CreateModel())//建立通讯信道
            {
                channel.QueueDelete(queueName, false, false);
                channel.QueueDelete(RabbitMQConstant.DLXPrefix + queueName, false, false);
                RabbitMQBaseHelper.ResetMQConnectionToFree(connection);
            }
            return true;
        }

        /// <summary>
        /// 获得队列的消息数量
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public uint GetQueueMessageCount(string queueName)
        {
            var connection = RabbitMQBaseHelper.CreateMQConnectionInPoolNew();
            uint messageCount = 0;
            using (var channel = connection.CreateModel())//建立通讯信道
            {
                try
                {
                    messageCount = channel.MessageCount(queueName);
                }
                catch
                {

                }
                finally
                {
                    RabbitMQBaseHelper.ResetMQConnectionToFree(connection);
                }
            }
            return messageCount;
        }
    }
}
