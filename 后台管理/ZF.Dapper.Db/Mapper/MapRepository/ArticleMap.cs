using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
   public sealed class ArticleMap:BaseClassMapper<Article>
    {

        public ArticleMap( )
        {
            Table( "t_Article" );

            Map( x => x.ArticleTitle ).Column( "ArticleTitle" );
            Map( x => x.ArticleContent ).Column( "ArticleContent" );
            Map( x => x.ArticleImage ).Column( "ArticleImage" );
            Map( x => x.AddTime ).Column( "AddTime" );
            Map( x => x.Type ).Column( "Type" );
            Map( x => x.IsDelete ).Column( "IsDelete" );
            this.AutoMap( );
        }
    }
}
