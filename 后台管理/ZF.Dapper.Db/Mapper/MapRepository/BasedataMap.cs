using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：Basedata 
    /// </summary>
    public sealed class BasedataMap : FullAuditEntityClassMapper<Basedata, Guid>
    {
		public BasedataMap ()
		{
			Table("t_Base_Basedata");
				
			Map(x => x.Name).Column("Name");
			Map(x => x.Code).Column("Code");
			
			
			this.AutoMap();
		}
    }
}

