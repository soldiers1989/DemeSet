using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：Project 
    /// </summary>
    public sealed class ProjectMap : FullAuditEntityClassMapper<Project, Guid>
    {
		public ProjectMap ()
		{
			Table("t_Base_Project");
				
			Map(x => x.ProjectName).Column("ProjectName");
			Map(x => x.ProjectClassId).Column("ProjectClassId");
            Map(x => x.OrderNo).Column("OrderNo");
            Map(x => x.Remark).Column("Remark");
			
			
			this.AutoMap();
		}
    }
}

