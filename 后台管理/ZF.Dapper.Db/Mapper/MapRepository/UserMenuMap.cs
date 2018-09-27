using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：用户菜单关系表 
    /// </summary>
    public sealed class UserMenuMap : BaseClassMapper<UserMenu, Guid>
    {
		public UserMenuMap ()
		{
			Table("t_Base_UserMenu");
				
			Map(x => x.UserId).Column("UserId");
			Map(x => x.MenuId).Column("MenuId");
			Map(x => x.Type).Column("Type");
			
			this.AutoMap();
		}
    }
}

