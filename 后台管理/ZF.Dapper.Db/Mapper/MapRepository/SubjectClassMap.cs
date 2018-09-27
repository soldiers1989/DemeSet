using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：SubjectClass 
    /// </summary>
    public sealed class SubjectClassMap : FullAuditEntityClassMapper<SubjectClass, Guid>
    {
		public SubjectClassMap ()
		{
			Table("t_Subject_Class");
				
			Map(x => x.ClassName).Column("ClassName");
			Map(x => x.ProjectId).Column("ProjectId");
			Map(x => x.Remark).Column("Remark");
			Map(x => x.OrderNo).Column("OrderNo");

            Map(x => x.BigType).Column("BigType");

            Map(x => x.Column_6).Column("Column_6");
			
			this.AutoMap();
		}
    }
}

