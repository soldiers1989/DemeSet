using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：SmsendLog 
    /// </summary>
    public sealed class SmsendLogMap : BaseClassMapper<SmsendLog, Guid>
    {
		public SmsendLogMap ()
		{
			Table("t_Base_SmsendLog");
				
			Map(x => x.TelphoneNum).Column("TelphoneNum");
			Map(x => x.SendTime).Column("SendTime");
            Map(x => x.FailureTime).Column("FailureTime");
            Map(x => x.Code).Column("Code");
			
			this.AutoMap();
		}
    }
}

