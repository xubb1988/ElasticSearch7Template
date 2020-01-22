using ElasticSearch7Template.Core;
using ElasticSearch7Template.Entity;
using ElasticSearch7Template.Utility;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch7Template.DAL
{
    internal abstract partial class AbstractRepository<TEntity, TCondition> where TEntity : class where TCondition : ESBaseCondition
    {


        #region 新增索引

        /// <summary>
        /// 新增单条索引记录
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="routing"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> IndexAsync<T>(InsertUpdateModel<T> insertUpdateModel) where T : class, new()
        {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool>();
            string indexName = insertUpdateModel.IndexName?.ToLower();
            if (string.IsNullOrEmpty(indexName))
            {
                indexName = PocoIndexName;
            }
            IndexRequest<T> indexRequest = new IndexRequest<T>(insertUpdateModel.Entity, indexName);
            if (!string.IsNullOrEmpty(insertUpdateModel.Routing))
            {
                indexRequest.Routing = insertUpdateModel.Routing;
            }
            var result = await client.IndexAsync(indexRequest).ConfigureAwait(false);
            GetDebugInfo(result);
            httpResponseResultModel.IsSuccess = result.IsValid;
            return httpResponseResultModel;

        }

        /// <summary>
        /// 批量索引
        /// </summary>
        /// <param name="entities">实体集合</param>
        /// <param name="routing">路由键</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> IndexBatchAsync<T>(InsertUpdateModel<IList<T>> insertUpdateModel) where T : class, new()
        {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool>();
            string indexName = insertUpdateModel.IndexName?.ToLower();
            if (string.IsNullOrEmpty(indexName))
            {
                indexName = PocoIndexName;
            }
            BulkDescriptor bulkDescriptor = new BulkDescriptor();
            bulkDescriptor.IndexMany(insertUpdateModel.Entity);
            bulkDescriptor.Index(indexName);
            if (!string.IsNullOrEmpty(insertUpdateModel.Routing))
            {
                bulkDescriptor.Routing(new Routing(insertUpdateModel.Routing));
            }
            var result = await client.BulkAsync(bulkDescriptor).ConfigureAwait(false);
            GetDebugInfo(result);
            //if (result.Errors || !result.ItemsWithErrors.IsNullOrEmpty())
            //{
            //    Task.Run(() =>
            //    {
            //        var json = JsonHelper.SerializeObject(insertUpdateModel.Entity);
            //        WriteLog.WriteLogsAsync(json, "batchError");
            //    });
            //}
            var isSuccess = result.ItemsWithErrors.IsListNullOrEmpty();
            string errorMessage = "";
            if (!isSuccess)
            {
                var errorIdList = result.ItemsWithErrors.Select(t => t.Id);
                errorMessage = string.Join(",", errorIdList);
            }
            httpResponseResultModel.IsSuccess = isSuccess;
            httpResponseResultModel.ErrorMessage = errorMessage;
            return httpResponseResultModel;

        }

        #endregion


    }
}
