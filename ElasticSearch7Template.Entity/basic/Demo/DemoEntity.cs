using System;
using Nest;
namespace ElasticSearch7Template.Entity
{

    /// <summary>
    /// 
    /// </summary>
    [ElasticsearchIndex(DefaultIndexName = "demo", Alias = "demo_alias", TemplateName = "demo_template")]
    [ElasticsearchType(RelationName = "_doc", IdProperty = "id")]
    public class DemoEntity : BaseField
    {

        /// <summary>
        /// 
        /// </summary>
        [Number(NumberType.Long, Name = "id")]
        public long Id { get; set; }
        /// <summary>
        ///  线体编码
        /// </summary>
        [Keyword(Name = "cashmereLineCode")]
        public string CashmereLineCode { get; set; }

        /// <summary>
        ///  碟片口编号
        /// </summary>
        [Number(NumberType.Integer, Name = "laminationNo")]
        public int? LaminationNo { get; set; }

        /// <summary>
        ///  硅片数量
        /// </summary>
        [Number(NumberType.Integer, Name = "siliconWaferCount")]
        public int? SiliconWaferCount { get; set; }

        /// <summary>
        ///  订单编码
        /// </summary>
        [Keyword(Name = "moNo")]
        public string MoNo { get; set; }

        /// <summary>
        ///  返工单编码
        /// </summary>
        [Keyword(Name = "roNo")]
        public string RoNo { get; set; }

        /// <summary>
        ///  归属日期
        /// </summary>
        [Keyword(Name = "belongDate")]
        public string BelongDate { get; set; }

        /// <summary>
        ///  班别编码
        /// </summary>
        [Keyword(Name = "classShiftCode")]
        public string ClassShiftCode { get; set; }

        /// <summary>
        ///  
        /// </summary>
        [Boolean(Name = "isSingleSideType")]
        public bool? IsSingleSideType { get; set; }

        /// <summary>
        ///  
        /// </summary>
        [Keyword(Name = "siliconWaferSupplierCode")]
        public string SiliconWaferSupplierCode { get; set; }

        /// <summary>
        ///  产品编码
        /// </summary>
        [Keyword(Name = "targetProductCode")]
        public string TargetProductCode { get; set; }

        /// <summary>
        ///  班组编码
        /// </summary>
        [Keyword(Name = "workGroupCode")]
        public string WorkGroupCode { get; set; }



        [Object(Name = "orderInfo")]
        public MyExClass OrderInfo { get; set; }


    }


    public class MyExClass
    {

        /// <summary>
        /// 
        /// </summary>
        [Keyword(Name = "orderCode")]
        public string OrderCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Keyword(Name = "orderCode2")]
        public string OrderCode2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Keyword(Name = "user")]
        public User User { get; set; }
    }


    public class User
    {

        /// <summary>
        /// 
        /// </summary>
        [Keyword(Name = "userName")]
        public string UserName{ get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [ElasticsearchIndex(DefaultIndexName = "demo", Alias = "demo_alias")]
    [ElasticsearchType(RelationName = "_doc", IdProperty = "id")]
    public class PartOfDemoEntity
    {

        /// <summary>
        /// 
        /// </summary>
        [Number(NumberType.Long, Name = "id")]
        public long Id { get; set; }
        /// <summary>
        ///  线体编码
        /// </summary>
        [Keyword(Name = "cashmereLineCode")]
        public string CashmereLineCode { get; set; }

        /// <summary>
        ///  碟片口编号
        /// </summary>
        [Number(NumberType.Integer, Name = "laminationNo")]
        public int? LaminationNo { get; set; }

        /// <summary>
        ///  硅片数量
        /// </summary>
        [Number(NumberType.Integer, Name = "siliconWaferCount")]
        public int? SiliconWaferCount { get; set; }

        /// <summary>
        ///  订单编码
        /// </summary>
        [Keyword(Name = "moNo")]
        public string MoNo { get; set; }

        /// <summary>
        ///  返工单编码
        /// </summary>
        [Keyword(Name = "roNo")]
        public string RoNo { get; set; }

        /// <summary>
        ///  归属日期
        /// </summary>
        [Keyword(Name = "belongDate")]
        public string BelongDate { get; set; }

        /// <summary>
        ///  班别编码
        /// </summary>
        [Keyword(Name = "classShiftCode")]
        public string ClassShiftCode { get; set; }

        /// <summary>
        ///  
        /// </summary>
        [Boolean(Name = "isSingleSideType")]
        public bool? IsSingleSideType { get; set; }


    }
}
