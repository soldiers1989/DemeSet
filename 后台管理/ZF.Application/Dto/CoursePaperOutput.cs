using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 课程试题
    /// </summary>
    public class CoursePaperOutput
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 课程编码
        /// </summary>     
        public string CourseId { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string CourseName { get; set; }

        /// <summary>
        /// 试卷编码
        /// </summary>     
        public string PaperInfoId { get; set; }

        /// <summary>
        /// 试卷名称
        /// </summary>     
        public string PaperInfoName { get; set; }

        /// <summary>
        /// 练习次数
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 试卷类别
        /// </summary>
        public int Type { get; set; }
    }
}
