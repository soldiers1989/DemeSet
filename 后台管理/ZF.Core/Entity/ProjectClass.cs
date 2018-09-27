using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：ProjectClass 
    /// </summary>
    public class ProjectClass:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 项目分类名称
       /// </summary>     
       public string ProjectClassName{ get; set; }

       /// <summary>
       /// 分类说明
       /// </summary>     
       public string Remark{ get; set; }

       /// <summary>
       /// 排序号
       /// </summary>     
       public int? OrderNo{ get; set; }

    }
}

