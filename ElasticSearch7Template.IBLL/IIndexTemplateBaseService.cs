using ElasticSearch7Template.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch7Template.IBLL
{
    public interface IIndexTemplateBaseService
    {


        /// <summary>
        /// 创建模板
        /// </summary>
        /// <typeparam name="T">索引实体</typeparam>
        /// <param name="indexSettings"></param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> CreateTemplateAsync<T>(ESIndexSettingsModel indexSettings = null) where T : class, new();



        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="templateName">模板名称</param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> DeleteTemplateAsync(string templateName);

        /// <summary>
        /// 修改模板（先删除新增）
        /// </summary>
        /// <param name="templateName">模板名称名称</param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> PutTemplateAsync<T>(string templateName) where T : class, new();

        /// <summary>
        ///   获得模板信息
        /// </summary>
        /// <param name="templateName">模板名称名称</param>
        /// <returns></returns>
        Task<string> GetTemplateAsync(string templateName);

    }
}
