using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：角色菜单关系表 
    /// </summary>
    public sealed class RoleMenuMap : BaseClassMapper<RoleMenu, Guid>
    {
		public RoleMenuMap ()
		{
			Table("t_Base_RoleMenu");
				
			Map(x => x.RoleId).Column("RoleId");
			Map(x => x.MenuId).Column("MenuId");
			Map(x => x.Type).Column("Type");
			
			this.AutoMap();
		}
    }
}

