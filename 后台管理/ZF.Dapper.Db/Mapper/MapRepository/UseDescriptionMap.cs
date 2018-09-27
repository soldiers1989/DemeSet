using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：使用指南 
    /// </summary>
    public sealed class UseDescriptionMap : FullAuditEntityClassMapper<UseDescription, Guid>
    {
		public UseDescriptionMap ()
		{
			Table("T_Base_UseDescription");
				
			Map(x => x.BigClassId).Column("BigClassId");
			Map(x => x.ClassId).Column("ClassId");
			Map(x => x.Content).Column("Content");
			
			this.AutoMap();
		}
    }
}

