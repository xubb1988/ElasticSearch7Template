

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Expressions;
using System;
using ElasticSearch7Template.Model;
using ElasticSearch7Template.IDAL;
using ElasticSearch7Template.Entity;
using ElasticSearch7Template.Utility;
using ElasticSearch7Template.Model.Conditions;
using ElasticSearch7Template.Core;
using ElasticSearch7Template.IBLL;
namespace ElasticSearch7Template.BLL
{
    internal class QueryDemoServiceImpl : IQueryDemoService, IAutoInject
    {
        private readonly IDemoRepository repository;

        public QueryDemoServiceImpl(IDemoRepository repository)
        {
            this.repository = repository;
        }



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
        public async Task<QueryPagedResponseModel<T>> GetListPagedAsync<T>(int page, int size, BaseDemoCondition condition, string field = null, string orderBy = null) where T : class, new()
        {
            return await repository.GetListPagedAsync<T>(page, size, condition, field, orderBy).ConfigureAwait(false);
        }




        /// <summary>
        /// 根据id查询单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPrimaryKeyType"></typeparam>
        /// <param name="id"></param>
        /// <param name="indexName"></param>
        /// <param name="routing"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(QueryModel<long> queryModel) where T : class
        {
            return await repository.GetAsync<T, long>(queryModel).ConfigureAwait(false);
        }


        /// <summary>
        /// 简单查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public async Task<List<T>> SimpleSqlQueryAsync<T>(SimpleSQLQueryModel queryModel) where T : class, new()
        {
            return await repository.SimpleSqlQueryAsync<T>(queryModel).ConfigureAwait(false);
        }

        
    }
}
