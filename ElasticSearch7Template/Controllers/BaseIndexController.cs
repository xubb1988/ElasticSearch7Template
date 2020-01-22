using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElasticSearch7Template.Core;
using ElasticSearch7Template.Filters;
using ElasticSearch7Template.IBLL;
using ElasticSearch7Template.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ElasticSearch7Template.Controllers
{
    /// <summary>
    /// 索引文件通用接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Index")]
    [Route("BaseIndex")]
    [ApiController]
    [ModelValidation]
    public class BaseIndexController : ControllerBase
    {
        private readonly IIndexFileBaseService indexFileBaseService;

        public BaseIndexController(IIndexFileBaseService indexFileBaseService)
        {
            this.indexFileBaseService = indexFileBaseService;
        }

        /// <summary>
        /// 关闭索引文件((默认创建ElasticsearchIndex特性上对应的索引)
        /// </summary>
        /// <param name="indexName">索引名</param>
        /// <returns></returns>
        [Route("CloseIndexFile")]
        [HttpPost]
        [ProducesResponseType(typeof(HttpResponseResultModel<bool>), 200)]
        public async Task<IActionResult> CloseIndexFileAsync(string indexName)
        {
            var result = await indexFileBaseService.CloseIndexFileAsync(indexName).ConfigureAwait(false);
            return Ok(result);
        }

        /// <summary>
        /// 打开索引文件((默认创建ElasticsearchIndex特性上对应的索引)
        /// </summary>
        /// <param name="indexName">索引名</param>
        /// <returns></returns>
        [Route("OpenIndexFile")]
        [HttpPost]
        [ProducesResponseType(typeof(HttpResponseResultModel<bool>), 200)]
        public async Task<IActionResult> OpenIndexFileAsync(string indexName)
        {
            if (string.IsNullOrEmpty(indexName))
            {
                return BadRequest(MessageFactory.CreateParamsIsNullMessage());
            }
            var result = await indexFileBaseService.OpenIndexFileAsync(indexName).ConfigureAwait(false);
            return Ok(result);
        }

        /// <summary>
        /// 删除索引文件((默认创建ElasticsearchIndex特性上对应的索引)
        /// </summary>
        /// <param name="indexName">索引名</param>
        /// <returns></returns>
        [Route("DeleteIndexFile")]
        [HttpDelete]
        [ProducesResponseType(typeof(HttpResponseResultModel<bool>), 200)]
        public async Task<IActionResult> DeleteIndexFileAsync(string indexName)
        {
            if (string.IsNullOrEmpty(indexName))
            {
                return BadRequest(MessageFactory.CreateParamsIsNullMessage());
            }
            var result = await indexFileBaseService.DeleteIndexFileAsync(indexName).ConfigureAwait(false);
            return Ok(result);
        }

        /// <summary>
        /// 获取所有的索引
        /// </summary>
        /// <returns></returns>
        [Route("CatAllIndex")]
        [HttpGet]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> CatAllIndexAsync()
        {

            var result = await indexFileBaseService.CatAllIndexAsync().ConfigureAwait(false);
            return Ok(result);
        }


        /// <summary>
        /// 根据别名获得索引名称
        /// </summary>
        /// <param name="aliasName"></param>
        /// <returns></returns>
        [Route("CatAliases")]
        [HttpGet]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> CatAliasesAsync(string aliasName)
        {
            var result = await indexFileBaseService.CatAliasesAsync(aliasName).ConfigureAwait(false);
            return Ok(result);
        }
    }
}