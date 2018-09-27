using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZF.API.Model
{
    /// <summary>
    /// 用户登录输入实体
    /// </summary>
    public class LoginInput
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        public string TelphoneNum { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string LoginPwd { get; set; }

        /// <summary>
        /// 登录方式
        /// </summary>
        public int? LoginType { get; set; }

        /// <summary>
        /// pc登录方式 0：密码登录 1：短信登录 2：扫码登录
        /// </summary>
        public int PcLoginType { get; set; }

        /// <summary>
        /// 短信验证码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 注册IP
        /// </summary>
        public string RegisterIp { get; set; }
        /// <summary>
        /// 注册方式
        /// </summary>
        public string RegiesterType { get; set; }

        /// <summary>
        /// 是否绑定微信号
        /// </summary>
        public int IsBindWiki { get; set; }
    }
}