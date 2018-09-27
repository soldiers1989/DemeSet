using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
   public class PurchaseDiscountMap: BaseClassMapper<PurchaseDiscount, Guid>
    {
        public PurchaseDiscountMap( ) {
            Table( "t_purchase_discount" );

            Map( x => x.TopNum ).Column( "TopNum" );
            Map( x => x.MinusNum ).Column( "MinusNum" );
            Map( x => x.BeginDate ).Column( "BeginDate" );
            Map( x => x.EndDate ).Column( "EndDate" );
            Map( x => x.TargetCourse ).Column( "TargetCourse" );
            this.AutoMap( );
        }
       
    }
}
