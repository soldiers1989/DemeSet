using System;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;

namespace ZF.Application.Dto
{
    /// <summary>
    /// 新增修改输入Input：角色菜单关系表 
    /// </summary>
    public class RoleMenuInput
    {
        /// <summary>
        /// 主键编号
        /// </summary>     
        public string Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Ids { get; set; }
    }
}

