using System;
using ZF.Core.Entity;

namespace ZF.Core.IRepository
{
    /// <summary>
    /// 数据表实体仓储接口：购物车明细表 
    /// </summary>
    public interface IOrderCartDetailRepository : IRepository<OrderCartDetail,Guid>
    {
	
    }
}

