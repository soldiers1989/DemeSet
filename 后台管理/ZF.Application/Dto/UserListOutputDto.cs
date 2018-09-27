namespace ZF.Application.Dto
{
    /// <summary>
    /// 用户信息查询输出Dto
    /// </summary>
    public class UserListOutputDto
    {
        /// <summary>
        /// 主键编号
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 登陆名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        ///是否管理员
        /// </summary>
        public int IsAdmin { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

    }
}