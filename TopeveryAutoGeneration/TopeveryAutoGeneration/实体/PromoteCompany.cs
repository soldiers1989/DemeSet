using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：推广公司表 
    /// </summary>
    public class PromoteCompany:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 公司名称
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
       /// 公司地址
       /// </summary>     
       public string Address{ get; set; }

       /// <summary>
       /// 提成比例
       /// </summary>     
       public decimal? CommissionRatio{ get; set; }

       /// <summary>
       /// 银行账号
       /// </summary>     
       public string BankCard{ get; set; }

    }
}

