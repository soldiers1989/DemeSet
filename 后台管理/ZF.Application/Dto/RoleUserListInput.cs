using System;
using ZF.Application.BaseDto;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输入Input：角色人员关系表 
    /// </summary>
    public class RoleUserListInput : BasePageInput
    {
        /// <summary>
        /// 角色编号
        /// </summary>     
        public string RoleId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>     
        public string UserName { get; set; }
    }
}
