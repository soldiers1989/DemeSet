using System.Collections.Generic;
using System.Data;
using Dapper;
using ZF.Application.WebApiDto.ErrorSubjeModule;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using System.Text;
using ZF.Application.Dto;
using System;
using ZF.Application.BaseDto;
using ZF.Application.WebApiDto.MyCollectionItemModule;

namespace ZF.Application.WebApiAppService.MyErrorSubjectModule
{
    /// <summary>
    /// 
    /// </summary>
    public class MyErrorSubjectAppService : BaseAppService<MyErrorSubject>
    {
        private readonly IMyErrorSubjectRepository _repository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public MyErrorSubjectAppService( IMyErrorSubjectRepository repository ) : base( repository )
        {
            _repository = repository;
        }



        /// <summary>
        /// 获取错题集
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<ErrorSubjectsModelOutput> GetList( ErrorSubjectModelInput input, out int count )
        {
            var strSql = @"select a.StuAnswer,a.AddTime,b.Id,b.QuestionTitle,b.QuestionContent,b.Option1,b.Option2,b.Option3,b.Option4,b.Option5,b.Option6,b.Option7,b.Option8,b.Number,b.SubjectId,b.KnowledgePointId,b.SubjectType,b.SubjectClassId,b.RightAnswer,b.ConsultAnswer,b.State,b.QuestionTextAnalysis,b.VideoId  ";
            var fromSql = new StringBuilder( @" from ( 
	            SELECT DISTINCT a.StuAnswer,a.AddTime, a.UserId,  CASE WHEN isnull(a.BigQuestionId, '') = '' THEN b.BigQuestionId ELSE a.BigQuestionId END AS BigQuestionId FROM  (SELECT   StuAnswer,  UserId, BigQuestionId, SmallQuestionId, MAX(AddTime) AS AddTime
                       FROM          dbo.t_My_ErrorSubject WHERE UserId=@UserId
                       GROUP BY StuAnswer,UserId, BigQuestionId, SmallQuestionId)a
                       LEFT JOIN dbo.t_Subject_Smallquestion b ON a.SmallQuestionId=b.Id
                 )a LEFT JOIN dbo.t_Subject_BigQuestion b ON a.BigQuestionId=b.Id where 1=1 " );

            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters( );
            dynamicParameters.Add( ":UserId", input.UserId, DbType.String );
            if ( !string.IsNullOrEmpty( input.QuestionContent ) )
            {
                fromSql.Append( " and b.QuestionContent like @QuestionContent " );
                dynamicParameters.Add( ":QuestionContent", "%" + input.QuestionContent + "%", DbType.String );
            }
            count = Db.ExecuteScalar<int>( sqlCount + fromSql, dynamicParameters );
            var list = Db.QueryList<ErrorSubjectsModelOutput>( GetPageSql( strSql + fromSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord ), dynamicParameters );
            var subsql = "";
            foreach ( var item in list )
            {
                subsql = string.Format( @" SELECT a.StuAnswer, b.Id,b.QuestionTitle,b.QuestionContent,b.Option1,b.Option2,b.Option3,b.Option4,b.Option5,b.Option6,b.Option7,b.Option8,b.Number,b.SubjectType,b.RightAnswer,b.ConsultAnswer,b.State,b.QuestionTextAnalysis,b.VideoId ,a.AddTime FROM dbo.t_My_ErrorSubject a LEFT JOIN t_Subject_Smallquestion b ON a.SmallQuestionId=b.Id WHERE ISNULL(a.SmallQuestionId,'')<>'' AND a.UserId='{0}' AND b.BigQuestionId='{1}'", input.UserId, item.Id );
                item.SubjectSmallquestions = Db.QueryList<SubjectSmallquestionOutput>( subsql, null );
            }
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="practiceNo"></param>
        /// <returns></returns>
        public List<SubjectPracticeModelOutput> GetPracticeDetail( string practiceNo )
        {
            var sql =string.Format( @" SELECT c.*,b.StuAnswer,'' Description,b.IsCorrect FROM dbo.t_SubjectPractice a 
        LEFT JOIN dbo.t_SubjectPracticeQuestions b ON a.Id=b.PracticeNo
        LEFT JOIN dbo.t_Subject_BigQuestion c ON b.BigQuestionId=c.Id
        WHERE a.Id='{0}' AND  ISNULL(b.BigQuestionId,'')<>''
        ", practiceNo);
            var list = Db.QueryList<SubjectPracticeModelOutput>( sql, null );

            var sql2 = string.Format( @"        SELECT c.*,b.StuAnswer,d.QuestionContent Description,b.IsCorrect FROM dbo.t_SubjectPractice a 
        LEFT JOIN dbo.t_SubjectPracticeQuestions b ON a.Id=b.PracticeNo
        LEFT JOIN dbo.t_Subject_smallQuestion c ON b.SmallQuestionId=c.Id
        LEFT JOIN dbo.t_Subject_BigQuestion  d ON c.BigQuestionId=d.id
        WHERE a.Id='{0}' AND  ISNULL(b.BigQuestionId,'')=''
        ", practiceNo );
            var list2 = Db.QueryList<SubjectPracticeModelOutput>( sql2, null );
            list.AddRange( list2);
            return list;
        }

        /// <summary>
        /// 保存试题练习记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut saveSubjectPracticeRecord( SubjectPracticeModelInput input )
        {
            var practiceNo = Guid.NewGuid( );
            var sql = $@"
INSERT INTO dbo.t_SubjectPractice
        ( Id, UserId, AddTime, Type )
VALUES  ( N'{practiceNo}', -- Id - nvarchar(36)
          N'{input.UserId}', -- UserId - nvarchar(36)
          GETDATE(), -- AddTime - datetime
          0  -- Type - int
          )";

            Db.ExecuteNonQuery( sql,null);
            
            var sqllist = "";
            var questions = input.SubjectPracticeList;
            if ( questions.Count > 0 ) {
                for ( var i = 0; i < questions.Count; i++ ) {
                    var item = questions[i];
                    sqllist += $@"INSERT INTO dbo.t_SubjectPracticeQuestions
        ( Id ,
          PracticeNo ,
          UserId ,
          BigQuestionId ,
          SmallQuestionId ,
          StuAnswer ,
          IsCorrect
        )
VALUES  ( N'{Guid.NewGuid()}' , -- Id - nvarchar(36)
          N'{practiceNo}' , -- PracticeNo - nvarchar(36)
          N'{input.UserId}' , -- UserId - nvarchar(36)
          N'{item.BigQuestionId}' , -- BigQuestionId - nvarchar(36)
          N'{item.SmallQuestionId}' , -- SmallQuestionId - nvarchar(36)
          N'{item.StuAnswer}' , -- StuAnswer - nvarchar(20)
          {item.IsCorrect} -- IsCorrect - int
        )";
                }               
                Db.ExecuteNonQuery( sqllist,null);
            }





            return new MessagesOutPut { Success = true, Message = practiceNo.ToString() };
        }

        public List<ErrorSubjectsModelOutput> GetErrList( ErrorSubjectModelInput input )
        {
            var sql1 = string.Format( @" SELECT '' description,b.*,c.Code,c.VideoName  FROM (
                 SELECT CASE when ISNULL(a.BigQuestionId,'')='' THEN b.BigQuestionId ELSE	 a.BigQuestionId END AS BigQuestionId,a.SmallQuestionId FROM dbo.t_My_ErrorSubject  a 
                 LEFT JOIN dbo.t_Subject_Smallquestion b ON a.SmallQuestionId=b.Id
                 WHERE UserId='{0}' 
                 )a
                 LEFT JOIN dbo.t_Subject_BigQuestion b ON a.BigQuestionId=b.Id
                 LEFT JOIN dbo.t_Course_Video c ON b.VideoId=c.Id
                 WHERE ISNULL(a.SmallQuestionId,'')='' ORDER BY a.BigQuestionId", input.UserId );
            var list1 = Db.QueryList<ErrorSubjectsModelOutput>( sql1, null );

            var sql2 = string.Format( @"SELECT c.QuestionContent description,b.*,d.Code,d.VideoName  FROM (
                 SELECT CASE when ISNULL(a.BigQuestionId,'')='' THEN b.BigQuestionId ELSE	 a.BigQuestionId END AS BigQuestionId,a.SmallQuestionId FROM dbo.t_My_ErrorSubject  a 
                 LEFT JOIN dbo.t_Subject_Smallquestion b ON a.SmallQuestionId=b.Id
                 WHERE UserId='{0}' 
                 )a
                 LEFT JOIN dbo.t_Subject_Smallquestion b ON a.SmallQuestionId=b.Id
                 LEFT JOIN dbo.t_Subject_BigQuestion c ON a.BigQuestionId=c.Id
                 LEFT JOIN dbo.t_Course_Video d ON b.VideoId=d.Id
                 WHERE ISNULL(a.SmallQuestionId,'')<>'' ORDER BY a.BigQuestionId", input.UserId );
            var list2 = Db.QueryList<ErrorSubjectsModelOutput>( sql2, null );

            var list = new List<ErrorSubjectsModelOutput>( );
             list1.AddRange( list2);
            return list1;

        }




        /// <summary>
        /// 删除我的错题
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public MessagesOutPut RemoveErrorSubject( string id, string type, string userid )
        {
            var sql = "";
            if ( type == "0" )
            {
                sql = string.Format( " delete from dbo.t_My_ErrorSubject WHERE UserId='{0}' AND SmallQuestionId='{1}'", userid, id );
            } else if ( type == "1" )
            {
                sql = string.Format( " delete from dbo.t_My_ErrorSubject WHERE UserId='{0}' AND BigQuestionId='{1}'", userid, id );
            }
            Db.ExecuteScalar<int>( sql, null );
            return new MessagesOutPut { Success = true };

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string InsertGetId( MyErrorSubject input )
        {
            var str = " select count(1) from t_My_ErrorSubject where UserId=@UserId ";
            var dy = new DynamicParameters( );
            dy.Add( ":UserId", input.UserId, DbType.String );
            if ( !string.IsNullOrEmpty( input.BigQuestionId ) )
            {
                str += " and BigQuestionId=@BigQuestionId";
                dy.Add( ":BigQuestionId", input.BigQuestionId, DbType.String );
            }
            if ( !string.IsNullOrEmpty( input.SmallQuestionId ) )
            {
                str += " and SmallQuestionId=@SmallQuestionId";
                dy.Add( ":SmallQuestionId", input.SmallQuestionId, DbType.String );
            }
            return Db.QueryFirstOrDefault<int>( str, dy ) > 0 ? "" : _repository.InsertGetId( input );
        }
    }
}
