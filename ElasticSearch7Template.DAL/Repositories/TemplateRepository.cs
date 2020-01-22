using ElasticSearch7Template.Core;
using ElasticSearch7Template.IDAL;
using ElasticSearch7Template.Utility;
using ElasticSearch7Template.Utility.ESHelper;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch7Template.DAL
{
    internal class TemplateRepository : ITemplateRepository, IAutoInject
    {
        protected IElasticClient client;
        protected static ElasticSearchPocoData PocoData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected string PocoAlias { get; set; }


        /// <summary>
        /// 
        /// </summary>
        protected string PocoTemplate { get; set; }

        public TemplateRepository()
        {
            client = new ElasticSearchConfiguration("").GetClient();
        }

        private void Init<T>()
        {
            PocoData = new ElasticSearchPocoDataFactory().ForType(typeof(T));
            PocoAlias = PocoData.Alias;
            PocoTemplate = PocoData.DefaultTemplateName;
        }


        /// <summary>
        /// 创建模板
        /// </summary>
        /// <typeparam name="T">索引实体</typeparam>
        /// <param name="indexSettings"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> CreateTemplateAsync<T>(ESIndexSettingsModel indexSettings = null) where T : class, new()
        {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool>();
            Init<T>();
            if (indexSettings == null)
            {
                indexSettings = new ESIndexSettingsModel();
            }
            #region 创建模板
            var index_patterns = PocoData.DefaultIndexName.ToLower();
            int numberOfReplicas = indexSettings.NumberOfReplicas;
            int numberOfShards = indexSettings.NumberOfShards;
            TimeSpan refreshInterval = new TimeSpan(0, 0, 0, indexSettings.RefreshInterval);
            string templateName = PocoTemplate;
            //设置模板
            //.Setting("index.lifecycle.name", "my_policy")
            //  await _client.IndexLifecycleManagement.PutLifecycleAsync("w", t => t.Policy(p => p.Phases(p1 => p1.Warm(w => w.MinimumAge("60d")))));
            var templateResponse = await client.Indices.PutTemplateAsync(templateName, template => template.Create(true).Map(m => m.AutoMap<T>()).IncludeTypeName(false)
            .Settings(setting => setting.NumberOfReplicas(numberOfReplicas).NumberOfShards(numberOfShards).RefreshInterval(refreshInterval))
               .Aliases(a => a.Alias(PocoAlias))
               .IndexPatterns(index_patterns + "_*")
               .IncludeTypeName(false)
              );
            bool isTemplateExist = templateResponse.Acknowledged;
            httpResponseResultModel.IsSuccess = isTemplateExist;
            if (!isTemplateExist)
            {
                httpResponseResultModel.ErrorMessage = "创建模板失败";
            }
            return httpResponseResultModel;
            #endregion

        }


        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="templateName">模板名称名称</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> DeleteTemplateAsync(string templateName)
        {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool>();
            var indexTemplateResponse = await client.Indices.TemplateExistsAsync(templateName);
            if (indexTemplateResponse.Exists)
            {
                //删除模板
                var templateResponse = await client.Indices.DeleteTemplateAsync(templateName);
                var isTemplateExist = templateResponse.Acknowledged;
                httpResponseResultModel.IsSuccess = isTemplateExist;
                if (!isTemplateExist)
                {
                    httpResponseResultModel.ErrorMessage = "删除模板失败";
                }
            }
            else
            {
                httpResponseResultModel.IsSuccess = true;
                httpResponseResultModel.ErrorMessage = "模板不存在";
            }
            return httpResponseResultModel;
        }


        /// <summary>
        /// 修改模板（先删除新增）
        /// </summary>
        /// <param name="templateName">模板名称名称</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> PutTemplateAsync<T>(string templateName) where T : class, new()
        {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool>();
            Init<T>();
            var indexTemplateResponse = await client.Indices.TemplateExistsAsync(templateName);
            if (indexTemplateResponse.Exists)
            {
                httpResponseResultModel = await DeleteTemplateAsync(templateName);
                if (httpResponseResultModel.IsSuccess)
                {
                    var indexSettings = new ESIndexSettingsModel();
                    httpResponseResultModel = await CreateTemplateAsync<T>(indexSettings);
                }
            }
            else
            {
                httpResponseResultModel.IsSuccess = false;
                httpResponseResultModel.ErrorMessage = "模板不存在";
            }
            return httpResponseResultModel;
        }

        /// <summary>
        ///   获得模板信息
        /// </summary>
        /// <param name="templateName">模板名称名称</param>
        /// <returns></returns>
        public async Task<string> GetTemplateAsync(string templateName)
        {

            var indexTemplateResponse = await client.Indices.TemplateExistsAsync(templateName);
            if (!indexTemplateResponse.Exists)
            {
                return null;
            }
            var templateInfo = await client.Indices.GetTemplateAsync(templateName);
            var json = JsonHelper.SerializeObject(templateInfo.TemplateMappings);
            return json;
        }
    }
}
