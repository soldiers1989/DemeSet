
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
    /// 数据表实体应用服务现实：视频文件管理 
    /// </summary>
    public class CourseVideoFileAppService : BaseAppService<CourseVideoFile>
    {
        private readonly ICourseVideoFileRepository _iCourseVideoFileRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iCourseVideoFileRepository"></param>
        public CourseVideoFileAppService(ICourseVideoFileRepository iCourseVideoFileRepository) : base(iCourseVideoFileRepository)
        {
            _iCourseVideoFileRepository = iCourseVideoFileRepository;
        }

        /// <summary>
        /// 查询列表实体：视频文件管理 
        /// </summary>
        public List<CourseVideoFileOutput> GetList(CourseVideoFileListInput input, out int count)
        {
            const string sql = "select  a.* ";
            var strSql = new StringBuilder(" from t_Course_VideoFile  a  where 1=1  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.Name))
            {
                strSql.Append(" and a.Name like  @Name ");
                dynamicParameters.Add(":Name", '%' + input.Name + '%', DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.VideoAlias))
            {
                strSql.Append(" and a.VideoAlias like  @VideoAlias ");
                dynamicParameters.Add(":VideoAlias", '%' + input.VideoAlias + '%', DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<CourseVideoFileOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 新增实体  视频文件管理
        /// </summary>
        public MessagesOutPut AddOrEdit(CourseVideoFileInput input)
        {
            CourseVideoFile model;
            model = input.MapTo<CourseVideoFile>();
            var keyId = _iCourseVideoFileRepository.InsertGetId(model);
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 新增实体  视频文件管理
        /// </summary>
        public MessagesOutPut Update(CourseVideoFileInput input)
        {
            var strSql = new StringBuilder(" select * from t_Course_VideoFile  a  where 1=1  ");
            var dynamicParameters = new DynamicParameters();
            strSql.Append(" and a.Id = @Id ");
            dynamicParameters.Add(":Id", input.Id, DbType.String);
            var model = Db.QueryFirstOrDefault<CourseVideoFile>(strSql.ToString(), dynamicParameters);
            if (model != null)
            {
                model.CoverURL = input.CoverURL;
                model.VideoUrl = input.VideoUrl;
                model.Duration = input.Duration;
            }
            _iCourseVideoFileRepository.Update(model);
            return new MessagesOutPut { Success = true, Message = "更新成功!" };
        }


    }
}

