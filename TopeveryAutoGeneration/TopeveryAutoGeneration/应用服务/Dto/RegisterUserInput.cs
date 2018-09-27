using System;
using Topevery.Application.BaseDto;

namespace Topevery.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：RegisterUser 
    /// </summary>
   [AutoMap(typeof(RegisterUser ))]
    public class RegisterUserInput
    {
       /// <summary>
       /// 注册用户编码
       /// </summary>     
       public string Id{ get; set; }
       /// <summary>
       /// 用户名
       /// </summary>     
       public string UserName{ get; set; }
       /// <summary>
       /// 密码
       /// </summary>     
       public string LoginPwd{ get; set; }
       /// <summary>
       /// 手机号
       /// </summary>     
       public string TelphoneNum{ get; set; }
       /// <summary>
       /// 注册时间
       /// </summary>     
       public DateTime AddTime{ get; set; }
       /// <summary>
       /// 注册IP
       /// </summary>     
       public string RegisterIp{ get; set; }
       /// <summary>
       /// 注册终端(方式)
       /// </summary>     
       public string RegiesterType{ get; set; }
       /// <summary>
       /// 状态
       /// </summary>     
       public int State{ get; set; }
       /// <summary>
       /// 用户昵称
       /// </summary>     
       public string NickNamw{ get; set; }
       /// <summary>
       /// 微信ID
       /// </summary>     
       public string WechatId{ get; set; }
       /// <summary>
       /// 最后登录时间
       /// </summary>     
       public DateTime? LastLoginTime{ get; set; }
       /// <summary>
       /// 所在省份(地区)
       /// </summary>     
       public string AreaCode{ get; set; }
       /// <summary>
       /// 用户头像
       /// </summary>     
       public string HeadImage{ get; set; }
    }
}

