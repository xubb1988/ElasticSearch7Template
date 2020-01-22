

using System.Collections.Generic;
using System.Threading.Tasks;
using ElasticSearch7Template.Model;
using ElasticSearch7Template.Entity;
using ElasticSearch7Template.Model.Conditions;
using ElasticSearch7Template.Core;

namespace ElasticSearch7Template.IDAL
{
    public interface IDemoRepository : IBaseRepository
    {
       
            /// <summary>
        /// 查询分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="condition"></param>
        /// <param name="field"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        Task<QueryPagedResponseModel<T>> GetListPagedAsync<T>(int page, int size, BaseDemoCondition condition = null, string field = null, string orderBy = null)where T : class, new();


     
    }
}
