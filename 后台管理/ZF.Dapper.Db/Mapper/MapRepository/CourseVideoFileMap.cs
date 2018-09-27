using System;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
    /// <summary>
    /// 数据表映射实体类：视频文件管理 
    /// </summary>
    public sealed class CourseVideoFileMap : BaseClassMapper<CourseVideoFile, Guid>
    {
        public CourseVideoFileMap()
        {
            Table("t_Course_VideoFile");
            Map(x => x.CoverURL).Column("CoverURL");
            Map(x => x.VideoUrl).Column("VideoUrl");
            Map(x => x.Name).Column("Name");
            Map(x => x.Type).Column("Type");
            Map(x => x.Duration).Column("Duration");
            Map(x => x.VideoAlias).Column("VideoAlias");

            this.AutoMap();
        }
    }
}

