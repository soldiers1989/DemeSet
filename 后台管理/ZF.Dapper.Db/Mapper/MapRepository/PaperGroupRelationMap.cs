using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：试卷组试卷关系表 
    /// </summary>
    public sealed class PaperGroupRelationMap : BaseClassMapper<PaperGroupRelation, Guid>
    {
		public PaperGroupRelationMap ()
		{
			Table("t_Paper_GroupRelation");
				
			Map(x => x.PaperGroupId).Column("PaperGroupId");
			Map(x => x.PaperId).Column("PaperId");
			
			this.AutoMap();
		}
    }
}

