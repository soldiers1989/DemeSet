using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Application.WebApiDto.CourseTeacherModule;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.CourseOnTeacherModule
{
    /// <summary>
    /// 
    /// </summary>
   public class CourseOnTeacherAppService:BaseAppService<CourseOnTeachers>
    {
        private readonly string defaultDomain = ConfigurationManager.AppSettings["DefuleDomain"];
        private readonly ICourseOnTeachersRepository _repository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public CourseOnTeacherAppService( ICourseOnTeachersRepository repository):base(repository) {
            _repository = repository;
        }

        /// <summary>
        /// 获取讲师个人信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public CourseOnTeachers GetOne( IdInput input ) {
            return _repository.Get( input.Id);
        }


        /// <summary>
        /// 查询列表实体：CourseOnTeachers 
        /// </summary>
        public List<CourseOnTeachersOutput> GetList( CourseOnTeachersListInput input, out int count )
        {
            const string sql = "select  a.*,b.ProjectName ";
            var strSql = new StringBuilder( " from t_Course_OnTeachers  a  left join dbo.t_Base_Project b ON a.ProjectId  =b.Id  where a.IsDelete=0  " );
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters( );
            if ( !string.IsNullOrWhiteSpace( input.TeachersName ) )
            {
                strSql.Append( " and a.TeachersName like @TeachersName " );
                dynamicParameters.Add( ":TeachersName", "%" + input.TeachersName + "%", DbType.String );
            }
            if ( input.IsFamous > -1 ) {
                strSql.Append( " and a.IsFamous=@IsFamous ");
                dynamicParameters.Add( ":IsFamous",input.IsFamous,DbType.Int16);
            }
            if ( !string.IsNullOrEmpty( input.ProjectId ) )
            {
                strSql.Append( "and a.ProjectId like @ProjectId " );
                dynamicParameters.Add( ":ProjectId", "%" + input.ProjectId + "%", DbType.String );
            }
            count = Db.ExecuteScalar<int>( sqlCount + strSql, dynamicParameters );
            var list = Db.QueryList<CourseOnTeachersOutput>( GetPageSql( sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord ), dynamicParameters );
            foreach ( var item in list ) {
                item.TeacherPhoto = defaultDomain + "/" + item.TeacherPhoto;
            }
            return list;
        }
    }
}
