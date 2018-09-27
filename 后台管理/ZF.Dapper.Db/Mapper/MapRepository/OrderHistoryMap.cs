using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：历史订单表 
    /// </summary>
    public sealed class OrderHistoryMap : BaseClassMapper<OrderHistory, Guid>
    {
		public OrderHistoryMap ()
		{
			Table("t_Order_History");
				
			Map(x => x.OrderNo).Column("OrderNo");
			Map(x => x.RegisterUserId).Column("RegisterUserId");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.OrderAmount).Column("OrderAmount");
			Map(x => x.State).Column("State");
			Map(x => x.FactPayAmount).Column("FactPayAmount");
			Map(x => x.OrderIp).Column("OrderIp");
			Map(x => x.PayType).Column("PayType");
			Map(x => x.PayTime).Column("PayTime");
			Map(x => x.OrderType).Column("OrderType");
			Map(x => x.TradeNo).Column("TradeNo");
			
			this.AutoMap();
		}
    }
}

