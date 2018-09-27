using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：课程章节试题练习主表 
    /// </summary>
    public class CourseChapterQuestions:BaseEntity<Guid>
    {
       /// <summary>
       /// 用户编码
       /// </summary>     
       public string UserId{ get; set; }

       /// <summary>
       /// 章节编号
       /// </summary>     
       public string ChapterId{ get; set; }

       /// <summary>
       /// 时间
       /// </summary>     
       public DateTime AddTime{ get; set; }

       /// <summary>
       /// 状态
       /// </summary>     
       public int? Status{ get; set; }

        /// <summary>
        /// 练习编号
        /// </summary>
        public  string PracticeNo { get; set; }

    }
}

