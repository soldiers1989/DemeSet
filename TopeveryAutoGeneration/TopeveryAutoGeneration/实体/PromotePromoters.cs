using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：推广员表 
    /// </summary>
    public class PromotePromoters:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 所属公司
       /// </summary>     
       public string CompanyId{ get; set; }

       /// <summary>
       /// 推广员名称
       /// </summary>     
       public string Name{ get; set; }

       /// <summary>
       /// 联系方式
       /// </summary>     
       public string Contact{ get; set; }

       /// <summary>
       /// 推广码
       /// </summary>     
       public string PromotionCode{ get; set; }

       /// <summary>
       /// 提成比例
       /// </summary>     
       public decimal? CommissionRatio{ get; set; }

    }
}

