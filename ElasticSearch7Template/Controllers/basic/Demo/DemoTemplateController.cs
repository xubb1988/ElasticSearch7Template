


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
    /// 索引模板接口
    /// </summary>
	[ApiExplorerSettings(GroupName = "Template")]
    [Route("DemoTemplate")]
    [ApiController]
    [ModelValidation]
    public class DemoTemplateController : ControllerBase
    {


        private readonly IIndexTemplateBaseService indexTemplateBaseService;
        public DemoTemplateController(IIndexTemplateBaseService indexTemplateBaseService)
        {
            this.indexTemplateBaseService = indexTemplateBaseService;

        }

        /// <summary>
        ///  创建索引模板（该类型下的所有索引的统一模板）
        /// </summary>
        /// <param name="indexSettings">索引配置项</param>
        /// <returns></returns>
        [Route("CreateTemplate")]
        [HttpPost]
        [ProducesResponseType(typeof(HttpResponseResultModel<bool>), 200)]
        public async Task<IActionResult> CreateTemplateAsync(ESIndexSettingsModel indexSettings = null)
        {
            var result = await indexTemplateBaseService.CreateTemplateAsync<DemoEntity>(indexSettings).ConfigureAwait(false);
            return Ok(result);
        }

        /// <summary>
        ///   修改索引模板(对应索引的模板，先删除在重新创建)
        /// </summary>
        /// <param name="templateName">模板名称</param>
        /// <returns></returns>
        [Route("PutTemplate")]
        [HttpPost]
        [ProducesResponseType(typeof(HttpResponseResultModel<bool>), 200)]
        public async Task<IActionResult> PutTemplateAsync(string templateName)
        {
            if (string.IsNullOrEmpty(templateName))
            {
                return BadRequest(MessageFactory.CreateParamsIsNullMessage());
            }
            var result = await indexTemplateBaseService.PutTemplateAsync<DemoEntity>(templateName).ConfigureAwait(false);
            return Ok(result);
        }

    }
}