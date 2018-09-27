
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using Dapper;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Application.WebApiDto.MyCourseModule;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;

namespace ZF.Application.WebApiAppService.MyCourseModule
{
    /// <summary>
    /// 数据表实体应用服务现实：视频观看历史记录 
    /// </summary>
    public class MyVideoWatchAppService : BaseAppService<MyVideoWatch>
    {
        private readonly IMyVideoWatchRepository _iMyVideoWatchRepository;

        private readonly RegisterUserAppService _registerUserAppService;


        private readonly SubjectAppService _subjectAppService;

        /// <summary>
        /// 
        /// </summary>
        public string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iMyVideoWatchRepository"></param>
        /// <param name="registerUserAppService"></param>
        public MyVideoWatchAppService(IMyVideoWatchRepository iMyVideoWatchRepository, RegisterUserAppService registerUserAppService, SubjectAppService subjectAppService) : base(iMyVideoWatchRepository)
        {
            _iMyVideoWatchRepository = iMyVideoWatchRepository;
            _registerUserAppService = registerUserAppService;
            _subjectAppService = subjectAppService;
        }

        /// <summary>
        /// 查询列表实体：视频观看历史记录 
        /// </summary>
        public List<MyVideoWatchOutput> GetList(MyVideoWatchListInput input, out int count)
        {
            string sql = @"SELECT  a.* ,
                b.VideoName ,
                b.VideoUrl ,
                c.Id AS chapterId ,
                c.CapterName chapterName ,
                d.Id courseId ,
                d.CourseName CourseName,
               '" + DefuleDomain + @"/'+ d.CourseIamge CourseIamge
        FROM    t_My_VideoWatch a
                LEFT JOIN dbo.t_Course_Video b ON a.VideoId = b.Id
                LEFT JOIN dbo.t_Course_Chapter c ON b.ChapterId = c.Id
                LEFT JOIN dbo.t_Course_Info d ON c.CourseId = d.Id
        WHERE   a.IsDelete = 0 ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(input.UserId))
            {
                sql += " and a.UserId=@UserId";
                dynamicParameters.Add(":UserId", input.UserId, DbType.String);
            }
            if (!string.IsNullOrEmpty(input.query))
            {
                sql += " and b.VideoName like  @query";
                dynamicParameters.Add(":query", '%' + input.query + '%', DbType.String);
            }
            count = Db.QueryFirstOrDefault<int>(" select count(1) from (" + sql + ")  b", dynamicParameters);
            return Db.QueryList<MyVideoWatchOutput>(GetPageSql(sql, input.Page, input.Rows, "WatchTime desc"), dynamicParameters);
        }

        /// <summary>
        /// 新增实体  视频观看历史记录
        /// </summary>
        public MessagesOutPut AddOrEdit(MyVideoWatchInput input)
        {
            if (string.IsNullOrEmpty(input.VideoId) || string.IsNullOrEmpty(input.UserId))
            {
                return new MessagesOutPut { Success = false };
            }
            string sql = @"select a.* from t_My_VideoWatch a WHERE   a.IsDelete = 0 ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(input.UserId))
            {
                sql += " and a.UserId=@UserId";
                dynamicParameters.Add(":UserId", input.UserId, DbType.String);
            }
            if (!string.IsNullOrEmpty(input.VideoId))
            {
                sql += " and a.VideoId=@VideoId";
                dynamicParameters.Add(":VideoId", input.VideoId, DbType.String);
            }
            var model = Db.QueryFirstOrDefault<MyVideoWatch>(sql, dynamicParameters) ?? new MyVideoWatch();
            if (!string.IsNullOrEmpty(model.Id))
            {
                model.WatchTime = DateTime.Now;
                _iMyVideoWatchRepository.Update(model);
                return new MessagesOutPut { Success = true };
            }
            model = input.MapTo<MyVideoWatch>();
            model.Id = Guid.NewGuid().ToString();
            model.WatchTime = DateTime.Now;
            _iMyVideoWatchRepository.InsertGetId(model);
            return new MessagesOutPut { Success = true };
        }

        /// <summary>
        /// 更新我的科目
        /// </summary>
        /// <param name="subjectId">科目编号</param>
        /// <param name="userId">用户编号</param>
        /// <returns></returns>
        public bool UpdateSubject(string subjectId, string userId)
        {
            if (!string.IsNullOrEmpty(subjectId))
            {
                var model = _registerUserAppService.GetOne(userId);
                if (model != null)
                {
                    model.SubjectId = subjectId;
                    _registerUserAppService.Update(model);
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}

