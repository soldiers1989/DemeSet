
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
    /// 数据表实体应用服务现实：CourseSubject 
    /// </summary>
    public class CourseSubjectAppService : BaseAppService<CourseSubject>
    {
        private readonly ICourseSubjectRepository _iCourseSubjectRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iCourseSubjectRepository"></param>
        /// <param name="operatorLogAppService"></param>
        public CourseSubjectAppService(ICourseSubjectRepository iCourseSubjectRepository, OperatorLogAppService operatorLogAppService) : base(iCourseSubjectRepository)
        {
            _iCourseSubjectRepository = iCourseSubjectRepository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：CourseSubject 
        /// </summary>
        public List<CourseSubjectOutput> GetList(CourseSubjectListInput input, out int count)
        {
            const string sql = "select  a.* ";
            var strSql = new StringBuilder(" from t_Course_Subject  a  where a.IsDelete=0  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.CourseId))
            {
                strSql.Append(" and a.CourseId = @CourseId ");
                dynamicParameters.Add(":CourseId", input.CourseId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.ChapterId))
            {
                strSql.Append(" and a.ChapterId = @ChapterId ");
                dynamicParameters.Add(":ChapterId", input.ChapterId, DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<CourseSubjectOutput>(GetPageSql(sql + strSql,
                  dynamicParameters,
                  input.Page,
                  input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 新增实体  CourseSubject
        /// </summary>
        public MessagesOutPut AddOrEdit(CourseSubjectInput input)
        {
            CourseSubject model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _iCourseSubjectRepository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;
                model.CourseId = input.CourseId;
                model.ChapterId = input.ChapterId;
                model.SubjectId = input.SubjectId;
                #endregion
                _iCourseSubjectRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.CourseSubject,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = string.Format("新增课程试题:{0}-{1}", model.CourseId, model.ChapterId)
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<CourseSubject>();
            model.Id = Guid.NewGuid().ToString();
            model.CourseId = input.CourseId;
            model.ChapterId = input.ChapterId;
            model.SubjectId = input.SubjectId;
            var keyId = _iCourseSubjectRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.CourseSubject,
                OperatorType = (int)OperatorType.Add,
                Remark = string.Format("新增课程试题:{0}-{1}", model.CourseId, model.SubjectId)
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }


        /// <summary>
        /// 查询试题是否已存在章节试题表中
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool Exist(CourseSubjectInput input)
        {
            var strSql = new StringBuilder(" SELECT COUNT(*) FROM dbo.t_Course_Subject WHERE CourseId=@CourseId AND ChapterId=@ChapterId AND SubjectId=@SubjectId ");
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add(":CourseId", input.CourseId, DbType.String);
            dynamicParameters.Add(":ChapterId", input.ChapterId, DbType.String);
            dynamicParameters.Add(":SubjectId", input.SubjectId, DbType.String);
            return Db.ExecuteScalar<int>(strSql.ToString(), dynamicParameters) > 0;

        }


        /// <summary>
        /// 根据条件获取章节已有的试题
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<SubjectBigQuestionOutput> GetExistBigQuestionWithSmallQuestion(SubjectBigQuestionInputForCourseChapter input, out int count)
        {
            if ( string.IsNullOrWhiteSpace( input.Chapter_Id )&&string.IsNullOrWhiteSpace(input.ParentId) )
            {
                count = 0;
                return new List<SubjectBigQuestionOutput>( );
            }
            //useTimes 统计t_Course_Subject表与t_Paper_Detatail表中试题的使用总次数
            const string strSql = " SELECT c.useTimes,a.Id,b.DifficultLevel, b.QuestionTitle,b.QuestionContent,b.SubjectId,b.KnowledgePointId,b.SubjectType,b.SubjectClassId,b.State   ";
            var fromSql = new StringBuilder(" FROM dbo.t_Course_Subject a LEFT JOIN dbo.t_Subject_BigQuestion b ON a.SubjectId=b.id left join (SELECT QuestionId, COUNT(*) AS useTimes FROM dbo.V_Question_Count GROUP BY QuestionId)c on a.SubjectId=c.QuestionId  where b.IsDelete=0 ");
            const string sqlCount = "select count(*)  ";
            var dynamicParameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(input.ParentId))
            {
                fromSql.Append(" and d.ParentId=@ParentId ");
                dynamicParameters.Add(":ParentId", input.ParentId, DbType.String);
            }
            if ( !string.IsNullOrWhiteSpace( input.Chapter_Id ) )
            {
                fromSql.Append( " and a.ChapterId = @ChapterId " );
                dynamicParameters.Add( ":ChapterId ", input.Chapter_Id, DbType.String );
            } 
            if (!string.IsNullOrWhiteSpace(input.Subject_Id))
            {
                fromSql.Append(" and b.SubjectId = @SubjectId ");
                dynamicParameters.Add(":SubjectId ", input.Subject_Id, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.KnowledgePoint_Id))
            {
                var knowledgePointIds = input.KnowledgePoint_Id.TrimEnd(',').Split(',');
                var inClouse = "";
                foreach (var item in knowledgePointIds)
                {
                    inClouse += "'" + item + "',";
                }
                inClouse = inClouse.TrimEnd(',');

                fromSql.Append(" and b.KnowledgePointId in  ( " + inClouse + " ) ");
                //dynamicParameters.Add( ":KnowledgePointId", "( "+inClouse+" )", DbType.String );
            }
            if (!string.IsNullOrWhiteSpace(input.SubjectClass_Id))
            {
                fromSql.Append(" and b.SubjectClassId = @SubjectClassId ");
                dynamicParameters.Add(":SubjectClassId ", input.SubjectClass_Id, DbType.String);
            }
            if (input.Subject_Type > 0)
            {
                fromSql.Append(" and b.SubjectType=@SubjectType ");
                dynamicParameters.Add(":SubjectType", input.Subject_Type, DbType.Int16);
            }
            if ( !string.IsNullOrWhiteSpace( input.QuestionTitle ) ) {
                fromSql.Append( " and b.QuestionTitle like @QuestionTitle " );
                dynamicParameters.Add( ":QuestionTitle ", "%"+input.QuestionTitle+"%", DbType.String );
            }

            count = Db.ExecuteScalar<int>(sqlCount + fromSql, dynamicParameters);
            var list = Db.QueryList<SubjectBigQuestionOutput>(GetPageSql(strSql + fromSql,
                dynamicParameters,
                input.Page,
                input.Rows, "SubjectType", "asc"), dynamicParameters);

            var sql = new StringBuilder(" SELECT * FROM dbo.t_Subject_Smallquestion WHERE BigQuestionId =@BigQuestionId ");
            var dynamicParametersForSmallQuestion = new DynamicParameters();
            var smallQuestionList = new List<SubjectSmallquestionOutput>();
            foreach (var item in list)
            {
                dynamicParametersForSmallQuestion.Add(":BigQuestionId", item.Id, DbType.String);
                smallQuestionList = Db.QueryList<SubjectSmallquestionOutput>(sql.ToString(), dynamicParametersForSmallQuestion);
                if (smallQuestionList.Count > 0)
                {
                    item.SubjectSmallquestion = smallQuestionList;
                }
            }
            return list;
        }
    }
}

