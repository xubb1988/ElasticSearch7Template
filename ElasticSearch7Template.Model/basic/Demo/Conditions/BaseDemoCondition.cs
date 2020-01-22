
using System;
using ElasticSearch7Template.Core;
namespace ElasticSearch7Template.Model.Conditions
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseDemoCondition : ESBaseCondition
    {
        /// <summary>
        ///单据编号
        /// </summary>
        public string MoNO { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string RoNO { get; set; }

        /// <summary>
        ///硅片厂商编号
        /// </summary>
        public string SiliconWaferSupplierCode { get; set; }

        /// <summary>
        ///线体编号
        /// </summary>
        public string CashmereLineCode { get; set; }

        /// <summary>
        ///班次班组code，如A B C班
        /// </summary>
        public string WorkGroupCode { get; set; }

        /// <summary>
        ///班别code，如白班夜班
        /// </summary>
        public string ClassShiftCode { get; set; }

        /// <summary>
        ///是否是单面 （0 双面 1单面） 
        /// </summary>
        public bool? IsSingleSideType { get; set; }

        /// <summary>
        /// 单据类型 1 订单 2返工单
        /// </summary>
        /// 
        public int? DocumentType { get; set; }


        /// <summary>
        ///线体编号集合（line1,line2）
        /// </summary>
        public string CashmereLineCodes { get; set; }

        /// <summary>
        /// 归属日
        /// </summary>
        public string BelongDate { get; set; }

        /// <summary>
        /// 下料口编号
        /// </summary>
        public int? LaminationNo { get; set; }

        public string OrderCode { get; set; }


    }
}
