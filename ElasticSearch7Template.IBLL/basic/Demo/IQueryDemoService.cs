
 

using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticSearch7Template.Model;
using ElasticSearch7Template.Entity;
using ElasticSearch7Template.Core;
using ElasticSearch7Template.Model.Conditions;

namespace ElasticSearch7Template.IBLL
{
    public interface IQueryDemoService
    {
	    /// <summary>
        /// 查询数据(分页) 返回指定实体T
        /// </summary>
        ///<typeparam name="T">返回实体类型</typeparam>
        /// <param name="page">页码</param>
        /// <param name="size">每页数量</param>
        /// <param name="condition">查询条件类</param>
        /// <param name="field">返回字段</param>
        /// <param name="orderBy">排序字段</param>
        /// <returns></returns>
        Task<QueryPagedResponseModel<T>> GetListPagedAsync<T>(int page, int size, BaseDemoCondition condition, string field = null, string orderBy = null) where T : class, new() ;

       


        


        /// <summary>
        /// 根据id查询单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPrimaryKeyType"></typeparam>
        /// <param name="id"></param>
        /// <param name="indexName"></param>
        /// <param name="routing"></param>
        /// <returns></returns>
        Task<T> GetAsync<T>(QueryModel<long> queryModel) where T : class;


        /// <summary>
        /// 简单查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        Task<List<T>> SimpleSqlQueryAsync<T>(SimpleSQLQueryModel queryModel) where T : class, new();

        /// <summary>
        /// 查询（返回json字符串）
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        Task<string> SqlQueryToJsonAsync(SimpleSQLQueryModel queryModel);
    }
}
