using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZF.Application.Dto;
using ZF.Application.WebApiDto.CourseSubjectModule;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.CourseSubjectModule
{
    /// <summary>
    /// 
    /// </summary>
    public class CourseSubjectAppService : BaseAppService<CourseSubject>
    {
        private readonly ICourseSubjectRepository _repository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        public CourseSubjectAppService( ICourseSubjectRepository repository ) : base( repository )
        {
            _repository = repository;
        }

        /// <summary>
        /// 获取课程试题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<CourseSubjectBigQuestionModelOutput> GetList( CourseSubjectModelInput input, out int count )
        {
            const string strSql = " SELECT *    ";
            var fromSql = new StringBuilder( " FROM dbo.t_Subject_BigQuestion where 1 = 1 " );
            const string sqlCount = "select count(*)  ";
            var dynamicParameters = new DynamicParameters( );

            if ( !string.IsNullOrWhiteSpace( input.SubjectId ) )
            {
                fromSql.Append( " and SubjectId = @SubjectId " );
                dynamicParameters.Add( ":SubjectId ", input.SubjectId, DbType.String );
            }



            count = Db.ExecuteScalar<int>( sqlCount + fromSql, dynamicParameters );
            var list = Db.QueryList<CourseSubjectBigQuestionModelOutput>( GetPageSql( strSql + fromSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord ), dynamicParameters );

            var sql = new StringBuilder( " SELECT * FROM dbo.t_Subject_Smallquestion WHERE BigQuestionId =@BigQuestionId " );
            var dynamicParametersForSmallQuestion = new DynamicParameters( );
            var smallQuestionList = new List<CourseSubjectSmallQuestionModelOutput>( );
            foreach ( var item in list )
            {
                dynamicParametersForSmallQuestion.Add( ":BigQuestionId", item.Id, DbType.String );
                smallQuestionList = Db.QueryList<CourseSubjectSmallQuestionModelOutput>( sql.ToString( ), dynamicParametersForSmallQuestion );
                if ( smallQuestionList.Count > 0 )
                {
                    item.SubjectSmallquestions = smallQuestionList;
                }
            }
            return list;
        }

        /// <summary>
        /// 获取收藏的试题
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<CourseSubjectBigQuestionModelOutput> GetCollectedSubject( MyCollectionListInput input, out int count )
        {
            //const string strSql = " SELECT b.*,a.Id as CollectionId ,c.VideoName,d.KnowledgePointName,a.AddTime   ";
            //var fromSql = new StringBuilder( @" FROM dbo.t_My_CollectionItem a 
            // LEFT JOIN (
            // SELECT   a.id,a.QuestionTitle,a.SubjectType,a.QuestionContent,a.Number,a.RightAnswer,a.QuestionTextAnalysis,a.KnowledgePointId,a.Option1,a.Option2,a.Option3,a.Option4,a.Option5,a.Option6,a.Option7,a.Option8,a.VideoId FROM dbo.t_Subject_BigQuestion a
            //UNION ALL
            //SELECT  b.Id,b.QuestionTitle,b.SubjectType,b.QuestionContent,b.Number,b.RightAnswer ,b.QuestionTextAnalysis,c.KnowledgePointId,b.Option1,b.Option2,b.Option3,b.Option4,b.Option5,b.Option6,b.Option7,b.Option8,b.VideoId FROM dbo.t_Subject_Smallquestion b
            //LEFT JOIN dbo.t_Subject_BigQuestion c ON c.Id=b.BigQuestionId
            // )b
            // ON a.questionId=b.Id
            // LEFT JOIN dbo.t_Course_Video c ON b.VideoId=c.Id
            //LEFT JOIN dbo.t_Subject_KnowledgePoint d ON b.KnowledgePointId=d.Id
            //WHERE  b.id IS NOT NULL AND a.UserId=@UserId " );

            //把小题id转换成大题id,重复数据取最近的时间
            const string strSql = "SELECT  a.AddTime,b.Id,b.QuestionTitle,b.QuestionContent,b.Option1,b.Option2,b.Option3,b.Option4,b.Option5,b.Option6,b.Option7,b.Option8,b.Number,b.SubjectType,b.RightAnswer,b.ConsultAnswer,b.State,b.QuestionTextAnalysis,b.VideoId ,c.VideoName,d.KnowledgePointName ";
            var fromSql = new StringBuilder( @"FROM (         
                    SELECT a.UserId,MAX(a.AddTime) AS AddTime ,a.QuestionId	 FROM (   
                    SELECT  a.UserId,a.AddTime,b.id, CASE WHEN ISNULL(b.Id,'')='' THEN a.QuestionId ELSE b.BigQuestionId END AS QuestionId FROM (SELECT a.UserId,a.QuestionId,MAX(a.AddTime) AS AddTime FROM dbo.t_My_CollectionItem  a GROUP BY a.UserId,a.QuestionId)a
                    LEFT JOIN dbo.t_Subject_Smallquestion b ON a.QuestionId=b.Id  
                    )a 
                    WHERE a.UserId=@UserId 
                    GROUP BY a.UserId,a.QuestionId
                    ) a 
                    LEFT JOIN dbo.t_Subject_BigQuestion b ON a.QuestionId=b.Id
                     LEFT JOIN dbo.t_Course_Video c ON b.VideoId=c.Id
                    LEFT JOIN dbo.t_Subject_KnowledgePoint d ON b.KnowledgePointId=d.Id 
                    where 1=1 " );
            const string sqlCount = "select count(*)  ";
            var dynamicParameters = new DynamicParameters( );
            dynamicParameters.Add( ":UserId", input.UserId, DbType.String );

            if ( !string.IsNullOrEmpty( input.QuestionContent ) )
            {
                fromSql.Append( " and b.QuestionContent like @QuestionContent " );
                dynamicParameters.Add( ":QuestionContent", "%" + input.QuestionContent + "%", DbType.String );
            }
            count = Db.ExecuteScalar<int>( sqlCount + fromSql, dynamicParameters );
            var list = Db.QueryList<CourseSubjectBigQuestionModelOutput>( GetPageSql( strSql + fromSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord ), dynamicParameters );

            //在收藏试题中，获取该用户收藏的试题中指定大题的所有小题
            var subsql = "";
            foreach ( var item in list )
            {
                subsql = string.Format( @"SELECT a.AddTime, b.Id,b.QuestionTitle,b.QuestionContent,b.Option1,b.Option2,b.Option3,b.Option4,b.Option5,b.Option6,b.Option7,b.Option8,b.Number,b.SubjectType,b.RightAnswer,b.ConsultAnswer,b.State,b.QuestionTextAnalysis,b.VideoId,c.VideoName FROM dbo.t_My_CollectionItem a LEFT JOIN 
                dbo.t_Subject_Smallquestion b ON a.QuestionId=b.Id
                LEFT JOIN dbo.t_Course_Video c ON b.VideoId=c.Id
                WHERE ISNULL(b.Id,'')<>'' and b.BigQuestionId='{0}' and a.UserId='{1}'", item.Id, input.UserId );
                item.SubjectSmallquestions = Db.QueryList<CourseSubjectSmallQuestionModelOutput>( subsql, null );
            }
            return list;
        }

        /// <summary>
        /// 获取收藏试题-公众号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<CourseSubjectBigQuestionModelOutput> GetCollectedSubjectList( MyCollectionListInput input )
        {
            var sql1 = string.Format( @"SELECT  b.*,'' Description,c.Code VideoId,c.VideoName FROM dbo.t_My_CollectionItem a 
                 LEFT JOIN dbo.t_Subject_BigQuestion b ON a.QuestionId=b.Id
                 LEFT JOIN dbo.t_Course_Video c ON b.VideoId = c.Id
                 WHERE ISNULL(b.Id,'')<>'' AND a.UserId='{0}'", input.UserId );
            var list1 = Db.QueryList<CourseSubjectBigQuestionModelOutput>( sql1, null );

            var sql2 = string.Format( @"SELECT b.*,c.QuestionContent Description ,d.Code VideoId,d.VideoName FROM dbo.t_My_CollectionItem a 
                 LEFT JOIN dbo.t_Subject_Smallquestion b ON a.QuestionId=b.Id
                 LEFT JOIN dbo.t_Subject_BigQuestion c ON b.BigQuestionId=c.Id
                 LEFT JOIN dbo.t_Course_Video d ON b.VideoId = d.Id
                 WHERE ISNULL(b.Id,'')<>'' AND a.UserId='{0}'", input.UserId );
            var list2 = Db.QueryList<CourseSubjectBigQuestionModelOutput>( sql2, null );
            list1.AddRange( list2);
            return list1;
        }
    }
}
