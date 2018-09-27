using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：网站相关配置表 
    /// </summary>
    public sealed class SysSetMap : FullAuditEntityClassMapper<SysSet, Guid>
    {
		public SysSetMap ()
		{
			Table("T_Base_SysSet");
				
			Map(x => x.Name).Column("Name");
			Map(x => x.ArguName).Column("ArguName");
			Map(x => x.ArguValue).Column("ArguValue");
			Map(x => x.Remark).Column("Remark");
			
			this.AutoMap();
		}
    }
}

