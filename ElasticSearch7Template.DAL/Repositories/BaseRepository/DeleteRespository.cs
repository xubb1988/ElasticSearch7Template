
using ElasticSearch7Template.Core;
using ElasticSearch7Template.Entity;
using ElasticSearch7Template.Utility;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticSearch7Template.DAL
{
    internal abstract partial class AbstractRepository<TEntity, TCondition> where TEntity : class where TCondition : ESBaseCondition
    {

        #region 删除索引
        /// <summary>
        ///  删除单条索引
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="routing">路由键</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> DeleteAsync<TPrimaryKeyType>(DeleteSingleModel<TPrimaryKeyType> deleteSingleModel)
        {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool>();
            string indexName = deleteSingleModel.IndexName?.ToLower();
            if (string.IsNullOrEmpty(indexName))
            {
                indexName = PocoIndexName;
            }

            DeleteRequest deleteRequest = new DeleteRequest(indexName, deleteSingleModel.Id.ToString());
            if (!string.IsNullOrEmpty(deleteSingleModel.Routing))
            {
                deleteRequest.Routing = deleteSingleModel.Routing;
            }
            var result = await client.DeleteAsync(deleteRequest).ConfigureAwait(false);
            GetDebugInfo(result);
            httpResponseResultModel.IsSuccess = (result.Result == Result.Deleted);
            return httpResponseResultModel;

        }

        /// <summary>
        /// 批量删除索引
        /// </summary>
        /// <typeparam name="TPrimaryKeyType"></typeparam>
        /// <param name="ids"></param>
        /// <param name="routing"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> DeleteBatchAsync<TPrimaryKeyType>(DeleteBatchModel<TPrimaryKeyType> deleteBatchModel)
        {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool>();
            string indexName = deleteBatchModel.IndexName?.ToLower();
            if (string.IsNullOrEmpty(indexName))
            {
                indexName = PocoIndexName;
            }
            BulkDescriptor bulkDescriptor = new BulkDescriptor();
            foreach (var id in deleteBatchModel.IdList)
            {
                bulkDescriptor.AddOperation(new BulkDeleteOperation<TEntity>(id.ToString()));
            }
            bulkDescriptor.Index(indexName);
            if (!string.IsNullOrEmpty(deleteBatchModel.Routing))
            {
                bulkDescriptor.Routing(new Routing(deleteBatchModel.Routing));
            }
            var result = await client.BulkAsync(bulkDescriptor).ConfigureAwait(false);
            GetDebugInfo(result);
            var isSuccess = result.ItemsWithErrors.IsListNullOrEmpty();
            httpResponseResultModel.IsSuccess = isSuccess;
            string errorMessage = "";
            if (!isSuccess)
            {
                var errorIdList = result.ItemsWithErrors.Select(t => t.Id);
                errorMessage = string.Join(",", errorIdList);
            }
            httpResponseResultModel.ErrorMessage = errorMessage;
            return httpResponseResultModel;
        }
        #endregion
    }
}
