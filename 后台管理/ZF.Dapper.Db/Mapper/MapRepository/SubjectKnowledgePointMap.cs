using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：SubjectKnowledgePoint 
    /// </summary>
    public sealed class SubjectKnowledgePointMap : FullAuditEntityClassMapper<SubjectKnowledgePoint, Guid>
    {
		public SubjectKnowledgePointMap ()
		{
			Table("t_Subject_KnowledgePoint");
				
			Map(x => x.KnowledgePointName).Column("KnowledgePointName");
			Map(x => x.KnowledgePointCode).Column("KnowledgePointCode");
			Map(x => x.SubjectId).Column("SubjectId");
			Map(x => x.ParentId).Column("ParentId");
            Map(x => x.DigitalBookPage).Column("DigitalBookPage");


            this.AutoMap();
		}
    }
}

