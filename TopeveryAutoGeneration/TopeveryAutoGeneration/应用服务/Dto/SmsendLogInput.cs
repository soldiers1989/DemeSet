using System;
using Topevery.Application.BaseDto;

namespace Topevery.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：SmsendLog 
    /// </summary>
   [AutoMap(typeof(SmsendLog ))]
    public class SmsendLogInput
    {
       /// <summary>
       /// 记录编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 手机号
       /// </summary>     
       public string TelhopheNum{ get; set; }
       /// <summary>
       /// 发送时间
       /// </summary>     
       public DateTime SendTime{ get; set; }
       /// <summary>
       /// 验证码
       /// </summary>     
       public string Code{ get; set; }
    }
}

