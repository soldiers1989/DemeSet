using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：SubjectSmallquestion 
    /// </summary>
    public sealed class SubjectSmallquestionMap : FullAuditEntityClassMapper<SubjectSmallquestion, Guid>
    {
		public SubjectSmallquestionMap ()
		{
			Table("t_Subject_Smallquestion");
				
			Map(x => x.QuestionTitle).Column("QuestionTitle");
			Map(x => x.QuestionContent).Column("QuestionContent");
			Map(x => x.BigQuestionId).Column("BigQuestionId");
			Map(x => x.Option1).Column("Option1");
			Map(x => x.Option2).Column("Option2");
			Map(x => x.Option3).Column("Option3");
			Map(x => x.Option4).Column("Option4");
			Map(x => x.Option5).Column("Option5");
			Map(x => x.Option6).Column("Option6");
			Map(x => x.Option7).Column("Option7");
			Map(x => x.Option8).Column("Option8");
            Map(x => x.Number).Column("Number");
            Map(x => x.SubjectType).Column("SubjectType");
			Map(x => x.RightAnswer).Column("RightAnswer");
			Map(x => x.ConsultAnswer).Column("ConsultAnswer");
			Map(x => x.State).Column("State");
			Map(x => x.QuestionTextAnalysis).Column("QuestionTextAnalysis");
			Map(x => x.QuestionAudioAnalysis).Column("QuestionAudioAnalysis");
			Map(x => x.QuestionVedioAnalysis).Column("QuestionVedioAnalysis");
			Map(x => x.DigitalBookPage).Column("DigitalBookPage");
            Map(x => x.VideoId).Column("VideoId");
            this.AutoMap();
		}
    }
}

