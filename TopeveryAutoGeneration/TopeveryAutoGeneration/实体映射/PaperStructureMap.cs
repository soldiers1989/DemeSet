using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：PaperStructure 
    /// </summary>
    public sealed class PaperStructureMap : BaseClassMapper<PaperStructure, Guid>
    {
		public PaperStructureMap ()
		{
			Table("t_Paper_Structure");
				
			Map(x => x.StuctureName).Column("StuctureName");
			Map(x => x.SubjectId).Column("SubjectId");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

