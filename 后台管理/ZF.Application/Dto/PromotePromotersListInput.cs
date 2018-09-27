using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：推广员表 
    /// </summary>
    public class PromotePromotersListInput: BasePageInput
    {
       /// <summary>
       /// 编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 所属公司
       /// </summary>     
       public string CompanyId{ get; set; }
       /// <summary>
       /// 推广员名称
       /// </summary>     
       public string Name{ get; set; }
       /// <summary>
       /// 联系人
       /// </summary>     
       public string TheContact{ get; set; }
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
       /// <summary>
       /// 创建时间
       /// </summary>     
       public DateTime? AddTime{ get; set; }
       /// <summary>
       /// 创建人
       /// </summary>     
       public string AddUserId{ get; set; }
       /// <summary>
       /// 修改时间
       /// </summary>     
       public DateTime? UpdateTime{ get; set; }
       /// <summary>
       /// 修改人
       /// </summary>     
       public string UpdateUserId{ get; set; }
       /// <summary>
       /// 是否删除
       /// </summary>     
       public bool? IsDelete{ get; set; }
    }
}
