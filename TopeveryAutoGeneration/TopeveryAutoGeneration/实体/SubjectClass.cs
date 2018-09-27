using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：SubjectClass 
    /// </summary>
    public class SubjectClass:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 分类名称
       /// </summary>     
       public string ClassName{ get; set; }

       /// <summary>
       /// 分类所属项目
       /// </summary>     
       public string ProjectId{ get; set; }

       /// <summary>
       /// 分类描述
       /// </summary>     
       public string Remark{ get; set; }

       /// <summary>
       /// 排序号
       /// </summary>     
       public int? OrderNo{ get; set; }

       /// <summary>
       /// 评分规则？
       /// </summary>     
       public string Column_6{ get; set; }

    }
}

