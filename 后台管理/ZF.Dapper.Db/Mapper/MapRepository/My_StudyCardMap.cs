using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
   public class My_StudyCardMap: BaseClassMapper<My_StudyCard, Guid>
    {
        public My_StudyCardMap( ) {
            Table( "t_My_StudyCard" );

            Map( x => x.UserId ).Column( "UserId" );
            Map( x => x.Amount ).Column( "Amount" );
            Map( x => x.AddTime ).Column( "AddTime" );
            this.AutoMap( );
        }
    }
}
