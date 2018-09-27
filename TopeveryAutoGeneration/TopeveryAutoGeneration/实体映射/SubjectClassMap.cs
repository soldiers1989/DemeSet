using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：SubjectClass 
    /// </summary>
    public sealed class SubjectClassMap : BaseClassMapper<SubjectClass, Guid>
    {
		public SubjectClassMap ()
		{
			Table("t_Subject_Class");
				
			Map(x => x.ClassName).Column("ClassName");
			Map(x => x.ProjectId).Column("ProjectId");
			Map(x => x.Remark).Column("Remark");
			Map(x => x.OrderNo).Column("OrderNo");
			Map(x => x.Column_6).Column("Column_6");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

