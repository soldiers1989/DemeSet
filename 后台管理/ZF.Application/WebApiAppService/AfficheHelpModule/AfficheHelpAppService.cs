using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.WebApiDto.AfficheHelpModule;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.AfficheHelpModule
{
    public class AfficheHelpAppService : BaseAppService<AfficheHelp>
    {
        private static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];

        private readonly IAfficheHelpRepository _iAfficheHelpRepository;

        public AfficheHelpAppService( IAfficheHelpRepository repository ) : base( repository ) {
            _iAfficheHelpRepository = repository;
        }


        /// <summary>
        /// 查询列表实体：资讯,帮助管理表 
        /// </summary>
        public List<AfficheHelpOutput> GetList( AfficheHelpListInput input, out int count )
        {
            const string sql = "select  a.*,b.Name,c.DataTypeName ";
            var strSql = new StringBuilder( " from T_Base_AfficheHelp  a left join t_Base_Basedata  b on a.ClassId=b.Id left join t_Base_DataType c on a.BigClassId=c.Id   where a.IsDelete=0  " );
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters( );
            if ( !string.IsNullOrWhiteSpace( input.BigClassId ) )
            {
                strSql.Append( " and a.BigClassId = @BigClassId " );
                dynamicParameters.Add( ":BigClassId", input.BigClassId, DbType.String );
            }
            if ( !string.IsNullOrWhiteSpace( input.ClassId ) )
            {
                strSql.Append( " and a.ClassId = @ClassId " );
                dynamicParameters.Add( ":ClassId", input.ClassId, DbType.String );
            }
            if ( input.Type.HasValue )
            {
                strSql.Append( " and a.Type = @Type " );
                dynamicParameters.Add( ":Type", input.Type, DbType.String );
            }
            if ( input.IsIndex.HasValue )
            {
                strSql.Append( " and a.IsIndex = @IsIndex " );
                dynamicParameters.Add( ":IsIndex", input.IsIndex, DbType.String );
            }
            if ( input.IsTop.HasValue )
            {
                strSql.Append( " and a.IsTop = @IsTop " );
                dynamicParameters.Add( ":IsTop", input.IsTop, DbType.String );
            }
            if ( !string.IsNullOrWhiteSpace( input.Title ) )
            {
                strSql.Append( " and a.Title like @Title " );
                dynamicParameters.Add( ":Title", '%' + input.Title + '%', DbType.String );
            }
            count = Db.ExecuteScalar<int>( sqlCount + strSql, dynamicParameters );
            var list = Db.QueryList<AfficheHelpOutput>( GetPageSql( sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord ), dynamicParameters );
            return list;
        }
    }
}
