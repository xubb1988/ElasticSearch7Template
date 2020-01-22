using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.Entity
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ElasticsearchIndexAttribute : Attribute
    {
        /// <summary>  
        /// 默认索引名  
        /// </summary>  
        public string DefaultIndexName { get; set; }

        /// <summary>  
        ///别名 
        /// </summary>  
        public string Alias { get; set; }

        /// <summary>  
        ///模板名称 
        /// </summary>  
        public string TemplateName { get; set; }

        public ElasticsearchIndexAttribute()
        {

        }


    }
}
