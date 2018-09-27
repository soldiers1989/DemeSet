using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：UserLoginLog 
    /// </summary>
    public sealed class UserLoginLogMap : BaseClassMapper<UserLoginLog, Guid>
    {
		public UserLoginLogMap ()
		{
			Table("t_Base_UserLoginLog");
				
			Map(x => x.UserId).Column("UserId");
			Map(x => x.LoginTime).Column("LoginTime");
			Map(x => x.LoginIp).Column("LoginIp");
			Map(x => x.LoginType).Column("LoginType");
			
			this.AutoMap();
		}
    }
}

