using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
  public sealed  class CourseDiscussionMap: FullAuditEntityClassMapper<CourseDiscussion, Guid>
    {
        public CourseDiscussionMap( ) {
            Table( "t_Course_Discussion" );

            Map( x => x.Id ).Column( "Id" );
            Map( x => x.CourseId ).Column( "CourseId" );
            Map( x => x.DiscussContent ).Column( "DiscussContent" );
            Map( x => x.Type ).Column( "Type" );
            Map( x => x.AddUserId ).Column( "AddUserId" );
            Map( x => x.AddTime ).Column( "AddTime" );
            Map( x => x.ParentId ).Column( "ParentId" );
            Map( x => x.IsDelete ).Column( "IsDelete" );
            this.AutoMap( );
        }
    }
}
