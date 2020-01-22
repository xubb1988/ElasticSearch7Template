using ElasticSearch7Template.Core;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ElasticSearch7Template.Entity
{
    public class BaseField
    {
        /// <summary>
        /// 删除标记
        /// </summary>
        /// 
        [Boolean(Name = "isDeleted")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 添加人
        /// </summary>
        /// 
        [Number(NumberType.Long, Name = "createrId")]
        public long? CreaterId { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        /// 
        [Date(Name = "createTime")]
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        /// 
        [Number(NumberType.Long, Name = "modifierId")]
        public long? ModifierId { get; set; }

        /// <summary>
        ///  修改时间
        /// </summary>
        [Date(Name = "ModifyTime")]
        public DateTime? ModifyTime { get; set; }
    }
}
