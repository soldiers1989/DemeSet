using System;
using Topevery.Application.BaseDto;

namespace Topevery.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：User 
    /// </summary>
   [AutoMap(typeof(User ))]
    public class UserInput
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

