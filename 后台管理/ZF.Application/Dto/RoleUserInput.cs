using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：角色人员关系表 
    /// </summary>
    public class RoleUserInput
    {
        /// <summary>
        /// 角色编号
        /// </summary>     
        public string RoleId { get; set; }
        /// <summary>
        /// 用户编号
        /// </summary>     
        public string UserIds { get; set; }
    }
}

