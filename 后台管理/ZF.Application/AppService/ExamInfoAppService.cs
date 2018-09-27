using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;

namespace ZF.Application.AppService
{
    public class ExamInfoAppService : BaseAppService<ExamInfo>
    {
        private readonly IExamInfoRepository _respository;
        private readonly OperatorLogAppService _operatorLogAppService;
        public ExamInfoAppService( IExamInfoRepository respository, OperatorLogAppService operatorLogAppService ) : base( respository )
        {
            _respository = respository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：资讯,帮助管理表 
        /// </summary>
        public List<ExamInfoOutput> GetList( ExamInfoListInput input, out int count )
        {
            const string sql = "select  * ";
            var strSql = new StringBuilder( " from t_ExamInfo   where 1=1  " );
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters( );
            if ( input.IfUse > -1 )
            {
                strSql.Append( " and Ifuse = @IfUse " );
                dynamicParameters.Add( ":IfUse", input.IfUse, DbType.Int16 );
            }
            //if ( !string.IsNullOrWhiteSpace( input.ClassId ) )
            //{
            //    strSql.Append( " and a.ClassId = @ClassId " );
            //    dynamicParameters.Add( ":ClassId", input.ClassId, DbType.String );
            //}
            //if ( input.Type.HasValue )
            //{
            //    strSql.Append( " and a.Type = @Type " );
            //    dynamicParameters.Add( ":Type", input.Type, DbType.String );
            //}
            //if ( input.IsIndex.HasValue )
            //{
            //    strSql.Append( " and a.IsIndex = @IsIndex " );
            //    dynamicParameters.Add( ":IsIndex", input.IsIndex, DbType.String );
            //}
            //if ( input.IsTop.HasValue )
            //{
            //    strSql.Append( " and a.IsTop = @IsTop " );
            //    dynamicParameters.Add( ":IsTop", input.IsTop, DbType.String );
            //}
            //if ( !string.IsNullOrWhiteSpace( input.Title ) )
            //{
            //    strSql.Append( " and a.Title like @Title " );
            //    dynamicParameters.Add( ":Title", '%' + input.Title + '%', DbType.String );
            //}
            count = Db.ExecuteScalar<int>( sqlCount + strSql, dynamicParameters );
            var list = Db.QueryList<ExamInfoOutput>( GetPageSql( sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord ), dynamicParameters );
            return list;
        }

        /// <summary>
        /// 新增实体  资讯,帮助管理表
        /// </summary>
        public MessagesOutPut AddOrEdit( ExamInfoInput input )
        {
            ExamInfo model;
            if ( !string.IsNullOrEmpty( input.Id ) )
            {
                model = _respository.Get( input.Id );
                #region 修改逻辑
                model.Id = Guid.NewGuid( ).ToString( );

                model.Description = input.Description;
                model.SignUp = input.SignUp;
                model.Content = input.Content;
                model.BeginTime = input.BeginTime;
                model.EndTime = input.EndTime;
                model.AddTime = DateTime.Now;
                model.IfUse = input.IfUse;

                model.ScoreManage = input.ScoreManage;
                model.TextBox = input.TextBox;
                #endregion
                _respository.Update( model );
                _operatorLogAppService.Add( new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = ( int )Model.ExamInfo,
                    OperatorType = ( int )OperatorType.Edit,
                    Remark = "修改备考信息表:" + model.Description
                } );

                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<ExamInfo>( );
            model.Id = Guid.NewGuid( ).ToString( );

            model.Id = Guid.NewGuid( ).ToString( );

            model.Description = input.Description;
            model.SignUp = input.SignUp;
            model.Content = input.Content;
            model.BeginTime = input.BeginTime;
            model.EndTime = input.EndTime;
            model.AddTime = DateTime.Now;
            model.IfUse = input.IfUse;
            model.ScoreManage = input.ScoreManage;
            model.TextBox = input.TextBox;
            var keyId = _respository.InsertGetId( model );
            _operatorLogAppService.Add( new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = ( int )Model.AfficheHelp,
                OperatorType = ( int )OperatorType.Add,
                Remark = "新增备考信息:" + model.Description
            } );
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        public bool UpdateState( string id )
        {
            var sql = @" UPDATE dbo.t_ExamInfo SET IfUse=1 WHERE Id=@Id ";
            var dy = new DynamicParameters( );
            dy.Add( ":Id",id,DbType.String);
            return Db.ExecuteScalar<int>( sql, dy )>0;
        }
    }

}
