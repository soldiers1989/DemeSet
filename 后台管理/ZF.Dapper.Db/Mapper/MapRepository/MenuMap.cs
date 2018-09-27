using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：Menu 
    /// </summary>
    public sealed class MenuMap : BaseClassMapper<Menu, Guid>
    {
		public MenuMap ()
		{
			Table("t_Base_Menu");
				
			Map(x => x.MenuName).Column("MenuName");
            Map(x => x.ModuleId).Column("ModuleId");
            Map(x => x.Url).Column("Url");
			Map(x => x.Sort).Column("Sort");
			Map(x => x.Class).Column("Class");
			Map(x => x.Description).Column("Description");
			
			this.AutoMap();
		}
    }
}

