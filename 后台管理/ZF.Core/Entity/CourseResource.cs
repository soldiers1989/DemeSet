using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：CourseResource 
    /// </summary>
    public class CourseResource:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 资源名称
       /// </summary>     
       public string ResourceName{ get; set; }

       /// <summary>
       /// 资源url
       /// </summary>     
       public string ResourceUrl{ get; set; }

       /// <summary>
       /// 资源大小
       /// </summary>     
       public string ResourceSize{ get; set; }

       /// <summary>
       /// 课程编码
       /// </summary>     
       public string CourseId{ get; set; }

    }
}

