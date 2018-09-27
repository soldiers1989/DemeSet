using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.MyCourseModule;
using ZF.Application.WebApiDto.CourseLearnProgressModule;
using ZF.Application.WebApiDto.MyCourseModule;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Dapper.Db.Repository;

namespace ZF.Application.WebApiAppService.CourseLearnProgressModule
{
    public class CourseLearnProgressAppService : BaseAppService<CourseLearnProgress>
    {
        private readonly ICourseLearnProgressRepository _repository;

        private readonly MyVideoWatchAppService _myVideoWatchAppService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public CourseLearnProgressAppService(ICourseLearnProgressRepository repository, MyVideoWatchAppService myVideoWatchAppService) : base(repository)
        {
            _repository = repository;
            _myVideoWatchAppService = myVideoWatchAppService;
        }

        /// <summary>
        /// 添加观看记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut Add(CourseLearnProgressModelInput input)
        {
            CourseLearnProgress model;
            var str = @"SELECT  COUNT(1)
        FROM    T_Course_LearnProgress
        WHERE   VideoId = @VideoId
                AND UserId = @UserID";
            var dy = new DynamicParameters();
            dy.Add(":UserId", input.UserId, DbType.String);
            dy.Add(":VideoId", input.VideoId, DbType.String);

            if (!IfExist(new CourseLearnProgressModelInput { VideoId = input.VideoId, UserId = input.UserId }))
            {
                _myVideoWatchAppService.AddOrEdit(new MyVideoWatchInput
                {
                    UserId = input.UserId,
                    VideoId = input.VideoId,
                    WatchTime = DateTime.Now
                });
            }
            if (Db.QueryFirstOrDefault<int>(str, dy) > 0)
            {
                return new MessagesOutPut { Success = false, Message = "新增失败1" };
            }
            model = input.MapTo<CourseLearnProgress>();
            model.Id = Guid.NewGuid().ToString();
            model.UserId = input.UserId;
            model.CourseId = input.CourseId;
            model.ChapterId = input.ChapterId;
            model.VideoId = input.VideoId;
            model.State = input.State;
            _repository.InsertGetId(model);

            return new MessagesOutPut { Success = true, Message = "新增成功" };
        }

        /// <summary>
        /// 更新观看视频状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool UpdateState(CourseLearnProgressModelInput input)
        {
            var sql = " update t_Course_LearnProgress set State=@State WHERE UserId=@UserId AND CourseId=@CourseId AND ChapterId=@ChapterId and VideoId=@VideoId ";
            var parameters = new DynamicParameters();
            parameters.Add(":UserId", input.UserId, DbType.String);
            parameters.Add(":CourseId", input.CourseId, DbType.String);
            parameters.Add(":ChapterId", input.ChapterId, DbType.String);
            parameters.Add(":State", input.State, DbType.Int16);
            parameters.Add(":VideoId", input.VideoId, DbType.String);
            return Db.ExecuteScalar<int>(sql, parameters) > 0;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string GetVideoLearnState(CourseLearnProgressModelInput input)
        {
            var sql = string.Format("select State FROM T_Course_LearnProgress where 1=1 and UserId='{0}' and CourseId='{1}' and VideoId='{2}'", input.UserId, input.CourseId, input.VideoId);
            var result = Db.ExecuteScalar<string>(sql, null);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public CourseLearnProgressModelOutput GetProgress(CourseLearnProgressModelInput input)
        {
            var sql = "SELECT d.CourseId,d.TotalLearnCount,c.UserId,c.LearnedCount FROM (SELECT a.CourseId,COUNT(b.Id) AS TotalLearnCount FROM  dbo.t_Course_Chapter a LEFT JOIN dbo.t_Course_Video b ON a.Id=b.ChapterId  WHERE a.IsDelete = 0 AND b.IsDelete = 0   GROUP BY a.CourseId)d LEFT JOIN( SELECT UserId, CourseId, COUNT( *) AS LearnedCount FROM dbo.T_Course_LearnProgress GROUP BY UserId, CourseId ) c ON d.CourseId = c.CourseId WHERE c.UserId = @UserId AND d.CourseId=@CourseId ";
            var parameters = new DynamicParameters();
            parameters.Add(":UserId", input.UserId, DbType.String);
            parameters.Add(":CourseId", input.CourseId, DbType.String);
            var model = Db.QueryFirstOrDefault<CourseLearnProgressModelOutput>(sql, parameters);
            if (model != null)
            {
                var strsql = " SELECT TOP 1  d.VideoName,c.VideoId  FROM t_My_VideoWatch  c LEFT JOIN dbo.t_Course_Video d ON c.VideoId = d.Id WHERE UserId = @UserId AND VideoId IN ( SELECT b.Id FROM dbo.t_Course_Chapter a right JOIN dbo.t_Course_Video b ON a.id = b.ChapterId WHERE a.CourseId = @CourseID )ORDER BY WatchTime DESC";
                var obj = Db.QueryFirstOrDefault<CourseLearnProgressModelOutput>(strsql, parameters);
                if (obj != null)
                {
                    model.LastWatch = obj.VideoName;
                    model.VideoId = obj.VideoId;
                }
            }
            return model;
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool IfExist(CourseLearnProgressModelInput input)
        {
            var sql = "select count(*) from t_My_VideoWatch  where UserId=@UserId and VideoId =@VideoId ";
            var parameters = new DynamicParameters();
            parameters.Add(":UserId", input.UserId, DbType.String);
            parameters.Add(":VideoId", input.VideoId, DbType.String);
            return Db.ExecuteScalar<int>(sql, parameters) > 0;
        }
    }
}
