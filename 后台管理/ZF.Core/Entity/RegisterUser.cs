using System;

namespace ZF.Core.Entity
{
    /// <summary>
    /// 数据表实体类：RegisterUser 
    /// </summary>
    public class RegisterUser : BaseEntity<Guid>
    {
        /// <summary>
        /// 用户名
        /// </summary>     
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>     
        public string LoginPwd { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>     
        public string TelphoneNum { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>     
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 注册IP
        /// </summary>     
        public string RegisterIp { get; set; }

        /// <summary>
        /// 注册终端(方式)
        /// </summary>     
        public string RegiesterType { get; set; }

        /// <summary>
        /// 状态
        /// </summary>     
        public int State { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>     
        public string NickNamw { get; set; }

        /// <summary>
        /// 微信ID
        /// </summary>     
        public string WechatId { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>     
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 所在省份(地区)
        /// </summary>     
        public string AreaCode { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>     
        public string HeadImage { get; set; }

        /// <summary>
        /// 推广员编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 感兴趣科目
        /// </summary>
        public string SubjectId { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// qq号码
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 是否绑定微信号
        /// </summary>
        public int IsBindWiki { get; set; }

        /// <summary>
        /// 机构编号
        /// </summary>
        public  string InstitutionsId { get; set; }
    }
}

