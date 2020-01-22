

using System.Collections.Generic;
using Nest;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ElasticSearch7Template.IDAL;
using ElasticSearch7Template.Model;
using ElasticSearch7Template.Entity;
using ElasticSearch7Template.Utility;
using ElasticSearch7Template.Model.Conditions;
using ElasticSearch7Template.Core;

namespace ElasticSearch7Template.DAL
{
    internal class DemoRepositoryImpl : AbstractRepository<DemoEntity, BaseDemoCondition>, IDemoRepository, IAutoInject
    {


        public override SearchRequest<DemoEntity> TrunConditionToSql<DemoEntity>(BaseDemoCondition condition)
        {
            var mustClauses = new List<QueryContainer>();
            if (condition != null)
            {
                if (!string.IsNullOrEmpty(condition.MoNO))
                {

                    var termQuery = new TermQuery { Field = "moNo", Value = condition.MoNO };
                    var query = new ConstantScoreQuery() { Filter = termQuery };
                    mustClauses.Add(query);
                }
                if (!string.IsNullOrEmpty(condition.RoNO))
                {

                    var termQuery = new TermQuery { Field = "roNo", Value = condition.RoNO };
                    var query = new ConstantScoreQuery() { Filter = termQuery };
                    mustClauses.Add(query);
                }
                if (!string.IsNullOrEmpty(condition.BelongDate))
                {

                    var termQuery = new TermQuery { Field = "belongDate", Value = condition.BelongDate };
                    var query = new ConstantScoreQuery() { Filter = termQuery };
                    mustClauses.Add(query);
                }
                if (!string.IsNullOrEmpty(condition.SiliconWaferSupplierCode))
                {

                    var termQuery = new TermQuery { Field = "siliconWaferSupplierCode", Value = condition.SiliconWaferSupplierCode };
                    var query = new ConstantScoreQuery() { Filter = termQuery };
                    mustClauses.Add(query);
                }
                if (!string.IsNullOrEmpty(condition.CashmereLineCode))
                {

                    var termQuery = new TermQuery { Field = "cashmereLineCode", Value = condition.CashmereLineCode };
                    var query = new ConstantScoreQuery() { Filter = termQuery };
                    mustClauses.Add(query);
                }
                if (!string.IsNullOrEmpty(condition.CashmereLineCodes))
                {
                    var arr_lineCode = condition.CashmereLineCodes.Split(',');
                    var termQuery = new TermsQuery { Field = "cashmereLineCode", Terms = arr_lineCode };
                    var query = new ConstantScoreQuery() { Filter = termQuery };
                    mustClauses.Add(query);

                }
                if (!string.IsNullOrEmpty(condition.WorkGroupCode))
                {
                    var termQuery = new TermQuery { Field = "workGroupCode", Value = condition.WorkGroupCode };
                    var query = new ConstantScoreQuery() { Filter = termQuery };
                    mustClauses.Add(query);
                }
                if (condition.IsSingleSideType.HasValue)
                {

                    var termQuery = new TermQuery { Field = "isSingleSideType", Value = condition.IsSingleSideType.Value };
                    var query = new ConstantScoreQuery() { Filter = termQuery };
                    mustClauses.Add(query);
                }

                if (!string.IsNullOrEmpty(condition.ClassShiftCode))
                {

                    var termQuery = new TermQuery { Field = "classShiftCode", Value = condition.ClassShiftCode };
                    var query = new ConstantScoreQuery() { Filter = termQuery };
                    mustClauses.Add(query);
                }

                if (!string.IsNullOrEmpty(condition.OrderCode))
                {

                    var termQuery = new TermQuery { Field = "orderInfo.orderCode", Value = condition.OrderCode };
                    var query = new ConstantScoreQuery() { Filter = termQuery };
                    mustClauses.Add(query);
                }
                if (condition.StartTime.HasValue && condition.EndTime.HasValue)
                {
                    //µ±Ìì23:59:59.999
                    var endTime = DateMath.Anchored(condition.EndTime.Value).RoundTo(DateMathTimeUnit.Day);

                    var dateQuery = new DateRangeQuery { GreaterThanOrEqualTo = condition.StartTime.Value, LessThanOrEqualTo = endTime, Field = "createTime", TimeZone = "+08:00" };

                    mustClauses.Add(dateQuery);
                }
                if (condition.DocumentType.HasValue)
                {
                    var termQuery = new TermQuery { Field = "documentType", Value = condition.DocumentType.Value };
                    var query = new ConstantScoreQuery() { Filter = termQuery };
                    mustClauses.Add(query);
                }

            }
            var searchRequest = new SearchRequest<DemoEntity>()
            {
                Query = new BoolQuery { Must = mustClauses }
            };
            return searchRequest;
        }
    }
}

