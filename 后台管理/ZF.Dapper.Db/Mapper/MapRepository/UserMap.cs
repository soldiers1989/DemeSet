using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：User 
    /// </summary>
    public sealed class UserMap : FullAuditEntityClassMapper<User, Guid>
    {
		public UserMap ()
		{
			Table("t_Base_User");
				
			Map(x => x.Phone).Column("Phone");
			Map(x => x.UserName).Column("UserName");
			Map(x => x.PassWord).Column("PassWord");
            Map(x => x.IsAdmin).Column("IsAdmin");
            Map(x => x.LoginName).Column("LoginName");
           
			this.AutoMap();
		}
    }
}

