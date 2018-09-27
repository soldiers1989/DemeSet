using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：OperatorLog 
    /// </summary>
    public sealed class OperatorLogMap : BaseClassMapper<OperatorLog, Guid>
    {
		public OperatorLogMap ()
		{
			Table("t_Base_OperatorLog");
				
			Map(x => x.ModuleId).Column("ModuleId");
			Map(x => x.KeyId).Column("KeyId");
			Map(x => x.OperatorName).Column("OperatorName");
			Map(x => x.OperatorDate).Column("OperatorDate");
			Map(x => x.OperatorType).Column("OperatorType");
			Map(x => x.OperatorId).Column("OperatorId");
			Map(x => x.Remark).Column("Remark");
			Map(x => x.OperatorIp).Column("OperatorIp");
			
			this.AutoMap();
		}
    }
}

