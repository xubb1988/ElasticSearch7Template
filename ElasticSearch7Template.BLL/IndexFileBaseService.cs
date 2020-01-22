using ElasticSearch7Template.Core;
using ElasticSearch7Template.IBLL;
using ElasticSearch7Template.IDAL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HSJM.Service.ElasticSearch.BLL
{
    public class IndexFileBaseService : IIndexFileBaseService, IAutoInject
    {
        private readonly IIndexFileRepository indexFileRepository;
        public IndexFileBaseService(IIndexFileRepository indexFileRepository)
        {
            this.indexFileRepository = indexFileRepository;
        }



        /// <summary>
        /// 创建索引文件((默认创建ElasticsearchIndex特性上对应的索引)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="indexSettings"></param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> CreateIndexFileAsync<T>(string indexName = "", ESIndexSettingsModel indexSettings = null) where T : class, new()
        {
            return await indexFileRepository.CreateIndexFileAsync<T>(indexName, indexSettings).ConfigureAwait(false);

        }


        /// <summary>
        /// 关闭索引文件((默认创建ElasticsearchIndex特性上对应的索引)
        /// </summary>
        /// <param name="indexName">索引名</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> CloseIndexFileAsync(string indexName = "")
        {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool>();
            var isSuccess = await indexFileRepository.CloseIndexFileAsync(indexName).ConfigureAwait(false);
            httpResponseResultModel.BackResult = isSuccess;
            httpResponseResultModel.IsSuccess = isSuccess;
            return httpResponseResultModel;
        }


        /// <summary>
        /// 打开索引文件((默认创建ElasticsearchIndex特性上对应的索引)
        /// </summary>
        /// <param name="indexName">索引名</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> OpenIndexFileAsync(string indexName = "")
        {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool>();
            var isSuccess = await indexFileRepository.OpenIndexFileAsync(indexName).ConfigureAwait(false);
            httpResponseResultModel.BackResult = isSuccess;
            httpResponseResultModel.IsSuccess = isSuccess;
            return httpResponseResultModel;

        }


        /// <summary>
        /// 删除索引文件((默认创建ElasticsearchIndex特性上对应的索引)
        /// </summary>
        /// <param name="indexName">索引名</param>
        /// <returns></returns>
        public async Task<HttpResponseResultModel<bool>> DeleteIndexFileAsync(string indexName = "")
        {
            HttpResponseResultModel<bool> httpResponseResultModel = new HttpResponseResultModel<bool>();
            var isSuccess = await indexFileRepository.DeleteIndexFileAsync(indexName).ConfigureAwait(false);
            httpResponseResultModel.BackResult = isSuccess;
            httpResponseResultModel.IsSuccess = isSuccess;
            return httpResponseResultModel;
        }


        /// <summary>
        /// 获取所有的索引
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> CatAllIndexAsync()
        {
            return await indexFileRepository.CatAllIndexAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// 根据别名获得索引名称
        /// </summary>
        /// <param name="aliasName"></param>
        /// <returns></returns>
        public async Task<List<string>> CatAliasesAsync(string aliasName)
        {
            return await indexFileRepository.CatAliasesAsync(aliasName).ConfigureAwait(false);
        }

    }
}
