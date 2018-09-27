using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：购物车表 
    /// </summary>
    public sealed class OrderCartMap : BaseClassMapper<OrderCart, Guid>
    {
		public OrderCartMap ()
		{
			Table("t_Order_Cart");
				
			Map(x => x.OrderNo).Column("OrderNo");
			Map(x => x.RegisterUserId).Column("RegisterUserId");
			
			this.AutoMap();
		}
    }
}

