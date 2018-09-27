using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：课程防伪码管理 
    /// </summary>
    public class CourseSecurityCodeListInput: BasePageInput
    {
       /// <summary>
       /// 编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 课程编码
       /// </summary>     
       public string CourseId{ get; set; }
       /// <summary>
       /// 防伪码
       /// </summary>     
       public string Code{ get; set; }
       /// <summary>
       /// 是否使用
       /// </summary>     
       public int? IsUse{ get; set; }
       /// <summary>
       /// 使用用户编号
       /// </summary>     
       public string UserId{ get; set; }
    }
}
