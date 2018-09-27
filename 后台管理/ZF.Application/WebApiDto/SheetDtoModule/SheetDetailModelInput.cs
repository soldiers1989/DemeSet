using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.WebApiDto.SheetDtoModule
{
    /// <summary>
    /// 
    /// </summary>
   public class SheetDetailModelInput
    {
        /// <summary>
        /// 课程ID
        /// </summary>
        public string CourseId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 课程类型
        /// </summary>
        public int CourseType { get; set; }
    }
}
