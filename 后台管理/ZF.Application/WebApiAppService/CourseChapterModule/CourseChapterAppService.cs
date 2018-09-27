using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.BaseDto;
using ZF.Application.WebApiDto.ChapterExerciseModule;
using ZF.Application.WebApiDto.CourseChapterModule;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.CourseChapterModule
{
    /// <summary>
    /// 
    /// </summary>
    public class CourseChapterAppService : BaseAppService<CourseChapter>
    {
        private static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];
        private readonly ICourseChapterRepository _repository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public CourseChapterAppService(ICourseChapterRepository repository) : base(repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// 获取章节列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<CourseChapterModelOutput> GetCourseChapterList(CourseChapterModelInput input)
        {
            const string strSql = @"SELECT  MAX(a.Id) Id ,
        MAX(a.CapterName) CapterName ,
        COUNT(c.Id) Count 
FROM    dbo.t_Course_Chapter a
        LEFT JOIN dbo.t_Course_Video b ON a.Id = b.ChapterId
        LEFT JOIN t_Course_Subject c ON a.Id = c.ChapterId
WHERE   ISNULL(a.ParentId, '') = ''
        AND a.IsDelete = 0
        AND a.CourseId = @CourseId
GROUP BY a.Id,a.OrderNo ORDER BY a.OrderNo asc ";
            var dynamicparameter = new DynamicParameters();
            dynamicparameter.Add(":CourseId", input.CourseId, DbType.String);
            var list = Db.QueryList<CourseChapterModelOutput>(strSql, dynamicparameter);

            if (list != null)
            {
                var RowIndex = 0;
                foreach (var obj in list)
                {
                    var sql = @" SELECT  MAX(a.Id) Id ,
        MAX(a.CapterName) CapterName ,
        '' VideoName ,
        '' VideoUrl ,
        '' VideoLongTime ,
        ISNULL(MAX(d.COUNT),0) COUNT
FROM    dbo.t_Course_Chapter a
        LEFT JOIN dbo.t_Course_Video b ON a.Id = b.ChapterId
        LEFT JOIN (SELECT c.ChapterId,COUNT(1) COUNT FROM   t_Course_Subject c  GROUP BY c.ChapterId)  d ON a.Id=d.ChapterId
WHERE   1 = 1
        AND a.ParentId =@ParentId
        AND a.IsDelete = 0
GROUP BY a.Id ,
        a.OrderNo
ORDER BY a.OrderNo asc ";
                    var subdynamicPara = new DynamicParameters();
                    subdynamicPara.Add(":ParentId", obj.Id, DbType.String);
                    var sublist = Db.QueryList<CourseChapterModelOutput>(sql, subdynamicPara);
                    if (sublist.Count > 0)
                    {
                        obj.SubChapterList = sublist;
                        foreach (var item in obj.SubChapterList)
                        {
                            var sqlstr = @"SELECT  a.ChapterId Id ,a.Id VideoId,
        a.VideoName CapterName ,
        a.VideoUrl ,
        a.VideoLongTime,
        ISNULL(d.COUNT,0) COUNT,
        a.IsTaste,
        a.TasteLongTime,
        a.TasteLongTime2,
        case when (ISNULL(d.COUNT,0)>0 AND ISNULL(a.VideoUrl,'')='') THEN 1 ELSE 0 end  IsExercise,
        ( ROW_NUMBER() OVER ( ORDER BY a.OrderNo , a.AddTime ASC ) )+" + RowIndex + @" AS RowsIndex
FROM    dbo.t_Course_Video a
        LEFT JOIN (SELECT c.ChapterId,COUNT(1) COUNT FROM   t_Course_Subject c  GROUP BY c.ChapterId)  d ON a.Id=d.ChapterId
WHERE   a.ChapterId = @ChapterId
        AND a.IsDelete = 0  Order by a.OrderNo,a.AddTime ASC ";
                            var dy = new DynamicParameters();
                            dy.Add(":ChapterId", item.Id, DbType.String);
                            var videoList = Db.QueryList<CourseChapterModelOutput>(sqlstr, dy);
                            item.SubChapterList = videoList;
                            RowIndex += videoList.Count;
                            var strsql2 = string.Format(@"SELECT CASE WHEN a.count=a.total THEN 1 ELSE 0 END   FROM (
    SELECT COUNT( *) count, SUM(case when( ISNULL( d.COUNT, 0 ) > 0 AND ISNULL( a.VideoUrl, '' ) = '' ) THEN 1 ELSE 0 END )  total  FROM dbo.t_Course_Video a
LEFT JOIN( SELECT c.ChapterId, COUNT( 1 ) COUNT FROM   t_Course_Subject c  GROUP BY c.ChapterId )  d ON a.Id = d.ChapterId
WHERE a.ChapterId = '{0}' )a", item.Id);
                            item.IsExercise = Db.ExecuteScalar<int>(strsql2, null);
                        }
                    }
                    else
                    {
                        var sqlstr = @"SELECT  a.ChapterId Id ,a.Id VideoId,
        a.VideoName CapterName ,
        a.VideoUrl ,
        a.VideoLongTime,
        ISNULL(d.COUNT,0) COUNT,
        a.IsTaste,
        a.TasteLongTime,
        a.TasteLongTime2,
        case when (ISNULL(d.COUNT,0)>0 AND ISNULL(a.VideoUrl,'')='') THEN 1 ELSE 0 end  IsExercise,
        ( ROW_NUMBER() OVER ( ORDER BY a.OrderNo , a.AddTime ASC ) )+" + RowIndex + @" AS RowsIndex
FROM    dbo.t_Course_Video a
        LEFT JOIN (SELECT c.ChapterId,COUNT(1) COUNT FROM   t_Course_Subject c  GROUP BY c.ChapterId)  d ON a.Id=d.ChapterId
WHERE   a.ChapterId = @ChapterId
        AND a.IsDelete = 0 Order by a.OrderNo,a.AddTime ASC";
                        var dy = new DynamicParameters();
                        dy.Add(":ChapterId", obj.Id, DbType.String);
                        var videoList = Db.QueryList<CourseChapterModelOutput>(sqlstr, dy);
                        obj.SubChapterList = videoList;
                        RowIndex += videoList.Count;
                        var strsql2 = string.Format(@"SELECT CASE WHEN a.count=a.total THEN 1 ELSE 0 END   FROM (
    SELECT COUNT( *) count, SUM(case when( ISNULL( d.COUNT, 0 ) > 0 AND ISNULL( a.VideoUrl, '' ) = '' ) THEN 1 ELSE 0 END )  total  FROM dbo.t_Course_Video a
LEFT JOIN( SELECT c.ChapterId, COUNT( 1 ) COUNT FROM   t_Course_Subject c  GROUP BY c.ChapterId )  d ON a.Id = d.ChapterId
WHERE a.ChapterId = '{0}' )a", obj.Id);
                        obj.IsExercise = Db.ExecuteScalar<int>(strsql2, null);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取章节列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<CourseChapterModelOutput> GetCourseChapterList(CourseChapterModelInput input, string userID)
        {
            const string strSql = @"SELECT  MAX(a.Id) Id ,
        MAX(a.CapterName) CapterName ,
        COUNT(c.Id) Count 
FROM    dbo.t_Course_Chapter a
        LEFT JOIN dbo.t_Course_Video b ON a.Id = b.ChapterId
        LEFT JOIN t_Course_Subject c ON a.Id = c.ChapterId
WHERE   ISNULL(a.ParentId, '') = ''
        AND a.IsDelete = 0
        AND a.CourseId = @CourseId
GROUP BY a.Id,a.OrderNo ORDER BY a.OrderNo asc ";
            var dynamicparameter = new DynamicParameters();
            dynamicparameter.Add(":CourseId", input.CourseId, DbType.String);
            var list = Db.QueryList<CourseChapterModelOutput>(strSql, dynamicparameter);

            if (list != null)
            {
                foreach (var obj in list)
                {
                    var sql = @" SELECT  MAX(a.Id) Id ,
        MAX(a.CapterName) CapterName ,
        '' VideoName ,
        '' VideoUrl ,
        '' VideoLongTime ,
        ISNULL(MAX(d.COUNT),0) COUNT
FROM    dbo.t_Course_Chapter a
        LEFT JOIN dbo.t_Course_Video b ON a.Id = b.ChapterId
        LEFT JOIN (SELECT c.ChapterId,COUNT(1) COUNT FROM   t_Course_Subject c  GROUP BY c.ChapterId)  d ON a.Id=d.ChapterId
WHERE   1 = 1
        AND a.ParentId =@ParentId
        AND a.IsDelete = 0
GROUP BY a.Id ,
        a.OrderNo
ORDER BY a.OrderNo asc ";
                    var subdynamicPara = new DynamicParameters();
                    subdynamicPara.Add(":ParentId", obj.Id, DbType.String);
                    var sublist = Db.QueryList<CourseChapterModelOutput>(sql, subdynamicPara);
                    if (sublist.Count > 0)
                    {
                        obj.SubChapterList = sublist;
                        foreach (var item in obj.SubChapterList)
                        {
                            var sqlstr = @"SELECT  a.ChapterId Id ,a.Id VideoId,
        a.VideoName CapterName ,
        a.VideoUrl ,
        a.VideoLongTime,
        a.IsTaste,
        b.State State,
        a.TasteLongTime,
        a.TasteLongTime2,
        ISNULL(d.COUNT,0) COUNT
FROM    dbo.t_Course_Video a
        LEFT JOIN (SELECT c.ChapterId,COUNT(1) COUNT FROM   t_Course_Subject c  GROUP BY c.ChapterId)  d ON a.Id=d.ChapterId
        left join T_Course_LearnProgress b on a.Id=b.VideoId and UserId=@UserId 
WHERE   a.ChapterId = @ChapterId
        AND a.IsDelete = 0  Order by a.OrderNo,a.AddTime ASC ";
                            var dy = new DynamicParameters();
                            dy.Add(":ChapterId", item.Id, DbType.String);
                            dy.Add(":UserId", userID, DbType.String);
                            var videoList = Db.QueryList<CourseChapterModelOutput>(sqlstr, dy);
                            item.SubChapterList = videoList;
                        }
                    }
                    else
                    {
                        var sqlstr = @"SELECT  a.ChapterId Id ,a.Id VideoId,
        a.VideoName CapterName ,
        a.VideoUrl ,
        a.VideoLongTime,
        a.IsTaste,
        b.State State,
        a.TasteLongTime,
        a.TasteLongTime2,
        ISNULL(d.COUNT,0) COUNT
FROM    dbo.t_Course_Video a
        LEFT JOIN (SELECT c.ChapterId,COUNT(1) COUNT FROM   t_Course_Subject c  GROUP BY c.ChapterId)  d ON a.Id=d.ChapterId
        left join T_Course_LearnProgress b on a.Id=b.VideoId and UserId=@UserId
WHERE   a.ChapterId = @ChapterId
        AND a.IsDelete = 0 Order by a.OrderNo,a.AddTime ASC";
                        var dy = new DynamicParameters();
                        dy.Add(":ChapterId", obj.Id, DbType.String);
                        dy.Add(":UserId", userID, DbType.String);
                        var videoList = Db.QueryList<CourseChapterModelOutput>(sqlstr, dy);
                        obj.SubChapterList = videoList;
                    }
                }
            }
            return list;
        }


        /// <summary>
        /// 获取章节信息 课程信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public CourseChapterOutput GetCourseChapter(IdInput input)
        {
            var strSql = @"
SELECT  a.VideoName ,
        a.VideoUrl ,
        c.FavourablePrice
FROM    dbo.t_Course_Video a
        LEFT JOIN dbo.t_Course_Chapter b ON a.ChapterId = b.Id
        LEFT JOIN dbo.t_Course_Info c ON b.CourseId = c.Id where a.Id=@Id";
            var parameters = new DynamicParameters();
            parameters.Add(":Id", input.Id, DbType.String);
            var model = Db.QueryFirstOrDefault<CourseChapterOutput>(strSql, parameters);
            return model;
        }
        /// <summary>
        /// 获取章节信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public CourseChapterModelOutput GetModel(CourseChapterModelInput input)
        {
            var strSql = "select * from t_Course_Chapter  where Id=@Id";
            var parameters = new DynamicParameters();
            parameters.Add(":Id", input.Id, DbType.String);
            var model = Db.QueryFirstOrDefault<CourseChapterModelOutput>(strSql, parameters);
            model.VideoUrl = string.IsNullOrEmpty(model.VideoUrl) ? "" : DefuleDomain + "/" + model.VideoUrl;
            return model;
        }

        /// <summary>
        ///  通过用户编号  章节编号 试题练习记录
        /// </summary>
        /// <param name="chapterId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<CourseChapterQuestions> GetCourseChapterQuestionses(string chapterId, string userId)
        {
            var strSql = @" SELECT * FROM t_Course_ChapterQuestions where 1=1 and Status=1  ";
            var dy = new DynamicParameters();
            strSql += " and ChapterId=@ChapterId and UserId=@UserId ";
            dy.Add(":ChapterId", chapterId, DbType.String);
            dy.Add(":UserId", userId, DbType.String);
            return Db.QueryList<CourseChapterQuestions>(strSql, dy);
        }

        /// <summary>
        ///  通过用户编号  试卷编号 获取试卷练习记录+
        /// </summary>
        /// <param name="paperId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<MyPaperRecordsOutput> GetMyPaperRecords(string paperId, string userId)
        {
            var strSql = @"SELECT  a.* ,
        b.RowNumber
FROM    t_My_PaperRecords a
        LEFT JOIN ( SELECT  ROW_NUMBER() OVER ( ORDER BY Score DESC ) AS RowNumber ,
                            Id
                    FROM    t_My_PaperRecords
                    WHERE   1 = 1
                            AND Status = 1
                            AND PaperId =@PaperId
                  ) b ON a.Id = b.Id
WHERE   1 = 1
        AND Status = 1  ";
            var dy = new DynamicParameters();
            strSql += " and PaperId=@PaperId and UserId=@UserId ";
            dy.Add(":PaperId", paperId, DbType.String);
            dy.Add(":UserId", userId, DbType.String);
            return Db.QueryList<MyPaperRecordsOutput>(strSql + " order by AddTime desc ", dy);
        }

        /// <summary>
        /// 通过试卷练习编号获取练习结果
        /// </summary>
        /// <param name="PaperRecordsId"></param>
        /// <returns></returns>
        public MyPaperRecordsJgOutput MyPaperRecordsJg(string PaperRecordsId)
        {
            var strSql = @"SELECT  j.Score,j.AddTime,
        ISNULL(f.c1, 0) c1 ,
        ISNULL(f.c2, 0) c2 ,
        p.PaperName
FROM    t_My_PaperRecords j
        LEFT JOIN ( SELECT  a.PaperRecordsId ,
                            ISNULL(SUM(CASE WHEN cc = 'yes' THEN [a].Count1
                                       END), 0) c1 ,
                            ISNULL(SUM(CASE WHEN cc = 'no' THEN a.Count1
                                       END), 0) c2
                    FROM    ( SELECT    d.PaperRecordsId ,
                                        COUNT(1) Count1 ,
                                        'yes' cc
                              FROM      dbo.t_My_AnswerRecords d
                              GROUP BY  d.PaperRecordsId
                              UNION ALL
                              SELECT    d.PaperRecordsId ,
                                        COUNT(1) Count1 ,
                                        'no' cc
                              FROM      dbo.t_My_AnswerRecords d
                              WHERE     LEN(d.StuAnswer) > 0
                                        AND d.Score > 0
                              GROUP BY  d.PaperRecordsId
                            ) a
                    GROUP BY PaperRecordsId
                  ) f ON j.Id = f.PaperRecordsId
        LEFT JOIN t_Paper_Info p ON j.PaperId = p.Id where 1=1   ";
            var dy = new DynamicParameters();
            strSql += " and j.Id=@PaperRecordsId ";
            dy.Add(":PaperRecordsId", PaperRecordsId, DbType.String);
            return Db.QueryFirstOrDefault<MyPaperRecordsJgOutput>(strSql, dy);
        }


        /// <summary>
        /// 获取章节练习记录
        /// </summary>
        /// <returns></returns>
        public List<CourseChapterExerciseOuput> GetExerciseRecords(CourseChapterExerciseInput input, out int count)
        {
            var sql = @"  SELECT DISTINCT
        ( a.Id ) ,
        a.AddTime ,
        a.PracticeNo ,
        b.VideoName ,
        c.CourseName ,
        CAST(v.c2 * 1.0 / v.c1 * 1.0 * 100 AS DECIMAL(18,
                                                              0)) Completion ,
        CAST(v.c3 * 1.0 / v.c1 * 1.0 * 100 AS DECIMAL(18,
                                                              0)) Correct
 FROM   t_Course_ChapterQuestions a
        LEFT JOIN dbo.t_Course_Video b ON a.ChapterId = b.Id
        LEFT JOIN dbo.t_Course_Chapter k ON b.ChapterId = k.Id
        LEFT JOIN ( SELECT  a.ChapterQuestionsId ,
                            ISNULL(SUM(CASE WHEN cc = 'yes' THEN [a].Count1
                                       END), 0) c1 ,
                            ISNULL(SUM(CASE WHEN cc = 'no' THEN a.Count1
                                       END), 0) c2 ,
                            ISNULL(SUM(CASE WHEN cc = '' THEN a.Count1
                                       END), 0) c3
                    FROM    ( SELECT    d.ChapterQuestionsId ,
                                        COUNT(1) Count1 ,
                                        'yes' cc
                              FROM      dbo.t_Course_ChapterQuestionsDetail d
                              GROUP BY  d.ChapterQuestionsId
                              UNION ALL
                              SELECT    d.ChapterQuestionsId ,
                                        COUNT(1) Count1 ,
                                        'no' cc
                              FROM      dbo.t_Course_ChapterQuestionsDetail d
                              WHERE     LEN(d.StuAnswer) > 0
                              GROUP BY  d.ChapterQuestionsId
                              UNION
                              SELECT    d.ChapterQuestionsId ,
                                        COUNT(1) Count ,
                                        '' cc
                              FROM      dbo.t_Course_ChapterQuestionsDetail d
                              WHERE     ISNULL(d.IsCorrect, 0) = 1
                              GROUP BY  d.ChapterQuestionsId
                            ) a
                    GROUP BY ChapterQuestionsId
                  ) v ON a.Id = v.ChapterQuestionsId
        LEFT JOIN dbo.t_Course_Info c ON c.Id = k.CourseId
 WHERE  1 = 1
        AND a.Status = 1 ";
            var dy = new DynamicParameters();
            sql += " and a.UserId=@UserId ";
            dy.Add(":UserId", input.UserId, DbType.String);
            if (!string.IsNullOrEmpty(input.query))
            {
                sql += " AND (b.VideoName LIKE @query OR c.CourseName LIKE @query)  ";
                dy.Add(":query", '%' + input.query + '%', DbType.String);
            }
            count = Db.QueryFirstOrDefault<int>(" select count(1) from (" + sql + ")  b", dy);
            return Db.QueryList<CourseChapterExerciseOuput>(GetPageSql(sql, input.Page, input.Rows, "AddTime desc"), dy);
        }
    }
}
