

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System;
using ElasticSearch7Template.Model;
using ElasticSearch7Template.IDAL;
using ElasticSearch7Template.Entity;
using ElasticSearch7Template.Utility;
using ElasticSearch7Template.Model.Conditions;
using ElasticSearch7Template.Core;
using ElasticSearch7Template.IBLL;
namespace ElasticSearch7Template.BLL
{
    internal class CommandDemoServiceImpl : ICommandDemoService, IAutoInject
    {
        private readonly IDemoRepository repository;
        private readonly IQueryDemoService queryDemoService;

        public CommandDemoServiceImpl(IDemoRepository repository, IQueryDemoService queryDemoService)
        {
            this.repository = repository;
            this.queryDemoService = queryDemoService;
        }

        #region 插入


        /// <summary>
        /// 插入单个实体
        /// </summary>
        /// <param name="insertUpdateModel"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> IndexAsync(InsertUpdateModel<DemoEntity> insertUpdateModel)
        {
            return await repository.IndexAsync(insertUpdateModel).ConfigureAwait(false);
        }


        /// <summary>
        /// 批量插入实体
        /// </summary>
        /// <param name="insertUpdateModel"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> IndexBatchAsync(InsertUpdateModel<IList<DemoEntity>> insertUpdateModel)
        {
            return await repository.IndexBatchAsync(insertUpdateModel).ConfigureAwait(false);
        }


        #endregion

        #region 更新
        /// <summary>
        /// 根据主键更新实体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="insertUpdateModel"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> UpdateAsync(long id, InsertUpdateModel<DemoEntity> insertUpdateModel)
        {
            return await repository.UpdateAsync(id, insertUpdateModel).ConfigureAwait(false);
        }


        /// <summary>
        /// 根据id更新部分实体字段
        /// </summary>
        /// <typeparam name="TPartialDocument">包含实体T中部分字段的是model</typeparam>
        /// <param name="id">主键</param>
        /// <param name="insertUpdateModel"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> UpdatePartialDocumentAsync(long id, InsertUpdateModel<PartOfDemoEntity> insertUpdateModel)
        {
            return await repository.UpdatePartialDocumentAsync(id, insertUpdateModel).ConfigureAwait(false);

        }






        #endregion

        #region 删除

        /// <summary>
        /// 根据根据主键删除
        /// </summary>
        /// <param name="deleteSingleModel"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> DeleteAsync(DeleteSingleModel<long> deleteSingleModel)
        {

            return await repository.DeleteAsync(deleteSingleModel).ConfigureAwait(false);

        }

        /// <summary>
        /// 批量删除 根据主键
        /// </summary>
        /// <param name="deleteBatchModel"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> DeleteBatchAsync(DeleteBatchModel<long> deleteBatchModel)
        {
            return await repository.DeleteBatchAsync(deleteBatchModel).ConfigureAwait(false);

        }


        #endregion


    }
}
