
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using Dapper;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;
using ZF.Infrastructure.RedisCache;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 数据表实体应用服务现实：SubjectBigQuestion 
    /// </summary>
    public class SubjectBigQuestionAppService : BaseAppService<SubjectBigQuestion>
    {
        private readonly ISubjectBigQuestionRepository _iSubjectBigQuestionRepository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        private readonly SubjectClassAppService _subjectClassAppService;

        private readonly FileRelationshipAppService _fileRelationshipAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iSubjectBigQuestionRepository"></param>
        /// <param name="operatorLogAppService"></param>
        /// <param name="fileRelationshipAppService"></param>
        /// <param name="subjectClassAppService"></param>
        public SubjectBigQuestionAppService(ISubjectBigQuestionRepository iSubjectBigQuestionRepository,
            OperatorLogAppService operatorLogAppService, FileRelationshipAppService fileRelationshipAppService, SubjectClassAppService subjectClassAppService) : base(iSubjectBigQuestionRepository)
        {
            _iSubjectBigQuestionRepository = iSubjectBigQuestionRepository;
            _operatorLogAppService = operatorLogAppService;
            _fileRelationshipAppService = fileRelationshipAppService;
            _subjectClassAppService = subjectClassAppService;
        }

        /// <summary>
        /// 查询列表实体：SubjectBigQuestion 
        /// </summary>
        public List<SubjectBigQuestionOutput> GetList(SubjectBigQuestionListInput input, out int count)
        {
            const string sql = "select  a.*,b.SubjectName,c.KnowledgePointName,d.ClassName ";
            var strSql = new StringBuilder(@"  from t_Subject_BigQuestion  a  
                                               left join t_Base_Subject b on  a.SubjectId=b.Id
                                               left join t_Subject_KnowledgePoint c  on a.KnowledgePointId=c.Id
                                               left join t_Subject_Class d on a.SubjectClassId=d.Id
                                               left join t_Base_Project f on d.ProjectId=f.Id where a.IsDelete=0  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
            if (!string.IsNullOrWhiteSpace(input.ProjectClassId))
            {
                strSql.Append(" and f.ProjectClassId = @ProjectClassId ");
                dynamicParameters.Add(":ProjectClassId", input.ProjectClassId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.ProjectId))
            {
                strSql.Append(" and f.Id = @ProjectId ");
                dynamicParameters.Add(":ProjectId", input.ProjectId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.SubjectClassId))
            {
                strSql.Append(" and a.SubjectClassId = @SubjectClassId ");
                dynamicParameters.Add(":SubjectClassId", input.SubjectClassId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.SubjectId))
            {
                strSql.Append(" and a.SubjectId = @SubjectId ");
                dynamicParameters.Add(":SubjectId", input.SubjectId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.KnowledgePointId))
            {
                strSql.Append(" and  ( a.KnowledgePointId in(" + GetIds("'" + input.KnowledgePointId + "'") + ") or  a.KnowledgePointId= '" + input.KnowledgePointId + "' )");
            }
            if (!string.IsNullOrWhiteSpace(input.QuestionTitle))
            {
                strSql.Append(" and a.QuestionTitle like @QuestionTitle ");
                dynamicParameters.Add(":QuestionTitle", '%' + input.QuestionTitle + '%', DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.SubjectType))
            {
                strSql.Append(" and a.SubjectType = @SubjectType ");
                dynamicParameters.Add(":SubjectType", input.SubjectType, DbType.String);
            }
            if (input.DifficultLevel.HasValue)
            {
                strSql.Append(" and a.DifficultLevel = @DifficultLevel ");
                dynamicParameters.Add(":DifficultLevel", input.DifficultLevel, DbType.Int32);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<SubjectBigQuestionOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="knowledgePointId"></param>
        /// <returns></returns>
        public string GetIds(string knowledgePointId)
        {
            var sql1 = $@" SELECT '%'+ id FROM (
  SELECT id= STUFF((SELECT '%'+Id+'%,'  FROM t_Subject_KnowledgePoint WHERE a.subjectid=subjectid 
  and   ParentId IN ( {knowledgePointId} )
  
  FOR XML PATH('')),1,1,'') 
  FROM      dbo.t_Subject_KnowledgePoint a
  WHERE     ParentId IN (  {knowledgePointId} )
	GROUP BY subjectid
  ) a";
            if (Db.QueryFirstOrDefault<string>(sql1, null) == null)
            {
                return knowledgePointId;
            }
            var list1 = Db.QueryFirstOrDefault<string>(sql1, null).TrimEnd(',').Replace("%", "'");
            if (!string.IsNullOrEmpty(list1))
            {
                return GetIds(list1);
            }
            return list1;
        }

        /// <summary>
        /// 审核反馈错题
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MessagesOutPut Audit(string id)
        {
            var sql = " UPDATE t_Question_checker SET  audit=1 WHERE Id=@Id";
            var dy = new DynamicParameters();
            dy.Add(":Id", id, DbType.String);
            Db.ExecuteScalar<int>(sql, dy);
            return new MessagesOutPut { Success = true };
        }


        /// <summary>
        /// 查询该试题是否被使用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IfUse(string id)
        {
            var sql = string.Format(@"SELECT COUNT(*) FROM (
        
        SELECT DISTINCT a.SubjectId AS qusetionId FROM dbo.t_Course_Subject a
        UNION
        SELECT DISTINCT b.QuestionId FROM dbo.t_Paper_Detatail b
        )a WHERE a.qusetionId='{0}'", id);
            return Db.ExecuteScalar<int>(sql, null) > 0;
        }


        /// <summary>
        /// /
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<SubjectBigQuestionOutput> GetBigQuestionExceptExistInChapter(SubjectBigQuestionInputForCourseChapter input, out int count)
        {
            //useTimes 统计t_Course_Subject表与t_Paper_Detatail表中试题的使用总次数
            const string strSql = " SELECT b.useTimes,a.Id,a.DifficultLevel, a.QuestionTitle,a.QuestionContent,a.SubjectId,a.KnowledgePointId,a.SubjectType,a.SubjectClassId,a.State ";
            var fromSql = new StringBuilder(" FROM dbo.t_Subject_BigQuestion a left join (SELECT QuestionId, COUNT(*) AS useTimes FROM dbo.V_Question_Count GROUP BY QuestionId)b on a.Id=b.QuestionId where IsDelete=0  and isnull(State,0)=0");
            const string sqlCount = "select count(*)  ";
            var dynamicParameters = new DynamicParameters();
            //if ( !string.IsNullOrEmpty( input.ChapterId ) ) {
            //    fromSql.Append( " and Id not in (SELECT SubjectId FROM dbo.t_Course_Subject WHERE ChapterId=@ChapterId)" );
            //    dynamicParameters.Add( ":ChapterId",input.ChapterId,DbType.String);
            //}
            //剔除已经存在该课程中的试题
            if (!string.IsNullOrEmpty(input.CourseId))
            {
                fromSql.Append(" and Id not in  (SELECT SubjectId FROM dbo.t_Course_Subject WHERE CourseId=@CourseId )");
                dynamicParameters.Add(":CourseId", input.CourseId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.SubjectId))
            {
                fromSql.Append(" and SubjectId = @SubjectId ");
                dynamicParameters.Add(":SubjectId ", input.SubjectId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.KnowledgePointId))
            {
                var knowledgePointIds = input.KnowledgePointId.TrimEnd(',').Split(',');
                var inClouse = "";
                foreach (var item in knowledgePointIds)
                {
                    inClouse += "'" + item + "',";
                }
                inClouse = inClouse.TrimEnd(',');

                fromSql.Append(" and KnowledgePointId in  ( " + inClouse + " ) ");
                //dynamicParameters.Add( ":KnowledgePointId", "( "+inClouse+" )", DbType.String );
            }
            if (!string.IsNullOrWhiteSpace(input.SubjectClassId))
            {
                fromSql.Append(" and SubjectClassId = @SubjectClassId ");
                dynamicParameters.Add(":SubjectClassId ", input.SubjectClassId, DbType.String);
            }
            if (input.SubjectType > 0)
            {
                fromSql.Append(" and SubjectType=@SubjectType ");
                dynamicParameters.Add(":SubjectType", input.SubjectType, DbType.Int16);
            }
            if (!string.IsNullOrEmpty(input.QuestionTitle))
            {
                fromSql.Append(" and QuestionTitle like @QuestionTitle ");
                dynamicParameters.Add(":QuestionTitle", input.QuestionTitle, DbType.String);
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




        /// <summary>
        /// 根据知识点ID集合查询试题
        /// </summary>
        /// <param name="knowIdList"></param>
        /// <returns></returns>
        public List<SubjectBigQuestionOutput> GetList(string knowIdList)
        {
            const string sql = "  select ROW_NUMBER() over(order by Id) rowid, Id,SubjectType from t_Subject_BigQuestion  ";
            var strSql = new StringBuilder();
            strSql.Append(knowIdList);
            var list = Db.QueryList<SubjectBigQuestionOutput>(sql + strSql);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void Option3(SubjectBigQuestion model)
        {
            model.Option3 = "";
            Option4(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void Option4(SubjectBigQuestion model)
        {
            model.Option4 = "";
            Option5(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void Option5(SubjectBigQuestion model)
        {
            model.Option5 = "";
            Option6(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void Option6(SubjectBigQuestion model)
        {
            model.Option6 = "";
            Option7(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void Option7(SubjectBigQuestion model)
        {
            model.Option7 = "";
            Option8(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void Option8(SubjectBigQuestion model)
        {
            model.Option8 = "";
        }

        /// <summary>
        /// 保存选项 答案逻辑
        /// </summary>
        /// <param name="model"></param>
        /// <param name="input"></param>
        public void SaveOption(SubjectBigQuestion model, SubjectBigQuestionInput input)
        {
            Regex reg = new Regex(@"<\/?p[^>]*>", RegexOptions.IgnoreCase);
            #region 保存选项 答案逻辑
            if (model.SubjectType > 0 && model.Number > 0)
            {
                switch (model.SubjectType)
                {
                    case (int)QuestionType.One:
                    case (int)QuestionType.Two:
                        switch (model.Number)
                        {
                            case 2:
                                model.Option1 = reg.Replace(input.Option1, "");
                                model.Option2 = reg.Replace(input.Option2, "");
                                Option3(model);
                                break;
                            case 3:
                                model.Option1 = reg.Replace(input.Option1, "");
                                model.Option2 = reg.Replace(input.Option2, "");
                                model.Option3 = reg.Replace(input.Option3, "");
                                Option4(model);
                                break;
                            case 4:
                                model.Option1 = reg.Replace(input.Option1, "");
                                model.Option2 = reg.Replace(input.Option2, "");
                                model.Option3 = reg.Replace(input.Option3, "");
                                model.Option4 = reg.Replace(input.Option4, "");
                                Option5(model);
                                break;
                            case 5:
                                model.Option1 = reg.Replace(input.Option1, "");
                                model.Option2 = reg.Replace(input.Option2, "");
                                model.Option3 = reg.Replace(input.Option3, "");
                                model.Option4 = reg.Replace(input.Option4, "");
                                model.Option5 = reg.Replace(input.Option5, "");
                                Option6(model);
                                break;
                            case 6:
                                model.Option1 = reg.Replace(input.Option1, "");
                                model.Option2 = reg.Replace(input.Option2, "");
                                model.Option3 = reg.Replace(input.Option3, "");
                                model.Option4 = reg.Replace(input.Option4, "");
                                model.Option5 = reg.Replace(input.Option5, "");
                                model.Option6 = reg.Replace(input.Option6, "");
                                Option7(model);
                                break;
                            case 7:
                                model.Option1 = reg.Replace(input.Option1, "");
                                model.Option2 = reg.Replace(input.Option2, "");
                                model.Option3 = reg.Replace(input.Option3, "");
                                model.Option4 = reg.Replace(input.Option4, "");
                                model.Option5 = reg.Replace(input.Option5, "");
                                model.Option6 = reg.Replace(input.Option6, "");
                                model.Option7 = reg.Replace(input.Option7, "");
                                Option8(model);
                                break;
                            case 8:
                                model.Option1 = reg.Replace(input.Option1, "");
                                model.Option2 = reg.Replace(input.Option2, "");
                                model.Option3 = reg.Replace(input.Option3, "");
                                model.Option4 = reg.Replace(input.Option4, "");
                                model.Option5 = reg.Replace(input.Option5, "");
                                model.Option6 = reg.Replace(input.Option6, "");
                                model.Option7 = reg.Replace(input.Option7, "");
                                model.Option8 = reg.Replace(input.Option8, "");
                                break;
                        }
                        model.RightAnswer = input.RightAnswer;
                        model.ConsultAnswer = input.ConsultAnswer;
                        break;
                    case (int)QuestionType.Three:
                        model.Option1 = "正确";
                        model.Option2 = "错误";
                        model.Option3 = "";
                        model.Option4 = "";
                        model.Option5 = "";
                        model.Option6 = "";
                        model.Option7 = "";
                        model.Option8 = "";
                        model.Number = 2;
                        model.RightAnswer = input.RightAnswer;
                        model.ConsultAnswer = input.ConsultAnswer;
                        break;
                    case (int)QuestionType.Seven:
                        model.Option1 = "";
                        model.Option2 = "";
                        model.Option3 = "";
                        model.Option4 = "";
                        model.Option5 = "";
                        model.Option6 = "";
                        model.Option7 = "";
                        model.Option8 = "";
                        model.RightAnswer = "";
                        model.ConsultAnswer = "";
                        model.Number = 0;
                        break;
                }
            }
            #endregion
        }

        /// <summary>
        /// 新增实体  SubjectBigQuestion
        /// </summary>
        public MessagesOutPut AddOrEdit(SubjectBigQuestionInput input)
        {
            SubjectBigQuestion model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                RedisCacheHelper.Remove("GetQuestion_" + input.Id);
                model = _iSubjectBigQuestionRepository.Get(input.Id);

                #region 修改逻辑

                model.Id = input.Id;
                model.QuestionTitle = input.QuestionTitle;
                model.QuestionContent = input.QuestionContent;
                model.SubjectClassId = input.SubjectClassId;
                model.State = input.State;
                model.DigitalBookPage = input.DigitalBookPage;
                model.VideoId = input.VideoId;

                var bigType = _subjectClassAppService.Get(input.SubjectClassId).BigType;
                if (bigType != null)
                    model.SubjectType = bigType.Value;

                model.DifficultLevel = input.DifficultLevel;
                //model.KnowledgePointId = input.KnowledgePointId;
                model.Number = input.Number;
                model.QuestionAudioAnalysis = input.QuestionAudioAnalysis;
                model.QuestionTextAnalysis = input.QuestionTextAnalysis;
                model.QuestionVedioAnalysis = input.QuestionVedioAnalysis;
                //  model.SubjectId = input.SubjectClassId;
                #region 保存选项 答案逻辑

                SaveOption(model, input);
                #endregion
                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion

                _iSubjectBigQuestionRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.SubjectBigQuestion,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改试题大表" + model.QuestionTitle
                });
                _fileRelationshipAppService.Add(new FileRelationshipInput
                {
                    ModuleId = input.Id,
                    IdFilehiddenFile = input.QuestionAudioAnalysis,
                    Type = 1
                });
                _fileRelationshipAppService.Add(new FileRelationshipInput
                {
                    ModuleId = input.Id,
                    IdFilehiddenFile = input.QuestionVedioAnalysis,
                    Type = 2
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<SubjectBigQuestion>();
            var bigType1 = _subjectClassAppService.Get(input.SubjectClassId).BigType;
            if (bigType1 != null)
                model.SubjectType = bigType1.Value;
            SaveOption(model, input);
            model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _iSubjectBigQuestionRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.SubjectBigQuestion,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增试题大表" + model.QuestionTitle
            });
            _fileRelationshipAppService.Add(new FileRelationshipInput
            {
                ModuleId = model.Id,
                IdFilehiddenFile = model.QuestionAudioAnalysis,
                Type = 1
            });
            _fileRelationshipAppService.Add(new FileRelationshipInput
            {
                ModuleId = model.Id,
                IdFilehiddenFile = model.QuestionVedioAnalysis,
                Type = 2
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }


        /// <summary>
        /// 查询列表实体：SubjectBigQuestion 
        /// </summary>
        public SubjectBigQuestionOutput GetOne(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            string sql =
                " select  a.*,b.VideoName  from t_Subject_BigQuestion  a  left join t_Course_Video b on a.VideoId=b.Id  where 1=1  ";
            var dynamicParameters = new DynamicParameters();
            sql += " and a.Id = @Id ";
            dynamicParameters.Add(":Id", id, DbType.String);
            return Db.QueryFirstOrDefault<SubjectBigQuestionOutput>(sql, dynamicParameters);
        }


        /// <summary>
        /// 删除大题数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new MessagesOutPut LogicDelete(string id)
        {
            var model = _iSubjectBigQuestionRepository.Get(id);
            if (model != null)
            {
                var sql = "SELECT COUNT(1) FROM t_Paper_Detatail WHERE IsDelete=0  and QuestionId=@QuestionId";
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(":QuestionId", model.Id, DbType.String);
                int count = Db.ExecuteScalar<int>(sql, dynamicParameters);
                if (count > 0)
                {
                    return new MessagesOutPut { Id = -1, Message = "删除失败,该试题已经被组卷过!", Success = false };
                }
                var sql1 = "SELECT COUNT(1) FROM t_Subject_Smallquestion WHERE  IsDelete=0  and  BigQuestionId=@BigQuestionId";
                var dynamicParameters1 = new DynamicParameters();
                dynamicParameters1.Add(":BigQuestionId", model.Id, DbType.String);
                int count1 = Db.ExecuteScalar<int>(sql1, dynamicParameters1);
                if (count1 > 0)
                {
                    return new MessagesOutPut { Id = -1, Message = "删除失败,该案例分析题存在小题数据,请先删除小题数据在进行操作!", Success = false };
                }
                _iSubjectBigQuestionRepository.LogicDelete(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.SubjectSmallquestion,
                    OperatorType = (int)OperatorType.Delete,
                    Remark = "删除试题大题" + model.QuestionTitle
                });
            }
            return new MessagesOutPut { Id = -1, Message = "删除成功!", Success = true };
        }



        /// <summary>
        /// 获取试题列表,大题嵌套小题列表
        /// </summary>
        /// <returns></returns>
        public List<SubjectBigQuestionOutput> GetBigQuestionWithSmallQuestion(SubjectBigQuestionInput input, out int count)
        {
            const string strSql = " SELECT *    ";
            var fromSql = new StringBuilder(" FROM dbo.t_Subject_BigQuestion where 1 = 1 ");
            const string sqlCount = "select count(*)  ";
            var dynamicParameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(input.SubjectId))
            {
                fromSql.Append(" and SubjectId = @SubjectId ");
                dynamicParameters.Add(":SubjectId ", input.SubjectId, DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.KnowledgePointId))
            {
                var knowledgePointIds = input.KnowledgePointId.TrimEnd(',').Split(',');
                var inClouse = "";
                foreach (var item in knowledgePointIds)
                {
                    inClouse += "'" + item + "',";
                }
                inClouse = inClouse.TrimEnd(',');

                fromSql.Append(" and KnowledgePointId in  ( " + inClouse + " ) ");
                //dynamicParameters.Add( ":KnowledgePointId", "( "+inClouse+" )", DbType.String );
            }
            if (!string.IsNullOrWhiteSpace(input.SubjectClassId))
            {
                fromSql.Append(" and SubjectClassId = @SubjectClassId ");
                dynamicParameters.Add(":SubjectClassId ", input.SubjectClassId, DbType.String);
            }
            if (input.SubjectType > 0)
            {
                fromSql.Append(" and SubjectType=@SubjectType ");
                dynamicParameters.Add(":SubjectType", input.SubjectType, DbType.Int16);
            }

            count = Db.ExecuteScalar<int>(sqlCount + fromSql, dynamicParameters);
            var list = Db.QueryList<SubjectBigQuestionOutput>(GetPageSql(strSql + fromSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);

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


        /// <summary>
        /// 错题反馈列表
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<ErrorSubjectFeedBackOutput> GetErrFeedBackList(ErrorSubjectFeedBackInput input, out int count)
        {
            const string strSql = " SELECT a.*    ";
            var fromSql = new StringBuilder(" FROM dbo.t_Question_checker a where 1 = 1 ");
            const string sqlCount = "select count(*)  ";
            var dynamicParameters = new DynamicParameters();
            if (input.BeginDate.HasValue)
            {
                fromSql.Append(" and isnull(a.AddTime,'9999-12-31') >=  @BeginDate ");
                dynamicParameters.Add(":BeginDate", input.BeginDate, DbType.String);
            }
            if (input.EndDate.HasValue)
            {
                fromSql.Append(" and isnull(a.AddTime,'0001-01-01') <=  @EndDate ");
                dynamicParameters.Add(":EndDate", input.EndDate, DbType.String);
            }
            fromSql.Append(" and isnull( a.Audit,0 )=@Audit");
            dynamicParameters.Add(":Audit", input.Audit, DbType.Int16);
            count = Db.ExecuteScalar<int>(sqlCount + fromSql, dynamicParameters);
            var list = Db.QueryList<ErrorSubjectFeedBackOutput>(GetPageSql(strSql + fromSql,
                dynamicParameters,
                input.Page,
                input.Rows, "AddTime", "Desc"), dynamicParameters);

            return list;
        }
    }
}

