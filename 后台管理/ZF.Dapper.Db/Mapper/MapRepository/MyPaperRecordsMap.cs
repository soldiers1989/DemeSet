using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：试卷作答记录表 
    /// </summary>
    public sealed class MyPaperRecordsMap : BaseClassMapper<MyPaperRecords, Guid>
    {
		public MyPaperRecordsMap ()
		{
			Table("t_My_PaperRecords");
				
			Map(x => x.UserId).Column("UserId");
			Map(x => x.PaperId).Column("PaperId");
			Map(x => x.Score).Column("Score");
            Map(x => x.ScoreSum).Column("ScoreSum");
            Map(x => x.PracticeNo).Column("PracticeNo");
            Map(x => x.AddTime).Column("AddTime");
			Map(x => x.Status).Column("Status");
			
			this.AutoMap();
		}
    }
}

