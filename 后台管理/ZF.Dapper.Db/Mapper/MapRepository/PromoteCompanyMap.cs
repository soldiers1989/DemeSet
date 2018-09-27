using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：推广公司表 
    /// </summary>
    public sealed class PromoteCompanyMap : BaseClassMapper<PromoteCompany, Guid>
    {
		public PromoteCompanyMap ()
		{
			Table("t_Promote_Company");
				
			Map(x => x.Name).Column("Name");
			Map(x => x.TheContact).Column("TheContact");
			Map(x => x.Contact).Column("Contact");
			Map(x => x.Address).Column("Address");
			Map(x => x.CommissionRatio).Column("CommissionRatio");
			Map(x => x.BankCard).Column("BankCard");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

