using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElasticSearch7Template.Utility
{
    /// <summary>
    /// 拓展工具类
    /// </summary>
    public static class ExtensionUnility
    {
        /// <summary>
        /// 将对象转成int类型
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int ToInt(this object val)
        {
            return Convert.ToInt32(val);
        }

        /// <summary>
        /// 将对象转成long类型
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static long ToLong(this object val)
        {
            return Convert.ToInt64(val);

        }

        /// <summary>
        /// IEnumerable 类型是否为null或空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsListNullOrEmpty<T>(this IEnumerable<T> value)
        {
            if (value != null && value.Any())
            {
                return false;
            }
            return true;
        }
    }
}
