using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：CourseOnTeachers 
    /// </summary>
    public class CourseOnTeachers:FullAuditEntity<Guid>
    {
       /// <summary>
       /// 教师名称
       /// </summary>     
       public string TeachersName{ get; set; }

       /// <summary>
       /// 擅长科目
       /// </summary>     
       public string TheLabel{ get; set; }

       /// <summary>
       /// 简介
       /// </summary>     
       public string Synopsis{ get; set; }
        
        /// <summary>
        /// 讲师照片
        /// </summary>
        public string TeacherPhoto { get; set; }
        /// <summary>
        /// 所属项目
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// 是否在名师栏
        /// </summary>
        public int IsFamous { get; set; }
        /// <summary>
        /// 教学风格
        /// </summary>
        public string TeachStyle { get; set; }
    }
}

