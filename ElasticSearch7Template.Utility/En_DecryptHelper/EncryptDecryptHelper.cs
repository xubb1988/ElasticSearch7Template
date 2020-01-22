using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ElasticSearch7Template.Utility
{
   public static class EncryptDecryptHelper
    {
        /// <summary>
        /// MD5加密方法
        /// </summary>
        /// <param name="value">需要加密的值</param>
        /// <param name="salt">加密盐值，默认不加盐</param>
        /// <returns>返回MD5加密值</returns>
        public static string Md5(string value, string salt = "")
        {
            var result = Encoding.Default.GetBytes(value + salt);
            var md5 = new MD5CryptoServiceProvider();
            var output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "").ToUpper();
        }
    }
}
