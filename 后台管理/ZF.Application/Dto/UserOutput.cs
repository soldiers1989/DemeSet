using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 数据表User 输出Dto
    /// </summary>
    public class UserOutput
    {
       /// <summary>
       /// 编号
       /// </summary>     
      public string Id{ get; set; }
       /// <summary>
       /// 手机号
       /// </summary>     
      public string Phone{ get; set; }
       /// <summary>
       /// 用户昵称
       /// </summary>     
      public string UserName{ get; set; }
       /// <summary>
       /// 密码
       /// </summary>     
      public string PassWord{ get; set; }
       /// <summary>
       /// 创建时间
       /// </summary>     
      public DateTime AddTime{ get; set; }
       /// <summary>
       /// 创建人编号
       /// </summary>     
      public int AddUserId{ get; set; }
       /// <summary>
       /// 最后更新时间
       /// </summary>     
      public DateTime? UpdateTime{ get; set; }
       /// <summary>
       /// 最后更新人编号
       /// </summary>     
      public int? UpdateUserId{ get; set; }
       /// <summary>
       /// 是否删除
       /// </summary>     
      public int IsDelete{ get; set; }
    }
}

