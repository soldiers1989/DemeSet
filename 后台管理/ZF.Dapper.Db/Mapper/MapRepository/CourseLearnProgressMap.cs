using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
   public class CourseLearnProgressMap: BaseClassMapper<CourseLearnProgress, Guid>
    {
        public CourseLearnProgressMap( )
        {
            Table( "T_Course_LearnProgress" );

            Map( x => x.CourseId ).Column( "CourseId" );
            Map( x => x.ChapterId ).Column( "ChapterId" );
            Map( x => x.UserId ).Column( "UserId" );
            Map( x => x.VideoId ).Column( "VideoId" );
            Map( x => x.State ).Column( "State" );
            this.AutoMap( );
        }
    }
}
