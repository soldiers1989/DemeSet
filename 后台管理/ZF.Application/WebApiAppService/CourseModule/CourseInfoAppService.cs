using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Application.WebApiAppService.SystemModule;
using ZF.Application.WebApiDto.CourseChapterModule;
using ZF.Application.WebApiDto.CourseModule;
using ZF.Application.WebApiDto.CoursePackModule;
using ZF.Application.WebApiDto.SystemModule;
using ZF.Core;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure.RedisCache;
using ZF.Application.WebApiDto.MyCollectionModule;
using ZF.Infrastructure.QiniuYun;
using ZF.Application.WebApiDto.CourseRelatedModule;
using ZF.AutoMapper.AutoMapper;
using ZF.Application.WebApiAppService.CourseOnTeacherModule;
using CourseChapterAppService = ZF.Application.WebApiAppService.CourseChapterModule.CourseChapterAppService;

namespace ZF.Application.WebApiAppService.CourseModule
{
    /// <summary>
    /// 课程管理webapi服务
    /// </summary>
    public class CourseInfoAppService : BaseAppService<CourseInfo>
    {
        private static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];
        private readonly ProjectApiService _projectApiService;
        private readonly ICourseInfoRepository _repository;

        private readonly ICourseFaceToFaceRepository _courseFaceToFaceRepository;


        private readonly CourseChapterAppService _courseChapterAppService;
        private readonly CourseOnTeacherAppService _courseOnTeacherAppService;

        private readonly SysSetAppService _setAppService;

        private readonly SubjectAppService _subjectAppService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="projectApiService"></param>
        /// <param name="courseFaceToFaceRepository"></param>
        /// <param name="courseChapterAppService"></param>
        /// <param name="courseOnTeacherAppService"></param>
        /// <param name="setAppService"></param>
        /// <param name="subjectAppService"></param>
        public CourseInfoAppService(ICourseInfoRepository repository, ProjectApiService projectApiService, ICourseFaceToFaceRepository courseFaceToFaceRepository, CourseChapterAppService courseChapterAppService, CourseOnTeacherAppService courseOnTeacherAppService, SysSetAppService setAppService, SubjectAppService subjectAppService) : base(repository)
        {
            _repository = repository;
            _projectApiService = projectApiService;
            _courseFaceToFaceRepository = courseFaceToFaceRepository;
            _courseChapterAppService = courseChapterAppService;
            _courseOnTeacherAppService = courseOnTeacherAppService;
            _setAppService = setAppService;
            _subjectAppService = subjectAppService;
        }

        /// <summary>
        /// 获取套餐课程  前四条推荐记录
        /// </summary>
        /// <returns></returns>

        public List<CourseInfoModel> GetCoursePackcourseInfoAll()
        {
            var strSql = @"  select * from V_Course_Packcourse_Info WHERE 1=1  AND State=1 and CourseType= 1 and IsRecommend=1 ";
            var list = Db.QueryList<CourseInfoModel>(GetPageSql(strSql, null, 1, 4, "ID", "desc"));
            return list;
        }

        /// <summary>
        /// 根据标签获取推荐课程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<CourseInfoModel> GetRecommendCourseByTagExceptSelf(CourseInfoModelInput input)
        {
            var strSql = " SELECT CourseTag FROM dbo.V_Course_Packcourse_Info WHERE Id=@Id ";
            var dy = new DynamicParameters();
            dy.Add(":Id", input.CourseId, DbType.String);
            var courseTag = Db.ExecuteScalar<string>(strSql, dy);
            if (!string.IsNullOrEmpty(courseTag))
            {
                var tags = courseTag.Split(',');
                var sql = "";
                for (var i = 0; i < tags.Length; i++)
                {
                    if (i == tags.Length - 1)
                    {
                        sql += string.Format(@" select a.Id,a.CourseName,a.CourseIamge,a.FavourablePrice,f.Count,a.IsRecommend,a.CourseType,a.SubjectId from V_Course_Packcourse_Info a left join ( SELECT  COUNT(1) Count ,
                            MAX( d.CourseId ) CourseId
                            FROM    dbo.t_My_Course d
                            GROUP BY d.CourseId
                            ) f ON a.Id = f.CourseId  WHERE CourseTag LIKE '%{0}%' and a.Id!='{1}' and  a.Type!= 1 and a.State=1 and isnull(a.IsValueAdded,0)=0", tags[i], input.CourseId);
                    }
                    else
                    {
                        sql += string.Format(@" select a.Id,a.CourseName,a.CourseIamge,a.FavourablePrice,f.Count,a.IsRecommend ,a.CourseType,a.SubjectId from V_Course_Packcourse_Info a left join ( SELECT  COUNT(1) Count ,
                            MAX( d.CourseId ) CourseId
                            FROM    dbo.t_My_Course d
                            GROUP BY d.CourseId
                            ) f ON a.Id = f.CourseId  WHERE CourseTag LIKE '%{0}%' and a.Id!='{1}' and  a.Type!=1 and a.State=1 and isnull(a.IsValueAdded,0)=0 union  ", tags[i], input.CourseId);
                    }
                }
                sql += "";
                var list = Db.QueryList<CourseInfoModel>(GetPageSql(sql, null, input.Page, input.Rows, "IsRecommend ", "Desc"));
                foreach (var item in list)
                {
                    item.CourseIamge = string.IsNullOrEmpty(item.CourseIamge) ? "" : DefuleDomain + "/" + item.CourseIamge;
                }
                return list;
            }
            return null;
        }


        /// <summary>
        /// 获取课程带分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<CourseInfoModel> GetCourseInfoAll(CourseInfoModelInput input, out int count)
        {

            var countSql = " select count(1)   ";
            var str = @"SELECT  a.* ,
        d.TeachersName ,
        b.SubjectName ,
       -- c.Id AS ProjectId ,
        f.Count ,
        j.Code ,
        k.AppraiseLevel ,
        l.Count StudyCount";
            string sql = @" FROM    V_Course_Packcourse_Info a
        LEFT JOIN t_Base_Subject b ON REPLACE(a.SubjectId, ',', '') = b.Id 
       -- LEFT JOIN t_Base_Project c ON a.ProjectId = c.Id
        LEFT JOIN t_Course_OnTeachers d ON a.TeacherId = d.Id
        LEFT JOIN ( SELECT  COUNT(1) Count ,
                            MAX(d.CourseId) CourseId
                    FROM    dbo.t_My_Course d
                    GROUP BY d.CourseId
                  ) f ON a.Id = f.CourseId
        LEFT JOIN t_Base_Basedata j ON a.CourseAttribute = j.Id
        LEFT JOIN ( SELECT  g.CourseId CourseId ,
                            AVG(g.AppraiseLevel) AppraiseLevel
                    FROM    dbo.t_Course_Appraise g
                    GROUP BY g.CourseId
                  ) k ON a.Id = k.CourseId
        LEFT JOIN ( SELECT  g.CourseId CourseId ,
                            COUNT(1) Count
                    FROM    dbo.t_My_Course g
                    GROUP BY g.CourseId
                  ) l ON a.Id = l.CourseId
WHERE   1 = 1
        AND a.State = 1  ";
            var dy = new DynamicParameters();
            if (!string.IsNullOrEmpty(input.ProjectClassId))
            {
                sql += " and a.ProjectClassId like @ProjectClassId ";
                dy.Add(":ProjectClassId", '%' + input.ProjectClassId + '%', DbType.AnsiString);
            }
            if (!string.IsNullOrEmpty(input.ProjectId))
            {
                sql += " and a.ProjectId like @ProjectId ";
                dy.Add(":ProjectId", '%' + input.ProjectId + '%', DbType.AnsiString);
            }
            if (!string.IsNullOrEmpty(input.SubjectId))
            {
                sql += " and a.SubjectId like @SubjectId ";
                dy.Add(":SubjectId", '%' + input.SubjectId + '%', DbType.AnsiString);
            }
            if (!string.IsNullOrEmpty(input.CourseId))
            {
                sql += " and a.Id !=@CourseId ";
                dy.Add(":CourseId", input.CourseId, DbType.AnsiString);
            }

            if (input.IsFree.HasValue)
            {
                switch (input.IsFree.Value)
                {
                    case 1:
                        input.Sidx = "AppraiseLevel,StudyCount,FavourablePrice";
                        break;
                    case 2:
                        input.Sidx = "AppraiseLevel";
                        break;
                    case 3:
                        input.Sidx = "StudyCount";
                        break;
                    case 4:
                        input.Sidx = "FavourablePrice";
                        break;
                    case 5:
                        input.Sidx = "OrderNo";
                        break;
                }
            }
            else
            {
                input.Sidx = "OrderNo";
            }

            if (input.IsValueAdded.HasValue)
            {
                sql += " and isnull(a.IsValueAdded,0)=@IsValueAdded ";
                dy.Add(":IsValueAdded", input.IsValueAdded, DbType.Int32);
            }
            if (input.IsRecommend.HasValue)
            {
                sql += " and a.IsRecommend=@IsRecommend ";
                dy.Add(":IsRecommend", input.IsRecommend, DbType.Int32);
            }
            if (input.CourseType.HasValue)
            {
                sql += " and a.CourseType=@CourseType ";
                dy.Add(":CourseType", input.CourseType, DbType.Int32);
            }
            if (input.Type.HasValue)
            {
                sql += " and a.Type=@Type ";
                dy.Add(":Type", input.Type, DbType.Int32);
            }


            if (!string.IsNullOrWhiteSpace(input.query))
            {
                sql += " and (a.CourseName like @query or a.CourseTag like @query  or d.TeachersName like @query)";
                dy.Add(":query", '%' + input.query + '%', DbType.String);
            }
            if (!string.IsNullOrEmpty(input.SubjectId))
            {
                sql += " and (b.IsEconomicBase = 0 OR b.IsEconomicBase IS NULL ) ";
            }
            var list = Db.QueryList<CourseInfoModel>(GetPageSql(str + sql,
                dy,
                input.Page,
                input.Rows, " OrderNo,IsTop Desc ,IsRecommend Desc ," + input.Sidx, input.Sord), dy);
            count = Db.QueryFirstOrDefault<int>(countSql + sql, dy);
            foreach (var items in list)
            {
                if (items.CourseType == 1)
                {
                    items.SubjectName = GetPackInfo(items.SubjectId).TrimEnd('+');
                }
            }
            var jjjc = new List<CourseInfoModel>();
            if (!string.IsNullOrEmpty(input.SubjectId) && input.CourseType != 1)
            {
                var projectmodel = _subjectAppService.Get(input.SubjectId);
                if (projectmodel != null)
                {
                    jjjc = GetJjjc(projectmodel.ProjectId, "OrderNo,IsTop Desc,IsRecommend Desc", input.CourseId, input.Type, input.IsValueAdded);
                }
                //list.AddRange( jjjc );
                list.InsertRange(0, jjjc);
                count += jjjc.Count;
            }
            foreach (var item in list)
            {
                item.CourseIamge = string.IsNullOrEmpty(item.CourseIamge) ? "" : DefuleDomain + "/" + item.CourseIamge;
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subjectIds"></param>
        /// <returns></returns>
        public string GetPackInfo(string subjectIds)
        {
            var subjectid = "";
            var ids = subjectIds.Split(',');
            for (int i = 0; i < ids.Length; i++)
            {
                if (i < 2)
                {
                    subjectid += "'" + ids[i] + "'" + ",";
                }
            }
            subjectid = subjectid.TrimEnd(',');
            var str = @"SELECT  SubjectName+'+'   FROM t_Base_Subject  WHERE Id IN (" + subjectid + ")FOR XML PATH('')";
            return Db.QueryFirstOrDefault<string>(str, null);
        }


        /// <summary>
        /// 获取制定项目下的经济基础课程
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public List<CourseInfoModel> GetJjjc(string projectId, string order, string CourseId, int? type, int? IsValueAdded)
        {
            string sql = @"SELECT  a.* ,
        d.TeachersName ,
        b.SubjectName ,
       -- c.Id AS ProjectId ,
        f.Count ,
        j.Code ,
        k.AppraiseLevel ,
        l.Count StudyCount FROM    V_Course_Packcourse_Info a
        LEFT JOIN t_Base_Subject b ON REPLACE(a.SubjectId, ',', '') = b.Id
       -- LEFT JOIN t_Base_Project c ON a.ProjectId = c.Id
        LEFT JOIN t_Course_OnTeachers d ON a.TeacherId = d.Id
        LEFT JOIN ( SELECT  COUNT(1) Count ,
                            MAX(d.CourseId) CourseId
                    FROM    dbo.t_My_Course d
                    GROUP BY d.CourseId
                  ) f ON a.Id = f.CourseId
        LEFT JOIN t_Base_Basedata j ON a.CourseAttribute = j.Id
        LEFT JOIN ( SELECT  g.CourseId CourseId ,
                            AVG(g.AppraiseLevel) AppraiseLevel
                    FROM    dbo.t_Course_Appraise g
                    GROUP BY g.CourseId
                  ) k ON a.Id = k.CourseId
        LEFT JOIN ( SELECT  g.CourseId CourseId ,
                            COUNT(1) Count
                    FROM    dbo.t_My_Course g
                    GROUP BY g.CourseId
                  ) l ON a.Id = l.CourseId
WHERE   1 = 1
        AND a.State = 1   and b.IsEconomicBase=1  ";
            var dy = new DynamicParameters();
            if (!string.IsNullOrEmpty(projectId))
            {
                sql += " and b.projectId=@projectId ";
                dy.Add(":projectId", projectId, DbType.String);
            }
            if (type.HasValue)
            {
                sql += " and a.Type=@Type ";
                dy.Add(":Type", type, DbType.Int32);
            }
            if (IsValueAdded.HasValue)
            {
                sql += " and a.IsValueAdded=@IsValueAdded ";
                dy.Add(":IsValueAdded", IsValueAdded, DbType.Int32);
            }
            if (!string.IsNullOrEmpty(CourseId))
            {
                sql += " and a.Id !=@CourseId ";
                dy.Add(":CourseId", CourseId, DbType.AnsiString);
            }
            var list = Db.QueryList<CourseInfoModel>(sql + " Order by " + order, dy);
            return list;
        }

        /// <summary>
        /// 获取项目分类下面的项目和科目
        /// </summary>
        /// <returns></returns>
        public List<ProjectClassModel1> GetProjectClass()
        {
            //if (RedisCacheHelper.Exists("GetProjectClass"))
            //{
            //    return RedisCacheHelper.Get<List<ProjectClassModel1>>("GetProjectClass");
            //}
            //获取所有项目分类
            var projectClass = _projectApiService.GetProjectClassAll();
            //获取所有项目
            var project = _projectApiService.GetProjectAll();
            //获取所有科目
            var subject = _projectApiService.GetSubjectAll();
            var projectCourseList = new List<ProjectClassModel1>();
            foreach (var item in projectClass)
            {
                var projectClassModel1 = new ProjectClassModel1();
                projectClassModel1.Id = item.Id;
                projectClassModel1.ProjectMode = project.Where(x => x.ProjectClassId == item.Id).ToList().MapTo<List<ProjectModel>>();
                foreach (var items in projectClassModel1.ProjectMode)
                {
                    var subjectModel = subject.Where(x => x.ProjectId != null && x.ProjectId.Contains(items.Id)).ToList();
                    items.SubjectModel = subjectModel;
                }
                projectCourseList.Add(projectClassModel1);
            }
            RedisCacheHelper.Add("GetProjectClass", projectCourseList);
            return projectCourseList;
        }

        /// <summary>
        /// 获取题库关联课程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<CourseInfoModelOutput> GetLinkCourse(IdInput input)
        {
            var strSql = string.Format("SELECT  LinkCourse FROM dbo.t_Course_Info WHERE id='{0}'", input.Id);
            var linkCourse = Db.ExecuteScalar<string>(strSql, null);
            if (!string.IsNullOrEmpty(linkCourse))
            {
                var linklist = linkCourse.TrimEnd(',').Split(',');
                var inClouse = "";
                foreach (var item in linklist)
                {
                    inClouse += "'" + item + "',";
                }
                var sql =
                    string.Format("SELECT * FROM dbo.t_Course_Info WHERE Id IN( {0}) AND State=1 AND IsDelete=0 and isnull(IsValueAdded,0)=0  ",
                        inClouse.TrimEnd(','));
                return Db.QueryList<CourseInfoModelOutput>(sql, null);
            }
            return new List<CourseInfoModelOutput>();
        }


        /// <summary>
        /// 获取项目下面的科目
        /// </summary>
        /// <returns></returns>
        public List<ProjectModel> GetProject()
        {
            if (RedisCacheHelper.Exists("GetProjectSubject"))
            {
                return RedisCacheHelper.Get<List<ProjectModel>>("GetProjectSubject");
            }
            //获取所有项目
            var project = _projectApiService.GetProjectAll();
            //获取所有科目
            var subject = _projectApiService.GetSubjectAll();
            var projectMode = project.ToList().MapTo<List<ProjectModel>>();
            foreach (var items in projectMode)
            {
                var subjectModel = subject.Where(x => x.ProjectId != null && x.ProjectId.Contains(items.Id)).ToList();
                items.SubjectModel = subjectModel;
            }
            RedisCacheHelper.Add("GetProjectSubject", projectMode);
            return projectMode;
        }



        /// <summary>
        /// 获取项目下面的科目和课程
        /// </summary>
        /// <returns></returns>
        public List<ProjectCourseModel> GetProjectCourseInfo()
        {
            if (RedisCacheHelper.Exists("GetProjectCourseInfo"))
            {
                return RedisCacheHelper.Get<List<ProjectCourseModel>>("GetProjectCourseInfo");
            }
            //获取所有项目
            var project = _projectApiService.GetProjectAll();
            //获取所有科目
            var subject = _projectApiService.GetSubjectAll();
            //获取所课程
            var count = 0;
            var courseInfo = GetCoursePackcourseInfoAll();
            var projectCourseList = new List<ProjectCourseModel>();
            foreach (var item in project)
            {
                var projectCourse = new ProjectCourseModel();
                projectCourse.Id = item.Id;
                projectCourse.SubjectModel = subject.Where(x => x.ProjectId == item.Id).ToList();
                foreach (var items in projectCourse.SubjectModel)
                {
                    var coursePackcourseInfo = courseInfo.Where(x => x.SubjectId != null && x.SubjectId.Contains(items.Id)).ToList();
                    foreach (var item1 in coursePackcourseInfo)
                    {
                        if (!projectCourse.CourseInfoModel.Contains(item1))
                            projectCourse.CourseInfoModel.Add(item1);
                    }
                }
                projectCourseList.Add(projectCourse);
            }
            RedisCacheHelper.Add("GetProjectCourseInfo", projectCourseList);
            return projectCourseList;
        }

        /// <summary>
        /// 获取课程排名 前十条
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<PaidListingsModel> GetPaidListings(PaidListingsInput input)
        {
            const string sql = "SELECT  a.Id,a.CourseName,a.CourseType,f.Count ";
            var strSql = @" FROM    V_Course_Packcourse_Info a
        LEFT JOIN (SELECT COUNT(1) Count,MAX(d.CourseId) CourseId FROM dbo.t_Order_Detail d  GROUP BY d.CourseId) f ON a.Id=f.CourseId
        WHERE 1=1  AND a.State=1 ";
            if (input.Charge > 0)
            {
                strSql += " and a.FavourablePrice>0 ";
            }
            else
            {
                strSql += " and a.FavourablePrice=0 ";
            }
            return Db.QueryList<PaidListingsModel>(GetPageSql(sql + strSql, null, 1, 10, "Count", "desc"));
        }

        /// <summary>
        /// 获取视频页面相关的课程信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CourseInfoVideoOutput GetCourseVideo(string id)
        {
            var strSql = new StringBuilder(@"SELECT  a.* ,
        ISNULL(p.COUNT,0) AppraiseNum,
        ISNULL(p.AppraiseLevel,0) EvaluationScore
FROM    dbo.t_Course_Info a
        left join (SELECT CourseId,COUNT(1) COUNT,AVG(AppraiseLevel) AppraiseLevel FROM dbo.t_Course_Appraise GROUP BY CourseId) p on a.Id=p.CourseId
WHERE   a.Id = @Id ");
            var dynamicparamters = new DynamicParameters();
            dynamicparamters.Add(":Id", id, DbType.String);
            var obj = Db.QueryFirstOrDefault<CourseInfoVideoOutput>(strSql.ToString(), dynamicparamters);
            if (obj != null)
            {
                obj.CourseIamge = string.IsNullOrEmpty(obj.CourseIamge) ? "" : DefuleDomain + "/" + obj.CourseIamge;
                obj.CourseChapterModelOutput =
                    _courseChapterAppService.GetCourseChapterList(new CourseChapterModelInput { CourseId = id });
            }
            return obj;
        }



        /// <summary>
        /// 获取指定课程信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public CourseInfoModelOutput GetOne(string id)
        {
            var strSql = new StringBuilder(@"SELECT  a.* ,m.CourseName as  LinkCourseName,
        b.TeachersName ,
        b.TheLabel ,
        b.Synopsis ,
        b.TeacherPhoto ,
        j.Code ,
        c.CollectionNum ,
        d.PurchaseNum ,
        g.VideoLong CourseLongTimes ,
        g.COUNT CourseWareCounts,
        p.COUNT AppraiseNum
FROM    dbo.t_Course_Info a
        LEFT JOIN dbo.t_Course_OnTeachers b ON a.TeacherId = b.Id
        LEFT JOIN dbo.t_Course_Info m ON a.LinkCourse = m.Id
        LEFT JOIN t_Base_Basedata j ON a.CourseAttribute = j.Id
        left join (SELECT CourseId,COUNT(1) COUNT FROM dbo.t_Course_Appraise WHERE AuditStatus=1 GROUP BY CourseId) p on a.Id=p.CourseId
        LEFT JOIN ( SELECT  SUM(b.VideoLong) VideoLong ,
                            a.CourseId ,
                            SUM(b.COUNT) COUNT
                    FROM    dbo.t_Course_Chapter a
                            INNER JOIN ( SELECT c.ChapterId ,
                                                COUNT(1) COUNT ,
                                                SUM(c.VideoLong) VideoLong
                                         FROM   dbo.t_Course_Video c
                                         WHERE  c.IsDelete = 0
                                                AND c.VideoLongTime != '0'
                                         GROUP BY c.ChapterId
                                       ) b ON a.Id = b.ChapterId
                    GROUP BY a.CourseId
                  ) g ON a.Id = g.CourseId
        LEFT JOIN ( SELECT  CourseId ,
                            COUNT(*) AS CollectionNum
                    FROM    dbo.t_My_Collection
                    GROUP BY CourseId
                  ) c ON a.Id = c.CourseId
        LEFT JOIN ( SELECT  CourseId ,
                            COUNT(*) AS PurchaseNum
                    FROM    dbo.t_My_Course
                    GROUP BY CourseId
                  ) d ON a.Id = d.CourseId
WHERE   a.Id = @Id ");
            var dynamicparamters = new DynamicParameters();

            dynamicparamters.Add(":Id", id, DbType.String);
            var obj = Db.QueryFirstOrDefault<CourseInfoModelOutput>(strSql.ToString(), dynamicparamters);
            if (obj != null)
            {
                obj.CourseLongTimes = obj.CourseLongTimes == null ? "" : getFormatterTime(obj.CourseLongTimes);
                obj.CourseIamge = string.IsNullOrEmpty(obj.CourseIamge) ? "" : DefuleDomain + "/" + obj.CourseIamge;
                obj.CourseVedio = string.IsNullOrEmpty(obj.CourseVedio) ? "" : DefuleDomain + "/" + obj.CourseVedio;

                //obj.CourseVedio = QiniuHelp.AntiDaolianGetUrl( obj.CourseVedio);
                obj.EvaluationScore = GetAverageAppraise(id);
                var list = new List<CourseOnTeachers>();
                if (!string.IsNullOrEmpty(obj.TeacherId))
                {
                    var teacherids = obj.TeacherId.Split(',');
                    for (var i = 0; i < teacherids.Length; i++)
                    {
                        var model = _courseOnTeacherAppService.Get(teacherids[i]);
                        model.TeacherPhoto = string.IsNullOrEmpty(model.TeacherPhoto) ? "" : (DefuleDomain + "/" + model.TeacherPhoto);
                        list.Add(model);
                    }
                    obj.TeacherList = list;
                }
            }
            return obj;
        }


        /// <summary>
        /// 格式化视频时长 01：12:30  1小时12分30秒
        /// </summary>
        /// <param name="timeStr"></param>
        /// <returns></returns>
        private string getFormatterTime(string timeStr)
        {
            var longtime = int.Parse(timeStr.Split('.')[0]);
            var hour = longtime / 60 / 60;
            var minute = longtime / 60 - hour * 60;
            if (hour > 0)
            {
                return twolengthNumber(hour) + "时" + twolengthNumber(minute) + "分" + twolengthNumber(longtime % 3600 - minute * 60) + "秒";
            }
            else if (minute > 0)
            {
                return "0时" + twolengthNumber(minute) + "分" + twolengthNumber(longtime % 60) + "秒";
            }
            else
            {
                return "0时0分" + twolengthNumber(longtime % 60) + "秒";
            }
        }

        private string twolengthNumber(int num)
        {
            return num > 9 ? "" + num : "0" + num;
        }

        /// <summary>
        /// 统计指定课程综合评价
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public int GetAverageAppraise(string courseId)
        {
            const string strSql = " SELECT AVG(AppraiseLevel) FROM dbo.t_Course_Appraise WHERE CourseId=@CourseId GROUP BY CourseId ";
            var paramters = new DynamicParameters();
            paramters.Add(":CourseId", courseId, DbType.String);
            var result = Db.ExecuteScalar<int>(strSql, paramters);
            return result;
        }

        /// <summary>
        /// 获取指定讲师下的课程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<CourseInfoModelOutput> GetList(CourseInfoModelInput input)
        {
            var strSql = new StringBuilder(" SELECT a.*,b.TeachersName FROM dbo.t_Course_Info a  left join t_Course_OnTeachers b on a.TeacherId =b.Id WHERE a.IsDelete=0 ");
            var dynamicParameter = new DynamicParameters();
            if (!string.IsNullOrEmpty(input.TeacherId))
            {
                strSql.Append(" and TeacherId=@TeacherId ");
                dynamicParameter.Add(":TeacherId", input.TeacherId, DbType.String);
            }
            var list = Db.QueryList<CourseInfoModelOutput>(strSql.ToString(), dynamicParameter);
            foreach (var item in list)
            {
                item.CourseIamge = string.IsNullOrEmpty(item.CourseIamge) ? "" : DefuleDomain + "/" + item.CourseIamge;
            }
            return list;
        }

        /// <summary>
        /// 获取用户已购课程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<CourseInfoModelOutput> GetPurchaseList(OrderSheetInput input)
        {

            var sql = "IF( OBJECT_ID( 'tempdb..#subcourse' ) > 0 ) DROP TABLE #subcourse "
                       + " SELECT DISTINCT a.CourseId AS PackCourseId,b.CourseId,a.UserId,c.CourseWareCount INTO #subcourse FROM dbo.t_My_Course a "
                       + " LEFT JOIN dbo.t_Course_SuitDetail b ON a.CourseId=b.PackCourseId "
                       + " LEFT JOIN dbo.t_Course_Info c ON b.CourseId=c.Id "
                       + " WHERE a.CourseType=1 AND a.UserId=@RegisterUserId "
                       + " IF( OBJECT_ID( 'tempdb..#packinfo' ) > 0 ) "
                       + " DROP TABLE #packinfo "
                       + " SELECT d.*INTO #packinfo FROM ( "
                        + " SELECT a.PackCourseId,SUM( a.CourseWareCount ) AS CourseWareCount, SUM( ISNULL( b.HasLearnCount, 0 ) ) AS HasLearnCount FROM #subcourse a "
                        + " LEFT JOIN( SELECT UserId, CourseId, COUNT( 1 ) AS HasLearnCount FROM T_Course_LearnProgress GROUP BY UserId, CourseId )b "
                         + " ON a.UserId = b.UserId AND a.CourseId = b.CourseId GROUP BY a.PackCourseId "
                         + " )d "
                        + "SELECT b.id,a.CourseWareCount,a.HasLearnCount,b.CourseName,b.CourseIamge,1 AS CourseType FROM #packinfo a LEFT JOIN dbo.t_Course_PackcourseInfo b ON a.PackCourseId=b.Id "
                        + " union "
                        + " SELECT DISTINCT c.Id,ISNULL(c.CourseWareCount,0)AS CourseWareCount ,ISNULL(e.HasLearnCount,0)AS HasLearnCount,C.CourseName,c.CourseIamge,0 AS CourseType   FROM dbo.t_My_Course a  LEFT JOIN dbo.t_Course_Info c ON a.CourseId = c.Id LEFT JOIN( SELECT COUNT( *) AS HasLearnCount, CourseId, UserId FROM T_Course_LearnProgress WHERE UserId = @RegisterUserId  GROUP BY CourseId, UserId ) e ON c.id = e.CourseId WHERE a.CourseType=0 AND a.UserId = @RegisterUserId ";
            var dynamicParameter = new DynamicParameters();
            dynamicParameter.Add(":RegisterUserId", input.RegisterUserId, DbType.String);
            var list = Db.QueryList<CourseInfoModelOutput>(sql, dynamicParameter);

            foreach (var item in list)
            {
                item.CourseIamge = string.IsNullOrEmpty(item.CourseIamge) ? "" : DefuleDomain + "/" + item.CourseIamge;
            }
            return list;
        }



        /// <summary>
        /// 获取收藏的课程 包含套餐课程
        /// </summary>
        /// <returns></returns>
        public List<CourseInfoModelOutput> GetCollectedCourseList(MyCollectionModuleInput input)
        {
            var strSql = "  SELECT DISTINCT b.Id,b.CourseName,b.CourseIamge,b.CourseType,c.TeachersName FROM dbo.t_My_Collection a LEFT JOIN ( SELECT d.Id, d.CourseName, d.CourseIamge,0 as CourseType, d.TeacherId FROM  dbo.t_Course_Info d UNION SELECT e.Id, e.CourseName, e.CourseIamge,1 as CourseType,'' AS TeacherId FROM dbo.t_Course_PackcourseInfo e ) b ON a.CourseId = b.Id LEFT join t_Course_OnTeachers c on b.TeacherId = c.Id WHERE  a.UserId=@UserId ";
            var dynamicParameter = new DynamicParameters();
            dynamicParameter.Add(":UserId", input.UserId, DbType.String);
            var list = Db.QueryList<CourseInfoModelOutput>(strSql, dynamicParameter);
            foreach (var item in list)
            {
                item.CourseIamge = DefuleDomain + "/" + item.CourseIamge;
            }
            return list;
        }

        /// <summary>
        /// 获取与指定课程所属相同的项目下的课程
        /// </summary>
        /// <returns></returns>
        public List<CoursePackcourseInfoModelOutput> GetPackcourseInfoList(string courseId)
        {
            var strSql =
                $@"SELECT  b.Id ,
        b.CourseName ,
        b.CourseIamge ,
        b.Price ,
        b.FavourablePrice ,
        d.COUNT CourseWareCount
FROM    dbo.t_Course_SuitDetail a
        LEFT JOIN t_Course_PackcourseInfo b ON a.PackCourseId = b.Id
        LEFT JOIN ( SELECT  c.PackCourseId PackCourseId ,
                            COUNT(1) COUNT
                    FROM    dbo.t_Course_SuitDetail c
                    GROUP BY c.PackCourseId
                  ) d ON a.PackCourseId = d.PackCourseId
WHERE  a.CourseId = '{courseId}' ";
            var list = Db.QueryList<CoursePackcourseInfoModelOutput>(strSql);
            foreach (var item in list)
            {
                item.CourseIamge = string.IsNullOrEmpty(item.CourseIamge) ? "" : DefuleDomain + "/" + item.CourseIamge;
            }
            return list;
        }

        /// <summary>
        /// 获取用户课程
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<MyCourseModelOutput> GetMyCourse(string userid)
        {

            //单个课程
            var sql = @"
            SELECT b.*,a.CourseType, ISNULL(c.PracticePaperNum,0) PracticePaperNum,ISNULL(c.TotalPaperNum,0) TotalPaperNum,ISNULL(e.ChapterPracticeNum,0) ChapterPracticeNum,ISNULL(d.TotalChapterPractice,0) TotalChapterPractice,a.EndTime FROM dbo.t_My_Course a 
            LEFT JOIN 
            (
            SELECT DISTINCT
        c.Id AS CourseId ,
        ISNULL(c.CourseWareCount, 0) AS CourseWareCount ,
        ISNULL(e.HasLearnCount, 0) AS HasLearnCount ,
        c.CourseName ,
        c.CourseIamge,
        c.Type 
 FROM   dbo.t_My_Course a
        LEFT JOIN dbo.V_Course_Packcourse_Info c ON a.CourseId = c.Id
        LEFT JOIN ( SELECT  COUNT(*) AS HasLearnCount ,
                            CourseId ,
                            UserId
                    FROM    T_Course_LearnProgress
                    WHERE   UserId = @UserId
                    GROUP BY CourseId ,
                            UserId
                  ) e ON c.Id = e.CourseId WHERE  a.UserId = @UserId
            ) b ON a.CourseId=b.CourseId
            LEFT JOIN (--试卷
	
	            SELECT  b.UserId, a.CourseId,COUNT(*) AS PracticePaperNum, COUNT(a.Id)AS TotalPaperNum FROM dbo.t_Course_Paper a RIGHT JOIN (
		            SELECT DISTINCT PaperId,UserId  FROM dbo.t_My_PaperRecords  GROUP BY PaperId,UserId
	            ) b ON a.PaperInfoId=b.PaperId  WHERE b.UserId=@UserId GROUP BY a.CourseId,b.UserId
            )c ON a.CourseId=c.CourseId
            --章节练习总数

            LEFT JOIN 
            (
	            SELECT a.CourseId,COUNT(a.ChapterId) AS TotalChapterPractice FROM 
	            (SELECT  CourseId,ChapterId,COUNT(SubjectId) AS subjectNum FROM dbo.t_Course_Subject   GROUP	BY CourseId,ChapterId)a
	            WHERE a.subjectNum>0 GROUP BY a.CourseId--总的章节练习数，试题总数为0的不计入到章节练习总数
            )d
            ON a.CourseId=d.CourseId
            -- 个人章节练习数
            LEFT JOIN (

	            SELECT a.courseId,COUNT(a.ChapterId) AS ChapterPracticeNum FROM (
	            (SELECT DISTINCT CourseId,ChapterId FROM dbo.t_Course_Subject  )
	            )a
	            LEFT JOIN (
		            SELECT DISTINCT UserId,ChapterId  FROM t_Course_ChapterQuestions 
	            )b
	            ON a.ChapterId=b.ChapterId
	            WHERE b.UserId=@UserId GROUP BY a.CourseId
            )e
            ON d.CourseId=e.CourseId
            WHERE a.UserId=@UserId AND b.Type!=1 and  a.CourseType=0 order by a.AddTime DESC ";
            var dynamicParameter = new DynamicParameters();
            dynamicParameter.Add(":UserId", userid, DbType.String);
            var list = Db.QueryList<MyCourseModelOutput>(sql, dynamicParameter);

            foreach (var item in list)
            {
                item.CourseIamge = string.IsNullOrEmpty(item.CourseIamge) ? "" : DefuleDomain + "/" + item.CourseIamge;
            }

            //套餐课程
            var sql2 = @"SELECT a.CourseType,a.courseId,b.CourseName,b.CourseIamge,ISNULL(SUM(a.CourseWareCount),0) CourseWareCount,ISNULL(SUM(a.HasLearnCount),0) HasLearnCount ,ISNULL(SUM(a.PracticePaperNum),0) PracticePaperNum,ISNULL(SUM(a.TotalPaperNum),0) TotalPaperNum,ISNULL(SUM(a.ChapterPracticeNum),0) ChapterPracticeNum,ISNULL(SUM(a.TotalChapterPractice),0) TotalChapterPractice
            FROM(
            SELECT a.CourseType,a.CourseId,ISNULL(c.CourseWareCount,0)AS CourseWareCount ,ISNULL(g.HasLearnCount,0)AS HasLearnCount,ISNULL(d.PracticePaperNum,0) PracticePaperNum,ISNULL(d.TotalPaperNum,0) TotalPaperNum,ISNULL(f.ChapterPracticeNum,0) ChapterPracticeNum,ISNULL(e.TotalChapterPractice,0) TotalChapterPractice FROM dbo.t_My_Course  a
            LEFT JOIN dbo.t_Course_SuitDetail b ON a.CourseId=b.PackCourseId
             LEFT JOIN (--试卷

            	            SELECT  b.UserId, a.CourseId,COUNT(*) AS PracticePaperNum, COUNT(a.Id)AS TotalPaperNum FROM dbo.t_Course_Paper a RIGHT JOIN (
            		            SELECT DISTINCT PaperId,UserId  FROM dbo.t_My_PaperRecords  GROUP BY PaperId,UserId
            	            ) b ON a.PaperInfoId=b.PaperId  WHERE b.UserId=@UserId GROUP BY a.CourseId,b.UserId
                        )d ON b.CourseId=d.CourseId
                        --章节练习总数

                        LEFT JOIN 
                        (
            	            SELECT a.CourseId,COUNT(a.ChapterId) AS TotalChapterPractice FROM 
            	            (SELECT  CourseId,ChapterId,COUNT(SubjectId) AS subjectNum FROM dbo.t_Course_Subject   GROUP	BY CourseId,ChapterId)a
            	            WHERE a.subjectNum>0 GROUP BY a.CourseId--总的章节练习数，试题总数为0的不计入到章节练习总数
                        )e
                        ON b.CourseId=e.CourseId
                        -- 个人章节练习数
                        LEFT JOIN (

            	            SELECT a.courseId,COUNT(a.ChapterId) AS ChapterPracticeNum FROM (
            	            (SELECT DISTINCT CourseId,ChapterId FROM dbo.t_Course_Subject  )
            	            )a
            	            LEFT JOIN (
            		            SELECT DISTINCT UserId,ChapterId  FROM t_Course_ChapterQuestions 
            	            )b
            	            ON a.ChapterId=b.ChapterId
            	            WHERE b.UserId=@UserId GROUP BY a.CourseId
                        )f
                        ON b.CourseId=f.CourseId

             LEFT JOIN dbo.t_Course_Info c ON b.CourseId = c.Id 
            LEFT JOIN(
             SELECT COUNT( *) AS HasLearnCount, CourseId, UserId FROM T_Course_LearnProgress WHERE UserId=@UserId   GROUP BY CourseId, UserId 
             ) g ON c.id = g.CourseId
            WHERE a.CourseType=1 AND a.Userid=@Userid
            )a
            LEFT JOIN dbo.t_Course_PackcourseInfo b ON a.CourseId=b.Id
             GROUP BY a.CourseId,b.CourseName,b.CourseIamge,a.CourseType";

            var list2 = Db.QueryList<MyCourseModelOutput>(sql2, dynamicParameter);
            foreach (var item in list2)
            {
                item.CourseIamge = string.IsNullOrEmpty(item.CourseIamge) ? "" : DefuleDomain + "/" + item.CourseIamge;
            }
            list.AddRange(list2);

            return list;

        }


        /// <summary>
        /// 获取用户题库
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<MyCourseModelOutput> MyQuestion(string userid)
        {

            //单个课程
            var sql = @"
            SELECT b.*,a.CourseType, ISNULL(c.PracticePaperNum,0) PracticePaperNum,ISNULL(c.TotalPaperNum,0) TotalPaperNum,ISNULL(e.ChapterPracticeNum,0) ChapterPracticeNum,ISNULL(d.TotalChapterPractice,0) TotalChapterPractice,a.EndTime FROM dbo.t_My_Course a 
            LEFT JOIN 
            (
            SELECT DISTINCT
        c.Id AS CourseId ,
        ISNULL(c.CourseWareCount, 0) AS CourseWareCount ,
        ISNULL(e.HasLearnCount, 0) AS HasLearnCount ,
        c.CourseName ,
        c.CourseIamge,
        c.Type 
 FROM   dbo.t_My_Course a
        LEFT JOIN dbo.V_Course_Packcourse_Info c ON a.CourseId = c.Id
        LEFT JOIN ( SELECT  COUNT(*) AS HasLearnCount ,
                            CourseId ,
                            UserId
                    FROM    T_Course_LearnProgress
                    WHERE   UserId = @UserId
                    GROUP BY CourseId ,
                            UserId
                  ) e ON c.Id = e.CourseId WHERE  a.UserId = @UserId   --获取题库
            ) b ON a.CourseId=b.CourseId
            LEFT JOIN (--试卷
	
	            SELECT  b.UserId, a.CourseId,COUNT(*) AS PracticePaperNum, COUNT(a.Id)AS TotalPaperNum FROM dbo.t_Course_Paper a RIGHT JOIN (
		            SELECT DISTINCT PaperId,UserId  FROM dbo.t_My_PaperRecords  GROUP BY PaperId,UserId
	            ) b ON a.PaperInfoId=b.PaperId  WHERE b.UserId=@UserId GROUP BY a.CourseId,b.UserId
            )c ON a.CourseId=c.CourseId
            --章节练习总数

            LEFT JOIN 
            (
	            SELECT a.CourseId,COUNT(a.ChapterId) AS TotalChapterPractice FROM 
	            (SELECT  CourseId,ChapterId,COUNT(SubjectId) AS subjectNum FROM dbo.t_Course_Subject   GROUP	BY CourseId,ChapterId)a
	            WHERE a.subjectNum>0 GROUP BY a.CourseId--总的章节练习数，试题总数为0的不计入到章节练习总数
            )d
            ON a.CourseId=d.CourseId
            -- 个人章节练习数
            LEFT JOIN (

	            SELECT a.courseId,COUNT(a.ChapterId) AS ChapterPracticeNum FROM (
	            (SELECT DISTINCT CourseId,ChapterId FROM dbo.t_Course_Subject  )
	            )a
	            LEFT JOIN (
		            SELECT DISTINCT UserId,ChapterId  FROM t_Course_ChapterQuestions 
	            )b
	            ON a.ChapterId=b.ChapterId
	            WHERE b.UserId=@UserId  GROUP BY a.CourseId
            )e
            ON d.CourseId=e.CourseId
            WHERE a.UserId=@UserId  and b.Type=1  order by a.AddTime DESC ";
            var dynamicParameter = new DynamicParameters();
            dynamicParameter.Add(":UserId", userid, DbType.String);
            var list = Db.QueryList<MyCourseModelOutput>(sql, dynamicParameter);
            foreach (var item in list)
            {
                item.CourseIamge = string.IsNullOrEmpty(item.CourseIamge) ? "" : DefuleDomain + "/" + item.CourseIamge;
            }
            return list;
        }

        /// <summary>
        /// 推荐课程
        /// </summary>
        /// <returns></returns>
        public List<CourseInfoModel> GetRecommendCourse(CourseInfoModelInput input)
        {
            string sql = @"SELECT  a.* ,
        d.TeachersName ,
        b.SubjectName ,
       -- c.Id AS ProjectId ,
        f.Count ,
        j.Code ,
        k.AppraiseLevel ,
        l.Count StudyCount FROM    V_Course_Packcourse_Info a
        LEFT JOIN t_Base_Subject b ON REPLACE(a.SubjectId, ',', '') = b.Id 
       -- LEFT JOIN t_Base_Project c ON a.ProjectId = c.Id
        LEFT JOIN t_Course_OnTeachers d ON a.TeacherId = d.Id
        LEFT JOIN ( SELECT  COUNT(1) Count ,
                            MAX(d.CourseId) CourseId
                    FROM    dbo.t_My_Course d
                    GROUP BY d.CourseId
                  ) f ON a.Id = f.CourseId
        LEFT JOIN t_Base_Basedata j ON a.CourseAttribute = j.Id
        LEFT JOIN ( SELECT  g.CourseId CourseId ,
                            AVG(g.AppraiseLevel) AppraiseLevel
                    FROM    dbo.t_Course_Appraise g
                    GROUP BY g.CourseId
                  ) k ON a.Id = k.CourseId
        LEFT JOIN ( SELECT  g.CourseId CourseId ,
                            COUNT(1) Count
                    FROM    dbo.t_My_Course g
                    GROUP BY g.CourseId
                  ) l ON a.Id = l.CourseId
WHERE   1 = 1
        AND a.State = 1 AND a.CourseType=0 and a.Type=0 and isnull(a.IsValueAdded,0)=0  and b.IsEconomicBase=0 ";
            var dy = new DynamicParameters();
            if (!string.IsNullOrEmpty(input.SubjectId))
            {
                sql += " and a.SubjectId like @SubjectId ";
                dy.Add(":SubjectId", '%' + input.SubjectId + '%', DbType.AnsiString);
            }
            if (input.IsFree.HasValue)
            {
                switch (input.IsFree.Value)
                {
                    case 1:
                        input.Sidx = "AppraiseLevel,StudyCount,FavourablePrice";
                        break;
                    case 2:
                        input.Sidx = "AppraiseLevel";
                        break;
                    case 3:
                        input.Sidx = "StudyCount";
                        break;
                    case 4:
                        input.Sidx = "FavourablePrice";
                        break;
                }
            }
            if (input.IsRecommend.HasValue)
            {
                sql += " and a.IsRecommend=@IsRecommend ";
                dy.Add(":IsRecommend", input.IsRecommend, DbType.Int32);
            }
            if (!string.IsNullOrEmpty(input.UserId))
            {
                sql += " AND a.Id NOT IN(SELECT DISTINCT CourseId FROM dbo.t_My_Course WHERE UserId = @UserId) ";
                dy.Add(":UserId", input.UserId, DbType.String);
            }
            var list = Db.QueryList<CourseInfoModel>(GetPageSql(sql,
                dy,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dy);

            foreach (var item in list)
            {
                item.CourseIamge = string.IsNullOrEmpty(item.CourseIamge) ? "" : DefuleDomain + "/" + item.CourseIamge;
            }
            return list;
        }



        /// <summary>
        /// 推荐课程
        /// </summary>
        /// <returns></returns>
        public List<CourseInfoModelOutput> GetRecommendCourse()
        {
            var strSql = string.Format(@" SELECT TOP 5
            a.* ,b.PurchaseCount
            FROM    t_Course_Info a
            LEFT JOIN (SELECT CourseId,COUNT(*) PurchaseCount FROM dbo.t_My_Course GROUP BY CourseId)b
            ON a.Id=b.CourseId
            WHERE   a.IsDelete = 0
            ORDER BY NEWID()");

            var list = Db.QueryList<CourseInfoModelOutput>(strSql);
            foreach (var item in list)
            {
                item.CourseIamge = string.IsNullOrEmpty(item.CourseIamge) ? "" : DefuleDomain + "/" + item.CourseIamge;
            }
            return list;
        }

        public List<MyCourseModelOutput> GetCollectedCourse(string userid)
        {
            var sql = @" SELECT b.CourseName,a.Id as CourseId,b.CourseIamge,a.AddTime FROM dbo.t_My_Collection a
                LEFT JOIN dbo.t_Course_Info b
                 ON a.CourseId=b.Id 
                 WHERE a.UserId=@UserId ";
            var dynamicParameter = new DynamicParameters();
            dynamicParameter.Add(":UserId", userid, DbType.String);
            var list = Db.QueryList<MyCourseModelOutput>(sql, dynamicParameter);
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    item.CourseIamge = string.IsNullOrEmpty(item.CourseIamge) ? "" : DefuleDomain + "/" + item.CourseIamge;
                    var subSql = string.Format(@"SELECT a.VideoName,a.VideoId FROM (
                    SELECT b.VideoName,b.Id AS VideoId,MAX(a.WatchTime) AS latestTime FROM t_My_VideoWatch a LEFT JOIN dbo.t_Course_Video b ON a.VideoId=b.Id 
                    WHERE b.ChapterId IN 
                    (SELECT b.ChapterId FROM dbo.t_Course_Chapter WHERE CourseId='{0}' AND a.UserId='{1}')
                    GROUP BY b.VideoName,b.Id
                    )a", item.CourseId, userid);
                    var model = Db.QueryFirstOrDefault<MyCourseModelOutput>(subSql, null);
                    if (model != null)
                    {
                        item.VideoName = model.VideoName;
                        item.VideoId = model.VideoId;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取与指定课程所属相同的项目下的课程
        /// </summary>
        /// <returns></returns>
        public List<CourseInfoModelOutput> GetRandomListExceptCurrent(string courseId)
        {
            var strSql = string.Format(@"SELECT TOP 10
        a.* ,
        b.TeachersName ,
        j.Code
FROM    V_Course_Packcourse_Info a
        LEFT JOIN dbo.t_Course_OnTeachers b ON a.TeacherId = b.Id
        LEFT JOIN t_Base_Basedata j ON a.CourseAttribute = j.Id
        LEFT JOIN dbo.t_Base_Subject n ON a.SubjectId = n.Id
        LEFT JOIN dbo.t_Base_Project c ON n.ProjectId = c.Id
WHERE   1=1
       -- AND a.FavourablePrice > 0
       -- AND a.Price > 0
        And a.Id <>'{0}'
        AND c.Id = ( SELECT c.Id
                     FROM   dbo.t_Course_Info a
                            LEFT JOIN dbo.t_Base_Subject b ON a.SubjectId = b.Id
                            LEFT JOIN dbo.t_Base_Project c ON b.ProjectId = c.Id
                     WHERE  a.Id = '{0}'
                   )
ORDER BY NEWID() ", courseId);

            var list = Db.QueryList<CourseInfoModelOutput>(strSql);
            foreach (var item in list)
            {
                item.CourseIamge = string.IsNullOrEmpty(item.CourseIamge) ? "" : DefuleDomain + "/" + item.CourseIamge;
            }
            return list;
        }





        /// <summary>
        /// 获取和面授课程匹配的打包课程
        /// </summary>
        /// <returns></returns>
        public List<CourseInfoModelOutput> GetRandomListFaceToFace(string courseId)
        {
            var model = _courseFaceToFaceRepository.Get(courseId);
            var strSql = string.Format(@" SELECT   a.* 
FROM    dbo.t_Course_PackcourseInfo a
 where 1=1 and a.CourseName  like '%{0}%' ", model.ClassName);
            var list = Db.QueryList<CourseInfoModelOutput>(strSql);
            foreach (var item in list)
            {
                item.CourseIamge = string.IsNullOrEmpty(item.CourseIamge) ? "" : DefuleDomain + "/" + item.CourseIamge;
            }
            return list;
        }

        /// <summary>
        /// 获取面授课程的主讲教师
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public List<CourseOnTeachers> GetOnTeachers(string courseId)
        {
            var model = _courseFaceToFaceRepository.Get(courseId);
            var teacherId = model.TeacherId.Split(',');
            var ids = teacherId.Aggregate("", (current, item) => current + ("'" + item + "',")).TrimEnd(',');
            var strSql = string.Format(@" SELECT   a.* 
FROM    dbo.t_Course_OnTeachers a
 where 1=1 and a.Id  in ({0}) ", ids);
            var list = Db.QueryList<CourseOnTeachers>(strSql);
            foreach (var item in list)
            {
                item.TeacherPhoto = string.IsNullOrEmpty(item.TeacherPhoto) ? "" : DefuleDomain + "/" + item.TeacherPhoto;
            }
            return list;
        }


        /// <summary>
        /// 搜索课程
        /// </summary>
        /// <param name="input"></param>
        /// /// <param name="count"></param>
        /// <returns></returns>
        public List<CourseInfoModelOutput> GetSearchList(CourseInfoModelInput input, out int count)
        {
            var strSql = "select *  ";
            var sql = " from  t_Course_Info where IsDelete=0 ";
            var strCount = "select count(*) ";
            var dynamicparameter = new DynamicParameters();
            if (String.IsNullOrEmpty(input.CourseName))
            {

                sql += " and CourseName like @CourseName";
                dynamicparameter.Add(":CourseName", "%" + input.CourseName + "%", DbType.String);
            }
            count = Db.ExecuteScalar<int>(strCount + sql, dynamicparameter);
            var list = Db.QueryList<CourseInfoModelOutput>(GetPageSql(strSql + sql, dynamicparameter, input.Page, input.Rows, input.Sidx, input.Sord), dynamicparameter);

            foreach (var item in list)
            {
                item.CourseIamge = string.IsNullOrEmpty(item.CourseIamge) ? "" : DefuleDomain + "/" + item.CourseIamge;
            }
            return list;
        }

        /// <summary>
        /// 获取增值服务推荐套餐课程
        /// </summary>
        /// <param name="courseId">课程编号</param>
        /// <param name="userId"></param>
        /// <param name="SubjectId"></param>
        /// <returns></returns>
        public List<AdvertisingOutput> Advertising(string courseId, string userId, string SubjectId)
        {
            var model = _repository.Get(courseId);
            var subjectModel = _subjectAppService.Get(model.SubjectId);
            var subjectSql = "";
            if (subjectModel.IsEconomicBase == 0)
            {
                subjectSql = $" a.SubjectId LIKE ( '%{model.SubjectId}%' )";
            }
            else
            {
                if (!string.IsNullOrEmpty(SubjectId))
                {
                    subjectSql = $" a.SubjectId LIKE ( '%{SubjectId}%' )";
                }
                else
                {
                    var jjjcSql =
                        $"select * from t_base_Subject where IsEconomicBase=1  and ProjectId='{subjectModel.ProjectId}' ";
                    var jjjcModel = Db.QueryFirstOrDefault<Subject>(jjjcSql, null);
                    subjectSql = $" ( a.SubjectId LIKE ( '%{model.SubjectId}%' ) or  a.SubjectId LIKE ( '%{jjjcModel.Id}%' ))";
                }
            }
            var strSql = $@"SELECT TOP 2
        a.*
FROM    dbo.V_Course_Packcourse_Info a
WHERE  {subjectSql}
        AND a.State = 1
        AND a.Type = 0
        AND a.IsValueAdded = 0
        AND a.Id NOT IN (
        SELECT  b.CourseId
        FROM    dbo.t_My_Course b
        WHERE   b.EndTime > GETDATE()
                AND b.UserId = '{userId}'
        UNION
        SELECT  c.CourseId
        FROM    dbo.t_My_Course b
                LEFT JOIN t_Course_SuitDetail c ON b.CourseId = c.PackCourseId
        WHERE   b.EndTime > GETDATE()
                AND b.CourseType = 1
                AND b.UserId = '{userId}' )
ORDER BY a.CourseType DESC,a.OrderNo asc ";
            var data = Db.QueryList<AdvertisingOutput>(strSql, null);
            foreach (var item in data)
            {
                item.CourseIamge = string.IsNullOrEmpty(item.CourseIamge) ? "" : DefuleDomain + "/" + item.CourseIamge;
            }
            return data;
        }



        /// <summary>
        /// 通过课程编号获取科目，项目编号信息
        /// </summary>
        /// <param name="courseId">课程编号</param>
        /// <returns></returns>
        public ProjectSubjectOutput GetProjectSubject(string courseId)
        {

            var strSql = @" SELECT  a.Id  ,
        a.CourseName ,
        b.Id  SubjectId,
        b.SubjectName  SubjectName,
        c.Id  ProjectId,
        c.ProjectName ProjectName,
        d.Id AS ProjectClassId,
        d.ProjectClassName ProjectClassName
FROM    dbo.t_Course_Info a
        LEFT JOIN dbo.t_Base_Subject b ON a.SubjectId = b.Id
        LEFT JOIN dbo.t_Base_Project c ON b.ProjectId = c.Id
        LEFT JOIN dbo.t_Base_ProjectClass d ON c.ProjectClassId=d.Id  where 1=1 ";
            strSql += " and a.Id=@Id ";
            var dy = new DynamicParameters();
            dy.Add(":Id", courseId, DbType.String);
            return Db.QueryFirstOrDefault<ProjectSubjectOutput>(strSql, dy);

        }

        /// <summary>
        /// 获取学习指定课程用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<UserLearnOutput> GetLearnPerson(CourseLearnUserInput input, out int count)
        {
            var sql = " SELECT distinct b.* ";
            var sqlFrom =
                " FROM dbo.t_My_Course a LEFT JOIN dbo.t_Base_RegisterUser b ON a.UserId=b.Id WHERE a.CourseId=@CourseId ";
            var sqlCount = " select count(*) ";
            var parameters = new DynamicParameters();
            parameters.Add(":CourseId", input.CourseId, DbType.String);

            count = Db.ExecuteScalar<int>(sqlCount + sqlFrom, parameters);
            var list =
                Db.QueryList<UserLearnOutput>(
                    GetPageSql(sql + sqlFrom, parameters, input.Page, input.Rows, input.Sidx, input.Sord), parameters);
            return list;
        }

        /// <summary>
        /// 递增浏览记录数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool UpdateViewCount(CourseInfoInput input)
        {
            var strSql = "UPDATE dbo.t_Course_Info SET ViewCount=ISNULL(ViewCount,0)+1 WHERE id= '" + input.Id + "'";
            return Db.ExecuteScalar<int>(strSql, null) > 0;
        }


        /// <summary>
        /// 获取标题 描述 关键字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public ModelJob GetTitle(TitleInput input)
        {
            var strSql = "";
            var dy = new DynamicParameters();
            if (input.Type == 0)
            {
                strSql = " SELECT Title,KeyWord,Description FROM V_Course_Packcourse_Info where 1=1 ";
                strSql += " and Id =@Id";
                dy.Add(":Id", input.Id, DbType.String);
                return Db.QueryFirstOrDefault<ModelJob>(strSql, dy);
            }
            return new ModelJob { Description = _setAppService.GetArguValue("webdescript"), KeyWord = _setAppService.GetArguValue("webkeywords"), Title = _setAppService.GetArguValue("webtitle") };
        }

        /// <summary>
        /// 获取指定套餐课程学习进度
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="courseid"></param>
        /// <returns></returns>
        public CourseInfoModelOutput LoadPackProgress(string userid, string courseid)
        {
            var sql = "IF( OBJECT_ID( 'tempdb..#subcourse' ) > 0 ) DROP TABLE #subcourse "
                      + " SELECT DISTINCT a.CourseId AS PackCourseId,b.CourseId,a.UserId,c.CourseWareCount INTO #subcourse FROM dbo.t_My_Course a "
                      + " LEFT JOIN dbo.t_Course_SuitDetail b ON a.CourseId=b.PackCourseId "
                      + " LEFT JOIN dbo.t_Course_Info c ON b.CourseId=c.Id "
                      + " WHERE a.CourseType=1 AND a.UserId=@RegisterUserId  AND a.CourseId=@CourseId "
                      + " IF( OBJECT_ID( 'tempdb..#packinfo' ) > 0 ) "
                      + " DROP TABLE #packinfo "
                      + " SELECT d.*INTO #packinfo FROM ( "
                       + " SELECT a.PackCourseId,SUM( a.CourseWareCount ) AS CourseWareCount, SUM( ISNULL( b.HasLearnCount, 0 ) ) AS HasLearnCount FROM #subcourse a "
                       + " LEFT JOIN( SELECT UserId, CourseId, COUNT( 1 ) AS HasLearnCount FROM T_Course_LearnProgress GROUP BY UserId, CourseId )b "
                        + " ON a.UserId = b.UserId AND a.CourseId = b.CourseId GROUP BY a.PackCourseId "
                        + " )d "
                       + "SELECT b.id,a.CourseWareCount,a.HasLearnCount,b.CourseName,b.CourseIamge,1 AS CourseType FROM #packinfo a LEFT JOIN dbo.t_Course_PackcourseInfo b ON a.PackCourseId=b.Id ";
            var dynamicParameter = new DynamicParameters();
            dynamicParameter.Add(":RegisterUserId", userid, DbType.String);
            dynamicParameter.Add(":CourseId", courseid, DbType.String);
            var model = Db.QueryFirstOrDefault<CourseInfoModelOutput>(sql, dynamicParameter);
            return model;
        }


        /// <summary>
        /// 根据视频code  获取课程编号
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>

        public string GetCourseId(string code)
        {
            var str = @" 
 SELECT b.CourseId FROM dbo.t_Course_Video a LEFT JOIN dbo.t_Course_Chapter b ON a.ChapterId=b.Id
WHERE a.Code=@code"; var dy = new DynamicParameters();
            dy.Add(":code", code, DbType.String);
            return Db.QueryFirstOrDefault<string>(str, dy);
        }


        /// <summary>
        /// 根据课程编号获取各子菜单的显示状态
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>

        public CourseStatus GetCourseStatus(string courseId)
        {
            var str = @"SELECT  *,g.CourseName,g.CourseIamge
FROM    ( SELECT    a.CourseId ,
                    ISNULL(SUM(CASE WHEN cc = 'no' THEN [a].Count1
                               END), 0) zjCount ,
                    ISNULL(SUM(CASE WHEN cc = 'jy' THEN [a].Count1
                               END), 0) jyCount ,
                    ISNULL(SUM(CASE WHEN cc = 'yes' THEN a.Count1
                               END), 0) mnsjCount ,
                    ISNULL(SUM(CASE WHEN cc = '' THEN a.Count1
                               END), 0) lnztCount
          FROM      ( SELECT    j.Id CourseId ,
                                COUNT(1) Count1 ,
                                '22' cc
                      FROM      dbo.t_Course_Info j
                      GROUP BY  j.Id
                      UNION ALL
                      SELECT    j.CourseId CourseId ,
                                COUNT(1) Count1 ,
                                'jy' cc
                      FROM      dbo.t_Course_Resource j
                      GROUP BY  j.CourseId
                      UNION ALL
                      SELECT    d.CourseId ,
                                COUNT(1) Count1 ,
                                'no' cc
                      FROM      dbo.t_Course_Chapter d
                      GROUP BY  CourseId
                      UNION ALL
                      SELECT    f.CourseId ,
                                COUNT(1) Count1 ,
                                'yes' cc
                      FROM      dbo.t_Paper_Group d
                                LEFT JOIN t_Course_Paper f ON d.Id = f.PaperInfoId
                      WHERE     d.Type = 1
                      GROUP BY  CourseId
                      UNION ALL
                      SELECT    f.CourseId ,
                                COUNT(1) Count1 ,
                                '' cc
                      FROM      dbo.t_Paper_Group d
                                LEFT JOIN t_Course_Paper f ON d.Id = f.PaperInfoId
                      WHERE     d.Type = 0
                      GROUP BY  CourseId
                    ) a
          GROUP BY  CourseId
        ) d
        LEFT JOIN ( SELECT  ArguValue
                    FROM    T_Base_SysSet
                    WHERE   ArguName = 'kcpl'
                  ) f ON 1 = 1 
                  LEFT JOIN dbo.t_Course_Info g ON d.CourseId=g.Id
where d.CourseId=@CourseId"; var dy = new DynamicParameters();
            dy.Add(":CourseId", courseId, DbType.String);
            var model = Db.QueryFirstOrDefault<CourseStatus>(str, dy);
            model.CourseIamge = DefuleDomain + "/" + model.CourseIamge;
            return model;
        }
    }
}