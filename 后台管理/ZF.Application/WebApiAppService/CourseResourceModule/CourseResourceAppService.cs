using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZF.Application.WebApiDto.CourseResourceModule;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.CourseResourceModule
{
    /// <summary>
    /// 
    /// </summary>
   public class CourseResourceAppService:BaseAppService<CourseResource>
    {
        private static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];
        private readonly ICourseResourceRepository _repository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public CourseResourceAppService( ICourseResourceRepository repository ):base(repository) {
            _repository = repository;
        }


        /// <summary>
        /// 获取课程资源列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<CourseResourceModelOutput> GetList( CourseResourceModelInput input ) {
             string sql = "select  * from t_Course_Resource  where 1 =1 ";
            var dynamicparameter = new DynamicParameters( );
            if ( !string.IsNullOrWhiteSpace( input.CourseId ) ) {
                sql += " and CourseId=@CourseId ";
                dynamicparameter.Add( ":CourseId",input.CourseId,DbType.String);
            }
            var list = Db.QueryList<CourseResourceModelOutput>( sql,dynamicparameter);
            
            foreach ( var item in list )
            {
                item.ResourceUrl = String.Format( DefuleDomain + "/" + item.ResourceUrl + "?attname={0}", item.ResourceUrl) ;
            }
            return list;
        }

    }
}
