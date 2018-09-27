
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
    /// 数据表实体应用服务现实：Smallquestion 
    /// </summary>
    public class SubjectSmallquestionAppService : BaseAppService<SubjectSmallquestion>
    {
        private readonly ISubjectSmallquestionRepository _iSubjectSmallquestionRepository;

        private readonly SubjectBigQuestionAppService _subjectBigQuestionAppService;


        private readonly FileRelationshipAppService _fileRelationshipAppService;
        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="iSubjectSmallquestionRepository"></param>
        /// <param name="operatorLogAppService"></param>
        /// <param name="fileRelationshipAppService"></param>
        /// <param name="subjectBigQuestionAppService"></param>
        public SubjectSmallquestionAppService(ISubjectSmallquestionRepository iSubjectSmallquestionRepository,
            OperatorLogAppService operatorLogAppService, FileRelationshipAppService fileRelationshipAppService, SubjectBigQuestionAppService subjectBigQuestionAppService) : base(iSubjectSmallquestionRepository)
        {
            _iSubjectSmallquestionRepository = iSubjectSmallquestionRepository;
            _operatorLogAppService = operatorLogAppService;
            _fileRelationshipAppService = fileRelationshipAppService;
            _subjectBigQuestionAppService = subjectBigQuestionAppService;
        }

        /// <summary>
        /// 查询列表实体：SubjectSmallquestion 
        /// </summary>
        public List<SubjectSmallquestionOutput> GetList(SubjectSmallquestionListInput input, out int count)
        {
            const string sql = "select  a.* ";
            var strSql = new StringBuilder("  from t_Subject_Smallquestion  a   where a.IsDelete=0  ");
            const string sqlCount = "select count(*) ";
            var dynamicParameters = new DynamicParameters();
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
            if (!string.IsNullOrWhiteSpace(input.BigQuestionId))
            {
                strSql.Append(" and a.BigQuestionId = @BigQuestionId ");
                dynamicParameters.Add(":BigQuestionId", input.BigQuestionId, DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<SubjectSmallquestionOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void Option3(SubjectSmallquestion model)
        {
            model.Option3 = "";
            Option4(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void Option4(SubjectSmallquestion model)
        {
            model.Option4 = "";
            Option5(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void Option5(SubjectSmallquestion model)
        {
            model.Option5 = "";
            Option6(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void Option6(SubjectSmallquestion model)
        {
            model.Option6 = "";
            Option7(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void Option7(SubjectSmallquestion model)
        {
            model.Option7 = "";
            Option8(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void Option8(SubjectSmallquestion model)
        {
            model.Option8 = "";
        }

        /// <summary>
        /// 保存选项 答案逻辑
        /// </summary>
        /// <param name="model"></param>
        /// <param name="input"></param>
        public void SaveOption(SubjectSmallquestion model, SubjectSmallquestionInput input)
        {
            Regex reg = new Regex( @"<\/?p[^>]*>", RegexOptions.IgnoreCase);
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
                                model.Option1 = reg.Replace(input.Option1,"");
                                model.Option2 = reg.Replace( input.Option2,"");
                                Option3(model);
                                break;
                            case 3:
                                model.Option1 = reg.Replace( input.Option1,"");
                                model.Option2 = reg.Replace( input.Option2, "" );
                                model.Option3 = reg.Replace( input.Option3, "" );
                                Option4(model);
                                break;
                            case 4:
                                model.Option1 = reg.Replace( input.Option1, "" );
                                model.Option2 = reg.Replace( input.Option2, "" );
                                model.Option3 = reg.Replace( input.Option3, "" );
                                model.Option4 = reg.Replace( input.Option4, "" );
                                Option5(model);
                                break;
                            case 5:
                                model.Option1 = reg.Replace( input.Option1, "" );
                                model.Option2 = reg.Replace( input.Option2, "" );
                                model.Option3 = reg.Replace( input.Option3, "" );
                                model.Option4 = reg.Replace( input.Option4, "" );
                                model.Option5 = reg.Replace( input.Option5, "" );
                                Option6(model);
                                break;
                            case 6:
                                model.Option1 = reg.Replace( input.Option1, "" );
                                model.Option2 = reg.Replace(input.Option2, "" );
                                model.Option3 = reg.Replace(input.Option3, "" );
                                model.Option4 = reg.Replace(input.Option4, "" );
                                model.Option5 = reg.Replace(input.Option5, "" );
                                model.Option6 = reg.Replace(input.Option6, "" );
                                Option7(model);
                                break;
                            case 7:
                                model.Option1 = reg.Replace(input.Option1, "" );
                                model.Option2 = reg.Replace(input.Option2, "" );
                                model.Option3 = reg.Replace(input.Option3, "" );
                                model.Option4 = reg.Replace(input.Option4, "" );
                                model.Option5 = reg.Replace(input.Option5, "" );
                                model.Option6 = reg.Replace(input.Option6, "" );
                                model.Option7 = reg.Replace(input.Option7, "" );
                                Option8(model);
                                break;
                            case 8:
                                model.Option1 = reg.Replace(input.Option1, "" );
                                model.Option2 = reg.Replace(input.Option2, "" );
                                model.Option3 = reg.Replace(input.Option3, "" );
                                model.Option4 = reg.Replace(input.Option4, "" );
                                model.Option5 = reg.Replace(input.Option5, "" );
                                model.Option6 = reg.Replace(input.Option6, "" );
                                model.Option7 = reg.Replace(input.Option7, "" );
                                model.Option8 = reg.Replace(input.Option8, "" );
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
        /// 新增实体  SubjectSmallquestion
        /// </summary>
        public MessagesOutPut AddOrEdit(SubjectSmallquestionInput input)
        {
            SubjectSmallquestion model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                RedisCacheHelper.Remove("GetQuestion_" + input.Id);
                model = _iSubjectSmallquestionRepository.Get(input.Id);

                #region 修改逻辑

                model.Id = input.Id;
                model.VideoId = input.VideoId;
                model.QuestionTitle = input.QuestionTitle;
                model.QuestionContent = input.QuestionContent;
                model.State = input.State;
                model.DigitalBookPage = input.DigitalBookPage;
                model.Number = input.Number;
                model.QuestionAudioAnalysis = input.QuestionAudioAnalysis;
                model.QuestionTextAnalysis = input.QuestionTextAnalysis;
                model.QuestionVedioAnalysis = input.QuestionVedioAnalysis;
                model.SubjectType = input.SubjectType;
                SaveOption(model, input);

                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion


                _iSubjectSmallquestionRepository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.SubjectSmallquestion,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改试题小题" + model.QuestionTitle
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
            model = input.MapTo<SubjectSmallquestion>();
            SaveOption(model, input);
            model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            //var bigType1 = _subjectBigQuestionAppService.GetOneId(input.BigQuestionId);
            //if (bigType1 != null)
            //    model.SubjectType = bigType1.Value;
            var keyId = _iSubjectSmallquestionRepository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.SubjectSmallquestion,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增试题小题" + model.QuestionTitle
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
        /// 查询列表实体：SubjectSmallquestion 
        /// </summary>
        public SubjectSmallquestionOutput GetOne(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;
            string sql =
                " select  a.* from t_Subject_Smallquestion  a  left join t_Course_Video b on a.VideoId=b.Id  where 1=1  ";
            var dynamicParameters = new DynamicParameters();
            sql += " and a.Id = @Id ";
            dynamicParameters.Add(":Id", id, DbType.String);
            return Db.QueryFirstOrDefault<SubjectSmallquestionOutput>(sql, dynamicParameters);
        }

        /// <summary>
        /// 删除小表数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new MessagesOutPut LogicDelete(string id)
        {
            var model = _iSubjectSmallquestionRepository.Get(id);
            if (model != null)
            {
                var sql = "SELECT COUNT(1) FROM t_Paper_Detatail WHERE IsDelete=0  and  QuestionId=@QuestionId";
                var dynamicParameters = new DynamicParameters();
                dynamicParameters.Add(":QuestionId", model.BigQuestionId, DbType.String);
                int count = Db.ExecuteScalar<int>(sql, dynamicParameters);
                if (count > 0)
                {
                    return new MessagesOutPut { Id = -1, Message = "删除失败,该试题已经被组卷过!", Success = false };
                }
                _iSubjectSmallquestionRepository.LogicDelete(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.SubjectSmallquestion,
                    OperatorType = (int)OperatorType.Delete,
                    Remark = "删除试题小题" + model.QuestionTitle
                });
            }
            return new MessagesOutPut { Id = -1, Message = "删除成功!", Success = true };
        }
    }
}

