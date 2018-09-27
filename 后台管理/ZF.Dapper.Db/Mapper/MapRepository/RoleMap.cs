using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：角色表 
    /// </summary>
    public sealed class RoleMap : FullAuditEntityClassMapper<Role, Guid>
    {
		public RoleMap ()
		{
			Table("t_Base_Role");
				
			Map(x => x.RoleName).Column("RoleName");
			Map(x => x.Description).Column("Description");
			
			this.AutoMap();
		}
    }
}

