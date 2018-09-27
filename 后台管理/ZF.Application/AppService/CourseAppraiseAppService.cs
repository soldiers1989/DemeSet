using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：CourseAppraise 
    /// </summary>
    public class CourseAppraiseAppService : BaseAppService<CourseAppraise>
    {
        private readonly ICourseAppraiseRepository _iCourseAppraiseRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iCourseAppraiseRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public CourseAppraiseAppService( ICourseAppraiseRepository iCourseAppraiseRepository, OperatorLogAppService operatorLogAppService ) : base( iCourseAppraiseRepository )
        {
            _iCourseAppraiseRepository = iCourseAppraiseRepository;
            _operatorLogAppService = operatorLogAppService;
        }


        /// <summary>
        /// 查询列表实体：CourseAppraise 
        /// </summary>
        public List<CourseAppraiseOutput> GetList( CourseAppraiseListInput input, out int count )
        {
            var dynamicParameters = new DynamicParameters( );
            const string sql = "select  a.*,b.CourseName ";
            var strSql = new StringBuilder( " from t_Course_Appraise  a left join (select Id, CourseName from V_Course_Packcourse_Info ) b on a.CourseId=b.Id  where 1=1 and  isnull(b.CourseName,'')<>'' " );
            const string sqlCount = "select count(*) ";
            if ( !string.IsNullOrWhiteSpace( input.CourseId ) )
            {
                strSql.Append( " and a.CourseId = @CourseId " );
                dynamicParameters.Add( ":CourseId", input.CourseId, DbType.String );
            }
            if (  input.CourseType>=1)
            {
                strSql.Append( " and a.CourseType = @CourseType " );
                dynamicParameters.Add( ":CourseType",input.CourseType,DbType.Int16); 
            }
            count = Db.ExecuteScalar<int>( sqlCount + strSql, dynamicParameters );
            var list = Db.QueryList<CourseAppraiseOutput>( GetPageSql( sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, "AppraiseTime", input.Sord ), dynamicParameters );
            return list;
        }

        /// <summary>
        /// 新增实体  CourseAppraise
        /// </summary>
        public MessagesOutPut AddOrEdit( CourseAppraiseInput input )
        {
            CourseAppraise model;
            model = input.MapTo<CourseAppraise>( );
            model.Id = Guid.NewGuid( ).ToString( );
            model.AppraiseCotent = input.AppraiseCotent;
            model.AppraiseLevel = input.AppraiseLevel;
            model.CourseId = input.CourseId;
            model.CourseType = input.CourseType;
            model.AppraiseIp = GetAddressIp( );
            model.UserId = UserObject.Id;
            model.AppraiseTime = DateTime.Now;
            var keyId = _iCourseAppraiseRepository.InsertGetId( model );
            _operatorLogAppService.Add( new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = ( int )Model.CourseAppraise,
                OperatorType = ( int )OperatorType.Add,
                Remark = string.Format( "新增课程评价:{0}", model.CourseId )
            } );
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut Reply( CourseAppraiseInput input ) {
            var sql = " UPDATE dbo.t_Course_Appraise SET ReplyContent=@ReplyConent ,ReplyTime=@ReplyTime,ReplyAdminUserId=@ReplyAdminUserId where  Id=@Id  ";
            var parameters = new DynamicParameters( );
            parameters.Add( ":ReplyConent",input.ReplyContent,DbType.String);
            parameters.Add( ":ReplyTime",DateTime.Now,DbType.DateTime);
            parameters.Add( ":ReplyAdminUserId",input.ReplyAdminUserId,DbType.String);
            parameters.Add( ":Id",input.Id,DbType.String);
            var result = Db.ExecuteNonQuery( sql, parameters );
            return new MessagesOutPut { Message = "操作成功", Success = true };

        }


        /// <summary>
        /// 更新上下架状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut UpdateState(CourseAppraiseInput input)
        {
            CourseAppraise model;
            model = _iCourseAppraiseRepository.Get(input.Id);
            #region 修改逻辑
            model.Id = input.Id;
            model.AuditStatus = input.AuditStatus;
            #endregion
            _iCourseAppraiseRepository.Update(model);
            return new MessagesOutPut { Success = true, Message = "修改成功!" };
        }
    }
}


	   

