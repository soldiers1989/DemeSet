using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：课程章节试题练习主表 
    /// </summary>
    public sealed class CourseChapterQuestionsMap : BaseClassMapper<CourseChapterQuestions, Guid>
    {
		public CourseChapterQuestionsMap ()
		{
			Table("t_Course_ChapterQuestions");
				
			Map(x => x.UserId).Column("UserId");
			Map(x => x.ChapterId).Column("ChapterId");
			Map(x => x.AddTime).Column("AddTime");
			Map(x => x.Status).Column("Status");
            Map(x => x.PracticeNo).Column("PracticeNo");

            this.AutoMap();
		}
    }
}

