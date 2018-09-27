using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 错题反馈
    /// </summary>
   public class ErrorSubjectFeedBackOutput
    {
        public string Id { get; set; }

        public string UserId{get;set;}

        public string Content { get; set; }

        public DateTime? AddTime { get; set; }

        public int Audit { get; set; }
    }

    public class ErrorSubjectFeedBackInput :BasePageInput{
        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int Audit { get; set; }
    }
}
