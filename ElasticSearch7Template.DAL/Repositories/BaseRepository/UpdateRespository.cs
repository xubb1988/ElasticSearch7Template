using ElasticSearch7Template.Core;
using ElasticSearch7Template.Entity;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch7Template.DAL
{
    internal abstract partial class AbstractRepository<TEntity, TCondition> where TEntity : class where TCondition : ESBaseCondition
    {
        #region 更新索引
        /// <summary>
        /// 根据id更新
        /// </summary>
        /// <param name="id">主键</param>
        /// <param name="entity">需要更新的实体</param>
        /// <param name="routing">路由键</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> UpdateAsync<T, TPrimaryKeyType>(TPrimaryKeyType id, InsertUpdateModel<T> insertUpdateModel) where T : class, new()
        {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool>();
            string indexName = insertUpdateModel.IndexName?.ToLower();
            if (string.IsNullOrEmpty(indexName))
            {
                indexName = PocoIndexName;
            }
            UpdateDescriptor<T, T> updRequest = new UpdateDescriptor<T, T>(indexName, id.ToString());
            updRequest.Doc(insertUpdateModel.Entity);
            updRequest.Index(indexName);
            if (!string.IsNullOrEmpty(insertUpdateModel.Routing))
            {
                updRequest.Routing(insertUpdateModel.Routing);
            }
            var response = await client.UpdateAsync<T, T>(updRequest).ConfigureAwait(false);
            GetDebugInfo(response);
            httpResponseResultModel.IsSuccess = (response.Result == Result.Updated);
            return httpResponseResultModel;
        }

        /// <summary>
        /// 根据id更新部分实体字段
        /// </summary>
        /// <typeparam name="TPartialDocument">包含实体T中部分字段的是model</typeparam>
        /// <param name="id">主键</param>
        /// <param name="partEntity">部分实体model</param>
        /// <param name="routing">路由键</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> UpdatePartialDocumentAsync<TPartialDocument, TPrimaryKeyType>(TPrimaryKeyType id, InsertUpdateModel<TPartialDocument> insertUpdateModel) where TPartialDocument : class, new()
        {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool>();
            string indexName = insertUpdateModel.IndexName?.ToLower();
            if (string.IsNullOrEmpty(indexName))
            {
                indexName = PocoIndexName;
            }
            UpdateDescriptor<TEntity, TPartialDocument> updRequest = new UpdateDescriptor<TEntity, TPartialDocument>(indexName, id.ToString());
            updRequest.Doc(insertUpdateModel.Entity);
            updRequest.Index(indexName);
            if (!string.IsNullOrEmpty(insertUpdateModel.Routing))
            {
                updRequest.Routing(insertUpdateModel.Routing);
            }
            var response = await client.UpdateAsync<TEntity, TPartialDocument>(updRequest).ConfigureAwait(false);
            GetDebugInfo(response);
            httpResponseResultModel.IsSuccess = (response.Result == Result.Updated);
            return httpResponseResultModel;

        }


        #endregion
    }
}
