


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
    /// 索引文件接口
    /// </summary>
	[ApiExplorerSettings(GroupName = "Index")]
    [Route("DemoIndex")]
    [ApiController]
    [ModelValidation]
    public class DemoIndexController : ControllerBase
    {

        private readonly IIndexFileBaseService indexFileBaseService;

        public DemoIndexController(IIndexFileBaseService indexFileBaseService)
        {
            this.indexFileBaseService = indexFileBaseService;
        }

        /// <summary>
        ///  创建索引
        /// </summary>
        /// <param name="indexName">索引名称，默认为空,对应实体</param>
        /// <returns></returns>
        [Route("CreateIndexFile")]
        [HttpPost]
        [ProducesResponseType(typeof(HttpResponseResultModel<bool>), 200)]
        public async Task<IActionResult> CreateIndexFileAsync(string indexName = "")
        {
            var result = await indexFileBaseService.CreateIndexFileAsync<DemoEntity>(indexName).ConfigureAwait(false);
            return Ok(result);
        }
    }
}