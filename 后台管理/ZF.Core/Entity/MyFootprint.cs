using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：我的足迹 
    /// </summary>
    public class MyFootprint:BaseEntity<Guid>
    {
       /// <summary>
       /// 课程编号
       /// </summary>     
       public string CourseId{ get; set; }

       /// <summary>
       /// 课程类别
       /// </summary>     
       public int CourseType{ get; set; }

       /// <summary>
       /// 浏览时间
       /// </summary>     
       public DateTime? BrowsingTime{ get; set; }

       /// <summary>
       /// 用户编号
       /// </summary>     
       public string UserId{ get; set; }

       /// <summary>
       /// 是否删除
       /// </summary>     
       public int IsDelete{ get; set; }

    }
}

