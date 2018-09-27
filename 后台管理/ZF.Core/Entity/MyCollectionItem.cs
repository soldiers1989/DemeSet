using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：My CollectionItem 
    /// </summary>
    public class MyCollectionItem:BaseEntity<Guid>
    {
       /// <summary>
       /// 用户编码
       /// </summary>     
       public string UserId{ get; set; }

       /// <summary>
       /// 试题编码
       /// </summary>     
       public string QuestionId{ get; set; }

       /// <summary>
       /// 添加时间
       /// </summary>     
       public DateTime? AddTime{ get; set; }

    }
}

