namespace ZF.Application.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class LoginInput
    {

        /// <summary>
        /// 登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 登陆方式
        /// </summary>
        public int? LoginType { get; set; }

        /// <summary>
        /// 是否记住我
        /// </summary>
        public string Check { get; set; }
    }
}