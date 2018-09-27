using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：SubjectSmallquestion 
    /// </summary>
    public class SubjectSmallquestionListInput: BasePageInput
    {
       /// <summary>
       /// 试题标题
       /// </summary>     
       public string QuestionTitle{ get; set; }
       /// <summary>
       /// 试题内容
       /// </summary>     
       public string SubjectType { get; set; }
       /// <summary>
       /// 大题编码
       /// </summary>     
       public string BigQuestionId { get; set; }
      
    }
}
