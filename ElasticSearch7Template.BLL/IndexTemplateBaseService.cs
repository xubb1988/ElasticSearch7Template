using ElasticSearch7Template.Core;
using ElasticSearch7Template.IBLL;
using ElasticSearch7Template.IDAL;
using System;
using System.Threading.Tasks;

namespace HSJM.Service.ElasticSearch.BLL
{
    public class IndexTemplateBaseService : IIndexTemplateBaseService, IAutoInject
    {

        private readonly ITemplateRepository templateRepository;
        public IndexTemplateBaseService(ITemplateRepository templateRepository)
        {
            this.templateRepository = templateRepository;
        }
        /// <summary>
        /// 创建模板
        /// </summary>
        /// <typeparam name="T">索引实体</typeparam>
        /// <param name="indexSettings"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> CreateTemplateAsync<T>(ESIndexSettingsModel indexSettings = null) where T : class, new()
        {

            return await templateRepository.CreateTemplateAsync<T>(indexSettings).ConfigureAwait(false);
        }

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="templateName"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> DeleteTemplateAsync(string templateName)
        {
            return await templateRepository.DeleteTemplateAsync(templateName).ConfigureAwait(false);
        }

        /// <summary>
        /// 修改模板（先删除新增）
        /// </summary>
        /// <param name="templateName">模板名称名称</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> PutTemplateAsync<T>(string templateName) where T : class, new()
        {

            return await templateRepository.PutTemplateAsync<T>(templateName).ConfigureAwait(false);

        }


        /// <summary>
        ///   获得模板信息
        /// </summary>
        /// <param name="templateName">模板名称名称</param>
        /// <returns></returns>
        public async Task<string> GetTemplateAsync(string templateName)
        {
            return await templateRepository.GetTemplateAsync(templateName).ConfigureAwait(false);

        }
    }
}
