using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：快递公司 
    /// </summary>
    public sealed class ExpressCompanyMap : BaseClassMapper<ExpressCompany, Guid>
    {
		public ExpressCompanyMap ()
		{
			Table("ExpressCompany");
				
			Map(x => x.Name).Column("Name");
			Map(x => x.Companyurl).Column("Companyurl");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUser).Column("AddUser");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUser).Column("UpdateUser");
			Map(x => x.IsDelete).Column("IsDelete");
            Map(x => x.IsDefault).Column("IsDefault");
			this.AutoMap();
		}
    }
}

