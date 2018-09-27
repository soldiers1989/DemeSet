using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：CourseVideo 
    /// </summary>
    public class CourseVideo:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 视频名称
       /// </summary>     
       public string VideoName{ get; set; }

       /// <summary>
       /// 视频地址
       /// </summary>     
       public string VideoUrl{ get; set; }

       /// <summary>
       /// 所属章节
       /// </summary>     
       public string ChapterId{ get; set; }

       /// <summary>
       /// 视频时长(分钟)
       /// </summary>     
       public string VideoLongTime{ get; set; }

       /// <summary>
       /// 课程目标
       /// </summary>     
       public string VideoContent{ get; set; }

       /// <summary>
       /// 是否可试听
       /// </summary>     
       public int? IsTaste{ get; set; }

       /// <summary>
       /// 可试听的时长(分钟)
       /// </summary>     
       public int? TasteLongTime{ get; set; }

       /// <summary>
       /// 学习次数
       /// </summary>     
       public int? StudyCount{ get; set; }

    }
}

