using System;
using Topevery.Application.BaseDto;

namespace Topevery.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：MyAnswerRecords 
    /// </summary>
   [AutoMap(typeof(MyAnswerRecords ))]
    public class MyAnswerRecordsInput
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
       public string BigQuestionId{ get; set; }
       /// <summary>
       /// 小题编码
       /// </summary>     
       public string SmallQuestionId{ get; set; }
       /// <summary>
       /// 试卷编码
       /// </summary>     
       public string PaperId{ get; set; }
       /// <summary>
       /// 考生答案
       /// </summary>     
       public string StuAnswer{ get; set; }
       /// <summary>
       /// 得分
       /// </summary>     
       public decimal? Score{ get; set; }
       /// <summary>
       /// 时间
       /// </summary>     
       public DateTime AddTime{ get; set; }
    }
}

