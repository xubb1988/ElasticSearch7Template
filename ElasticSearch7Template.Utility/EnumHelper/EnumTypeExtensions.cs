using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace ElasticSearch7Template.Utility
{
    /// <summary>
    /// 枚举拓展类
    /// </summary>
    public static class EnumTypeExtensions
    {
        
        private static Dictionary<string, string> dic = new Dictionary<string, string>();

        /// <summary>
        /// 扩展方法，获得枚举的Description
        /// </summary>
        /// <param name="value">枚举值</param>
        /// <param name="nameInstend">当枚举没有定义DescriptionAttribute,是否用枚举名代替，默认使用</param>
        /// <returns>枚举的Description</returns>
        public static string GetDescription(this Enum value, bool nameInstend = true)
        {

            Type type = value.GetType();
            string fullName = type.FullName + "." + value.ToString(); ;
            if (!dic.ContainsKey(fullName))
            {
                string name = Enum.GetName(type, value);
                if (name == null)
                {
                    return null;
                }
                FieldInfo field = type.GetField(name);
                DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute == null && nameInstend == true)
                {
                    return name;
                }

                var description = (attribute == null ? null : attribute.Description);
                dic.Add(fullName, description);
                return description;

            }
            else
            {
                string description = "";
                dic.TryGetValue(fullName, out description);
                return description;
            }
        }

    }
}
