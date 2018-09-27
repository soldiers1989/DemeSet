using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：角色人员关系表 
    /// </summary>
    public sealed class RoleUserMap : BaseClassMapper<RoleUser, Guid>
    {
		public RoleUserMap ()
		{
			Table("t_Base_RoleUser");
				
			Map(x => x.RoleId).Column("RoleId");
			Map(x => x.UserId).Column("UserId");
			
			this.AutoMap();
		}
    }
}

