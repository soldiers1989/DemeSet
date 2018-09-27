using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
   public class QuestionCheckerMap: BaseClassMapper<QuestionChecker, Guid>
    {
        public QuestionCheckerMap( ) {
            Table( "t_Question_checker" );

            Map( x => x.Content ).Column( "Content" );
            Map( x => x.AddTime ).Column( "AddTime" );
            Map( x => x.UserId ).Column( "UserId" );

            this.AutoMap( );
        }
    }
}
