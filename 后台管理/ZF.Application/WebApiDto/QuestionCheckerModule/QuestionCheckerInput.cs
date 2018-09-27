using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.WebApiDto.CourseSubjectModule
{

    [AutoMap( typeof( QuestionChecker ) )]
    public class QuestionCheckerInput
    {
        public string Id { get; set; }
        public string Content { get; set; }

        public DateTime? AddTime { get; set; }

        public string UserId { get; set; }
    }
}
