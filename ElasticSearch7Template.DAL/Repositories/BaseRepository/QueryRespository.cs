using ElasticSearch7Template.Core;
using ElasticSearch7Template.Entity;
using ElasticSearch7Template.Utility;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearch7Template.DAL
{
    internal abstract partial class AbstractRepository<TEntity, TCondition> where TEntity : class where TCondition : ESBaseCondition
    {
        public abstract SearchRequest<T> TrunConditionToSql<T>(TCondition condition);

        /// <summary>
        /// 查看请求/响应日志
        /// </summary>
        /// <param name="searchResponse"></param>
        protected void GetDebugInfo(IResponse searchResponse)
        {
            if (ElasticSearchConfig.IsOpenDebugger.HasValue)
            {
                if (ElasticSearchConfig.IsOpenDebugger.Value)
                {
                    string requestStr = searchResponse.ApiCall.RequestBodyInBytes == null ? "" : Encoding.Default.GetString(searchResponse.ApiCall.RequestBodyInBytes);
                    string responseStr = searchResponse.ApiCall.ResponseBodyInBytes == null ? "" : Encoding.Default.GetString(searchResponse.ApiCall.ResponseBodyInBytes);
                }
            }
        }

        /// <summary>
        /// 根据id查询单条数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="routing"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T, TPrimaryKeyType>(TPrimaryKeyType id, string routing = "") where T : class
        {
            SearchRequest<T> searchRequest = new SearchRequest<T>();
            if (!string.IsNullOrEmpty(routing))
            {
                searchRequest.Routing = new Routing(routing);
            }
            var termQuery = new TermQuery { Field = PocoData.PrimaryKey, Value = id.ToString() };
            var query = new ConstantScoreQuery() { Filter = termQuery };
            searchRequest.Query = query;
            var response = await client.SearchAsync<T>(searchRequest).ConfigureAwait(false);
            GetDebugInfo(response);
            if (response.IsValid)
            {
                if (response.Documents.Any())
                {
                    return response.Documents.First();
                }
            }
            return default(T);
        }


        /// <summary>
        /// 根据id查询单条数据(知道具体索引)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TPrimaryKeyType"></typeparam>
        /// <param name="id"></param>
        /// <param name="indexName"></param>
        /// <param name="routing"></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T, TPrimaryKeyType>(QueryModel<TPrimaryKeyType> queryModel) where T : class
        {
            string indexName = queryModel.IndexName?.ToLower();
            if (!string.IsNullOrEmpty(indexName))
            {
                GetDescriptor<T> descriptor = new GetDescriptor<T>(indexName, queryModel.Id.ToString()); ;
                if (!string.IsNullOrEmpty(queryModel.Routing))
                {
                    descriptor.Routing(queryModel.Routing);
                }
                var response = await client.GetAsync<T>(descriptor).ConfigureAwait(false);

                if (response.Found)
                {
                    return response.Source;
                }
                return default(T);
            }
            else
            {
                //不知道indexName 另外一种方式查询
                return await GetAsync<T, TPrimaryKeyType>(queryModel.Id, queryModel.Routing);
            }
        }

        #region  SQL QUERY

        public async Task<List<T>> SimpleSqlQueryAsync<T>(SimpleSQLQueryModel queryModel) where T : class, new()
        {
            var result = await client.Sql.QueryAsync(t => t.FetchSize(queryModel.FetchSize).Format(queryModel.Format.ToString()).Query(queryModel.Sql)).ConfigureAwait(false);
            var rows = result.Rows.ToList();
            var colunms = result.Columns.ToList();
            List<T> entityList = new List<T>();
            GetDebugInfo(result);
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                T entity = new T();
                for (int j = 0; j < colunms.Count; j++)
                {
                    var column = colunms[j];
                    var val = row[j];
                    if (!column.Name.Contains("."))
                    {
                        var emitSetter = EmitHelper.EmitSetter<T>(column.Name);
                        emitSetter(entity, GetValue(column, val));
                    }
                    else
                    {
                        var type = typeof(T);
                        var pop = type.GetProperties(BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic);
                        for (int n = 0; n < pop.Length; n++)
                        {
                            string childClass = column.Name.Split('.')[0];
                            string childField = column.Name.Split('.')[1];
                            object childValue = GetValue(column, val);
                            if (pop[n].Name.ToLower() == childClass.ToLower())
                            {
                                var childType = pop[n].PropertyType;
                                var childEntityClass = childType.Assembly.CreateInstance(childType.FullName, true);
                                var popchilds = childType.GetProperties(BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic);
                                var popchild = popchilds.First(t => t.Name.ToLower() == childField.ToLower());
                                var emitSetter = EmitHelper.CreatePropertiesFunc(popchilds);
                                popchild.SetValue(childEntityClass, childValue);
                                pop[n].SetValue(entity, childEntityClass);
                            }
                        }

                    }
                }
                entityList.Add(entity);
            }
            return entityList;
        }

        /// <summary>
        /// 获得属性值
        /// </summary>
        /// <param name="column"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        private object GetValue(SqlColumn column, SqlValue val)
        {
            if (column.Type == "text" || column.Type == "keyword")
            {
                return val.As<string>();
            }
            else if (column.Type == "datetime")
            {
                return val.As<DateTime>();
            }
            else if (column.Type == "long")
            {
                return val.As<long>();
            }
            else if (column.Type == "integer")
            {
                return val.As<int>();
            }
            else if (column.Type == "float")
            {
                return val.As<float>();
            }
            else if (column.Type == "double")
            {
                return val.As<double>();
            }
            else if (column.Type == "boolean")
            {
                return val.As<bool>();
            }
            else
            {
                return val.As<string>();
            }
        }

        private void GetChildProperty(object entity,PropertyInfo[] propertyInfos, object property, SqlColumn column, SqlValue val)
        {
            for (int n = 0; n < propertyInfos.Length; n++)
            {
                //A.B.C 最后一个C是属性，前面的是类
                var classInfo = column.Name.Split('.');
                string childFirstClass = classInfo[0];
                string childField = classInfo[classInfo.Length - 1];
                object childValue = GetValue(column, val);
                if (classInfo.Length > 2)
                {
                    for (int m = 0; m < classInfo.Length - 1; m++)
                    {
                        string childClass1 = classInfo[m];
                        if (propertyInfos[n].Name.ToLower() == childClass1.ToLower())
                        {
                            var childType = propertyInfos[n].PropertyType;//myclass
                            var childEntityClass = childType.Assembly.CreateInstance(childType.FullName, true);
                            var popchilds = childType.GetProperties(BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic);
                            propertyInfos[n].SetValue(entity, childEntityClass);
                            GetChildProperty(childEntityClass, popchilds, property, column, val);
                        }

                    }
                }
                else
                {

                    if (propertyInfos[n].Name.ToLower() == childFirstClass.ToLower())
                    {
                        var childType = propertyInfos[n].PropertyType;
                        var childEntityClass = childType.Assembly.CreateInstance(childType.FullName, true);
                        var popchilds = childType.GetProperties(BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic);
                        var popchild = popchilds.First(t => t.Name.ToLower() == childField.ToLower());
                        var emitSetter = EmitHelper.CreatePropertiesFunc(popchilds);
                        popchild.SetValue(childEntityClass, childValue);
                        propertyInfos[n].SetValue(entity, childEntityClass);
                    }
                }
            }
        }

        private object CreateInstance()
        {
            return "";
        }

        public async Task<string> SqlQueryToJsonAsync(SimpleSQLQueryModel queryModel)
        {
            var result = await client.Sql.QueryAsync(t => t.FetchSize(queryModel.FetchSize).Format(queryModel.Format.ToString()).Query(queryModel.Sql)).ConfigureAwait(false);
            var rows = result.Rows.ToList();
            var colunms = result.Columns.ToList();
            var jsonString = new StringBuilder();
            GetDebugInfo(result);
            jsonString.Append("[");
            for (int i = 0; i < rows.Count; i++)
            {
                var row = rows[i];
                jsonString.Append("{");
                for (int j = 0; j < colunms.Count; j++)
                {
                    var column = colunms[j];

                    if (!column.Name.Contains("."))
                    {
                        var val = row[j];
                        bool isLastOne = (j == colunms.Count - 1);

                        var comma = isLastOne ? "" : ",";
                        if (column.Type == "text" || column.Type == "keyword")
                        {
                            jsonString.Append($"\"{column.Name}\":\"{ val.As<string>()}\"{comma}");
                        }
                        else if (column.Type == "datetime")
                        {
                            jsonString.Append($"\"{column.Name}\":\"{ val.As<DateTime>()}\"{comma}");
                        }
                        else if (column.Type == "long")
                        {
                            jsonString.Append($"\"{column.Name}\":{ val.As<long>()}{comma}");
                        }
                        else if (column.Type == "integer")
                        {
                            jsonString.Append($"\"{column.Name}\":{ val.As<int>()}{comma}");
                        }
                        else if (column.Type == "float")
                        {
                            jsonString.Append($"\"{column.Name}\":{ val.As<float>()}{comma}");
                        }
                        else if (column.Type == "double")
                        {
                            jsonString.Append($"\"{column.Name}\":{ val.As<double>()}{comma}");
                        }
                        else if (column.Type == "boolean")
                        {
                            var boolVal = val.As<bool>().ToString().ToLower();
                            jsonString.Append($"\"{column.Name}\":{ boolVal}{comma}");
                        }
                        else
                        {
                            jsonString.Append($"\"{column.Name}\":\"{ val.As<string>()}\"{comma}");
                        }
                    }
                    else
                    {
                    }
                    if (i == rows.Count - 1)
                    {
                        jsonString.Append("}");
                    }
                    else
                    {
                        jsonString.Append("},");
                    }
                }
            }
            jsonString.Append("]");
            return jsonString.ToString();
        }
        #endregion




        #region  分页查询
        public async Task<QueryPagedResponseModel<T>> GetListPagedAsync<T>(int page, int size, TCondition condition, string field = null, string orderBy = null) where T : class, new()
        {
            var searchRequest = TrunConditionToSql<T>(condition);
            int fromToSearch = (page - 1) * size;
            searchRequest.From = fromToSearch;
            searchRequest.Size = size;
            if (!string.IsNullOrEmpty(condition.KeyRouting))
            {
                searchRequest.Routing = new Routing(condition.KeyRouting);
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                searchRequest.Sort = GetSortList(orderBy).ToArray();
            }
            if (!string.IsNullOrEmpty(field))
            {
                Field[] fields = GetSourceField(field).ToArray();
                searchRequest.Source = new SourceFilter() { Includes = fields };
            }
            var searchResponse = await client.SearchAsync<T>(searchRequest);
            GetDebugInfo(searchResponse);
            var entities = searchResponse.Documents;
            List<T> list = new List<T>();
            if (!searchResponse.TimedOut)
            {
                foreach (var entity in entities)
                {
                    list.Add(entity);
                }
            }
            var isSuccess = !searchResponse.TimedOut;
            var total = searchResponse.Total;
            return new QueryPagedResponseModel<T> { Data = list, Total = total, IsSuccess = isSuccess };
        }
        #region
        /// <summary>
        /// 获取排序规则  orderBy 模式   a desc,b asc
        /// </summary>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        protected List<FieldSort> GetSortList(string orderBy)
        {
            var sorts = GetSortInfoArray(orderBy);
            List<FieldSort> sortFieldList = new List<FieldSort>();
            foreach (var singleOrderInfo in sorts)
            {
                string[] singleField = GetSingleSortFieldAndOrderTypeArray(singleOrderInfo);
                string field = singleField[0];
                string orderType = singleField[1];
                SortOrder sortOrder = SortOrder.Descending;
                if (orderType.ToLower() == "asc")
                {
                    sortOrder = SortOrder.Ascending;
                }
                FieldSort sortField = new FieldSort() { Field = field, Order = sortOrder };
                sortFieldList.Add(sortField);
            }
            return sortFieldList;
        }

        private string[] GetSortInfoArray(string orderBy)
        {
            string[] sorts = orderBy.Split(',');
            return sorts;
        }

        private string[] GetSingleSortFieldAndOrderTypeArray(string singleOrderInfo)
        {
            string[] singleFieldOrderInfo = singleOrderInfo.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return singleFieldOrderInfo;
        }

        protected IEnumerable<Field> GetSourceField(string needField)
        {

            var fields = needField.Trim().Split(',');
            foreach (var field in fields)
            {
                if (!string.IsNullOrEmpty(field))
                {
                    yield return new Field(field); ;
                }
            }
        }

        #endregion

        #endregion

    }
}
