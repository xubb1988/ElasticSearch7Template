
 

using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticSearch7Template.Model;
using ElasticSearch7Template.Entity;
using ElasticSearch7Template.Core;
using System.Linq.Expressions;
using System;
namespace ElasticSearch7Template.IBLL
{
    public interface ICommandDemoService 
    {
	  /// <summary>
        /// 插入单个实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> IndexAsync(InsertUpdateModel<DemoEntity>  insertUpdateModel);


        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> IndexBatchAsync(InsertUpdateModel<IList<DemoEntity>> entity);


        /// <summary>
        /// 根据主键更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> UpdateAsync(long id, InsertUpdateModel<DemoEntity> entity);


        /// <summary>
        /// 根据id更新部分实体字段
        /// </summary>
        /// <typeparam name="TPartialDocument">包含实体T中部分字段的是model</typeparam>
        /// <param name="id">主键</param>
        /// <param name="insertUpdateModel"></param>
        /// <param name="routing">路由键</param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> UpdatePartialDocumentAsync(long id, InsertUpdateModel<PartOfDemoEntity>  insertUpdateModel);


        /// <summary>
        /// 根据根据主键删除
        /// </summary>
        /// <param name="int id">主键</param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> DeleteAsync(DeleteSingleModel<long>  deleteSingleModel);

        /// <summary>
        /// 批量删除 根据主键
        /// </summary>
        /// <param name="idList">主键集合</param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> DeleteBatchAsync(DeleteBatchModel<long>  deleteBatchModel);
    }
}
