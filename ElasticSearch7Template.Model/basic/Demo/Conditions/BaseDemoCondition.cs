
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
        ///���ݱ��
        /// </summary>
        public string MoNO { get; set; }

        /// <summary>
        ///
        /// </summary>
        public string RoNO { get; set; }

        /// <summary>
        ///��Ƭ���̱��
        /// </summary>
        public string SiliconWaferSupplierCode { get; set; }

        /// <summary>
        ///������
        /// </summary>
        public string CashmereLineCode { get; set; }

        /// <summary>
        ///��ΰ���code����A B C��
        /// </summary>
        public string WorkGroupCode { get; set; }

        /// <summary>
        ///���code����װ�ҹ��
        /// </summary>
        public string ClassShiftCode { get; set; }

        /// <summary>
        ///�Ƿ��ǵ��� ��0 ˫�� 1���棩 
        /// </summary>
        public bool? IsSingleSideType { get; set; }

        /// <summary>
        /// �������� 1 ���� 2������
        /// </summary>
        /// 
        public int? DocumentType { get; set; }


        /// <summary>
        ///�����ż��ϣ�line1,line2��
        /// </summary>
        public string CashmereLineCodes { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        public string BelongDate { get; set; }

        /// <summary>
        /// ���Ͽڱ��
        /// </summary>
        public int? LaminationNo { get; set; }

        public string OrderCode { get; set; }


    }
}
