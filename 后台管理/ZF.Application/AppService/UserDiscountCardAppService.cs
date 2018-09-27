using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.AppService
{
   public class UserDiscountCardAppService:BaseAppService<User_Discount_Card>
    {
        public readonly IUserDiscountCardRepository _repository;

        public UserDiscountCardAppService( IUserDiscountCardRepository repository ) : base( repository ) {
            _repository = repository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<UserDiscountCardOutput> GetList( UserDiscountCardListInput input,out int count) {
            const string sql = " select distinct a.Id,a.UserId,b.TelphoneNum,MAX(e.AddTime) AS UseTime,a.AddTime ";
            var strSql = new StringBuilder( " FROM dbo.t_User_Discount_Card a LEFT JOIN dbo.t_Base_RegisterUser b ON a.UserId = b.Id LEFT JOIN dbo.t_Order_Card d ON a.CardId=d.CardCode LEFT JOIN dbo.t_Order_Sheet e ON d.OrderNo = e.OrderNo    where a.IfUse=1 AND a.CardId=@CardId " );
            var dynamicParameters = new DynamicParameters( );
            
            const string sqlCount = "select count(*) ";
            if ( input != null )
            {
                dynamicParameters.Add( ":CardId",input.CardId,DbType.String);
                if ( !string.IsNullOrWhiteSpace( input.TelphoneNum ) )
                {
                    strSql.Append( " and b.TelphoneNum like  @TelphoneNum " );
                    dynamicParameters.Add( ":TelphoneNum", "%"+input.TelphoneNum+"%", DbType.String );
                }
                strSql.Append( " and a.Type=@Type ");
                dynamicParameters.Add( ":Type",input.Type,DbType.Int16);
            }
            strSql.Append( " GROUP BY a.Id,a.UserId,b.TelphoneNum,a.AddTime " );
            count = Db.ExecuteScalar<int>( sqlCount + strSql, dynamicParameters );
            var list = Db.QueryList<UserDiscountCardOutput>( GetPageSql( sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, "AddTime", "Desc" ), dynamicParameters );
            return list;
        }
    }
}
