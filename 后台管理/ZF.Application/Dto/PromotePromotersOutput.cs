using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输出Output:推广员表 
    /// </summary>
    public class PromotePromotersOutput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 所属公司
        /// </summary>     
        public string CompanyId { get; set; }
        /// <summary>
        /// 推广员名称
        /// </summary>     
        public string Name { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>     
        public string TheContact { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>     
        public string Contact { get; set; }
        /// <summary>
        /// 推广码
        /// </summary>     
        public string PromotionCode { get; set; }
        /// <summary>
        /// 提成比例
        /// </summary>     
        public decimal? CommissionRatio { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 推广码地址
        /// </summary>     
        public string PromotionCodeUrl { get; set; }

    }
}

