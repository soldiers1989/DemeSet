using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.WebApiDto.CoursediscussionModule;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.CourseDiscussionModule
{
    /// <summary>
    /// 
    /// </summary>
    public class CourseDiscussionAppService : BaseAppService<CourseDiscussion>
    {
        private static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];
        private static ICourseDiscussionRepository _repository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public CourseDiscussionAppService( ICourseDiscussionRepository repository):base(repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 讨论列表
        /// </summary>
        /// <returns></returns>
        public List<CourseDiscussionModelOutput> GetList( CourseDiscussionModelInput input ,out int count) {

            var sql = "SELECT a.*,b.UserName,b.NickNamw,b.HeadImage ";
            var sqlCount = " select count(*) ";
            var sqlFrom = " FROM dbo.t_Course_Discussion a LEFT JOIN dbo.t_Base_RegisterUser b ON a.AddUserId=b.Id  where 1=1 and IsDelete=0 and isnull(ParentId,'')<>'' ";
            var parameters = new DynamicParameters( );
            if ( String.IsNullOrWhiteSpace( input.Type.ToString() ) ) {
                sql += " and a.Type=@Type ";
                parameters.Add( ":Type",input.Type,DbType.Int16);
            }
            count = Db.ExecuteScalar<int>(sqlCount+sqlFrom,parameters);
            var list = Db.QueryList<CourseDiscussionModelOutput>( GetPageSql( sql+sqlFrom, parameters, input.Page, input.Rows, "AddTime", "desc" ), parameters );
            for ( var i = 0; i < list.Count; i++ ) {
                var model = list[i];
                var subSql = " SELECT a.*,b.UserName,b.NickNamw,b.HeadImage FROM dbo.t_Course_Discussion a LEFT JOIN dbo.t_Base_RegisterUser b ON a.AddUserId=b.Id WHERE a.ParentId='"+model.Id+"'";
                model.SubDiscussion = Db.QueryList<CourseDiscussionModelOutput>(subSql );
            }
            return list;
        }
    }
}
