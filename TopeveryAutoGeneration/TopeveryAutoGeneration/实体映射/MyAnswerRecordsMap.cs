using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：MyAnswerRecords 
    /// </summary>
    public sealed class MyAnswerRecordsMap : BaseClassMapper<MyAnswerRecords, Guid>
    {
		public MyAnswerRecordsMap ()
		{
			Table("t_My_AnswerRecords");
				
			Map(x => x.UserId).Column("UserId");
			Map(x => x.BigQuestionId).Column("BigQuestionId");
			Map(x => x.SmallQuestionId).Column("SmallQuestionId");
			Map(x => x.PaperId).Column("PaperId");
			Map(x => x.StuAnswer).Column("StuAnswer");
			Map(x => x.Score).Column("Score");
			Map(x => x.AddTime).Column("AddTime");
			
			this.AutoMap();
		}
    }
}

