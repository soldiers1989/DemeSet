using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：SubjectKnowledgePoint 
    /// </summary>
    public sealed class SubjectKnowledgePointMap : BaseClassMapper<SubjectKnowledgePoint, Guid>
    {
		public SubjectKnowledgePointMap ()
		{
			Table("t_Subject_KnowledgePoint");
				
			Map(x => x.KnowledgePointName).Column("KnowledgePointName");
			Map(x => x.KnowledgePointCode).Column("KnowledgePointCode");
			Map(x => x.SubjectId).Column("SubjectId");
			Map(x => x.ParentId).Column("ParentId");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

