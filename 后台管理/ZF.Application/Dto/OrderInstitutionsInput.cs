using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：下单机构表 
    /// </summary>
   [AutoMap(typeof(OrderInstitutions ))]
    public class OrderInstitutionsInput
    {
       /// <summary>
       /// 编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 机构名称
       /// </summary>     
       public string Name{ get; set; }
       /// <summary>
       /// 机构联系人
       /// </summary>     
       public string Contact{ get; set; }
       /// <summary>
       /// 机构联系方式
       /// </summary>     
       public string ContactPhone{ get; set; }
       /// <summary>
       /// 机构联系地址
       /// </summary>     
       public string ContactAddress{ get; set; }
       /// <summary>
       /// 机构备注
       /// </summary>     
       public string Remark{ get; set; }
       /// <summary>
       /// 创建时间
       /// </summary>     
       public DateTime AddTime{ get; set; }
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
       public int IsDelete{ get; set; }
    }
}

