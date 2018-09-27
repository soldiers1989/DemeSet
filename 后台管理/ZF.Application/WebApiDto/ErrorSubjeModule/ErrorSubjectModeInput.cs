using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;

namespace ZF.Application.WebApiDto.ErrorSubjeModule
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorSubjectModelInput:BasePageInput
    {
        /// <summary>
        /// 错题主键
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 试题Id
        /// </summary>
        public string QuestionId { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public string AddTime { get; set; }

        /// <summary>
        /// 试题标题
        /// </summary>
        public string QuestionContent { get; set;}

    }
}
