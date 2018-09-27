using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：购物车明细表 
    /// </summary>
    public sealed class OrderCartDetailMap : BaseClassMapper<OrderCartDetail, Guid>
    {
		public OrderCartDetailMap ()
		{
			Table("t_Order_CartDetail");
				
			Map(x => x.OrderNo).Column("OrderNo");
			Map(x => x.CourseId).Column("CourseId");
			Map(x => x.Price).Column("Price");
			Map(x => x.FavourablePrice).Column("FavourablePrice");
			Map(x => x.Num).Column("Num");
			Map(x => x.Amount).Column("Amount");
			Map(x => x.CourseType).Column("CourseType");
            Map(x => x.NologinGroup).Column("NologinGroup");
			this.AutoMap();
		}
    }
}

