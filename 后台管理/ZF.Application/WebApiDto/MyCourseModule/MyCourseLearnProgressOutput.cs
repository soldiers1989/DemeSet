using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.WebApiDto.MyCourseModule
{
    /// <summary>
    /// 
    /// </summary>
   public class MyCourseLearnProgressOutput
    {
        /// <summary>
        /// 已学习课时数
        /// </summary>
        public int HasLearnCount { get; set; }

        /// <summary>
        /// 总课时数
        /// </summary>
        public int TotalLearnCount { get; set; }
    }
}
