using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;
using ZF.Application.WebApiDto.CourseVideoModule;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.CourseVideoModule
{
    /// <summary>
    /// 
    /// </summary>
    public  class CourseVideoAppService : BaseAppService<CourseVideo>
    {
        private readonly ICourseVideoRepository _repository;
        private static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public CourseVideoAppService( ICourseVideoRepository repository):base(repository) {
            _repository = repository;
        }

        /// <summary>
        /// 获取指定章节视频信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public CourseVideo GetOne( CourseVideoModelInput input ) {
            string strSql = string.Empty;
            var parameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(input.Code))
            {
                strSql = @"select * from t_Course_Video where (Code=@Code or Id=@Code) ";
                parameters.Add(":CourseId", input.CourseId, DbType.String);
                parameters.Add(":Code", input.Code, DbType.String);
            }
            else
            {
                strSql = "select * from t_Course_Video where Id=@Id";
                parameters.Add(":Id", input.Id, DbType.String);
            }
            var model= Db.QueryFirstOrDefault<CourseVideo>(strSql, parameters);
            model.VideoUrl = model.VideoUrl;
            return model;
        }

        public CourseVideo GetVideoInfo( IdInput input )
        {
            var sql = string.Format( @" select * from  t_Course_VideoFile where Id='{0}'", input.Id );
            return Db.QueryFirstOrDefault<CourseVideo>( sql, null );
        }
    }
}
