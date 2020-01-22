

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace ElasticSearch7Template.Utility
{
    /// <summary>
    /// json帮助类，序列化，反序列化
    /// </summary>
    public static class TextJsonHelper
    {
        static JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        };
        /// <summary>
        /// 对象转字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string SerializeObject<T>(T message)
        {


            //Ssytem.Text.Json的用法
            var str_Message = message is string ? message.ToString() : JsonSerializer.Serialize(message, options);
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
            //Ssytem.Text.Json的用法
            var str_Message = message is string ? message.ToString() : JsonSerializer.Serialize(message, options);
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


            //Ssytem.Text.Json的用法
            var obj = JsonSerializer.Deserialize<T>(message, options);
            return obj;
        }
    }
}
