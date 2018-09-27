using System.Collections.Generic;
using ZF.Application.WebApiDto.SystemModule;

namespace ZF.Application.WebApiDto.CourseModule
{
    /// <summary>
    /// 
    /// </summary>
    public class MyCourseSubjectOutput
    {
        /// <summary>
        /// 科目编号
        /// </summary>
        public string SubjectId { get; set; }

        /// <summary>
        /// 科目名称
        /// </summary>
        public string SubjectName { get; set; }

        public List<SubjectModel> SubjectModel { get; set; }
    }
}