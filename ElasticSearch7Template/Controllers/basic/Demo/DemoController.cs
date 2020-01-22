


using ElasticSearch7Template.Entity;
using ElasticSearch7Template.IBLL;
using ElasticSearch7Template.Model;
using ElasticSearch7Template.Model.Conditions;
using ElasticSearch7Template.Utility;
using ElasticSearch7Template.Filters;
using ElasticSearch7Template.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearch7Template.Controllers
{
    /// <summary>
    /// 接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Basic")]
    [Route("Demo")]
    [ApiController]
    [ModelValidation]
    public class DemoController : ControllerBase
    {
        private readonly ICommandDemoService commandDemoService;
        private readonly IQueryDemoService queryDemoService;

        public DemoController(ICommandDemoService commandDemoService, IQueryDemoService queryDemoService)
        {
            this.commandDemoService = commandDemoService;
            this.queryDemoService = queryDemoService;

        }




        /// <summary>
        ///  根据id查询单条数据(知道具体索引)
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [Route("Get")]
        [ProducesResponseType(typeof(DemoEntity), 200)]
        [HttpGet]
        public async Task<ActionResult> GetAsync([FromQuery]QueryModel<long> queryModel)
        {
            var entity = await queryDemoService.GetAsync<DemoEntity>(queryModel).ConfigureAwait(false);
            return Ok(entity);
        }

        /// <summary>
        /// 插入新实体
        /// </summary>
        /// <param name="insertUpdateModel"></param>
        /// <returns></returns>
        [Route("insert")]
        [HttpPost]
        [ProducesResponseType(typeof(HttpResponseResultModel<bool>), 200)]
        public async Task<ActionResult> IndexAsync([FromBody]InsertUpdateModel<DemoEntity> insertUpdateModel)
        {
            if (insertUpdateModel == null)
            {
                return BadRequest(MessageFactory.CreateParamsIsNullMessage());
            }
            var result = await commandDemoService.IndexAsync(insertUpdateModel).ConfigureAwait(false);

            return Ok(result);
        }


        /// <summary>
        /// 批量插入新实体
        /// </summary>
        /// <param name="insertUpdateModel"></param>
        /// <returns></returns>
        [Route("InsertBatch")]
        [HttpPost]
        [ProducesResponseType(typeof(HttpResponseResultModel<bool>), 200)]
        public async Task<ActionResult> IndexBatchAsync([FromBody]InsertUpdateModel<IList<DemoEntity>> insertUpdateModel)
        {
            if (insertUpdateModel == null)
            {
                return BadRequest(MessageFactory.CreateParamsIsNullMessage());
            }

            var result = await commandDemoService.IndexBatchAsync(insertUpdateModel).ConfigureAwait(false);

            return Ok(result);
        }


        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="insertUpdateModel"></param>
        /// <returns></returns>
        [Route("UpdateById/{id}")]
        [ProducesResponseType(typeof(HttpResponseResultModel<bool>), 200)]
        [HttpPost]
        public async Task<ActionResult> UpdateByIdAsync(long id, [FromBody]InsertUpdateModel<DemoEntity> insertUpdateModel)
        {
            if (insertUpdateModel == null)
            {
                return BadRequest(MessageFactory.CreateParamsIsNullMessage());
            }
            insertUpdateModel.Entity.Id = id;
            var result = await commandDemoService.UpdateAsync(id, insertUpdateModel).ConfigureAwait(false);

            return Ok(result);
        }

        /// <summary>
        /// 更新部分实体
        /// </summary>
        /// <param name="id">主键id</param>
        /// <param name="insertUpdateModel"></param>
        /// <returns></returns>
        [Route("UpdatePart/{id}")]
        [ProducesResponseType(typeof(HttpResponseResultModel<bool>), 200)]
        [HttpPost]
        public async Task<ActionResult> UpdatePartialDocumentAsync(long id, [FromBody]InsertUpdateModel<PartOfDemoEntity> insertUpdateModel)
        {
            if (insertUpdateModel == null)
            {
                return BadRequest(MessageFactory.CreateParamsIsNullMessage());
            }
            insertUpdateModel.Entity.Id = id;
            var result = await commandDemoService.UpdatePartialDocumentAsync(id, insertUpdateModel).ConfigureAwait(false);

            return Ok(result);
        }


        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="deleteSingleModel"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpDelete]
        [ProducesResponseType(typeof(HttpResponseResultModel<bool>), 200)]
        public async Task<ActionResult> DeleteAsync([FromBody]DeleteSingleModel<long> deleteSingleModel)
        {
            if (deleteSingleModel == null)
            {
                return BadRequest(MessageFactory.CreateParamsIsNullMessage());
            }
            var result = await commandDemoService.DeleteAsync(deleteSingleModel).ConfigureAwait(false);
            return Ok(result);
        }


        /// <summary>
        /// 批量删除实体
        /// </summary>
        /// <param name="deleteBatchModel"></param>
        /// <returns></returns>
        [Route("DeleteBatch")]
        [HttpDelete]
        [ProducesResponseType(typeof(HttpResponseResultModel<bool>), 200)]
        public async Task<ActionResult> DeleteBatchAsync([FromBody]DeleteBatchModel<long> deleteBatchModel)
        {
            if (deleteBatchModel == null)
            {
                return BadRequest(MessageFactory.CreateParamsIsNullMessage());
            }
            var result = await commandDemoService.DeleteBatchAsync(deleteBatchModel).ConfigureAwait(false);
            return Ok(result);
        }

        /// <summary>
        /// 查询数据(分页) 返回表实体
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="limit">每页数量</param>
        /// <param name="condition">查询条件类</param>
        /// <returns></returns>
        [Route("GetListPaged")]
        [ProducesResponseType(typeof(QueryPagedResponseModel<DemoEntity>), 200)]
        [HttpGet]
        public async Task<ActionResult> GetListPagedAsync(int page = 1, int limit = 10, [FromQuery]BaseDemoCondition condition = null)
        {

            var queryPagedResponse = await queryDemoService.GetListPagedAsync<DemoEntity>(page, limit, condition, null, "id desc").ConfigureAwait(false);
            return Ok(queryPagedResponse);
        }

        /// <summary>
        /// 简单sql查询（es sql语句,不支持复杂的入Object,Nest等，复杂的请注意调试）
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [Route("SimpleSqlQuery")]
        [ProducesResponseType(typeof(QueryPagedResponseModel<DemoEntity>), 200)]
        [HttpGet]
        public async Task<ActionResult> SimpleSqlQueryAsync([FromQuery]SimpleSQLQueryModel queryModel)
        {
            var queryResponse = await queryDemoService.SimpleSqlQueryAsync<DemoEntity>(queryModel).ConfigureAwait(false);
            return Ok(queryResponse);
        }

         

    }
}