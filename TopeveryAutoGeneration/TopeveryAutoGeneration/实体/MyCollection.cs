using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：MyCollection 
    /// </summary>
    public class MyCollection:BaseEntity<Guid>
    {
       /// <summary>
       /// 课程编码
       /// </summary>     
       public string CourseId{ get; set; }

       /// <summary>
       /// 用户编码
       /// </summary>     
       public string UserId{ get; set; }

       /// <summary>
       /// 收藏时间
       /// </summary>     
       public DateTime? AddTime{ get; set; }

    }
}

