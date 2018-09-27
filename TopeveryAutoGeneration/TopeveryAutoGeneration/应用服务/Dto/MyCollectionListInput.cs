using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：MyCollection 
    /// </summary>
    public class MyCollectionListInput: BasePageInput
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
       /// 用户编码
       /// </summary>     
       public string UserId{ get; set; }
       /// <summary>
       /// 收藏时间
       /// </summary>     
       public DateTime? AddTime{ get; set; }
    }
}
