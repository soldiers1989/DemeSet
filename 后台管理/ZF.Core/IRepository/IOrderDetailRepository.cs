using System;
using ZF.Core.Entity;

namespace ZF.Core.IRepository
{
    /// <summary>
    /// 数据表实体仓储接口：订单明细表 
    /// </summary>
    public interface IOrderDetailRepository : IRepository<OrderDetail,Guid>
    {
	
    }
}

