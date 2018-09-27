using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
   public class UserDiscountCardMap:BaseClassMapper<User_Discount_Card,Guid>
    {
        public UserDiscountCardMap( ) {

            Table( "t_User_Discount_Card" );

            Map( x => x.UserId ).Column( "UserId" );
            Map( x => x.CardId ).Column( "CardId" );
            Map( x => x.AddTime ).Column( "AddTime" );
            Map( x => x.IfUse ).Column( "IfUse" );
            Map( x => x.Type ).Column( "Type" );
            this.AutoMap( );
        }
    }
}
