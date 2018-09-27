using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{

    /// <summary>
    /// 课程试题
    /// </summary>
    [AutoMap( typeof( CoursePaper ) )]
    public class CoursePaperInput
    {

        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set;}
        /// <summary>
        /// 课程编码
        /// </summary>     
        public string CourseId { get; set; }

        /// <summary>
        /// 试卷编码
        /// </summary>     
        public string PaperInfoId { get; set; }

        /// <summary>
        /// 试卷名称
        /// </summary>     
        public string PaperInfoName { get; set; }
    }
}
