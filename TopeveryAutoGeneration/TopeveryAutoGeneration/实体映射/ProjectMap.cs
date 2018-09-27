using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：Project 
    /// </summary>
    public sealed class ProjectMap : BaseClassMapper<Project, Guid>
    {
		public ProjectMap ()
		{
			Table("t_Base_Project");
				
			Map(x => x.ProjectName).Column("ProjectName");
			Map(x => x.ProjectClassId).Column("ProjectClassId");
			Map(x => x.Remark).Column("Remark");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

