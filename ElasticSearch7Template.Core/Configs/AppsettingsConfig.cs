using System.Collections.Generic;

namespace ElasticSearch7Template.Core
{
    /// <summary>
    /// 服务器配置类
    /// </summary>
    public class AppsettingsConfig
    {
        /// <summary>
        /// 默认数据库连接
        /// </summary>
        public static string DefaultConnectionString { get; set; } = "";

        /// <summary>
        /// 数据库类型SQLSERVER，MYSQL,ORACLE
        /// </summary>
        public static string DatabaseType { get; set; } = "SQLSERVER";


        /// <summary>
        /// 生产主键机器码
        /// </summary>
        public static int? MachineId { get; set; } = 1;

        /// <summary>
        /// 生产主键数据中心id
        /// </summary>
        public static int? DataCenterId { get; set; } = 1;





        /// <summary>
        /// ApiService 配置
        /// </summary>
        public static ServiceApiHosts ServiceApiHosts { get; set; }

    }


    public class ServiceApiHosts
    {
        public Dictionary<string, string> ApiHostOptions { get; set; }

    }




}
