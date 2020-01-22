using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.Utility.RabbitMQHelper
{
    public class ExchangeHelper
    {
        public bool DeleteExchangeNameOnServer(string exchangeName)
        {
            var connection = RabbitMQBaseHelper.CreateMQConnectionInPoolNew();
            using (var channel = connection.CreateModel())//建立通讯信道
            {
                channel.ExchangeDelete(exchangeName, false);
                channel.ExchangeDelete(RabbitMQConstant.DLXPrefix + exchangeName, false);
            }
            return true;
        }
    }
}
