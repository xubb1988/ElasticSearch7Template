using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElasticSearch7Template.Utility
{
    public static class CustomHeaderHelper
    {


        /// <summary>
        /// 获取请求头中的Custom信息
        /// </summary>
        public static CustomHeader GetCustomHeader()
        {
            var accountInfo = GetCustomHeaderAsString();

            CustomHeader header;
            if (string.IsNullOrEmpty(accountInfo))
            {
                header = new CustomHeader();
            }
            else
            {

                header = JsonHelper.DeserializeObject<CustomHeader>(accountInfo);
            }
            return header;
        }

        /// <summary>
        /// 获取请求头中的Custom信息
        /// </summary>
        private static string GetCustomHeaderAsString()
        {

            return CommonHttpContext.Current != null && CommonHttpContext.Current.Items.Count > 0 ? CommonHttpContext.Current.Request.Headers["AccountInfo"].ToString() : null;
        }
    }

    public class CustomHeader
    {
        /// <summary>
        /// 操作人
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 操作人id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
    }
}
