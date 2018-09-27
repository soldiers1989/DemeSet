using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
  public sealed  class BaseTagMap:BaseClassMapper<Base_Tag,Guid>
    {

        public BaseTagMap( )
        {
            Table( "t_Base_Tag" );

            Map( x => x.ModelCode ).Column( "ModelCode" );
            Map( x => x.TagName ).Column( "TagName" );
            Map( x => x.Remark ).Column( "Remark" );
            this.AutoMap( );
        }
    }
}
