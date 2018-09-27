
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
    /// 数据表实体应用服务现实：CourseVideo 
    /// </summary>
    public class CourseVideoAppService : BaseAppService<CourseVideo>
    {
        private readonly ICourseVideoRepository _iCourseVideoRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;
        private readonly FileRelationshipAppService _fileRelationshipAppService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iCourseVideoRepository"></param>
        /// <param name="operatorLogAppService"></param>
        /// <param name="fileRelationshipAppService"></param>
        public CourseVideoAppService(ICourseVideoRepository iCourseVideoRepository, OperatorLogAppService operatorLogAppService, FileRelationshipAppService fileRelationshipAppService) : base(iCourseVideoRepository)
        {
            _iCourseVideoRepository = iCourseVideoRepository;
            _operatorLogAppService = operatorLogAppService;
            _fileRelationshipAppService = fileRelationshipAppService;
        }

        /// <summary>
        /// 查询列表实体：CourseVideo 
        /// </summary>
        public List<CourseVideoOutput> GetLists(CourseVideoListInput input, out int count)
        {
            const string sql = "select  a.* ";
            const string sqlCount = "select count(*) ";
            var strSql = new StringBuilder(" from t_Course_Video  a  where a.IsDelete=0  ");
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.VideoName))
            {
                strSql.Append(" and a.VideoName like @VideoName ");
                dynamicParameters.Add(":VideoName", "%" + input.VideoName + "%", DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.QcodeTitle))
            {
                strSql.Append(" and a.QcodeTitle like @QcodeTitle ");
                dynamicParameters.Add(":QcodeTitle", "%" + input.QcodeTitle + "%", DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.Code))
            {
                strSql.Append(" and a.Code like @Code ");
                dynamicParameters.Add(":Code", "%" + input.Code + "%", DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.ChapterId))
            {
                strSql.Append(" and a.ChapterId = @ChapterId ");
                dynamicParameters.Add(":ChapterId", input.ChapterId, DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<CourseVideoOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 查询列表实体：CourseVideo 
        /// </summary>
        public List<CourseVideoOutput> GetList(CourseVideoListInput input)
        {
            const string sql = "select  a.* ";
            var strSql = new StringBuilder(" from t_Course_Video  a  where a.IsDelete=0  ");
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.VideoName))
            {
                strSql.Append(" and a.VideoName like @VideoName ");
                dynamicParameters.Add(":VideoName", "%" + input.VideoName + "%", DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.ChapterId))
            {
                strSql.Append(" and a.ChapterId = @ChapterId ");
                dynamicParameters.Add(":ChapterId", input.ChapterId, DbType.String);
            }
            strSql.Append("  Order by OrderNo,AddTime ASC  ");
            var list = Db.QueryList<CourseVideoOutput>(sql + strSql, dynamicParameters);
            return list;
        }

        /// <summary>
        /// 新增实体  CourseVideo
        /// </summary>
        public MessagesOutPut AddOrEdit(CourseVideoInput input)
        {
            CourseVideo model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iCourseVideoRepository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;
                model.VideoName = input.VideoName;
                model.VideoUrl = input.VideoUrl;
                model.ChapterId = input.ChapterId;
                model.VideoLongTime = input.VideoLongTime;
                model.VideoLong = input.VideoLong;
                model.VideoContent = input.VideoContent;
                model.IsTaste = input.IsTaste;
                model.TasteLongTime = input.TasteLongTime;
                model.StudyCount = input.StudyCount;
                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                model.Code = input.Code;
                model.QcodeTitle = input.QcodeTitle;
                model.OrderNo = input.OrderNo;
                model.TasteLongTime2 = input.TasteLongTime2;
                #endregion
                _iCourseVideoRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.CourseVideo,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = string.Format("修改课程视频:{0}", model.VideoName)
                });

                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<CourseVideo>();
            model.Id = Guid.NewGuid().ToString();
            model.VideoName = input.VideoName;
            model.VideoUrl = input.VideoUrl;
            model.ChapterId = input.ChapterId;
            model.VideoLong = input.VideoLong;
            model.VideoLongTime = input.VideoLongTime;
            model.VideoContent = input.VideoContent;
            model.IsTaste = input.IsTaste;
            model.TasteLongTime = input.TasteLongTime;
            model.StudyCount = input.StudyCount;
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            model.QcodeTitle = input.QcodeTitle;
            model.OrderNo = input.OrderNo;
            model.TasteLongTime2 = input.TasteLongTime2;
            var keyId = _iCourseVideoRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.CourseVideo,
                OperatorType = (int)OperatorType.Add,
                Remark = string.Format("新增课程视频:{0}", model.VideoName)
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }



        /// <summary>
        /// 新增实体  CourseVideo
        /// </summary>
        public MessagesOutPut UpdateVideoTasteLongTime(CourseVideoInput input)
        {
            var strSql = @" UPDATE dbo.t_Course_Video
 SET    TasteLongTime = @TasteLongTime
 WHERE  Id IN ( SELECT  a.Id
                FROM    dbo.t_Course_Video a
                        LEFT JOIN t_Course_VideoFile b ON a.VideoUrl = b.Id
                WHERE   a.ChapterId = @ChapterId
                        AND a.IsTaste = 1 AND b.Duration>@TasteLongTime )";
            var parameters = new DynamicParameters();
            parameters.Add(":ChapterId", input.ChapterId, DbType.String);
            parameters.Add(":TasteLongTime", input.TasteLongTime, DbType.Int32);


            var count1 = Db.QueryFirstOrDefault<int>(@"select count(1)  FROM   dbo.t_Course_Video a
                        LEFT JOIN t_Course_VideoFile b ON a.VideoUrl = b.Id
                WHERE   a.ChapterId = @ChapterId
                        AND a.IsTaste = 1 AND b.Duration > @TasteLongTime  ", parameters);
            if (count1 > 0)
            {
                return new MessagesOutPut { Success = true, Message = "批量修改失败,存在视频可播放时长小于当前修改可试听时长!" };
            }
            int count = Db.ExecuteNonQuery(strSql, parameters);
            return new MessagesOutPut { Success = true, Message = "批量修改成功" + count + "条数据的视频试听时长为" + input.TasteLongTime + "分钟!" };
        }

        /// <summary>
        /// 是否已维护章节视频信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool HasVideo(CourseVideoInput input)
        {
            const string sql = " select count(*) FROM dbo.t_Course_Video WHERE ChapterId=@ChapterId ";
            var parameters = new DynamicParameters();
            parameters.Add(":ChapterId", input.ChapterId, DbType.String);
            return Db.ExecuteScalar<int>(sql, parameters) > 0;
        }

        /// <summary>
        /// 是否已维护章节视频信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public CourseVideoOutput GetOne(IdInput input)
        {
            const string sql = @" SELECT  a.*,b.VideoAlias VideoName1
FROM    dbo.t_Course_Video a
        LEFT JOIN t_Course_VideoFile b ON a.VideoUrl = b.Id
WHERE   a.Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add(":Id", input.Id, DbType.String);
            return Db.QueryFirstOrDefault<CourseVideoOutput>(sql, parameters);
        }


        /// <summary>
        /// 判断视频文件是否被使用
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public bool Exsit(string ids)
        {
            string sql = "select count(*) FROM dbo.t_Course_Video WHERE 1=1 and VideoUrl in (@ids) ";
            var parameters = new DynamicParameters();
            parameters.Add(":ids", ids, DbType.String);
            return Db.ExecuteScalar<int>(sql, parameters) > 0;
        }

        /// <summary>
        /// 判断视频编号是否唯一
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public MessagesOutPut VideoIsOnlyOne(CourseVideoInput input)
        {
            string sql = " select * from t_Course_Video where IsDelete=0  and Code=@Code ";

            var parament = new DynamicParameters();
            parament.Add(":Code", input.Code, DbType.String);
            if (!string.IsNullOrEmpty(input.Id))
            {
                sql += " and Id<>@Id ";
                parament.Add(":Id", input.Id, DbType.String);
            }
            var model = Db.QueryFirstOrDefault<CourseVideo>(sql, parament);
            if (model != null)
            {
                return new MessagesOutPut { Success = false, Message = "视频编号已存在" };
            }
            else
            {
                return new MessagesOutPut { Success = true };
            }
        }
    }
}

