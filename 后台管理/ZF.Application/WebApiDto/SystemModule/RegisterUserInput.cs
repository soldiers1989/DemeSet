using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.WebApiDto.SystemModule
{
    /// <summary>
    /// 新增修改输入Input：RegisterUser 
    /// </summary>
    [AutoMap(typeof(RegisterUser))]
    public class RegisterUserInput
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 密码
        /// </summary>     
        public string LoginPwd { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>     
        public string TelphoneNum { get; set; }

        /// <summary>
        /// 注册终端(方式)
        /// </summary>     
        public string RegiesterType { get; set; }

        /// <summary>
        /// 注册IP
        /// </summary>     
        public string RegisterIp { get; set; }

        /// <summary>
        /// 微信ID
        /// </summary>     
        public string WechatId { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadImage { get; set; }

        /// <summary>
        /// 推广码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickNamw { get; set; }

        /// <summary>
        /// qq号码
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
    }
}

