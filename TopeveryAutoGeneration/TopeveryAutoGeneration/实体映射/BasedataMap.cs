using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：Basedata 
    /// </summary>
    public sealed class BasedataMap : BaseClassMapper<Basedata, Guid>
    {
		public BasedataMap ()
		{
			Table("t_Base_Basedata");
				
			Map(x => x.DataTypeId).Column("DataTypeId");
			Map(x => x.Name).Column("Name");
			Map(x => x.Code).Column("Code");
			Map(x => x.Desc).Column("Desc");
			Map(x => x.Sort).Column("Sort");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

