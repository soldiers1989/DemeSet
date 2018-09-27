using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;

namespace ZF.Application.WebApiDto.SubjectModule
{
    /// <summary>
    /// /
    /// </summary>
   public class QuestionListInput:BasePageInput
    {
        public string ChapterId { get; set; }

    }
}
