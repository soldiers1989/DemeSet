using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：MyErrorSubject 
    /// </summary>
    public class MyErrorSubjectListInput: BasePageInput
    {
       /// <summary>
       /// 编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 用户编码
       /// </summary>     
       public string UserId{ get; set; }
       /// <summary>
       /// 试题编码
       /// </summary>     
       public string QuestionId{ get; set; }
       /// <summary>
       /// 添加时间
       /// </summary>     
       public DateTime? AddTime{ get; set; }
    }
}
