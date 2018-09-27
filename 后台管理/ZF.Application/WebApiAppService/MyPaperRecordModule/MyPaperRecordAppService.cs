using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.WebApiDto.ChapterExerciseModule;
using ZF.Application.WebApiDto.CoursePaperRecordModule;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using static ZF.Application.WebApiDto.CoursePaperRecordModule.CoursePaperRecordOutput;

namespace ZF.Application.WebApiAppService.MyPaperRecordModule
{
    /// <summary>
    /// 
    /// </summary>
    public class MyPaperRecordAppService : BaseAppService<MyPaperRecords>
    {
        private readonly IMyPaperRecordsRepository _repository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public MyPaperRecordAppService( IMyPaperRecordsRepository repository ) : base( repository )
        {
            _repository = repository;
        }

        /// <summary>
        /// 测评试卷
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<CoursePaperClassByPaper> GetPaperClassList( CoursePaperRecordInput input, out int count ) {
            var sql = @"
SELECT b.PaperName FROM dbo.t_My_PaperRecords a
LEFT JOIN dbo.t_Paper_Info b ON a.PaperId = b.Id
 WHERE UserId = '{0}' GROUP BY UserId,PaperId,b.PaperName";
            var dy = new DynamicParameters( );
            if ( !string.IsNullOrEmpty( input.query ) )
            {
                sql += " AND  b.PaperName like @query ";
                dy.Add( ":query", '%' + input.query + '%', DbType.String );
            }
            count = Db.QueryFirstOrDefault<int>( " select count(1) from (" + sql + ")  b", null );


            var list = Db.QueryList<CoursePaperClassByPaper>( GetPageSql( sql,input.Page,input.Rows,"PageName Desc"), null );
            if ( list.Count > 0 ) {
                foreach ( var item in list ) {
                var sql2=  string.Format( @"SELECT  a.Id ,
                    a.PracticeNo ,
                    b.PaperName ,
                    a.AddTime ,
                    a.ScoreSum ,
                    a.Score ,
                    --d.CourseName ,
                   -- d.Id AS CourseId ,
                    a.PaperId ,
                    g.RowNumber1
                    FROM    dbo.t_My_PaperRecords a
                            LEFT JOIN dbo.t_Paper_Info b ON a.PaperId = b.Id
                            LEFT JOIN ( SELECT  ROW_NUMBER() OVER ( ORDER BY Score DESC ) AS RowNumber1 ,
                                                MAX(Id) Id
                                        FROM    t_My_PaperRecords
                                        WHERE   1 = 1
                                                AND Status = 1
                                        GROUP BY PaperId ,
                                                Score
                                      ) g ON a.Id = g.Id
                    WHERE   1 = 1
                            AND a.Status = 1 AND  a.UserId = '{0}'  ", input.UserId);
                    var list2 = Db.QueryList<CoursePaperRecordOutput>( sql2,null);
                    item.PaperRecordList = list2;
                }
            }
            return list;

        }


        /// <summary>
        /// 測評試卷下拉篩選
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<string> GetPaperRecordSelect( string userId )
        {
            var sql = @"SELECT  b.PaperName ,
            FROM    dbo.t_My_PaperRecords a
                    LEFT JOIN dbo.t_Paper_Info b ON a.PaperId = b.Id
            WHERE   1 = 1 AND a.Status = 1 ";
            var dy = new DynamicParameters( );
            sql += " AND  a.UserId = @UserId ";
            dy.Add( ":UserId", userId, DbType.String );
            return Db.QueryList<string>(sql, dy );
        }

        /// <summary>
        /// 试卷测评记录
        /// </summary>
        /// <returns></returns>
        public List<CoursePaperRecordOutput> GetList( CoursePaperRecordInput input, out int count )
        {
            var sql = @"SELECT  a.Id ,
        a.PracticeNo ,
        b.PaperName ,
        a.AddTime ,
        a.ScoreSum ,
        a.Score ,
        --d.CourseName ,
       -- d.Id AS CourseId ,
        a.PaperId ,
        g.RowNumber1
FROM    dbo.t_My_PaperRecords a
        LEFT JOIN dbo.t_Paper_Info b ON a.PaperId = b.Id
        LEFT JOIN ( SELECT  ROW_NUMBER() OVER ( ORDER BY Score DESC ) AS RowNumber1 ,
                            MAX(Id) Id
                    FROM    t_My_PaperRecords
                    WHERE   1 = 1
                            AND Status = 1
                    GROUP BY PaperId ,
                            Score
                  ) g ON a.Id = g.Id
WHERE   1 = 1
        AND a.Status = 1 ";
            var dy = new DynamicParameters( );
            sql += " AND  a.UserId = @UserId ";
            dy.Add( ":UserId", input.UserId, DbType.String );
            if ( !string.IsNullOrEmpty( input.query ) )
            {
                sql += " AND  b.PaperName like @query ";
                dy.Add( ":query", '%' + input.query + '%', DbType.String );
            }
            count = Db.QueryFirstOrDefault<int>( " select count(1) from (" + sql + ")  b", dy );
            return Db.QueryList<CoursePaperRecordOutput>( GetPageSql( sql, input.Page, input.Rows, "AddTime desc" ), dy );
        }


        /// <summary>
        /// 获取测评统计结果
        /// </summary>
        /// <param name="paperId"></param>
        /// <param name="paperRecordsId"></param>
        /// <returns></returns>
        public CoursePaperRecordMoreOutput Get( string paperId, string paperRecordsId )
        {
            var sql = @"SELECT d.PaperName, b.PaperId,b.Id,b.ScoreSum,b.Score,b.AddTime,ISNULL(a.Total,0) AS Total,ISNULL(c.ErrorCount,0) ErrorCount from t_My_PaperRecords b
        LEFT JOIN (
        SELECT PaperRecordsId,PaperId,COUNT(*) AS Total FROM t_My_AnswerRecords GROUP BY PaperId ,PaperRecordsId
        )a ON b.id =a.PaperRecordsId
        LEFT JOIN (
			SELECT PaperId,PaperRecordsId,COUNT(*) ErrorCount FROM t_My_AnswerRecords WHERE Score=0  GROUP BY PaperId ,PaperRecordsId
        )c ON b.Id=c.PaperRecordsId
        LEFT JOIN dbo.t_Paper_Info d ON a.PaperId=d.Id
        WHERE b.PaperId=@PaperId AND a.PaperRecordsId=@PaperRecordsId ";

            var dy = new DynamicParameters( );
            dy.Add( ":PaperId", paperId, DbType.String );
            dy.Add( ":PaperRecordsId", paperRecordsId, DbType.String );
            return Db.QueryFirstOrDefault<CoursePaperRecordMoreOutput>( sql, dy );

        }

        /// <summary>
        /// 测评试卷总览信息
        /// </summary>
        /// <param name="paperId"></param>
        /// <returns></returns>
        public PaperInfoOutput GetPaperInfo( string paperId )
        {
            var sql = @"
        SELECT a.id PaperId, a.PaperName,a.TestTime ,c.UseTime,b.ScoreSum FROM dbo.t_Paper_Info a 
        LEFT JOIN (
			SELECT paperId ,SUM(QuestionScore) ScoreSum FROM dbo.t_Paper_Detatail GROUP BY PaperId
        )b ON a.id=b.PaperId
        LEFT JOIN (
			SELECT PaperId,COUNT(*) AS UseTime FROM dbo.t_My_PaperRecords GROUP BY PaperId
        )c
        ON a.Id=c.PaperId
        WHERE a.Id=@PaperId ";
            var dy = new DynamicParameters( );
            dy.Add( ":PaperId",paperId,DbType.String);
            var model = Db.QueryFirstOrDefault<PaperInfoOutput>( sql, dy );
            if ( model != null ) {
                var sql2 = string.Format( @"SELECT a.PaperId,a.QuestionTypeId,COUNT(a.QuestionId) count ,SUM(a.QuestionScore) Score FROM dbo.t_Paper_Detatail a WHERE a.PaperId='{0}' GROUP BY a.PaperId,a.QuestionTypeId ", model.PaperId);
                model.Structures = Db.QueryList<PaperInfoStructure>( sql2,null);
            }
            return model;
        }

    }

}
