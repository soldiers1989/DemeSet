using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：推广员表 
    /// </summary>
    public sealed class PromotePromotersMap : BaseClassMapper<PromotePromoters, Guid>
    {
		public PromotePromotersMap ()
		{
			Table("t_Promote_Promoters");
				
			Map(x => x.CompanyId).Column("CompanyId");
			Map(x => x.Name).Column("Name");
			Map(x => x.Contact).Column("Contact");
			Map(x => x.PromotionCode).Column("PromotionCode");
			Map(x => x.CommissionRatio).Column("CommissionRatio");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

