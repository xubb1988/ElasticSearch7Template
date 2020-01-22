using ElasticSearch7Template.Entity;
using Nest;
using System;
using System.Reflection;

namespace ElasticSearch7Template.DAL
{
    public class ElasticSearchPocoData
    {

        /// <summary>
        /// 
        /// </summary>
        public string DefaultIndexName { get; set; }

        public string DefaultTypeName { get; set; }
        public string Alias { get; set; }

        public string PrimaryKey { get; set; }
        public string RelationName { get; set; }

        /// <summary>  
        ///模板名称 
        /// </summary>  
        public string DefaultTemplateName { get; set; }

        public ElasticSearchPocoData(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            // Get the table name
            var indexAttribute = typeInfo.GetCustomAttribute<ElasticsearchIndexAttribute>();
            DefaultIndexName = (indexAttribute != null ? indexAttribute.DefaultIndexName.ToLower() : typeInfo.Name.ToLower());
            Alias = (indexAttribute != null ? indexAttribute.Alias.ToLower() : string.Empty);
            DefaultTemplateName = (indexAttribute != null ? indexAttribute.TemplateName.ToLower() : string.Empty);

            var elasticsearchTypeAttribute = typeInfo.GetCustomAttribute<ElasticsearchTypeAttribute>();
            PrimaryKey = (elasticsearchTypeAttribute != null ? elasticsearchTypeAttribute.IdProperty : typeInfo.Name);
            RelationName = (elasticsearchTypeAttribute != null ? elasticsearchTypeAttribute.RelationName : typeInfo.Name);
        }
    }
}
