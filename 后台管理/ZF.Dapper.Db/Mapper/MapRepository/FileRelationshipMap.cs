using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：FileRelationship 
    /// </summary>
    public sealed class FileRelationshipMap : BaseClassMapper<FileRelationship, Guid>
    {
		public FileRelationshipMap ()
		{
			Table("t_Base_FileRelationship");
				
			Map(x => x.ModuleId).Column("ModuleId");
			Map(x => x.Type).Column("Type");
			Map(x => x.QlyName).Column("QlyName");
			Map(x => x.CreateTime).Column("CreateTime");
			
			this.AutoMap();
		}
    }
}

