using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;

namespace ZF.Application.WebApiDto.CourseSubjectModule
{
    /// <summary>
    /// 
    /// </summary>
  public class CourseSubjectModelInput : BasePageInput
    {
        /// <summary>
        /// 编码
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 课程编码
        /// </summary>     
        public string CourseId { get; set; }
        /// <summary>
        /// 章节编码
        /// </summary>     
        public string ChapterId { get; set; }
        /// <summary>
        /// 试题编码
        /// </summary>     
        public string SubjectId { get; set; }
    }
}
