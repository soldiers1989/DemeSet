using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：Module 
    /// </summary>
    public sealed class ModuleMap : BaseClassMapper<Module, Guid>
    {
		public ModuleMap ()
		{
			Table("t_Base_Module");
				
			Map(x => x.ModuleName).Column("ModuleName");
			Map(x => x.Class).Column("Class");
			Map(x => x.Sort).Column("Sort");
			
			this.AutoMap();
		}
    }
}

