using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper
{
   public class DiscountCardMap:BaseClassMapper<Discount_Card, Guid>
    {
        public DiscountCardMap( )
        {
            Table( "t_discount_card" );

            Map( x => x.CardCode ).Column( "CardCode" );
            Map( x => x.CardName ).Column( "CardName" );
            Map( x => x.Amount ).Column( "Amount" );
            Map( x => x.BeginDate ).Column( "BeginDate" );
            Map( x => x.EndDate ).Column( "EndDate" );
            Map( x => x.TargetCourse ).Column( "TargetCourse");
            this.AutoMap( );
        }
    }
}
