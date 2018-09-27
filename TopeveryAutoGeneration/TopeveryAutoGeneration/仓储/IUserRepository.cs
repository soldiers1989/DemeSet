using System;
using ZF.Core.Entity;

namespace ZF.Core.IRepository
{
    /// <summary>
    /// 数据表实体仓储接口：User 
    /// </summary>
    public interface IUserRepository : IRepository<User,Guid>
    {
	
    }
}

