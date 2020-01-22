using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.Core
{
    public class SimpleSQLQueryModel
    {
        /// <summary>
        /// sql语句
        /// </summary>
        public string Sql { get; set; }

        /// <summary>
        /// sql语句
        /// </summary>
        public SqlFormatEnum Format { get; set; } = SqlFormatEnum.Json;

        /// <summary>
        ///  返回数量
        /// </summary>
        public int FetchSize { get; set; } = 10;
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SqlFormatEnum
    {
        /// <summary>
        /// json格式
        /// </summary>
        Json,
        /// <summary>
        /// Txt格式
        /// </summary>
        Txt,
    }
}
