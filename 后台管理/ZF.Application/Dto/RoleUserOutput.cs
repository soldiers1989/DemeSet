using System;
using ZF.Core;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 查询输出Output:角色人员关系表 
    /// </summary>
    public class RoleUserOutput
    {
        /// <summary>
        /// 主键编号
        /// </summary>     
        public string Id { get; set; }
        /// <summary>
        /// 角色编号
        /// </summary>     
        public string RoleId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>     
        public string UserName { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>     
        public string LoginName { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>     
        public string Phone { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId { get; set; }
    }

}

