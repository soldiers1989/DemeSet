using System;
using System.Data;
using System.Text;
using Dapper;
using ZF.Core;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Dapper.Db.Repository
{
    /// <summary>
    /// 数据表实体仓储实现：User 
    /// </summary>
    public class UserRepository : BaseRepositoryEntity<User>, IUserRepository
    {
    }
}

