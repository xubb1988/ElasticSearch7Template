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
    /// 模板通用接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Template")]
    [Route("BaseTemplate")]
    [ApiController]
    [ModelValidation]
    public class BaseTemplateController : ControllerBase
    {
        private readonly IIndexTemplateBaseService indexTemplateBaseService;
        public BaseTemplateController(IIndexTemplateBaseService indexTemplateBaseService)
        {
            this.indexTemplateBaseService = indexTemplateBaseService;

        }

        /// <summary>
        ///   删除索引模板(对应索引的模板，会将所有应用到这个模板的都删除索引模板都删除)
        /// </summary>
        /// <param name="templateName">模板名称</param>
        /// <returns></returns>
        [Route("DeleteTemplate")]
        [HttpPost]
        [ProducesResponseType(typeof(HttpResponseResultModel<bool>), 200)]
        public async Task<IActionResult> DeleteTemplateAsync(string templateName)
        {
            if (string.IsNullOrEmpty(templateName))
            {
                return BadRequest(MessageFactory.CreateParamsIsNullMessage());
            }
            var result = await indexTemplateBaseService.DeleteTemplateAsync(templateName).ConfigureAwait(false);
            return Ok(result);
        }


        /// <summary>
        ///   获取模板信息
        /// </summary>
        /// <param name="templateName">模板名称</param>
        /// <returns></returns>
        [Route("GetTemplate")]
        [HttpGet]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> GetTemplateAsync(string templateName)
        {
            if (string.IsNullOrEmpty(templateName))
            {
                return BadRequest(MessageFactory.CreateParamsIsNullMessage());
            }
            var result = await indexTemplateBaseService.GetTemplateAsync(templateName).ConfigureAwait(false);
            return Ok(result);
        }
    }
}