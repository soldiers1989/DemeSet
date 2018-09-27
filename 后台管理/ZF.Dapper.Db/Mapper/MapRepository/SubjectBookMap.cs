using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：科目关联书籍管理 
    /// </summary>
    public sealed class SubjectBookMap : BaseClassMapper<SubjectBook, Guid>
    {
		public SubjectBookMap ()
		{
			Table("t_Base_SubjectBook");
				
			Map(x => x.BookName).Column("BookName");
			Map(x => x.SubjectId).Column("SubjectId");
			Map(x => x.OrderNo).Column("OrderNo");
			Map(x => x.ImageUrl).Column("ImageUrl");
			Map(x => x.Url).Column("Url");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.AddUserId).Column("AddUserId");
			Map(x => x.UpdateTime).Column("UpdateTime");
			Map(x => x.UpdateUserId).Column("UpdateUserId");
			Map(x => x.IsDelete).Column("IsDelete");
			
			this.AutoMap();
		}
    }
}

