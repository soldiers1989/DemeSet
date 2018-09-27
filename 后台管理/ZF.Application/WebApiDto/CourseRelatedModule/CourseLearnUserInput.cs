using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;

namespace ZF.Application.WebApiDto.CourseRelatedModule
{
    /// <summary>
    /// 
    /// </summary>
   public class CourseLearnUserInput:BasePageInput
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 课程Id
        /// </summary>
        public string CourseId { get; set; }

    }
}
