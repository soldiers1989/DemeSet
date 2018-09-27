using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
   public class OrderCardMap:BaseClassMapper<OrderCard>
    {
        public OrderCardMap( ) {
            Table( "T_Order_Card" );

            Map( x => x.OrderNo ).Column( "OrderNo" );
            Map( x => x.CardCode ).Column( "CardCode" );
            Map(x => x.State).Column("State");
            Map(x => x.CardId).Column("CardId");

            this.AutoMap( );
        }
    }
}
