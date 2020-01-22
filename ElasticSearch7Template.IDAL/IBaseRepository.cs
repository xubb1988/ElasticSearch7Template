using ElasticSearch7Template.Core;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ElasticSearch7Template.IDAL
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBaseRepository
    {



         

        /// <summary>
        /// 根据id查询单条数据(知道具体索引)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPrimaryKeyType"></typeparam>
        /// <param name="id"></param>
        /// <param name="indexName"></param>
        /// <param name="routing"></param>
        /// <returns></returns>
        Task<T> GetAsync<T, TPrimaryKeyType>(QueryModel<TPrimaryKeyType> queryModel) where T : class;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="routing"></param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> IndexAsync<T>(InsertUpdateModel<T> insertUpdateModel) where T : class, new();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="routing"></param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> IndexBatchAsync<T>(InsertUpdateModel<IList<T>> insertUpdateModel) where T : class, new();

        /// <summary>
        /// 单个删除
        /// </summary>
        /// <param name="id"></param>
        /// <param name="routing"></param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> DeleteAsync<TPrimaryKeyType>(DeleteSingleModel<TPrimaryKeyType> deleteSingleModel);



        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entities">实体</param>
        /// <param name="routing">路由</param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> DeleteBatchAsync<TPrimaryKeyType>(DeleteBatchModel<TPrimaryKeyType> idList);


        /// <summary>
        /// 根据id更新
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="entity">实体</param>
        /// <param name="routing">路由</param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> UpdateAsync<T, TPrimaryKeyType>(TPrimaryKeyType id, InsertUpdateModel<T> entity) where T : class, new();

        /// <summary>
        /// 根据id更新部分实体字段
        /// </summary>
        /// <typeparam name="TPartialDocument">包含实体T中部分字段的是model</typeparam>
        /// <param name="id">主键</param>
        /// <param name="partEntity">部分实体model</param>
        /// <param name="routing">路由键</param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> UpdatePartialDocumentAsync<TPartialDocument, TPrimaryKeyType>(TPrimaryKeyType id, InsertUpdateModel<TPartialDocument> partEntity) where TPartialDocument : class, new();



        /// <summary>
        /// 简单查询（返回实体）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        Task<List<T>> SimpleSqlQueryAsync<T>(SimpleSQLQueryModel queryModel) where T : class, new();

        


    }
}
