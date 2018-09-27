using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：Module 
    /// </summary>
    public class Module:BaseEntity<Guid>
    {
       /// <summary>
       /// 模块名称
       /// </summary>     
       public string ModuleName{ get; set; }

       /// <summary>
       /// 样式
       /// </summary>     
       public string Class{ get; set; }

       /// <summary>
       /// 排序号
       /// </summary>     
       public int? Sort{ get; set; }

    }
}

