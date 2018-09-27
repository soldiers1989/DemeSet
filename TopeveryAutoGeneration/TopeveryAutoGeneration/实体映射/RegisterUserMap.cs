using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：RegisterUser 
    /// </summary>
    public sealed class RegisterUserMap : BaseClassMapper<RegisterUser, Guid>
    {
		public RegisterUserMap ()
		{
			Table("t_Base_RegisterUser");
				
			Map(x => x.UserName).Column("UserName");
			Map(x => x.LoginPwd).Column("LoginPwd");
			Map(x => x.TelphoneNum).Column("TelphoneNum");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.RegisterIp).Column("RegisterIp");
			Map(x => x.RegiesterType).Column("RegiesterType");
			Map(x => x.State).Column("State");
			Map(x => x.NickNamw).Column("NickNamw");
			Map(x => x.WechatId).Column("WechatId");
			Map(x => x.LastLoginTime).Column("LastLoginTime");
			Map(x => x.AreaCode).Column("AreaCode");
			Map(x => x.HeadImage).Column("HeadImage");
			
			this.AutoMap();
		}
    }
}

