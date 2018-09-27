using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：User 
    /// </summary>
    public sealed class UserMap : BaseClassMapper<User, Guid>
    {
		public UserMap ()
		{
			Table("t_Base_User");
				
			Map(x => x.LoginName).Column("LoginName");
			Map(x => x.UserName).Column("UserName");
			Map(x => x.PassWord).Column("PassWord");
			Map(x => x.IsAdmin).Column("IsAdmin");
			Map(x => x.Phone).Column("Phone");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

