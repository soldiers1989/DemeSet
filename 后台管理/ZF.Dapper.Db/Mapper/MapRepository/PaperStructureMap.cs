using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：PaperStructure 
    /// </summary>
    public sealed class PaperStructureMap : FullAuditEntityClassMapper<PaperStructure, Guid>
    {
		public PaperStructureMap ()
		{
			Table("t_Paper_Structure");
				
			Map(x => x.StuctureName).Column("StuctureName");
			Map(x => x.SubjectId).Column("SubjectId");
			
			
			this.AutoMap();
		}
    }
}

