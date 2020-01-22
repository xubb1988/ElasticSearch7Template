using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ElasticSearch7Template.Utility.EnumClass
{
    public enum ExchangeTypeEnum
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        [Description("fanout")]
        Fanout=1,
        /// <summary>
        /// 
        /// </summary>
        [Description("direct")]
        Direct=2,
        /// <summary>
        /// 
        /// </summary>
        [Description("topic")]
        Topic =3
    }
}
