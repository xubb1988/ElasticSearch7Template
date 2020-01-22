using ElasticSearch7Template.Core;
using ElasticSearch7Template.IDAL;
using ElasticSearch7Template.Utility.ESHelper;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch7Template.DAL
{
    internal class IndexFileRepository : IIndexFileRepository, IAutoInject
    {

        protected IElasticClient client;
        protected static ElasticSearchPocoData PocoData { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected string PocoAlias { get; set; }

        /// <summary>
        /// 索引名
        /// </summary>
        protected string PocoIndexName { get; set; }


        /// <summary>
        /// 
        /// </summary>
        protected string PocoTemplate { get; set; }

        public IndexFileRepository()
        {
            client = new ElasticSearchConfiguration("").GetClient();
        }

        private void Init<T>()
        {
            PocoData = new ElasticSearchPocoDataFactory().ForType(typeof(T));
            PocoIndexName = PocoData.DefaultIndexName.ToLower() + "_" + DateTime.Now.Year.ToString();
            PocoAlias = PocoData.Alias;
            PocoTemplate = PocoData.DefaultTemplateName;
        }
        #region 操作索引

        /// <summary>
        /// 创建索引文件((默认创建ElasticsearchIndex特性上对应的索引)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="indexSettings"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> CreateIndexFileAsync<T>(string indexName = "", ESIndexSettingsModel indexSettings = null) where T : class, new()
        {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool>();
            Init<T>();
            if (string.IsNullOrEmpty(indexName))
            {
                indexName = PocoIndexName;
            }
            if (indexSettings == null)
            {
                indexSettings = new ESIndexSettingsModel();
            }
            IIndexState indexState = new IndexState()
            {
                Settings = new IndexSettings()
                {
                    NumberOfShards = indexSettings.NumberOfShards,//分片数, 
                    NumberOfReplicas = indexSettings.NumberOfReplicas,//副本数
                }
            };
            bool isIndexExists = client.Indices.Exists(indexName.ToLower()).Exists;
            if (!isIndexExists)
            {
                var isExistTemplateResponse = await client.Indices.TemplateExistsAsync(PocoTemplate);

                if (isExistTemplateResponse.Exists)
                {
                    //创建并Mapping
                    var result = await client.Indices.CreateAsync(indexName.ToLower(), p => p.InitializeUsing(indexState)
                      .Map(mp => mp.AutoMap())
                      .IncludeTypeName(false)
                      .Aliases(a => a.Alias(PocoAlias))
                      ).ConfigureAwait(false);
                    //a => a.Add(add => add.Index(indexName.ToLower()).Alias(PocoAlias).IndexRouting(indexName.ToLower()))
                    var aliasResult = await client.Indices.PutAliasAsync(indexName.ToLower(), PocoAlias, t => t.IndexRouting(indexName.ToLower()));
                    bool isSuccess = result.Acknowledged && aliasResult.IsValid;
                    httpResponseResultModel.IsSuccess = isSuccess;
                    return httpResponseResultModel;
                }
                httpResponseResultModel.ErrorMessage = "没有模板请先创建模板";
                return httpResponseResultModel;
            }
            else
            {
                httpResponseResultModel.ErrorMessage = "索引已存在";
                return httpResponseResultModel;
            }
        }




        /// <summary>
        /// 关闭指定的索引文件(默认关闭ElasticsearchIndex特性上对应的索引)
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public async Task<bool> CloseIndexFileAsync(string indexName)
        {
            var isIndexExists = client.Indices.Exists(indexName).Exists;
            if (isIndexExists)
            {
                var result = await client.Indices.CloseAsync(indexName).ConfigureAwait(false);
                return result.IsValid;
            }
            return true;
        }


        /// <summary>
        /// 打开指定的索引文件(默认打开ElasticsearchIndex特性上对应的索引)
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public async Task<bool> OpenIndexFileAsync(string indexName)
        {

            var isIndexExists = client.Indices.Exists(indexName).Exists;
            if (isIndexExists)
            {
                var result = await client.Indices.OpenAsync(indexName).ConfigureAwait(false);
                return result.IsValid;
            }
            return true;
        }


        /// <summary>
        /// 删除指定的索引文件(默认删除ElasticsearchIndex特性上对应的索引)
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        public async Task<bool> DeleteIndexFileAsync(string indexName)
        {
            var isIndexExists = client.Indices.Exists(indexName).Exists;
            if (isIndexExists)
            {
                var result = await client.Indices.DeleteAsync(indexName).ConfigureAwait(false);
                return result.IsValid;
            }
            return true;
        }


        /// <summary>
        /// 获取所有的索引
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> CatAllIndexAsync()
        {
            var list = await client.Cat.AliasesAsync().ConfigureAwait(false);
            if (list.IsValid)
            {
                var allIndexNameList = list.Records.Select(t => t.Index).ToList();
                return allIndexNameList;
            }
            return new List<string>();
        }

        /// <summary>
        /// 根据别名获得索引名称
        /// </summary>
        /// <param name="aliasName"></param>
        /// <returns></returns>
        public async Task<List<string>> CatAliasesAsync(string aliasName)
        {

            CatAliasesDescriptor catAliasesDescriptor = new CatAliasesDescriptor();
            catAliasesDescriptor.Name(aliasName);
            var list = await client.Cat.AliasesAsync(catAliasesDescriptor).ConfigureAwait(false);
            if (list.IsValid)
            {
                var allIndexNameList = list.Records.Select(t => t.Index).ToList();
                return allIndexNameList;
            }
            return new List<string>();
        }

        #endregion
    }
}
