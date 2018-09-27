using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：SmsendLog 
    /// </summary>
    public class SmsendLogListInput: BasePageInput
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
