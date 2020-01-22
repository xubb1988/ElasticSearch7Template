using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ElasticSearch7Template.Utility
{
    /// <summary>
    /// json帮助类，序列化，反序列化
    /// </summary>
    public static class JsonHelper
    {



        /// <summary>
        /// 对象转字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string SerializeObject<T>(T message)
        {

            //Newtonsoft.Json的用法
            var str_Message = message is string ? message.ToString() : JsonConvert.SerializeObject(message, Formatting.None,
                 new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            return str_Message;


        }

        /// <summary>
        /// 对象转JSON字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string ToJson<T>(this T message)
        {

            //Newtonsoft.Json的用法
            var str_Message = message is string ? message.ToString() : JsonConvert.SerializeObject(message, Formatting.None,
                 new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });
            return str_Message;
        }

        /// <summary>
        /// 字符串转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string message)
        {
            //Newtonsoft.Json的用法
            var obj = JsonConvert.DeserializeObject<T>(message);
            return obj;



        }
    }
}
