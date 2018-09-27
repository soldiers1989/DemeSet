using Dapper;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Application.WebApiDto.CourseModule;
using ZF.Application.WebApiDto.CoursePackModule;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.CoursePackModule
{
    /// <summary>
    /// 
    /// </summary>
    public class CoursePackAppService : BaseAppService<CoursePackcourseInfo>
    {
        private static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];
        private readonly ICoursePackcourseInfoRepository _repository;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public CoursePackAppService(ICoursePackcourseInfoRepository repository) : base(repository)
        {
            _repository = repository;
        }


        /// <summary>
        /// 获取套餐课程列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<CoursePackModelOutput> GetList(CoursePackModelInput input, out int count)
        {
            const string sql = "select  a.* ";
            var strSql = new StringBuilder(" from t_Course_PackcourseInfo  a  where a.IsDelete=0  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.CourseName))
            {
                strSql.Append(" and a.CourseName = @CourseName ");
                dynamicParameters.Add(":CourseName", input.CourseName, DbType.String);
            }



            if (string.IsNullOrEmpty(input.SubjectedsId))
            {
                strSql.Append(" and  SubjectsId like @subje");
                dynamicParameters.Add(":subjectName", input.SubjectedsId.ToString(), DbType.String);
            }

            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<CoursePackModelOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
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
        /// 获取套餐课程详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public CoursePackModelOutput GetCoursePackDetail(CoursePackModelInput input)
        {
            var strSql = @" SELECT a.* ,
        b.CollectionNum ,
        c.PurchaseNum ,
        f.COUNT CourseWareCount ,
        l.VideoLong  CourseLongTime,
        p.COUNT AppraiseNum
 FROM   t_Course_PackcourseInfo a
        LEFT JOIN ( SELECT  e.PackCourseId ,
                            COUNT(1) COUNT
                    FROM    t_Course_SuitDetail e
                            LEFT JOIN dbo.t_Course_Info h ON e.CourseId = h.Id
                    GROUP BY e.PackCourseId
                  ) f ON a.Id = f.PackCourseId
        left join (SELECT CourseId,COUNT(1) COUNT FROM dbo.t_Course_Appraise GROUP BY CourseId) p on a.Id=p.CourseId
        LEFT JOIN ( SELECT  CourseId ,
                            COUNT(*) AS CollectionNum
                    FROM    dbo.t_My_Collection
                    GROUP BY CourseId
                  ) b ON a.Id = b.CourseId
        LEFT JOIN ( SELECT  CourseId ,
                            COUNT(*) AS PurchaseNum
                    FROM    dbo.t_My_Course
                    GROUP BY CourseId
                  ) c ON a.Id = c.CourseId
        LEFT JOIN ( SELECT  SUM(k.VideoLong) VideoLong ,
                            j.PackCourseId
                    FROM    t_Course_SuitDetail j
                            LEFT JOIN ( SELECT  SUM(b.VideoLong) VideoLong ,
                                                a.CourseId
                                        FROM    dbo.t_Course_Chapter a
                                                INNER JOIN ( SELECT
                                                              c.ChapterId ,
                                                              COUNT(1) COUNT ,
                                                              SUM(c.VideoLong) VideoLong
                                                             FROM
                                                              dbo.t_Course_Video c
                                                             WHERE
                                                              c.IsDelete = 0
                                                              AND c.VideoLongTime != '0'
                                                             GROUP BY c.ChapterId
                                                           ) b ON a.Id = b.ChapterId
                                        GROUP BY a.CourseId
                                      ) k ON j.CourseId = k.CourseId
                    GROUP BY j.PackCourseId
                  ) l ON a.Id = l.PackCourseId
 WHERE  1 = 1 ";
            var dynamicparameter = new DynamicParameters();
            if (!string.IsNullOrEmpty(input.Id))
            {
                strSql += " and Id=@Id";
                dynamicparameter.Add(":Id", input.Id, DbType.String);
            }
            var model = Db.QueryFirstOrDefault<CoursePackModelOutput>(strSql, dynamicparameter);
            model.CourseLongTime = model.CourseLongTime == null ? "" : getFormatterTime(model.CourseLongTime);
            model.CourseVedio = string.IsNullOrEmpty(model.CourseVedio) ? "" : DefuleDomain + "/" + model.CourseVedio;
            model.CourseIamge = string.IsNullOrEmpty(model.CourseIamge) ? "" : (DefuleDomain + "/" + model.CourseIamge);
            model.EvaluationScore = GetAverageAppraise(model.Id);
            var sql = @"SELECT b.*,c.TeachersName,f.Count FROM dbo.t_Course_SuitDetail a 
                            LEFT JOIN dbo.t_Course_Info b ON a.CourseId=b.Id
                            LEFT JOIN dbo.t_Course_OnTeachers c ON b.TeacherId=c.Id
                            LEFT JOIN (SELECT COUNT(1) Count,MAX(d.CourseId) CourseId FROM dbo.t_Order_Detail d  GROUP BY d.CourseId) f ON b.Id=f.CourseId
                            where a.PackCourseId=@PackCourseId order by a.OrderNo";
            var parameters = new DynamicParameters();
            parameters.Add(":PackCourseId", model.Id, DbType.String);
            var list = Db.QueryList<CourseInfoModelOutput>(sql, parameters);
            foreach (var item in list)
            {
                item.CourseIamge = string.IsNullOrEmpty(item.CourseIamge) ? "" : (DefuleDomain + "/" + item.CourseIamge);
                item.TeacherPhoto = string.IsNullOrEmpty(item.TeacherPhoto)
                    ? ""
                    : (DefuleDomain + "/" + item.TeacherPhoto);
            }
            model.SubCourseList = list;
            return model;
        }


        /// <summary>
        /// 获取面授课详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public FaceToFaceModelOupput GetFaceToFace(IdInput input)
        {
            var strSql = @" SELECT a.* ,
        b.CollectionNum ,
        c.PurchaseNum ,
        p.COUNT AppraiseNum
 FROM   t_Course_FaceToFace a
        LEFT JOIN ( SELECT  e.PackCourseId ,
                            COUNT(1) COUNT
                    FROM    t_Course_SuitDetail e
                            LEFT JOIN dbo.t_Course_Info h ON e.CourseId = h.Id
                    GROUP BY e.PackCourseId
                  ) f ON a.Id = f.PackCourseId
        left join (SELECT CourseId,COUNT(1) COUNT FROM dbo.t_Course_Appraise GROUP BY CourseId) p on a.Id=p.CourseId
        LEFT JOIN ( SELECT  CourseId ,
                            COUNT(*) AS CollectionNum
                    FROM    dbo.t_My_Collection
                    GROUP BY CourseId
                  ) b ON a.Id = b.CourseId
        LEFT JOIN ( SELECT  CourseId ,
                            COUNT(*) AS PurchaseNum
                    FROM    dbo.t_My_Course
                    GROUP BY CourseId
                  ) c ON a.Id = c.CourseId
 WHERE  1 = 1 ";
            var dynamicparameter = new DynamicParameters();
            if (!string.IsNullOrEmpty(input.Id))
            {
                strSql += " and Id=@Id";
                dynamicparameter.Add(":Id", input.Id, DbType.String);
            }
            var model = Db.QueryFirstOrDefault<FaceToFaceModelOupput>(strSql, dynamicparameter);
            model.CourseIamge = string.IsNullOrEmpty(model.CourseIamge) ? "" : (DefuleDomain + "/" + model.CourseIamge);
            model.EvaluationScore = GetAverageAppraise(model.Id);
            return model;
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
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool UpdateViewCount( CoursePackModelInput input )
        {
            var strSql = " UPDATE dbo.t_Course_PackcourseInfo SET ViewCount=ISNULL(ViewCount,0)+1 WHERE id ='" + input.Id + "'";
            return Db.ExecuteScalar<int>( strSql, null ) > 0;
        }

    }
}
