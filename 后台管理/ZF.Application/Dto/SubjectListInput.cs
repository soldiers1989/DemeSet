using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：Subject 
    /// </summary>
    public class SubjectListInput : BasePageInput
    {
        /// <summary>
        /// 课程编码 -作级联查询使用
        /// </summary>
        public string CourseId { get; set; }
        /// <summary>
        /// 科目名称
        /// </summary>     
        public string SubjectName { get; set; }
        /// <summary>
        /// 所属项目
        /// </summary>     
        public string ProjectId { get; set; }

        /// <summary>
        /// 所属项目分类
        /// </summary>
        public string ProjectClassId { get; set; }
    }
}
