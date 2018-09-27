namespace ZF.Application.Dto
{
    /// <summary>
    /// 密码修改输入Dto 
    /// wyf
    /// 20180312
    /// </summary>
    public class ModifyPassWordInput
    {
        /// <summary>
        /// 原密码
        /// </summary>
        public string OldPassWord { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        public string PassWord { get; set; }
    }
}