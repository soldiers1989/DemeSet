using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：文本配置表(联系我们,注册协议) 
    /// </summary>
    public class Danye:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 编码
       /// </summary>     
       public string Code{ get; set; }

       /// <summary>
       /// 名称
       /// </summary>     
       public string Name{ get; set; }

       /// <summary>
       /// 内容
       /// </summary>     
       public string Content{ get; set; }

    }
}

