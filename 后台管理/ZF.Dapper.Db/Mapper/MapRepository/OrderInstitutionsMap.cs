using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：下单机构表 
    /// </summary>
    public sealed class OrderInstitutionsMap : BaseClassMapper<OrderInstitutions, Guid>
    {
		public OrderInstitutionsMap ()
		{
			Table("t_Order_Institutions");
				
			Map(x => x.Name).Column("Name");
			Map(x => x.Contact).Column("Contact");
			Map(x => x.ContactPhone).Column("ContactPhone");
			Map(x => x.ContactAddress).Column("ContactAddress");
			Map(x => x.Remark).Column("Remark");
            Map(x => x.Url).Column("Url");
            Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

