using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：PaperInfo 
    /// </summary>
    public sealed class PaperInfoMap : BaseClassMapper<PaperInfo, Guid>
    {
		public PaperInfoMap ()
		{
			Table("t_Paper_Info");
				
			Map(x => x.PaperName).Column("PaperName");
			Map(x => x.PaperStructureId).Column("PaperStructureId");
			Map(x => x.SubjectId).Column("SubjectId");
			Map(x => x.TestTime).Column("TestTime");
			Map(x => x.State).Column("State");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

