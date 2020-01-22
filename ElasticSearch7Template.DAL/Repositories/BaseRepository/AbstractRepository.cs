using ElasticSearch7Template.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Nest;
using ElasticSearch7Template.Utility.ESHelper;

namespace ElasticSearch7Template.DAL
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TCondition"></typeparam>
    internal abstract partial class AbstractRepository<TEntity, TCondition> where TEntity : class where TCondition : ESBaseCondition
    {
        protected IElasticClient client;
        protected static ElasticSearchPocoData PocoData { get; set; }
        /// <summary>
        /// 索引名
        /// </summary>
        protected string PocoIndexName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected string PocoAlias { get; set; }
        public AbstractRepository()
        {
            PocoData = new ElasticSearchPocoDataFactory().ForType(typeof(TEntity));
            PocoIndexName = PocoData.DefaultIndexName.ToLower() + "_" + DateTime.Now.Year.ToString();
            PocoAlias = string.IsNullOrEmpty(PocoData.Alias) ? PocoIndexName + "_alias" : PocoData.Alias;
            client = new ElasticSearchConfiguration(PocoAlias).GetClient();
        }

    }

}
