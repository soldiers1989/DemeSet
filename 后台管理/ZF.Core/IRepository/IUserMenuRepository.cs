using System;
using ZF.Core.Entity;

namespace ZF.Core.IRepository
{
    /// <summary>
    /// 数据表实体仓储接口：用户菜单关系表 
    /// </summary>
    public interface IUserMenuRepository : IRepository<UserMenu,Guid>
    {
	
    }
}

