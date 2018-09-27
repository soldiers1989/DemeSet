using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：意见反馈表 
    /// </summary>
    public class MyFeedback:BaseEntity<Guid>
    {
       /// <summary>
       /// 标题
       /// </summary>     
       public string Title{ get; set; }

       /// <summary>
       /// 内容
       /// </summary>     
       public string Advice{ get; set; }

       /// <summary>
       /// 联系方式
       /// </summary>     
       public decimal Relation{ get; set; }

       /// <summary>
       /// 时间
       /// </summary>     
       public DateTime AddTime{ get; set; }

    }
}

