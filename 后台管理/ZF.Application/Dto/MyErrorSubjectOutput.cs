using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表MyErrorSubject 输出Dto
    /// </summary>
    public class MyErrorSubjectOutput
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

        /// <summary>
        /// 學生答案
        /// </summary>
        public string StuAnswer { get; set; }
    }
}

