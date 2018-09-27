using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：CourseVideo 
    /// </summary>
    public sealed class CourseVideoMap : FullAuditEntityClassMapper<CourseVideo, Guid>
    {
        public CourseVideoMap()
        {
            Table("t_Course_Video");

            Map(x => x.VideoName).Column("VideoName");
            Map(x => x.VideoUrl).Column("VideoUrl");
            Map(x => x.ChapterId).Column("ChapterId");
            Map(x => x.VideoLong).Column("VideoLong");
            Map(x => x.VideoLongTime).Column("VideoLongTime");
            Map(x => x.VideoContent).Column("VideoContent");
            Map(x => x.IsTaste).Column("IsTaste");
            Map(x => x.TasteLongTime).Column("TasteLongTime");
            Map(x => x.StudyCount).Column("StudyCount");
            Map(x => x.Code).Column("Code");
            Map(x => x.QcodeTitle).Column("QcodeTitle");
            Map(x => x.OrderNo).Column("OrderNo");
            Map( x => x.TasteLongTime2 ).Column( "TasteLongTime2" );
            this.AutoMap();
        }
    }
}

