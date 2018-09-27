using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：Subject 
    /// </summary>
    public sealed class SubjectMap : BaseClassMapper<Subject, Guid>
    {
		public SubjectMap ()
		{
			Table("t_Base_Subject");
				
			Map(x => x.SubjectName).Column("SubjectName");
			Map(x => x.ProjectId).Column("ProjectId");
			Map(x => x.OrderNo).Column("OrderNo");
			Map(x => x.Remark).Column("Remark");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

