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
       /// 登录名
       /// </summary>     
      public string LoginName{ get; set; }
       /// <summary>
       /// 用户昵称
       /// </summary>     
      public string UserName{ get; set; }
       /// <summary>
       /// 密码
       /// </summary>     
      public string PassWord{ get; set; }
       /// <summary>
       /// 是否管理员
       /// </summary>     
      public int? IsAdmin{ get; set; }
       /// <summary>
       /// 手机号
       /// </summary>     
      public string Phone{ get; set; }
       /// <summary>
       /// 创建时间
       /// </summary>     
      public DateTime AddTime{ get; set; }
       /// <summary>
       /// 创建人
       /// </summary>     
      public string AddUserId{ get; set; }
       /// <summary>
       /// 修改时间
       /// </summary>     
      public DateTime? UpdateTime{ get; set; }
       /// <summary>
       /// 修改人
       /// </summary>     
      public string UpdateUserId{ get; set; }
       /// <summary>
       /// 是否删除
       /// </summary>     
      public bool IsDelete{ get; set; }
    }
}

