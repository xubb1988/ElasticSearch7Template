using ElasticSearch7Template.Utility.EnumClass;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch7Template.Utility.RabbitMQHelper
{
    public class SubscribeHelper 
    {




        /// <summary>
        ///  消息消费（fanout）
        /// </summary>`
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="handler">回调</param>
        /// <param name="exChangeName">交换器名</param>
        /// <param name="queueName">队列名</param>
        /// <param name="routingKey">路由名(默认和队列一致)</param>
        /// <param name="isNeedDLX">是否需要死信队列</param>
        public static IModel FanoutConsume(Func<string, Task<ConsumeActionEnum>> handler, RMQSubscribeModel subscribeModel)
        {
            var connection = RabbitMQBaseHelper.CreateMQConnectionInPoolNew();
            try
            {
                return Consume(connection, handler, subscribeModel);
            }
            catch (Exception)
            {
                // throw ex;
                // return null;
                //重新连接
                return Consume(connection, handler, subscribeModel);
            }
            finally
            {
                RabbitMQBaseHelper.ResetMQConnectionToFree(connection);
            }

        }

        public static IModel DefineQueue(IConnection connection, RMQSubscribeModel subscribeModel)
        {
            var exchangeTypeString = RabbitMQBaseHelper.GetExchangeTypeString(subscribeModel.ExchangeType);
            string routingKey = subscribeModel.RoutingKey.Trim(); 
            string queueName = subscribeModel.QueueName.Trim();
            string exChangeName = subscribeModel.ExchangeName.Trim();
            if (string.IsNullOrEmpty(routingKey))
            {
                routingKey = queueName;
            }
            var channel = connection.CreateModel();
            //死信交换器
            Dictionary<string, object> args = new Dictionary<string, object>();
            string dlx_exchange = RabbitMQConstant.DLXPrefix + exChangeName;
            string dlx_queue = RabbitMQConstant.DLXPrefix + queueName;
            args.Add("x-dead-letter-exchange", dlx_exchange);
            args.Add("x-dead-letter-routing-key", routingKey);
            channel.ExchangeDeclare(exChangeName, exchangeTypeString, true, false, null);
            //死信交换器
            if (subscribeModel.IsNeedDLX)
            {
                channel.QueueDeclare(queueName, true, false, false, args);
            }
            else
            {
                channel.QueueDeclare(queueName, true, false, false, null);
            }
            channel.QueueBind(queueName, exChangeName, routingKey);
            if (subscribeModel.IsNeedDLX)
            {
                channel.ExchangeDeclare(dlx_exchange, exchangeTypeString, true, false, null);
                channel.QueueDeclare(dlx_queue, true, false, false, null);
                channel.QueueBind(dlx_queue, dlx_exchange, routingKey.Trim());
            }
            channel.BasicQos(0, subscribeModel.SendMessageCount, false); //分发机制为触发式
            return channel;
        }

        /// <summary>
        /// 重新连接
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="handler"></param>
        /// <param name="exChangeName"></param>
        /// <param name="queueName"></param>
        /// <param name="routingKey"></param>
        /// <param name="isNeedDLX"></param>
        /// <returns></returns>
        private static IModel Consume(IConnection connection, Func<string, Task<ConsumeActionEnum>> handler, RMQSubscribeModel subscribeModel)
        {
            var channel = DefineQueue(connection, subscribeModel);
            var consumer = new EventingBasicConsumer(channel);
            ConsumeActionEnum consumeResult = ConsumeActionEnum.RETRY;
            Task<ConsumeActionEnum> result;
            consumer.Received += async (ch, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                result = handler(message);
                consumeResult = await result;
                if (consumeResult == ConsumeActionEnum.ACCEPT)
                {
                    channel.BasicAck(ea.DeliveryTag, false);  //消息从队列中删除
                }
                else if (consumeResult == ConsumeActionEnum.RETRY)
                {
                    channel.BasicNack(ea.DeliveryTag, false, true); //消息重回队列
                }
                else
                {
                    channel.BasicNack(ea.DeliveryTag, false, false); //消息直接丢弃
                }
            };
            channel.BasicConsume(subscribeModel.QueueName, false, consumer);
            return channel;
        }

        /// <summary>
        ///  消息死信队列消费（fanout）
        /// </summary>`
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="handler">回调</param>
        /// <param name="exChangeName">交换器名</param>
        /// <param name="queueName">队列名</param>
        /// <param name="routingKey">路由名(默认和队列一致)</param>
        /// <param name="isNeedDLX">是否需要死信队列</param>
        public static IModel FanoutConsumeForDLX(Func<string, Task<ConsumeActionEnum>> handler, RMQSubscribeModel subscribeModel)
        {
            var connection = RabbitMQBaseHelper.CreateMQConnectionInPoolNew();

            try
            {


                return ConsumeForDLX(connection, handler, subscribeModel);
            }
            catch (Exception)
            {
                //重新连接
                return ConsumeForDLX(connection, handler, subscribeModel);
                // throw ex;
            }
            finally
            {
                RabbitMQBaseHelper.ResetMQConnectionToFree(connection);
            }
        }

        /// <summary>
        /// 拉模式消费死信队列
        /// </summary>
        /// <returns></returns>
        public static async Task<IModel> PullMessageConsumeForDLXAsync(Func<string, Task<ConsumeActionEnum>> handler, RMQSubscribeModel subscribeModel)
        {
            var connection = RabbitMQBaseHelper.CreateMQConnectionInPoolNew();
          // var channel = connection.CreateModel();
            var channel= DefineQueue(connection, subscribeModel);
            BasicGetResult response = channel.BasicGet(RabbitMQConstant.DLXPrefix + subscribeModel.QueueName, false);
            if (response != null)
            {
                string message = Encoding.UTF8.GetString(response.Body);
                ConsumeActionEnum consumeResult = ConsumeActionEnum.RETRY;
                Task<ConsumeActionEnum> result = handler(message);
                consumeResult = await result;
                if (consumeResult == ConsumeActionEnum.ACCEPT)
                {
                    channel.BasicAck(response.DeliveryTag, false);  //消息从队列中删除
                }
                else if (consumeResult == ConsumeActionEnum.RETRY)
                {
                    channel.BasicNack(response.DeliveryTag, false, true); //消息重回队列
                }
                else
                {
                    channel.BasicNack(response.DeliveryTag, false, false); //消息直接丢弃
                }
            }
            else {
                Console.WriteLine($"{RabbitMQConstant.DLXPrefix + subscribeModel.QueueName}暂无可消费信息");
            }
            RabbitMQBaseHelper.ResetMQConnectionToFree(connection);
            return channel;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="exChangeName"></param>
        /// <param name="queueName"></param>
        /// <param name="routingKey"></param>
        /// <returns></returns>
        private static IModel ConsumeForDLX(IConnection connection, Func<string, Task<ConsumeActionEnum>> handler, RMQSubscribeModel subscribeModel)
        {
            subscribeModel.IsNeedDLX = false;
            var channel = DefineQueue(connection, subscribeModel);
            var consumer = new EventingBasicConsumer(channel);
            ConsumeActionEnum consumeResult = ConsumeActionEnum.RETRY;
            Task<ConsumeActionEnum> result;
            consumer.Received += async (ch, ea) =>
            {

                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                result = handler(message);
                consumeResult = await result;
                if (consumeResult == ConsumeActionEnum.ACCEPT)
                {
                    channel.BasicAck(ea.DeliveryTag, false);  //消息从队列中删除
                }
                else if (consumeResult == ConsumeActionEnum.RETRY)
                {
                    channel.BasicNack(ea.DeliveryTag, false, true); //消息重回队列
                }
                else
                {
                    channel.BasicNack(ea.DeliveryTag, false, false); //消息直接丢弃
                }


            };
            channel.BasicConsume(subscribeModel.QueueName, false, consumer);

            return channel;
        }


    }

}
