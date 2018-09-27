using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Core.Entity;

namespace ZF.Dapper.Db.Mapper.MapRepository
{
   public class ExamInfoMap:BaseClassMapper<ExamInfo,Guid>
    {
        public ExamInfoMap( )
        {
            Table( "t_ExamInfo" );

            Map( x => x.Description ).Column( "Description" );
            Map( x => x.SignUp ).Column( "SignUp" );
            Map( x => x.Content ).Column( "Content" );
            Map( x => x.BeginTime ).Column( "BeginTime" );
            Map( x => x.EndTime ).Column( "EndTime" );
            Map( x => x.ScoreManage ).Column( "ScoreManage" );
            Map( x => x.TextBox ).Column( "TextBox" );
            Map( x => x.IfUse ).Column( "IfUse" );
            Map( x => x.AddTime ).Column( "AddTime" );
            this.AutoMap( );
        }
    }
}
