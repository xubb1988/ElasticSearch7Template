using ElasticSearch7Template.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch7Template.IBLL
{
    public interface IIndexFileBaseService
    {
        /// <summary>
        /// 创建索引文件((默认创建ElasticsearchIndex特性上对应的索引)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="indexName"></param>
        /// <param name="indexSettings"></param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> CreateIndexFileAsync<T>(string indexName = "", ESIndexSettingsModel indexSettings = null) where T : class, new();






        /// <summary>
        /// 关闭指定的索引文件(默认关闭ElasticsearchIndex特性上对应的索引)
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> CloseIndexFileAsync(string indexName = "");

        /// <summary>
        /// 打开指定的索引文件(默认打开ElasticsearchIndex特性上对应的索引)
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> OpenIndexFileAsync(string indexName = "");

        /// <summary>
        /// 删除指定的索引文件(默认删除ElasticsearchIndex特性上对应的索引)
        /// </summary>
        /// <param name="indexName"></param>
        /// <returns></returns>
        Task<HttpResponseResultModel<bool>> DeleteIndexFileAsync(string indexName = "");



        /// <summary>
        /// 获取所有的索引
        /// </summary>
        /// <returns></returns>
        Task<List<string>> CatAllIndexAsync();

        /// <summary>
        /// 根据别名获得索引名称
        /// </summary>
        /// <param name="aliasName"></param>
        /// <returns></returns>
        Task<List<string>> CatAliasesAsync(string aliasName);
    }
}
