using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：课程章节试题练习(明细)表 
    /// </summary>
    public sealed class CourseChapterQuestionsDetailMap : BaseClassMapper<CourseChapterQuestionsDetail, Guid>
    {
		public CourseChapterQuestionsDetailMap ()
		{
			Table("t_Course_ChapterQuestionsDetail");
				
			Map(x => x.ChapterQuestionsId).Column("ChapterQuestionsId");
			Map(x => x.UserId).Column("UserId");
			Map(x => x.BigQuestionId).Column("BigQuestionId");
			Map(x => x.SmallQuestionId).Column("SmallQuestionId");
			Map(x => x.StuAnswer).Column("StuAnswer");
			Map(x => x.AddTime).Column("AddTime");
            Map(x => x.IsCorrect).Column("IsCorrect");

            this.AutoMap();
		}
    }
}

