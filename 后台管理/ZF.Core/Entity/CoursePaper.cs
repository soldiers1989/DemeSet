using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：课程试卷表 
    /// </summary>
    public class CoursePaper:BaseEntity<Guid>
    {
       /// <summary>
       /// 课程编码
       /// </summary>     
       public string CourseId{ get; set; }

       /// <summary>
       /// 试卷编码
       /// </summary>     
       public string PaperInfoId{ get; set; }

       /// <summary>
       /// 试卷名称
       /// </summary>     
       public string PaperInfoName{ get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int OrderNo { get; set; }

    }
}

