using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
   public sealed class BaseTagMiddleMap:BaseClassMapper<Base_TagMiddle,Guid>
    {
        public BaseTagMiddleMap( ) {
            Table( "t_Base_TagMiddle" );

            Map( x => x.ModelId ).Column( "ModelId" );
            Map( x => x.TagId ).Column( "TagId" );
            this.AutoMap( );
        }
    }
}
