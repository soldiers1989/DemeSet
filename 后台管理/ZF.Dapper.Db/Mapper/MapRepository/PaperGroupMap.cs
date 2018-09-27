using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：试卷组表 
    /// </summary>
    public sealed class PaperGroupMap : BaseClassMapper<PaperGroup, Guid>
    {
		public PaperGroupMap ()
		{
			Table("t_Paper_Group");
				
			Map(x => x.PaperGroupName).Column("PaperGroupName");
			Map(x => x.SubjectId).Column("SubjectId");
			Map(x => x.State).Column("State");
			Map(x => x.Type).Column("Type");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

