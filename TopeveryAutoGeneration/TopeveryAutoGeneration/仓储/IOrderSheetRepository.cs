using System;
using ZF.Core.Entity;

namespace ZF.Core.IRepository
{
    /// <summary>
    /// 数据表实体仓储接口：订单表 
    /// </summary>
    public interface IOrderSheetRepository : IRepository<OrderSheet,Guid>
    {
	
    }
}

