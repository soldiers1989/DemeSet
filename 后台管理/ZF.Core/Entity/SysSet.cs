using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：网站相关配置表 
    /// </summary>
    public class SysSet:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 名称
       /// </summary>     
       public string Name{ get; set; }

       /// <summary>
       /// 参数名称
       /// </summary>     
       public string ArguName{ get; set; }

       /// <summary>
       /// 参数值
       /// </summary>     
       public string ArguValue{ get; set; }

       /// <summary>
       /// 备注
       /// </summary>     
       public string Remark{ get; set; }

    }
}

