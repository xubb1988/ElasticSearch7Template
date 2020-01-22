using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ElasticSearch7Template.Core
{
    /// <summary>
    /// 插入model
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InsertUpdateModel<T> : BaseModel where T : class
    {
        /// <summary>
        /// 实体名称
        /// </summary>
        [Required(AllowEmptyStrings = false, ErrorMessage = "实体不能为空")]
        public T Entity { get; set; }

    }
}
