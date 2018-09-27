using System;
using ZF.Core.Entity;

namespace ZF.Core.IRepository
{
    /// <summary>
    /// 数据表实体仓储接口：历史订单表 
    /// </summary>
    public interface IOrderHistoryRepository : IRepository<OrderHistory,Guid>
    {
	
    }
}

