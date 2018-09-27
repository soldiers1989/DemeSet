using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：文本配置表(联系我们,注册协议) 
    /// </summary>
    public sealed class DanyeMap : FullAuditEntityClassMapper<Danye, Guid>
    {
		public DanyeMap ()
		{
			Table("T_Base_Danye");
				
			Map(x => x.Code).Column("Code");
			Map(x => x.Name).Column("Name");
			Map(x => x.Content).Column("Content");
			
			this.AutoMap();
		}
    }
}

