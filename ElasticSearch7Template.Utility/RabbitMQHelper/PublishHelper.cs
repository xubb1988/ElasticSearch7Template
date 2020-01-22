using ElasticSearch7Template.Utility.EnumClass;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.Utility.RabbitMQHelper
{
    public class PublishHelper
    {

        /// <summary>
        /// fanout 类型发布(非发布者确认模式)
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="message">消息内容</param>
        /// <param name="exChangeName">交换器名称</param>
        /// <param name="isNeedDLX">是否需要死信交换器</param>
        /// <returns></returns>
        public static bool Pubish<T>(RMQPublishModel<T> publishModel)
        {
            var exchangeTypeString = RabbitMQBaseHelper. GetExchangeTypeString(publishModel.ExchangeType);
            var connection = RabbitMQBaseHelper. CreateMQConnectionInPoolNew();
            try
            {
                using (var channel = connection.CreateModel())//建立通讯信道
                {
                    bool isSuccess = true;
                    channel.ExchangeDeclare(publishModel.ExchangeName, exchangeTypeString, true, false, null);
                    //死信交换器
                    if (publishModel.IsNeedDLX)
                    {
                        string dlx_exchange = RabbitMQConstant.DLXPrefix + publishModel.ExchangeName.Trim();
                        channel.ExchangeDeclare(dlx_exchange, exchangeTypeString, true, false, null);
                    }
                    channel.BasicQos(0, publishModel.SendMessageCount, false); //分发机制为触发式
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2;//1表示不持久,2.表示持久化
                    string msgJson = JsonHelper.SerializeObject(publishModel.Message);
                    var body = Encoding.UTF8.GetBytes(msgJson);
                    bool mandatory = false;
                    channel.BasicPublish(publishModel.ExchangeName, publishModel.RoutingKey, mandatory, properties, body);
                    return isSuccess;
                }
            }
            catch (Exception)
            {
                return false;

            }
            finally
            {
                RabbitMQBaseHelper. ResetMQConnectionToFree(connection);
            }
        }


        /// <summary>
        /// 发布（发布者确认模式）
        /// </summary>
        /// <typeparam name="T">消息类型</typeparam>
        /// <param name="message">消息内容</param>
        /// <param name="exChangeName">交换器名称</param>
        /// <param name="isNeedDLX">是否需要死信交换器</param>
        /// <returns></returns>
        public static bool ConfirmsPush<T>(RMQPublishModel<T> publishModel)
        {
            var exchangeTypeString = RabbitMQBaseHelper. GetExchangeTypeString(publishModel.ExchangeType);
            var connection = RabbitMQBaseHelper.CreateMQConnectionInPoolNew();
            try
            {
                using (var channel = connection.CreateModel())//建立通讯信道
                {
                    bool isSuccess = true;
                    channel.ExchangeDeclare(publishModel.ExchangeName, exchangeTypeString, true, false, null);
                    //死信交换器
                    if (publishModel.IsNeedDLX)
                    {
                        string dlx_exchange = RabbitMQConstant.DLXPrefix + publishModel.ExchangeName.Trim();
                        channel.ExchangeDeclare(dlx_exchange, exchangeTypeString, true, false, null);
                    }
                    // 开启发送方确认模式
                    channel.ConfirmSelect();
                    channel.BasicQos(0, publishModel.SendMessageCount, false); //分发机制为触发式
                    var properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2;//1表示不持久,2.表示持久化
                    string msgJson = JsonHelper.SerializeObject(publishModel.Message);
                    var body = Encoding.UTF8.GetBytes(msgJson);
                    bool mandatory = true;
                    channel.BasicPublish(publishModel.ExchangeName, publishModel.RoutingKey, mandatory, properties, body);
                    channel.BasicReturn += (ch, ea) =>
                    {
                        isSuccess = false;
                    };
                    channel.WaitForConfirmsOrDie();
                    return isSuccess;
                }
            }
            catch (Exception)
            {
                return false;

            }
            finally
            {
                RabbitMQBaseHelper.ResetMQConnectionToFree(connection);
            }
        }
    }

}
