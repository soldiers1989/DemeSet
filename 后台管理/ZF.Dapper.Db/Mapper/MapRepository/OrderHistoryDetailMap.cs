using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：订单明细表(历史表) 
    /// </summary>
    public sealed class OrderHistoryDetailMap : BaseClassMapper<OrderHistoryDetail, Guid>
    {
		public OrderHistoryDetailMap ()
		{
			Table("t_Order_HistoryDetail");
				
			Map(x => x.OrderNo).Column("OrderNo");
			Map(x => x.CourseId).Column("CourseId");
			Map(x => x.Price).Column("Price");
			Map(x => x.FavourablePrice).Column("FavourablePrice");
			Map(x => x.Num).Column("Num");
			Map(x => x.Amount).Column("Amount");
			Map(x => x.CourseType).Column("CourseType");
			
			this.AutoMap();
		}
    }
}

