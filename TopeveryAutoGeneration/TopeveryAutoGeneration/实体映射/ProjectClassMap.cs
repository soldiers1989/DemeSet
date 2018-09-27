using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：ProjectClass 
    /// </summary>
    public sealed class ProjectClassMap : BaseClassMapper<ProjectClass, Guid>
    {
		public ProjectClassMap ()
		{
			Table("t_Base_ProjectClass");
				
			Map(x => x.ProjectClassName).Column("ProjectClassName");
			Map(x => x.Remark).Column("Remark");
			Map(x => x.OrderNo).Column("OrderNo");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

