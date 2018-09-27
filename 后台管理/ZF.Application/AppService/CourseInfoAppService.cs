
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
using ZF.Infrastructure.zTree;
using System.Linq;
using ZF.Infrastructure.RedisCache;
using System.Configuration;
using ZF.Infrastructure.TwoDimensionalCode;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：CourseInfo 
    /// </summary>
    public class CourseInfoAppService : BaseAppService<CourseInfo>
    {
        private static string CourseTwoDimensionalCodeUrl = ConfigurationManager.AppSettings["CourseTwoDimensionalCodeUrl"];
        private static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];
        private static string CourseTwoDimensionalCodeVideoUrl = ConfigurationManager.AppSettings["CourseTwoDimensionalCodeVideoUrl"];

        private readonly ICourseInfoRepository _iCourseInfoRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;
        private readonly FileRelationshipAppService _fileRelationshipAppService;
        private readonly CourseOnTeachersAppService _courseOnTeachersAppService;
        private readonly ProjectAppService _projectAppService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iCourseInfoRepository"></param>
        /// <param name="operatorLogAppService"></param>
        /// <param name="fileRelationshipAppService"></param>

        public CourseInfoAppService(ICourseInfoRepository iCourseInfoRepository, OperatorLogAppService operatorLogAppService, FileRelationshipAppService fileRelationshipAppService, CourseOnTeachersAppService courseOnTeachersAppService, ProjectAppService projectAppService) : base(iCourseInfoRepository)
        {
            _iCourseInfoRepository = iCourseInfoRepository;
            _operatorLogAppService = operatorLogAppService;
            _fileRelationshipAppService = fileRelationshipAppService;
            _courseOnTeachersAppService = courseOnTeachersAppService;
            _projectAppService = projectAppService;
        }


        /// <summary>
        /// 查询列表实体：CourseInfo 
        /// </summary>
        public List<CourseInfoOutput> SelectLinkCourseList(string id)
        {
            var strSql = @"SELECT Id,CourseName FROM dbo.t_Course_Info WHERE 1=1 AND IsDelete=0 and Type=0 ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(id))
            {
                strSql += " and Id !=@Id";
                dynamicParameters.Add(":Id", id, DbType.String);
            }
            return Db.QueryList<CourseInfoOutput>(strSql, dynamicParameters);
        }



        /// <summary>
        /// 查询列表实体：CourseInfo 
        /// </summary>
        public List<SecurityCodeStatisticsOutput> GetSecurityCodeStatistics(SecurityCodeUsageInput input, out int count)
        {
            var sqlstring = string.Empty;
            var sqlstring1 = string.Empty;
            var dynamicParameters = new DynamicParameters();
            if (input.StartTime.HasValue)
            {
                sqlstring += " and b.GetDateTime >= @StartTime ";
                dynamicParameters.Add(":StartTime", input.StartTime, DbType.DateTime);
            }
            if (input.EndTime.HasValue)
            {
                sqlstring += " and b.GetDateTime <= @EndTime ";
                dynamicParameters.Add(":EndTime", input.EndTime.Value.AddDays(1), DbType.DateTime);
            }
            if (input.Type.HasValue)
            {
                sqlstring += " and b.IsValueAdded = @Type ";
                sqlstring1 += " and c.IsValueAdded = @Type1 ";
                dynamicParameters.Add(":Type", input.Type, DbType.Int32);
                dynamicParameters.Add(":Type1", input.Type, DbType.Int32);
            }
            const string sql = @"SELECT  a.Id ,
        a.CourseName ,
        v.c1 Count1 ,
        v.c2 Count2 ";
            var strSql = new StringBuilder(@" FROM    dbo.t_Course_Info a
        LEFT JOIN ( SELECT  e.CourseId ,
                            ISNULL(SUM(CASE WHEN cc = 'yes' THEN e.Count1
                                       END), 0) c1 ,
                            ISNULL(SUM(CASE WHEN cc = 'no' THEN e.Count1
                                       END), 0) c2
                    FROM    ( SELECT    b.CourseId ,
                                        COUNT(1) Count1 ,
                                        'yes' cc
                              FROM      dbo.t_Course_SecurityCode b
                              WHERE     b.IsUse = 1" + sqlstring + @"
                              GROUP BY  b.CourseId
                              UNION
                              SELECT    c.CourseId ,
                                        COUNT(1) Count1 ,
                                        'no' cc
                              FROM      dbo.t_Course_SecurityCode c
                                where  1=1 " + sqlstring1 + @"
                              GROUP BY  c.CourseId
                            ) e
                    GROUP BY e.CourseId
                  ) v ON a.Id = v.CourseId
WHERE   a.IsDelete = 0
        AND v.c1 IS NOT NULL
        AND v.c2 IS NOT NULL ");
            const string sqlCount = "select count(*) ";
            if (!string.IsNullOrWhiteSpace(input.CourseName))
            {
                strSql.Append(" and a.CourseName like @CourseName ");
                dynamicParameters.Add(":CourseName", '%' + input.CourseName + '%', DbType.String);
            }

            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<SecurityCodeStatisticsOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);

            return list;
        }

        /// <summary>
        /// 查询列表实体：CourseInfo 
        /// </summary>
        public List<SecurityCodeUsageOutput> GetSecurityCodeUsage(SecurityCodeUsageInput input, out int count)
        {
            const string sql = @"SELECT  b.CourseName ,
        c.NickNamw UserName ,
        a.GetDateTime ,
        a.IsValueAdded ,
        a.IsUse,a.Code ";
            var strSql = new StringBuilder(@" FROM    dbo.t_Course_SecurityCode a
        LEFT JOIN dbo.t_Course_Info b ON a.CourseId = b.Id
        LEFT JOIN dbo.t_Base_RegisterUser c ON a.UserId = c.Id
WHERE   a.IsUse = 1 and b.Isdelete=0 ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.CourseName))
            {
                strSql.Append(" and b.CourseName like @CourseName ");
                dynamicParameters.Add(":CourseName", '%' + input.CourseName + '%', DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.Code))
            {
                strSql.Append(" and a.Code like @Code ");
                dynamicParameters.Add(":Code", '%' + input.Code + '%', DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.UserName))
            {
                strSql.Append(" and c.NickNamw like @UserName ");
                dynamicParameters.Add(":UserName", '%' + input.UserName + '%', DbType.String);
            }
            if (input.StartTime.HasValue)
            {
                strSql.Append(" and a.GetDateTime >= @StartTime ");
                dynamicParameters.Add(":StartTime", input.StartTime, DbType.DateTime);
            }
            if (input.EndTime.HasValue)
            {
                strSql.Append(" and a.GetDateTime <= @EndTime ");
                dynamicParameters.Add(":EndTime", input.EndTime.Value.AddDays(1), DbType.DateTime);
            }
            if (input.Type.HasValue)
            {
                strSql.Append(" and b.Type = @Type ");
                dynamicParameters.Add(":Type", input.Type, DbType.Int32);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<SecurityCodeUsageOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);

            return list;
        }

        /// <summary>
        /// 查询列表实体：CourseInfo 
        /// </summary>
        public List<CourseInfoOutput> GetList(CourseInfoListInput input, out int count)
        {
            const string sql = "select  a.*,b.ProjectId,b.SubjectName,CAST(v.c1 AS VARCHAR(100)) + '/' + CAST(v.c2 AS VARCHAR(100)) AS Number ";
            var strSql = new StringBuilder(@" FROM    t_Course_Info a
        LEFT JOIN t_Base_Subject b ON a.SubjectId = b.Id
        LEFT JOIN t_Base_Project d ON b.ProjectId = d.Id
        LEFT JOIN t_Base_ProjectClass e ON d.ProjectClassId = e.Id
        LEFT JOIN ( SELECT  e.CourseId ,
                            ISNULL(SUM(CASE WHEN cc = 'yes' THEN e.Count1
                                       END), 0) c1 ,
                            ISNULL(SUM(CASE WHEN cc = 'no' THEN e.Count1
                                       END), 0) c2
                    FROM    ( SELECT    b.CourseId ,
                                        COUNT(1) Count1 ,
                                        'yes' cc
                              FROM      dbo.t_Course_SecurityCode b
                              WHERE     b.IsUse = 1
                              GROUP BY  b.CourseId
                              UNION
                              SELECT    c.CourseId ,
                                        COUNT(1) Count1 ,
                                        'no' cc
                              FROM      dbo.t_Course_SecurityCode c
                              GROUP BY  c.CourseId
                            ) e
                    GROUP BY e.CourseId
                  ) v ON a.Id = v.CourseId
WHERE   a.IsDelete = 0 ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.CourseName))
            {
                strSql.Append(" and a.CourseName like @Name ");
                dynamicParameters.Add(":Name", '%' + input.CourseName + '%', DbType.String);
            }
            if (!string.IsNullOrEmpty(input.SubjectId))
            {
                strSql.Append(" and a.SubjectId=@SubjectId ");
                dynamicParameters.Add(":SubjectId ", input.SubjectId, DbType.String);
            }
            if (!string.IsNullOrEmpty(input.ProjectId))
            {
                strSql.Append(" and b.ProjectId=@ProjectId");
                dynamicParameters.Add(":ProjectId", input.ProjectId, DbType.String);
            }

            if (!string.IsNullOrEmpty(input.TeacherId))
            {
                strSql.Append(" and a.TeacherId like @TeacherId ");
                dynamicParameters.Add(":TeacherId", "%" + input.TeacherId + "%", DbType.String);
            }
            if (!string.IsNullOrEmpty(input.ProjectClassId))
            {
                strSql.Append("and d.ProjectClassId=@ProjectClassId ");
                dynamicParameters.Add(":ProjectClassId ", input.ProjectClassId, DbType.String);
            }
            if (!string.IsNullOrEmpty(input.ClassId))
            {
                strSql.Append("and d.ClassId like @ClassId ");
                dynamicParameters.Add(":ClassId ", "%" + input.ClassId + "%", DbType.String);
            }

            if (input.Type.HasValue && input.Type > -1)
            {
                strSql.Append(" and a.Type=@Type ");
                dynamicParameters.Add(":Type", input.Type, DbType.Int16);
            }

            if (input.State.HasValue)
            {
                strSql.Append(" and isnull(a.State,0)=@State ");
                dynamicParameters.Add(":State", input.State, DbType.Int32);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<CourseInfoOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            foreach (var item in list)
            {
                if (!string.IsNullOrWhiteSpace(item.TeacherId))
                {
                    var arrId = item.TeacherId.Split(',');
                    var teachersName = "";
                    for (int i = 0; i < arrId.Length; i++)
                    {
                        teachersName += _courseOnTeachersAppService.Get(arrId[i]).TeachersName + ",";
                    }
                    item.TeachersName = teachersName.TrimEnd(',');
                }

                if (!string.IsNullOrWhiteSpace(item.ClassId))
                {
                    var arrId = item.ClassId.Split(',');
                    var className = "";
                    for (int i = 0; i < arrId.Length; i++)
                    {
                        className += _projectAppService.Get(arrId[i]).ProjectName;
                    }
                    item.ClassName = className;
                }
            }
            return list;
        }

        /// <summary>
        /// 查询课程二维码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<CourseInfoOutput> GetTwoDimensionalCode(CourseInfoListInput input)
        {
            const string sql = "select  a.Id,a.CourseName ";
            var strSql = new StringBuilder(@" from t_Course_Info a 
                left join t_Base_Subject b on a.SubjectId=b.Id 
                left join t_Base_Project d on b.ProjectId=d.Id 
                left join t_Base_ProjectClass e on d.ProjectClassId =e.Id 
                where a.IsDelete=0  ");
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.CourseName))
            {
                strSql.Append(" and a.CourseName like @Name ");
                dynamicParameters.Add(":Name", '%' + input.CourseName + '%', DbType.String);
            }
            if (!string.IsNullOrEmpty(input.SubjectId))
            {
                strSql.Append(" and a.SubjectId=@SubjectId ");
                dynamicParameters.Add(":SubjectId ", input.SubjectId, DbType.String);
            }
            if (!string.IsNullOrEmpty(input.ProjectId))
            {
                strSql.Append(" and b.ProjectId=@ProjectId");
                dynamicParameters.Add(":ProjectId", input.ProjectId, DbType.String);
            }

            if (!string.IsNullOrEmpty(input.TeacherId))
            {
                strSql.Append(" and a.TeacherId like @TeacherId ");
                dynamicParameters.Add(":TeacherId", "%" + input.TeacherId + "%", DbType.String);
            }
            if (!string.IsNullOrEmpty(input.ProjectClassId))
            {
                strSql.Append("and d.ProjectClassId=@ProjectClassId ");
                dynamicParameters.Add(":ProjectClassId ", input.ProjectClassId, DbType.String);
            }
            if (input.State.HasValue)
            {
                strSql.Append(" and isnull(a.State,0)=@State ");
                dynamicParameters.Add(":State", input.State, DbType.Int32);
            }
            var list = Db.QueryList<CourseInfoOutput>(sql + strSql, dynamicParameters);
            Dictionary<string, string> DimensionalCode = new Dictionary<string, string>();
            foreach (var item in list)
            {
                item.CourseIamge = CourseTwoDimensionalCodeUrl + item.Id;
                item.CourseVedio = DefuleDomain + "/" + item.Id + ".jpg";
                DimensionalCode.Add(item.Id, item.CourseIamge);
            }
            if (DimensionalCode != null)
            {
                CreateTwoDimensionalCode.DoWaitProcess(DimensionalCode);
                return list;
            }
            else
            {
                return null;
            }
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<CourseInfoOutput> GetListExceptInPackCourse(CourseInfoListExceptInPackCourse input, out int count)
        {
            const string sql = "select  a.*,b.SubjectName,c.TeachersName ";
            var strSql = new StringBuilder(" from t_Course_Info a "
                + " left join t_Base_Subject b on a.SubjectId=b.Id "
                + " left join t_Course_OnTeachers c on a.TeacherId=c.Id  "
                + " LEFT JOIN dbo.t_Base_Project e ON b.ProjectId=e.Id "
                + "  LEFT JOIN dbo.t_Base_ProjectClass f ON f.id = e.ProjectClassId"
                + " where a.IsDelete=0 and  "
                + " a.Id NOT IN (SELECT CourseId FROM  dbo.t_Course_SuitDetail d WHERE 1=1 ");
            const string sqlCount = "select count(*) ";

            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.PackCourseId))
            {

                strSql.Append(" and d.PackCourseId = @PackCourseId ");
                dynamicParameters.Add(":PackCourseId", input.PackCourseId, DbType.String);
            }
            strSql.Append(" ) ");


            if (!string.IsNullOrWhiteSpace(input.TeacherId))
            {

                strSql.Append(" and a.TeacherId = @TeacherId ");
                dynamicParameters.Add(":TeacherId", input.TeacherId, DbType.String);
            }

            if (!string.IsNullOrWhiteSpace(input.CourseName))
            {

                strSql.Append(" and a.CourseName like @Name ");
                dynamicParameters.Add(":Name", "%" + input.CourseName + "%", DbType.String);
            }

            if (!string.IsNullOrEmpty(input.SubjectId))
            {
                strSql.Append(" and a.SubjectId=@SubjectId ");
                dynamicParameters.Add(":SubjectId ", input.SubjectId, DbType.String);
            }
            if (!string.IsNullOrEmpty(input.ProjectId))
            {
                strSql.Append(" and b.ProjectId=@ProjectId ");
                dynamicParameters.Add(":ProjectId", input.ProjectId, DbType.String);
            }
            if (!string.IsNullOrEmpty(input.ProjectClassId))
            {
                strSql.Append(" and e.ProjectClassId=@ProjectClassId ");
                dynamicParameters.Add(":ProjectClassId", input.ProjectClassId, DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<CourseInfoOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,

                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 更新课程时长，课件数统计数据
        /// </summary>
        /// <param name="chapterid"></param>
        /// <returns></returns>
        public bool UpdateCourseLongTime(string chapterid)
        {
            var sql = "DECLARE @courseid VARCHAR(36) "
                      + " DECLARE @CourseLongTime int "
                      + " DECLARE @CourseWareCount int "
                      + " SELECT @courseid = CourseId  FROM dbo.t_Course_Chapter WHERE id =@ChapterId "
                      + " SELECT @CourseLongTime = SUM( dbo.getMinutes( VideoLongTime ) ), @CourseWareCount = COUNT( *) FROM dbo.t_Course_Video WHERE ChapterId IN ( SELECT ChapterId FROM dbo.t_Course_Chapter WHERE CourseId = @courseid) "
                      + " UPDATE dbo.t_Course_Info SET CourseLongTime = @CourseLongTime,CourseWareCount = @CourseWareCount WHERE id = @courseid ";
            var parameters = new DynamicParameters();
            parameters.Add(":ChapterId", chapterid, DbType.String);
            return Db.Execute(sql, parameters);
        }

        /// <summary>
        /// 判断是否在订单或购物车中
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool ExistInOrderOrCart(CourseInfoInput input)
        {
            var sql = " SELECT  COUNT(*) FROM (SELECT CourseId FROM t_Order_Detail UNION SELECT CourseId FROM dbo.t_Order_CartDetail)a  WHERE a.CourseId='" + input.Id + "'";
            return Db.ExecuteScalar<int>(sql, null) > 0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool ExistTeacherInCourse(CourseInfoInput input)
        {
            const string sql = "SELECT * FROM dbo.t_Course_Info WHERE TeacherId=@TeacherId";
            var parameters = new DynamicParameters();
            parameters.Add(":TeacherId", input.TeacherId, DbType.String);
            var list = Db.QueryList<CourseInfoOutput>(sql, parameters);
            return list.Count > 0;
        }



        /// <summary>
        /// 获取课程下拉列表-不分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<CourseInfoOutput> GetList(CourseInfoListInput input)
        {
            var strSql = new StringBuilder(" select Id,CourseName from t_Course_Info where 1=1 and IsDelete=0");
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrEmpty(input.SubjectId))
            {
                strSql.Append(" and SubjectId = @SubjectId ");
                dynamicParameters.Add(":SubjectId", input.SubjectId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.TeacherId))
            {
                strSql.Append(" and TeacherId = @TeacherId ");
                dynamicParameters.Add(":TeacherId", input.TeacherId, DbType.String);
            }
            if (input.State == 0)
            {

                strSql.Append(" and State=0 ");
            }
            else if (input.State == 1)
            {

                strSql.Append(" and State=1 ");
            }
            var list = Db.QueryList<CourseInfoOutput>(strSql.ToString(), dynamicParameters);
            return list;

        }

        /// <summary>
        /// 新增实体  CourseInfo
        /// </summary>
        public MessagesOutPut AddOrEdit(CourseInfoInput input)
        {
            RedisCacheHelper.Remove("GetProjectCourseInfo");
            CourseInfo model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iCourseInfoRepository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;
                model.CourseName = input.CourseName;
                model.SubjectId = input.SubjectId;
                model.CourseIamge = input.IdFilehiddenFile ?? "";
                model.CourseVedio = input.CourseVediohiddenFile;
                model.CourseContent = input.CourseContent ?? "";
                model.Price = input.Price;
                model.FavourablePrice = input.FavourablePrice;
                model.TeacherId = input.TeacherId;
                model.IsTop = input.IsTop;
                model.IsRecommend = input.IsRecommend;
                model.CourseTag = input.CourseTag;
                model.ValidityPeriod = input.ValidityPeriod;
                model.UpdateUserId = UserObject.Id;
                model.CourseAttribute = input.CourseAttribute;
                model.UpdateTime = DateTime.Now;
                model.Title = input.Title;
                model.KeyWord = input.KeyWord;
                model.Description = input.Description;
                model.ClassId = input.ClassId;
                model.EmailNotes = input.EmailNotes;
                model.Type = input.Type;
                model.DiscountCardId = input.DiscountCardId;
                model.LinkCourse = input.LinkCourse;
                model.ValidityEndDate = input.ValidityEndDate;
                model.IsValueAdded = input.IsValueAdded;
                model.OrderNo = input.OrderNo;
                //model.ShowTime = input.ShowTime;

                //2018-05-18 wyf  add   新增项目分类属性
                if (!string.IsNullOrEmpty(input.ClassId))
                {
                    var classId = input.ClassId.Split(',');
                    var projectClassId = classId.Select(item => _projectAppService.Get(item).ProjectClassId)
                        .Aggregate("", (current, id) => current + (id + ","));
                    model.ProjectClassId = projectClassId;
                }

                #endregion
                _iCourseInfoRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.CourseInfo,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = string.Format("修改课程信息:{0}-{1}", model.Id, model.CourseName)
                });
                if (!string.IsNullOrWhiteSpace(input.IdFilehiddenFile))
                {
                    _fileRelationshipAppService.Add(new FileRelationshipInput
                    {
                        ModuleId = input.Id,
                        IdFilehiddenFile = input.IdFilehiddenFile,
                        Type = 0//图片
                    });
                }
                if (!string.IsNullOrWhiteSpace(input.CourseVediohiddenFile))
                {
                    _fileRelationshipAppService.Add(new FileRelationshipInput
                    {
                        ModuleId = input.Id,
                        IdFilehiddenFile = input.CourseVediohiddenFile,
                        Type = 1//视频
                    });
                }
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<CourseInfo>();
            model.Id = Guid.NewGuid().ToString();
            model.CourseName = input.CourseName;
            model.SubjectId = input.SubjectId;
            model.CourseIamge = input.IdFilehiddenFile ?? "";
            model.CourseVedio = input.CourseVediohiddenFile;
            model.CourseContent = input.CourseContent == null ? "" : input.CourseContent;
            model.Price = input.Price;
            model.FavourablePrice = input.FavourablePrice;
            model.TeacherId = input.TeacherId;
            model.IsTop = input.IsTop;
            model.IsRecommend = input.IsRecommend;
            model.CourseTag = input.CourseTag;
            model.ValidityPeriod = input.ValidityPeriod;
            model.State = input.State;
            model.CourseAttribute = input.CourseAttribute;
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            model.Title = input.Title;
            model.KeyWord = input.KeyWord;
            model.Description = input.Description;
            model.ClassId = input.ClassId;
            model.Type = input.Type;
            model.LinkCourse = input.LinkCourse;
            model.ValidityEndDate = input.ValidityEndDate;
            //model.ShowTime = input.ShowTime;
            //2018-05-18 wyf  add   新增项目分类属性
            if (!string.IsNullOrEmpty(input.ClassId))
            {
                var classId = input.ClassId.Split(',');
                var projectClassId = classId.Select(item => _projectAppService.Get(item).ProjectClassId)
                    .Aggregate("", (current, id) => current + (id + ","));
                model.ProjectClassId = projectClassId;
            }
            var keyId = _iCourseInfoRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.CourseInfo,
                OperatorType = (int)OperatorType.Add,
                Remark = string.Format("新增课程信息:'{0}'-'{1}'", model.Id, model.CourseName)
            });
            if (!string.IsNullOrWhiteSpace(input.IdFilehiddenFile))
            {
                _fileRelationshipAppService.Add(new FileRelationshipInput
                {
                    ModuleId = model.Id,
                    IdFilehiddenFile = input.IdFilehiddenFile,
                    Type = 0//图片
                });
            }
            if (!string.IsNullOrWhiteSpace(input.CourseVediohiddenFile))
            {
                _fileRelationshipAppService.Add(new FileRelationshipInput
                {
                    ModuleId = model.Id,
                    IdFilehiddenFile = input.CourseVediohiddenFile,
                    Type = 1//图片
                });
            }
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }


        /// <summary>
        /// 更新上下架状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut UpdateState(CourseInfoInput input)
        {
            CourseInfo model;
            model = _iCourseInfoRepository.Get(input.Id);
            #region 修改逻辑
            model.Id = input.Id;
            model.State = input.State;
            model.UpdateUserId = UserObject.Id;
            model.UpdateTime = DateTime.Now;
            #endregion
            _iCourseInfoRepository.Update(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = model.Id,
                ModuleId = (int)Model.CourseInfo,
                OperatorType = (int)OperatorType.Edit,
                Remark = string.Format("修改课程上下架状态:({0}-{1})", model.Id, model.CourseName)
            });
            return new MessagesOutPut { Success = true, Message = "修改成功!" };
        }

        /// <summary>
        /// 获取所有课程
        /// </summary>
        /// <returns></returns>
        public List<CourseInfoOutput> GetAllCourseList(int courseType)
        {
            var unionSql = " SELECT     a.Id, a.CourseName FROM t_Course_PackcourseInfo a WHERE  a.IsDelete = 0 UNION SELECT     b.Id, b.CourseName FROM  t_Course_Info b WHERE b.IsDelete = 0 ";
            var singleSql = "SELECT     b.Id, b.CourseName FROM  t_Course_Info b WHERE b.IsDelete = 0 ";
            var packSql = " SELECT     a.Id, a.CourseName FROM t_Course_PackcourseInfo a WHERE  a.IsDelete = 0 ";
            if (courseType == 0)
            {
                return Db.QueryList<CourseInfoOutput>(singleSql);
            }
            else if (courseType == 1)
            {
                return Db.QueryList<CourseInfoOutput>(packSql);
            }
            return Db.QueryList<CourseInfoOutput>(unionSql);
        }

        /// <summary>
        /// 获取课程节点树,包括项目分类->项目->科目->课程
        /// </summary>
        /// <returns></returns>
        public List<Tree.zTree> CourseInfoPointTree()
        {
            const string sql2 = @" SELECT  * FROM   V_CourseInfoPoint  ";
            List<Tree.zTree> dt = Db.QueryList<Tree.zTree>(sql2);
            List<Tree.zTree> drList = dt.Where(x => x.pId == "").ToList();
            var row = new List<Tree.zTree>();
            foreach (var dr in drList)
            {
                Tree.zTree jt = new Tree.zTree();
                jt.id = dr.id;
                jt.name = dr.name;
                jt.type = dr.type;
                jt.subjectId = dr.subjectId;
                jt.children = CreatePointTree(dt, jt);
                row.Add(jt);
            }
            return row;
        }

        /// <summary>
        /// 项目列表
        /// </summary>
        /// <returns></returns>
        public List<Tree.zTree> CourseProjectClassTree()
        {
            const string sql2 = @" SELECT  b.Id, a.ProjectClassName+'/'+b.ProjectName AS Name,1 AS Type FROM  t_Base_Project b LEFT JOIN dbo.t_Base_ProjectClass a ON a.id=b.ProjectClassId WHERE     b.IsDelete = 0 ORDER BY a.ProjectClassName  ";
            List<Tree.zTree> dt = Db.QueryList<Tree.zTree>(sql2);
            List<Tree.zTree> drList = dt.ToList();
            return drList;
        }

        private List<Tree.zTree> CreatePointTree(List<Tree.zTree> dt, Tree.zTree jt)
        {
            string keyid = jt.id;                                        //根节点ID
            List<Tree.zTree> nodeList = new List<Tree.zTree>();
            var children = dt.Where(x => x.pId == keyid).ToList();
            foreach (var dr in children)
            {
                Tree.zTree node = new Tree.zTree();
                node.id = dr.id;
                node.name = dr.name;
                node.pId = keyid;
                node.type = dr.type;
                node.subjectId = dr.subjectId;
                node.children = CreatePointTree(dt, node);
                nodeList.Add(node);
            }
            return nodeList.Count == 0 ? null : nodeList;
        }



        /// <summary>
        /// 获取所有上架的课程  以及套餐课程
        /// </summary>
        /// <returns></returns>
        public List<CourseInfoOutput> GetPackcourseInfoInfoList(CourseInfoListInput input, out int count)
        {
            const string sql = "select *  ";
            const string sqlCount = "select count(*) ";
            var strSql = new StringBuilder(@" from (SELECT  a.Id ,
        a.CourseName ,
        a.CourseTag ,
        a.Price ,
        a.FavourablePrice ,
        a.CourseWareCount,
        0 as CourseType,
        '课程' as TypeName,
        a.ValidityEndDate

FROM    t_Course_Info a WHERE a.IsDelete = 0 AND a.State = 1
UNION
SELECT  b.Id ,
        b.CourseName ,
        b.CourseTag ,
        b.Price ,
        b.FavourablePrice ,
        b.CourseWareCount,
        1 as CourseType,
        '套餐' as TypeName,
        b.ValidityEndDate

FROM    t_Course_PackcourseInfo b WHERE b.IsDelete = 0 AND b.State = 1 ) c where 1=1 ");
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.CourseName))
            {
                strSql.Append(" and (c.CourseName like @CourseName or c.CourseTag like @CourseName )");
                dynamicParameters.Add(":CourseName", '%' + input.CourseName + '%', DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<CourseInfoOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;

        }

        /// <summary>
        /// 获取课程二维码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public CourseInfoCode GetTwoDimensionalCode(CourseInfoInput input)
        {
            try
            {
                CourseInfo courseInfo = _iCourseInfoRepository.Get(input.Id);
                Dictionary<string, string> info = new Dictionary<string, string>();
                info.Add(input.Id, DefuleDomain + input.Id);
                info = CreateTwoDimensionalCode.DoWaitProcess(info);
                var data = (from message in info where message.Key == input.Id select message).LastOrDefault();
                //获取视频信息
                const string sql = @" SELECT  a.CourseId ,
        b.ChapterId ,
        b.VideoName ,
        b.VideoUrl ,
        b.Id ,
        a.CapterName ,
        b.QcodeTitle ,
        b.Code,
        a.OrderNo
FROM    t_course_chapter a
        LEFT JOIN t_course_video b ON a.Id = b.ChapterId
        LEFT JOIN t_course_chapter c ON a.ParentId=c.Id
WHERE   a.CourseId = @CourseId
        AND ISNULL(b.VideoUrl, '') <> ''
ORDER BY c.OrderNo,a.OrderNo ,
        ISNULL(b.OrderNo, 0) ,
        b.AddTime ";
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(":CourseId", input.Id, DbType.String);
                var dataInfo = Db.QueryList<ChapterCode>(sql, dynamicParameters);
                CourseInfoCode course = new CourseInfoCode();
                course.ImgName = courseInfo.CourseName;
                course.ImgUrl = data.Value;
                if (input.EmailNotes == 0)
                {
                    course.videoCode = GetVideoCode(input.CourseWareCount, input.Id);
                }
                else if (input.EmailNotes == 1)
                {
                    course.videoCode = dataInfo.Count <= 0 ? null : GetVideoCode(dataInfo);
                }
                return course;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 视频地址
        /// </summary>
        /// <param name="chapterCode"></param>
        /// <returns></returns>
        public List<VideoCode> GetVideoCode(List<ChapterCode> chapterCode)
        {
            List<VideoCode> videoCode = new List<VideoCode>();
            Dictionary<string, string> info = new Dictionary<string, string>();
            string md5 = string.Empty;
            foreach (var item in chapterCode)
            {
                md5 = Guid.NewGuid().ToString();
                VideoCode code = new VideoCode();
                code.VideoCodeName = item.VideoName;
                code.VideoCodeUrl = DefuleDomain + "/" + item.QcodeTitle + "-" + md5 + ".jpg";
                try
                {
                    info.Add(item.QcodeTitle + "@" + md5, CourseTwoDimensionalCodeVideoUrl + string.Format("code={0}", item.Code));
                }
                catch (Exception)
                {
                    continue;
                }
                videoCode.Add(code);
            }
            info = CreateTwoDimensionalCode.DoWaitProcess(info);
            if (info != null)
            {
                return videoCode;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 视频地址
        /// </summary>
        /// <param name="coursewarecount">需要生成的二维码数量</param>
        /// <param name="courseid">课程ID</param>
        /// <returns></returns>
        public List<VideoCode> GetVideoCode(int? coursewarecount, string courseid)
        {
            List<VideoCode> videoCode = new List<VideoCode>();
            Dictionary<string, string> info = new Dictionary<string, string>();

            string[] number = new string[] { "00000", "0000", "000", "00", "0", "" };

            //获取视频最大code值
            const string sql = "select MAX(CAST(ISNULL(Code,0) as int))+1 Code from t_Course_Video where IsDelete=0";
            int maxCode = Db.ExecuteScalar<int>(sql, null);
            for (int i = 0; i < coursewarecount; i++)
            {
                string gid = Guid.NewGuid().ToString();
                int j = i.ToString().Length;
                string newNumber = number[j - 1] + (i + maxCode).ToString();
                VideoCode code = new VideoCode();
                code.VideoCodeName = newNumber;
                code.VideoCodeUrl = DefuleDomain + "/" + newNumber + ".jpg";
                info.Add(newNumber, CourseTwoDimensionalCodeVideoUrl + string.Format("code={0}", newNumber));
                videoCode.Add(code);
            }
            info = CreateTwoDimensionalCode.DoWaitProcess(info);
            if (info != null)
            {
                return videoCode;
            }
            else
            {
                return null;
            }
        }
    }
}

