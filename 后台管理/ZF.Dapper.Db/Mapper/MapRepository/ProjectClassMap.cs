using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：ProjectClass 
    /// </summary>
    public sealed class ProjectClassMap : FullAuditEntityClassMapper<ProjectClass, Guid>
    {
		public ProjectClassMap ()
		{
			Table("t_Base_ProjectClass");
				
			Map(x => x.ProjectClassName).Column("ProjectClassName");
			Map(x => x.Remark).Column("Remark");
			Map(x => x.OrderNo).Column("OrderNo");
			
			
			this.AutoMap();
		}
    }
}

