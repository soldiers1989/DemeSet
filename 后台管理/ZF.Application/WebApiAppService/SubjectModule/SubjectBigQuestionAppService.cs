using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Castle.Components.DictionaryAdapter;
using Dapper;
using ServiceStack;
using ZF.Application.Dto;
using ZF.Application.WebApiAppService.MyErrorSubjectModule;
using ZF.Application.WebApiDto.SubjectModule;
using ZF.Core;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;
using ZF.Infrastructure.RandomHelper;
using PaperInfoOutput = ZF.Application.WebApiDto.SubjectModule.PaperInfoOutput;
using SubjectBigQuestionInput = ZF.Application.WebApiDto.SubjectModule.SubjectBigQuestionInput;
using SubjectBigQuestionOutput = ZF.Application.WebApiDto.SubjectModule.SubjectBigQuestionOutput;
using ZF.Application.WebApiDto.CourseSubjectModule;

namespace ZF.Application.WebApiAppService.SubjectModule
{
    /// <summary>
    /// 试题大小表  webapi服务
    /// </summary>
    public class SubjectBigQuestionAppService : BaseAppService<SubjectBigQuestion>
    {

        private readonly IMyAnswerRecordsRepository iMyAnswerRecordsRepository;

        private readonly IPaperInfoRepository iPaperInfoRepository;

        private readonly IMyPaperRecordsRepository _iMyPaperRecordsRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ISubjectBigQuestionRepository _repository;


        /// <summary>
        /// 
        /// </summary>
        private readonly ISubjectSmallquestionRepository _iSubjectSmallquestionRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ICourseChapterQuestionsRepository _iCourseChapterQuestionsRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly ICourseChapterQuestionsDetailRepository _iCourseChapterQuestionsDetailRepository;

        private readonly MyErrorSubjectAppService _iMyErrorSubjectRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="iMyAnswerRecordsRepository"></param>
        /// <param name="iPaperInfoRepository"></param>
        /// <param name="iSubjectSmallquestionRepository"></param>
        /// <param name="iMyPaperRecordsRepository"></param>
        /// <param name="iCourseChapterQuestionsRepository"></param>
        /// <param name="iCourseChapterQuestionsDetailRepository"></param>
        /// <param name="iMyErrorSubjectRepository"></param>
        public SubjectBigQuestionAppService(ISubjectBigQuestionRepository repository, IMyAnswerRecordsRepository iMyAnswerRecordsRepository, IPaperInfoRepository iPaperInfoRepository, ISubjectSmallquestionRepository iSubjectSmallquestionRepository, IMyPaperRecordsRepository iMyPaperRecordsRepository, ICourseChapterQuestionsRepository iCourseChapterQuestionsRepository, ICourseChapterQuestionsDetailRepository iCourseChapterQuestionsDetailRepository, MyErrorSubjectAppService iMyErrorSubjectRepository) : base(repository)
        {
            _repository = repository;
            this.iMyAnswerRecordsRepository = iMyAnswerRecordsRepository;
            this.iPaperInfoRepository = iPaperInfoRepository;
            _iSubjectSmallquestionRepository = iSubjectSmallquestionRepository;
            _iMyPaperRecordsRepository = iMyPaperRecordsRepository;
            _iCourseChapterQuestionsRepository = iCourseChapterQuestionsRepository;
            _iCourseChapterQuestionsDetailRepository = iCourseChapterQuestionsDetailRepository;
            _iMyErrorSubjectRepository = iMyErrorSubjectRepository;
        }

        /// <summary>
        /// 获取题目详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public SubjectBigQuestionOutput GetSubjectBigQuestionOne(SubjectBigQuestionInput input)
        {
            if (input.Type == 1 || input.Type == 2 || input.Type == 3)
            {
                var strSql = @" SELECT a.*,'' as Description  FROM t_Subject_BigQuestion a 
                               -- LEFT JOIN t_My_AnswerRecords b ON a.Id=b.BigQuestionId  and b.UserId =@UserId AND b.PaperRecordsId = @PaperRecordsId
                                where  a.IsDelete=0   ";
                var dy = new DynamicParameters();
                if (!string.IsNullOrEmpty(input.BigQuestionId))
                {
                    strSql += "  and a.Id =@BigQuestionId ";
                    dy.Add(":BigQuestionId", input.BigQuestionId, DbType.String);
                }
                //  dy.Add(":PaperRecordsId", input.PaperRecordsId, DbType.String);
                //  dy.Add(":UserId", input.UserId, DbType.String);
                var model = Db.QueryFirstOrDefault<SubjectBigQuestionOutput>(strSql, dy);
                var option = new List<Option>();
                string[] str = new string[8] { model.Option1, model.Option2, model.Option3, model.Option4, model.Option5, model.Option6, model.Option7, model.Option8 };
                string[] str1 = new string[8] { "A.", "B.", "C.", "D.", "E.", "F.", "G.", "H." };
                for (int i = 0; i < model.Number; i++)
                {
                    option.Add(new Option { id = i + 1, content = "<span style=\"float:left;\">" + str1[i] + " </span>" + str[i], ischeck = "0" });
                }
                model.option = option;
                switch (model.SubjectType)
                {
                    case 1:
                        model.input_type = "radio";
                        model.SubjectTypeName = "单选题";
                        break;
                    case 2:
                        model.input_type = "checkbox";
                        model.SubjectTypeName = "多选题";
                        break;
                    case 3:
                        model.input_type = "radio";
                        model.SubjectTypeName = "判断题";
                        break;
                }
                return model;
            }
            else if (input.Type == 7)
            {
                var strSql = @" SELECT a.*,c.QuestionContent as Description FROM t_Subject_Smallquestion a 
                            --  LEFT JOIN t_My_AnswerRecords b ON a.Id=b.SmallQuestionId  and b.UserId =@UserId  and b.PaperRecordsId =@PaperRecordsId 
                              Left join t_Subject_BigQuestion c on a.BigQuestionId=c.Id 
                            where  a.IsDelete=0   ";
                var dy = new DynamicParameters();
                if (!string.IsNullOrEmpty(input.BigQuestionId))
                {
                    strSql += "  and a.Id =@BigQuestionId ";
                    dy.Add(":BigQuestionId", input.BigQuestionId, DbType.String);
                }
                // dy.Add(":PaperRecordsId", input.PaperRecordsId, DbType.String);
                // dy.Add(":UserId", input.UserId, DbType.String);
                var model = Db.QueryFirstOrDefault<SubjectBigQuestionOutput>(strSql, dy);
                var option = new List<Option>();
                string[] str = new string[8] { model.Option1, model.Option2, model.Option3, model.Option4, model.Option5, model.Option6, model.Option7, model.Option8 };
                string[] str1 = new string[8] { "A.", "B.", "C.", "D.", "E.", "F.", "G.", "H." };
                for (int i = 0; i < model.Number; i++)
                {
                    option.Add(new Option { id = i + 1, content = "<span style=\"float:left;\">" + str1[i] + " </span>" + str[i], ischeck =  "0" });
                }
                model.option = option;
                switch (model.SubjectType)
                {
                    case 1:
                        model.input_type = "radio";
                        model.SubjectTypeName = "单选题";
                        break;
                    case 2:
                        model.input_type = "checkbox";
                        model.SubjectTypeName = "多选题";
                        break;
                    case 3:
                        model.input_type = "radio";
                        model.SubjectTypeName = "判断题";
                        break;
                }
                model.SubjectType = 7;
                return model;
            }
            return null;
        }



        /// <summary>
        /// 获取题目详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public SubjectBigQuestionOutputView GetSubjectBigQuestionOneView(SubjectBigQuestionInput input)
        {
            if (input.Type == 1 || input.Type == 2 || input.Type == 3)
            {
                var strSql = @" SELECT a.*,b.StuAnswer,'' as Description,h.Id CollectionId, c.Code VideoId,c.VideoName,
                                CASE WHEN h.Id IS NULL THEN 0
                                ELSE 1
                                END AS IsCollection
                                FROM t_Subject_BigQuestion a 
                                LEFT JOIN t_My_AnswerRecords b ON a.Id=b.BigQuestionId  and b.UserId =@UserId AND b.PaperRecordsId = @PaperRecordsId
                                left join t_My_CollectionItem h on a.Id=h.QuestionId  and h.UserId=@UserId
                                left join t_Course_Video c on a.VideoId=c.Id
                                where  a.IsDelete=0   ";
                var dy = new DynamicParameters();
                if (!string.IsNullOrEmpty(input.BigQuestionId))
                {
                    strSql += "  and a.Id =@BigQuestionId ";
                    dy.Add(":BigQuestionId", input.BigQuestionId, DbType.String);
                }
                dy.Add(":PaperRecordsId", input.PaperRecordsId, DbType.String);
                dy.Add(":UserId", input.UserId, DbType.String);
                var model = Db.QueryFirstOrDefault<SubjectBigQuestionOutputView>(strSql, dy);
                var option = new List<Option>();
                string[] str = new string[8] { model.Option1, model.Option2, model.Option3, model.Option4, model.Option5, model.Option6, model.Option7, model.Option8 };
                string[] str1 = new string[8] { "A.", "B.", "C.", "D.", "E.", "F.", "G.", "H." };
                for (int i = 0; i < model.Number; i++)
                {
                    option.Add(new Option { id = i + 1, content = "<span style=\"float:left;\">" + str1[i] + " </span>" + str[i], ischeck = model.StuAnswer == null ? "0" : model.StuAnswer.Contains((i + 1).ToString()) ? "1" : "0" });
                }
                model.option = option;
                switch (model.SubjectType)
                {
                    case 1:
                        model.input_type = "radio";
                        model.SubjectTypeName = "单选题";
                        break;
                    case 2:
                        model.input_type = "checkbox";
                        model.SubjectTypeName = "多选题";
                        break;
                    case 3:
                        model.input_type = "radio";
                        model.SubjectTypeName = "判断题";
                        break;
                }
                model.RightAnswer =
                   model.RightAnswer.Replace('1', 'A')
                       .Replace('2', 'B')
                       .Replace('3', 'C')
                       .Replace('4', 'D')
                       .Replace('5', 'E')
                       .Replace('6', 'F')
                       .Replace('7', 'G')
                       .Replace('8', 'H');
                if (model.StuAnswer != null)
                    model.StuAnswer =
                        model.StuAnswer.Replace('1', 'A')
                            .Replace('2', 'B')
                            .Replace('3', 'C')
                            .Replace('4', 'D')
                            .Replace('5', 'E')
                            .Replace('6', 'F')
                            .Replace('7', 'G')
                            .Replace('8', 'H');
                return model;
            }
            else if (input.Type == 7)
            {
                var strSql = @" SELECT a.*,b.StuAnswer,c.QuestionContent as Description,h.Id CollectionId,e.Code VideoId,e.VideoName,
                                CASE WHEN h.Id IS NULL THEN 0
                                ELSE 1
                                END AS IsCollection
                                FROM t_Subject_Smallquestion a 
                                LEFT JOIN t_My_AnswerRecords b ON a.Id=b.SmallQuestionId  and b.UserId =@UserId  and b.PaperRecordsId =@PaperRecordsId 
                                Left join t_Subject_BigQuestion c on a.BigQuestionId=c.Id 
                                left join t_My_CollectionItem h on a.Id=h.QuestionId  and h.UserId=@UserId
                                left join t_Course_Video e on a.VideoId=e.Id
                                where  a.IsDelete=0   ";
                var dy = new DynamicParameters();
                if (!string.IsNullOrEmpty(input.BigQuestionId))
                {
                    strSql += "  and a.Id =@BigQuestionId ";
                    dy.Add(":BigQuestionId", input.BigQuestionId, DbType.String);
                }
                dy.Add(":PaperRecordsId", input.PaperRecordsId, DbType.String);
                dy.Add(":UserId", input.UserId, DbType.String);
                var model = Db.QueryFirstOrDefault<SubjectBigQuestionOutputView>(strSql, dy);
                var option = new List<Option>();
                string[] str = new string[8] { model.Option1, model.Option2, model.Option3, model.Option4, model.Option5, model.Option6, model.Option7, model.Option8 };
                string[] str1 = new string[8] { "A.", "B.", "C.", "D.", "E.", "F.", "G.", "H." };
                for (int i = 0; i < model.Number; i++)
                {
                    option.Add(new Option { id = i + 1, content = "<span style=\"float:left;\">" + str1[i] + " </span>" + str[i], ischeck = model.StuAnswer == null ? "0" : model.StuAnswer.Contains((i + 1).ToString()) ? "1" : "0" });
                }
                model.RightAnswer =
                    model.RightAnswer.Replace('1', 'A')
                        .Replace('2', 'B')
                        .Replace('3', 'C')
                        .Replace('4', 'D')
                        .Replace('5', 'E')
                        .Replace('6', 'F')
                        .Replace('7', 'G')
                        .Replace('8', 'H');
                if (model.StuAnswer != null)
                    model.StuAnswer =
                        model.StuAnswer.Replace('1', 'A')
                            .Replace('2', 'B')
                            .Replace('3', 'C')
                            .Replace('4', 'D')
                            .Replace('5', 'E')
                            .Replace('6', 'F')
                            .Replace('7', 'G')
                            .Replace('8', 'H');
                model.option = option;
                switch (model.SubjectType)
                {
                    case 1:
                        model.input_type = "radio";
                        model.SubjectTypeName = "单选题";
                        break;
                    case 2:
                        model.input_type = "checkbox";
                        model.SubjectTypeName = "多选题";
                        break;
                    case 3:
                        model.input_type = "radio";
                        model.SubjectTypeName = "判断题";
                        break;
                }
                return model;
            }
            return null;
        }



        /// <summary>
        /// 获取试卷下的试题明细
        /// </summary>
        /// <param name="paperId"></param>
        /// <returns></returns>
        public List<SubjectBigQuestionOutput> GetPaperInfoQuestions(string paperId)
        {

            var sql1 = @" 
SELECT  * ,
        ( ROW_NUMBER() OVER ( ORDER BY c.AddTime ASC ) ) AS RowsIndex
FROM    ( SELECT    b.Id ,
                    b.AddTime ,
                    '' Description ,
                    b.QuestionTitle ,
                    CONVERT(NVARCHAR(4000), b.QuestionContent) QuestionContent ,
                    CONVERT(NVARCHAR(4000), b.Option1) Option1 ,
                    CONVERT(NVARCHAR(4000), b.Option2) Option2 ,
                    CONVERT(NVARCHAR(4000), b.Option3) Option3 ,
                    CONVERT(NVARCHAR(4000), b.Option4) Option4 ,
                    CONVERT(NVARCHAR(4000), b.Option5) Option5 ,
                    CONVERT(NVARCHAR(4000), b.Option6) Option6 ,
                    CONVERT(NVARCHAR(4000), b.Option7) Option7 ,
                    CONVERT(NVARCHAR(4000), b.Option8) Option8 ,
                    b.RightAnswer StuAnswer ,
                    '' BigQuestionId ,
                    b.SubjectType
          FROM      t_Paper_Detatail a
                    LEFT JOIN t_Subject_BigQuestion b ON a.QuestionId = b.Id
                    LEFT JOIN dbo.t_Subject_Smallquestion c ON b.Id = c.BigQuestionId
          WHERE     b.SubjectType != 7
                    AND a.IsDelete = 0
                    AND b.IsDelete = 0
                    AND a.PaperId = @PaperId
          UNION
          SELECT    c.Id ,
                    b.AddTime ,
                    CONVERT(NVARCHAR(4000), b.QuestionContent) Description ,
                    c.QuestionTitle ,
                    CONVERT(NVARCHAR(4000), c.QuestionContent) QuestionContent ,
                    CONVERT(NVARCHAR(4000), c.Option1) Option1 ,
                    CONVERT(NVARCHAR(4000), c.Option2) Option2 ,
                    CONVERT(NVARCHAR(4000), c.Option3) Option3 ,
                    CONVERT(NVARCHAR(4000), c.Option4) Option4 ,
                    CONVERT(NVARCHAR(4000), c.Option5) Option5 ,
                    CONVERT(NVARCHAR(4000), c.Option6) Option6 ,
                    CONVERT(NVARCHAR(4000), c.Option7) Option7 ,
                    CONVERT(NVARCHAR(4000), c.Option8) Option8 ,
                    c.RightAnswer StuAnswer ,
                    b.Id BigQuestionId ,
                    b.SubjectType
          FROM      t_Paper_Detatail a
                    LEFT JOIN t_Subject_BigQuestion b ON a.QuestionId = b.Id
                    LEFT JOIN dbo.t_Subject_Smallquestion c ON b.Id = c.BigQuestionId
          WHERE     b.SubjectType = 7
                    AND a.IsDelete = 0
                    AND b.IsDelete = 0
                    AND c.IsDelete = 0
                    AND a.PaperId =@PaperId
        ) c
WHERE   c.Id IS NOT NULL;";
            var dy1 = new DynamicParameters();
            dy1.Add(":PaperId", paperId, DbType.String);
            var model1 = Db.QueryList<SubjectBigQuestionOutput>(sql1, dy1);
            return model1;
        }




        /// <summary>
        /// 获取试卷下的试卷结构和试题
        /// </summary>
        /// <param name="paperId"></param>
        /// <returns></returns>
        public List<PaperInfoOutput> GetPaperInfo(string paperId)
        {
            var strSql = @" SELECT MAX(c.Id) AS Id ,
        SUM(e.QuestionScoreSum) AS QuestionTypeScoreSum ,
        MAX(d.ClassName) AS ClassName ,
        MAX(c.QuestionType) QuestionType ,
        MAX(c.QuestionClass) QuestionClass ,
        MAX(c.DifficultLevel) DifficultLevel
 FROM   t_Paper_Info a                                                              --试卷表
        LEFT JOIN t_Paper_PaperParam b ON a.PaperParamId = b.Id                     --试卷参数明细表
        LEFT JOIN t_Paper_StructureDetail c ON c.StuctureId = b.StuctureId          --试卷结构明细
        LEFT JOIN t_Subject_Class d ON c.QuestionClass = d.Id
        LEFT JOIN t_Paper_ParamDetail e ON a.PaperParamId = e.PaperParamId AND e.PaperStuctureDetailId=c.Id
 WHERE  1 = 1
        AND a.Id = @PaperId
        AND a.IsDelete = 0
        AND b.IsDelete = 0
        AND c.IsDelete = 0
        AND d.IsDelete = 0
        AND e.IsDelete = 0
 GROUP BY c.QuestionClass,
        d.OrderNo
 ORDER BY d.OrderNo ASC ";
            var dy = new DynamicParameters();
            dy.Add(":PaperId", paperId, DbType.String);
            var model = Db.QueryList<PaperInfoOutput>(strSql, dy);
            var i = 0;
            foreach (var item in model)
            {
                var sql1 = @" 
SELECT  *,( ROW_NUMBER() OVER ( ORDER BY c.AddTime ASC ) ) +" + i + @" AS RowsIndex
FROM    ( SELECT    b.Id ,b.AddTime,
                    '' BigQuestionId ,
                    b.SubjectType
          FROM      t_Paper_Detatail a
                    LEFT JOIN t_Subject_BigQuestion b ON a.QuestionId = b.Id
                    LEFT JOIN dbo.t_Subject_Smallquestion c ON b.Id = c.BigQuestionId WHERE b.SubjectType!=7 AND a.IsDelete=0 AND b.IsDelete=0 
                                                         AND b.SubjectClassId = @QuestionClass
                                                         AND b.SubjectType = @QuestionType
                                                        -- AND b.DifficultLevel = @DifficultLevel
                                                         AND a.PaperId=@PaperId
          UNION
          SELECT    c.Id,b.AddTime,
                    b.Id BigQuestionId ,
                    b.SubjectType
          FROM      t_Paper_Detatail a
                    LEFT JOIN t_Subject_BigQuestion b ON a.QuestionId = b.Id
                    LEFT JOIN dbo.t_Subject_Smallquestion c ON b.Id = c.BigQuestionId WHERE b.SubjectType=7 and a.IsDelete=0 AND b.IsDelete=0  AND  c.IsDelete=0
													     AND b.SubjectClassId = @QuestionClass
                                                        -- AND b.SubjectType = @QuestionType
                                                        -- AND b.DifficultLevel = @DifficultLevel
                                                         AND a.PaperId=@PaperId
        ) c
WHERE   c.Id IS NOT NULL";
                var dy1 = new DynamicParameters();
                dy1.Add(":QuestionClass", item.QuestionClass, DbType.String);
                dy1.Add(":PaperId", paperId, DbType.String);
                dy1.Add(":QuestionType", item.QuestionType, DbType.Int32);
                //dy1.Add(":DifficultLevel", item.DifficultLevel, DbType.Int32);
                var model1 = Db.QueryList<Question>(sql1, dy1);
                i += model1.Count;
                item.Question = model1;
                if (model1.Count > 0)
                {
                    item.QuestionCount = model1.Count;
                    item.QuestionScore = ((float)item.QuestionTypeScoreSum / model1.Count).ToString("f1");
                }
            }
            return model;
        }

        /// <summary>
        /// 获取练习编号
        /// </summary>
        /// <param name="paperRecordsId"></param>
        /// <returns></returns>
        public MyPaperRecords GetPaperRecords(string paperRecordsId)
        {
            var model = _iMyPaperRecordsRepository.Get(paperRecordsId); ;
            return model;
        }


        /// <summary>
        /// 获取试卷下的试卷结构和试题
        /// </summary>
        /// <param name="paperId"></param>
        /// <param name="paperRecordsId"></param>
        /// <returns></returns>
        public List<PaperInfoOutputView> GetPaperInfoView(string paperId, string paperRecordsId)
        {
            var strSql = @"  SELECT MAX(c.Id) AS Id ,
        SUM(e.QuestionScoreSum) AS QuestionTypeScoreSum ,
        MAX(d.ClassName) AS ClassName ,
        MAX(c.QuestionType) QuestionType ,
        MAX(c.QuestionClass) QuestionClass ,
        MAX(c.DifficultLevel) DifficultLevel
 FROM   t_Paper_Info a                                                              --试卷表
        LEFT JOIN t_Paper_PaperParam b ON a.PaperParamId = b.Id                     --试卷参数明细表
        LEFT JOIN t_Paper_StructureDetail c ON c.StuctureId = b.StuctureId          --试卷结构明细
        LEFT JOIN t_Subject_Class d ON c.QuestionClass = d.Id
        LEFT JOIN t_Paper_ParamDetail e ON a.PaperParamId = e.PaperParamId AND e.PaperStuctureDetailId=c.Id
 WHERE  1 = 1
        AND a.Id = @PaperId
        AND a.IsDelete = 0
        AND b.IsDelete = 0
        AND c.IsDelete = 0
        AND d.IsDelete = 0
        AND e.IsDelete = 0
 GROUP BY c.QuestionClass,
        d.OrderNo
 ORDER BY d.OrderNo ASC";
            var dy = new DynamicParameters();
            dy.Add(":PaperId", paperId, DbType.String);
            var model = Db.QueryList<PaperInfoOutputView>(strSql, dy);
            var i = 0;
            foreach (var item in model)
            {
                var sql1 = @" 
SELECT  *,( ROW_NUMBER() OVER ( ORDER BY c.AddTime ASC ) ) +" + i + @" AS RowsIndex
FROM    ( SELECT    b.Id ,b.AddTime,
                    '' BigQuestionId ,
                    b.SubjectType,
                    CASE WHEN ISNULL(d.StuAnswer,'')<> b.RightAnswer THEN 0 ELSE 1 END IsCollection
          FROM      t_Paper_Detatail a
                    LEFT JOIN t_Subject_BigQuestion b ON a.QuestionId = b.Id
                    LEFT JOIN dbo.t_Subject_Smallquestion c ON b.Id = c.BigQuestionId
                    LEFT JOIN t_My_AnswerRecords d ON d.BigQuestionId=b.id  AND d.PaperRecordsId=@PaperRecordsId
                                                         WHERE b.SubjectType!=7 AND a.IsDelete=0 AND b.IsDelete=0 
                                                         AND b.SubjectClassId = @QuestionClass
                                                         AND b.SubjectType = @QuestionType
                                                        -- AND b.DifficultLevel = @DifficultLevel
                                                         AND a.PaperId=@PaperId
                                                        
          UNION
          SELECT    c.Id ,b.AddTime,
                    b.Id BigQuestionId ,
                    b.SubjectType,
                    CASE WHEN ISNULL(d.StuAnswer,'')<> c.RightAnswer THEN 0 ELSE 1 END ss
          FROM      t_Paper_Detatail a
                    LEFT JOIN t_Subject_BigQuestion b ON a.QuestionId = b.Id
                    LEFT JOIN dbo.t_Subject_Smallquestion c ON b.Id = c.BigQuestionId
                    LEFT JOIN t_My_AnswerRecords d ON d.SmallQuestionId=c.id   AND d.PaperRecordsId=@PaperRecordsId
                                                         Where b.SubjectType=7  AND a.IsDelete=0 AND b.IsDelete=0  AND  c.IsDelete=0
													     AND b.SubjectClassId = @QuestionClass
                                                        -- AND b.SubjectType = @QuestionType
                                                        -- AND b.DifficultLevel = @DifficultLevel
                                                         AND a.PaperId=@PaperId
                                                       
        ) c
WHERE   c.Id IS NOT NULL";
                var dy1 = new DynamicParameters();
                dy1.Add(":QuestionClass", item.QuestionClass, DbType.String);
                dy1.Add(":PaperId", paperId, DbType.String);
                dy1.Add(":QuestionType", item.QuestionType, DbType.Int32);
                //dy1.Add(":DifficultLevel", item.DifficultLevel, DbType.Int32);
                dy1.Add(":PaperRecordsId", paperRecordsId, DbType.String);
                var model1 = Db.QueryList<QuestionView>(sql1, dy1);
                i += model1.Count;
                item.Question = model1;
                if (model1.Count > 0)
                {
                    item.QuestionCount = model1.Count;
                    item.QuestionScore = ((float)item.QuestionTypeScoreSum / model1.Count).ToString("f1");
                }
            }
            return model;
        }


        /// <summary>
        /// 获取试卷下的试卷信息
        /// </summary>
        /// <param name="paperId"></param>
        /// <returns></returns>
        public PaperInfoModel GetExaminationPaper(string paperId)
        {
            var strSql = @"SELECT  e.Id ,
        e.PaperName ,
        e.TestTime ,
        d.Count ,
        ISNULL(d.COUNT1, 0) Count1 ,
        d.QuestionScore
FROM    t_Paper_Info e
        LEFT
JOIN ( SELECT   MAX(a.Id) AS Id ,
                MAX(f.COUNT) Count ,
                MAX(f.QuestionScore) QuestionScore ,
                MAX(g.COUNT) COUNT1
       FROM     t_Paper_Info a
                LEFT JOIN ( SELECT  COUNT(1) COUNT ,
                                    PaperId
                            FROM    t_My_PaperRecords
                            WHERE   Status = 1
                            GROUP BY PaperId
                          ) g ON a.Id = g.PaperId
                LEFT JOIN t_Paper_PaperParam b ON a.PaperParamId = b.Id
                LEFT JOIN t_Paper_StructureDetail c ON b.StuctureId = c.StuctureId
                LEFT JOIN ( SELECT  h.PaperId AS DetatailId ,
                                    COUNT(1) AS COUNT ,
                                    SUM(h.QuestionScore) QuestionScore
                            FROM    t_Paper_Detatail h
                            GROUP BY h.PaperId
                          ) f ON a.Id = f.DetatailId
       WHERE    a.IsDelete = 0
                AND b.IsDelete = 0
                AND c.IsDelete = 0
       GROUP BY a.Id
     ) AS d ON e.Id = d.Id
WHERE   1 = 1
        AND IsDelete = 0 and e.Id=@Id ";
            var dy = new DynamicParameters();
            dy.Add(":Id", paperId, DbType.String);
            var model = Db.QueryFirstOrDefault<PaperInfoModel>(strSql, dy);
            return model;
        }

        /// <summary>
        /// 试题作答记录保存
        /// </summary>
        /// <param name="input"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool AnswerQuestion(List<AnswerQuestionInput> input, string userId)
        {
            var answerQuestionInput = input.FirstOrDefault();
            decimal scoreCount = 0;
            if (answerQuestionInput != null)
            {
                var model = _iMyPaperRecordsRepository.Get(answerQuestionInput.PaperRecordsId);
                if (model.Status == 1)
                {
                    return false;
                }
                model.Status = 1;
                var insertStr = "";
                foreach (var item in input)
                {
                    item.answer = item.answer.TrimEnd(',');
                    decimal correctScore = 0;
                    if (item.type == (int)QuestionType.One || item.type == (int)QuestionType.Three || item.type == (int)QuestionType.Two)
                    {
                        var subjectBigQuestion = _repository.Get(item.questionId);
                        if (subjectBigQuestion != null)
                        {
                            if (item.answer == subjectBigQuestion.RightAnswer)
                            {
                                correctScore = item.Score;
                                scoreCount += item.Score;
                            }
                            else
                            {
                                if (item.answer != subjectBigQuestion.RightAnswer &&
                                    !string.IsNullOrEmpty(item.answer))
                                {
                                    _iMyErrorSubjectRepository.InsertGetId(new MyErrorSubject
                                    {
                                        AddTime = DateTime.Now,
                                        BigQuestionId = item.questionId,
                                        SmallQuestionId = "",
                                        UserId = userId,
                                        StuAnswer = item.answer,
                                        Id = Guid.NewGuid().ToString()
                                    });
                                }
                            }
                        }
                        insertStr +=
                            $@" INSERT INTO dbo.t_My_AnswerRecords
        ( Id ,
          PaperRecordsId ,
          UserId ,
          BigQuestionId ,
          SmallQuestionId ,
          PaperId ,
          StuAnswer ,
          Score ,
          AddTime
        )
VALUES  ( N'{Guid.NewGuid().ToString()}' , -- Id - nvarchar(36)
          N'{item.PaperRecordsId}' , -- PaperRecordsId - nvarchar(36)
          N'{userId}' , -- UserId - nvarchar(36)
          N'{item.questionId}' , -- BigQuestionId - nvarchar(36)
          N'{""}' , -- SmallQuestionId - nvarchar(36)
          N'{item.paperId}' , -- PaperId - nvarchar(36)
          N'{item.answer}' , -- StuAnswer - nvarchar(20)
          {correctScore} , -- Score - numeric
          '{DateTime.Now}'  -- AddTime - datetime
        ); ";
                    }
                    else if (item.type == (int)QuestionType.Seven)
                    {
                        var subjectSmallquestion = _iSubjectSmallquestionRepository.Get(item.questionId);
                        if (subjectSmallquestion != null)
                        {
                            if (item.answer == subjectSmallquestion.RightAnswer)
                            {

                                correctScore = item.Score;
                                scoreCount += item.Score;
                            }
                            else
                            {

                                if (item.answer != subjectSmallquestion.RightAnswer &&
                                    !string.IsNullOrEmpty(item.answer))
                                {
                                    _iMyErrorSubjectRepository.InsertGetId(new MyErrorSubject
                                    {
                                        AddTime = DateTime.Now,
                                        BigQuestionId = "",
                                        SmallQuestionId = item.questionId,
                                        UserId = userId,
                                        StuAnswer = item.answer,
                                        Id = Guid.NewGuid().ToString()
                                    });
                                }
                            }
                        }


                        insertStr +=
                           $@" INSERT INTO dbo.t_My_AnswerRecords
        ( Id ,
          PaperRecordsId ,
          UserId ,
          BigQuestionId ,
          SmallQuestionId ,
          PaperId ,
          StuAnswer ,
          Score ,
          AddTime
        )
VALUES  ( N'{Guid.NewGuid()}' , -- Id - nvarchar(36)
          N'{item.PaperRecordsId}' , -- PaperRecordsId - nvarchar(36)
          N'{userId}' , -- UserId - nvarchar(36)
          N'{item.questionId}' , -- BigQuestionId - nvarchar(36)
          N'{item.questionId}' , -- SmallQuestionId - nvarchar(36)
          N'{item.paperId}' , -- PaperId - nvarchar(36)
          N'{item.answer}' , -- StuAnswer - nvarchar(20)
          '{correctScore}' , -- Score - numeric
          '{DateTime.Now}'  -- AddTime - datetime
        ); ";
                    }
                }
                model.Score = scoreCount;
                if (!string.IsNullOrEmpty(insertStr))
                {
                    Db.ExecuteScalar<bool>(insertStr, null);
                }
                _iMyPaperRecordsRepository.Update(model);
            }
            return true;
        }


        /// <summary>
        /// 洗牌算法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listtemp"></param>
        public void Reshuffle<T>(List<T> listtemp)
        {
            //随机交换
            Random ram = new Random();
            int currentIndex;
            T tempValue;
            for (int i = 0; i < listtemp.Count; i++)
            {
                currentIndex = ram.Next(0, listtemp.Count - i);
                tempValue = listtemp[currentIndex];
                listtemp[currentIndex] = listtemp[listtemp.Count - 1 - i];
                listtemp[listtemp.Count - 1 - i] = tempValue;
            }
        }


        /// <summary>
        /// 新增试卷作答记录
        /// </summary>
        /// <param name="paperId">试卷编号</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public MyPaperRecordsOutput InsertIntoPaperRecords1(string paperId, string userId)
        {
            var Model = Db.QueryFirstOrDefault<PaperGroupRelation>($"select * from t_Paper_GroupRelation where PaperId='{paperId}'", null);
            var list = Db.QueryList<PaperGroupRelation>($"select a.* from t_Paper_GroupRelation a  LEFT JOIN dbo.t_Paper_Info b ON b.Id = a.PaperId where a.PaperGroupId='{Model.PaperGroupId}' and   b.State=1 AND b.IsDelete=0");
            if (list.Count > 0)
            {
                Reshuffle(list);
                var strSql = "SELECT SUM(QuestionScore) FROM t_Paper_Detatail WHERE IsDelete=0  and PaperId=@PaperId ";
                var dy = new DynamicParameters();
                dy.Add(":PaperId", paperId, DbType.String);
                var questionScore = Db.QueryFirstOrDefault<decimal?>(strSql, dy);
                var paperGroupRelation = list.FirstOrDefault();
                if (paperGroupRelation != null)
                {
                    var paperRecordsId =
                        _iMyPaperRecordsRepository.InsertGetId(new MyPaperRecords
                        {
                            AddTime = DateTime.Now,
                            Id = Guid.NewGuid().ToString(),
                            PaperId = paperGroupRelation.PaperId,
                            Score = 0,
                            Status = 0,
                            ScoreSum = questionScore,
                            PracticeNo =
                                DateTime.Now.ToString("yyyyMMddHHmm") + RandomHelper.GetRandom(3, 1)[0].ToString(),
                            UserId = userId
                        });
                    return new MyPaperRecordsOutput
                    {
                        PaperId = paperGroupRelation.PaperId,
                        PaperRecordsId = paperRecordsId,
                        PaperGroupId = paperGroupRelation.PaperGroupId
                    };
                }
            }
            return new MyPaperRecordsOutput
            {
                PaperId = "-1",
                PaperRecordsId = "",
                PaperGroupId = ""
            };
        }

        /// <summary>
        /// 新增试卷作答记录
        /// </summary>
        /// <param name="paperId">试卷组编号</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public MyPaperRecordsOutput InsertIntoPaperRecords(string paperId, string userId)
        {
            var list = Db.QueryList<PaperGroupRelation>($"select a.* from t_Paper_GroupRelation a  LEFT JOIN dbo.t_Paper_Info b ON b.Id = a.PaperId where a.PaperGroupId='{paperId}' and   b.State=1 AND b.IsDelete=0");
            if (list.Count > 0)
            {
                Reshuffle(list);
                var strSql = "SELECT SUM(QuestionScore) FROM t_Paper_Detatail WHERE IsDelete=0  and PaperId=@PaperId ";
                var dy = new DynamicParameters();
                dy.Add(":PaperId", paperId, DbType.String);
                var questionScore = Db.QueryFirstOrDefault<decimal?>(strSql, dy);
                var paperGroupRelation = list.FirstOrDefault();
                if (paperGroupRelation != null)
                {
                    var paperRecordsId = _iMyPaperRecordsRepository.InsertGetId(new MyPaperRecords
                    {
                        AddTime = DateTime.Now,
                        Id = Guid.NewGuid().ToString(),
                        PaperId = paperGroupRelation.PaperId,
                        Score = 0,
                        Status = 0,
                        ScoreSum = questionScore,
                        PracticeNo = DateTime.Now.ToString("yyyyMMddHHmm") + RandomHelper.GetRandom(3, 1)[0].ToString(),
                        UserId = userId
                    });
                    return new MyPaperRecordsOutput
                    {
                        PaperId = paperGroupRelation.PaperId,
                        PaperRecordsId = paperRecordsId,
                        PaperGroupId = paperGroupRelation.PaperGroupId
                    };
                }
            }
            return new MyPaperRecordsOutput
            {
                PaperId = "-1",
                PaperRecordsId = "",
                PaperGroupId = ""
            };
        }


        /// <summary>
        /// 获取章节下练习题目-分页
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<CourseSubjectBigQuestionModelOutput> GetChapterQuestionViewListByPage(QuestionListInput input, out int count)
        {
            var strSql = @" SELECT b.* ";
            var strFrom = @"  FROM dbo.t_Course_Subject a
                LEFT JOIN dbo.t_Subject_BigQuestion b ON a.SubjectId = b.Id
                WHERE ChapterId = @ChapterId  ";
            var strCount = "select count(*) ";
            var dy = new DynamicParameters();
            dy.Add(":ChapterId", input.ChapterId);
            count = Db.ExecuteScalar<int>(strCount + strFrom, dy);
            var list = Db.QueryList<CourseSubjectBigQuestionModelOutput>(GetPageSql(strSql + strFrom, dy, input.Page, input.Rows, "SubjectType", "Desc"), dy);
            foreach (var item in list)
            {
                var sql = " SELECT * FROM dbo.t_Subject_Smallquestion WHERE BigQuestionId=@BigQuestionId ";
                var dy2 = new DynamicParameters();
                dy2.Add("BigQuestionId", item.Id);
                var sublist = Db.QueryList<CourseSubjectSmallQuestionModelOutput>(sql, dy2);
                item.SubjectSmallquestions = sublist;
            }
            return list;
        }


        //        public List<ExaminationModel> GetErrorSubjectList( string userId ) {
        //            var examinationStr = @"SELECT  MAX(b.SubjectType) SubjectType ,
        //        CASE b.SubjectType
        //          WHEN 1 THEN '单选题'
        //          WHEN 2 THEN '多选题'
        //          WHEN 3 THEN '判断题'
        //          WHEN 7 THEN '案例分析题'
        //        END SubjectName
        //FROM    t_Course_Subject a
        //        LEFT JOIN t_Course_Video d ON a.ChapterId = d.Id
        //        LEFT JOIN dbo.t_Subject_BigQuestion b ON a.SubjectId = b.Id
        //WHERE   d.Id = @VideoId
        //GROUP BY b.SubjectType ";
        //            var dy2 = new DynamicParameters( );
        //            dy2.Add( ":VideoId", videoId, DbType.String );
        //            var model3 = Db.QueryList<ExaminationModel>( examinationStr, dy2 );

        //            var list = new List<ChapterPracticeOutput>( );
        //            var strSql = @" SELECT  a.CourseId,a.SubjectId  ,b.Id,
        //        b.QuestionTitle ,
        //        '' Description,
        //        b.QuestionContent ,
        //        b.Option1 ,
        //        b.Option2 ,
        //        b.Option3 ,
        //        b.Option4 ,
        //        b.Option5 ,
        //        b.Option6 ,
        //        b.Option7 ,
        //        b.Option8 ,
        //        b.SubjectType,
        //        b.Number,
        //        1 as Type
        //FROM    t_Course_Subject a
        //        LEFT JOIN t_Course_Video d ON a.ChapterId = d.Id
        //        LEFT JOIN dbo.t_Subject_BigQuestion b ON a.SubjectId = b.Id
        //WHERE   b.SubjectType != 7   ";
        //            var dy = new DynamicParameters( );
        //            strSql += " and d.Id=@VideoId ";
        //            dy.Add( ":VideoId", videoId, DbType.String );
        //            var model = Db.QueryList<ChapterPracticeOutput>( strSql, dy );

        //            var strSql1 = @" SELECT a.CourseId,a.SubjectId ,c.Id,
        //        b.QuestionContent Description,
        //        c.QuestionTitle ,
        //        c.QuestionContent ,
        //        c.Option1 ,
        //        c.Option2 ,
        //        c.Option3 ,
        //        c.Option4 ,
        //        c.Option5 ,
        //        c.Option6 ,
        //        c.Option7 ,
        //        c.Option8 ,
        //        c.SubjectType,
        //        c.Number,
        //        2 as Type
        // FROM   t_Course_Subject a
        //        LEFT JOIN t_Course_Video d ON a.ChapterId = d.Id
        //        LEFT JOIN dbo.t_Subject_BigQuestion b ON a.SubjectId = b.Id
        //        LEFT JOIN dbo.t_Subject_Smallquestion c ON c.BigQuestionId = b.Id
        // WHERE  b.SubjectType = 7  ";
        //            var dy1 = new DynamicParameters( );
        //            strSql1 += " and d.Id=@VideoId ";
        //            dy1.Add( ":VideoId", videoId, DbType.String );
        //            var model1 = Db.QueryList<ChapterPracticeOutput>( strSql1, dy1 );
        //            foreach ( var item in model )
        //            {
        //                var option = new List<Option>( );
        //                string[] str = new string[8] { item.Option1, item.Option2, item.Option3, item.Option4, item.Option5, item.Option6, item.Option7, item.Option8 };
        //                string[] str1 = new string[8] { "A.", "B.", "C.", "D.", "E.", "F.", "G.", "H." };
        //                for ( int i = 0; i < item.Number; i++ )
        //                {
        //                    option.Add( new Option { id = i + 1, content = "<span style=\"float:left;\">" + str1[i] + " </span>" + str[i], ischeck = "0" } );
        //                }
        //                item.option = option;
        //                switch ( item.SubjectType )
        //                {
        //                    case 1:
        //                        item.input_type = "radio";
        //                        break;
        //                    case 2:
        //                        item.input_type = "checkbox";
        //                        break;
        //                    case 3:
        //                        item.input_type = "radio";
        //                        break;
        //                }
        //                item.QuestionContent = item.QuestionContent;
        //                list.Add( item );

        //            }
        //            foreach ( var item in model1 )
        //            {
        //                var option = new List<Option>( );
        //                string[] str = new string[8] { item.Option1, item.Option2, item.Option3, item.Option4, item.Option5, item.Option6, item.Option7, item.Option8 };
        //                string[] str1 = new string[8] { "A.", "B.", "C.", "D.", "E.", "F.", "G.", "H." };
        //                for ( int i = 0; i < item.Number; i++ )
        //                {
        //                    option.Add( new Option { id = i + 1, content = "<span style=\"float:left;\">" + str1[i] + " </span>" + str[i], ischeck = "0" } );
        //                }
        //                item.option = option;
        //                switch ( item.SubjectType )
        //                {
        //                    case 1:
        //                        item.input_type = "radio";
        //                        break;
        //                    case 2:
        //                        item.input_type = "checkbox";
        //                        break;
        //                    case 3:
        //                        item.input_type = "radio";
        //                        break;
        //                }
        //                item.QuestionContent = item.QuestionContent;
        //                list.Add( item );
        //            }
        //            foreach ( var item in model3 )
        //            {
        //                item.ChapterPracticeOutput = new EditableList<ChapterPracticeOutput>( );
        //                foreach ( var item2 in list )
        //                {
        //                    if ( item.SubjectType == item2.SubjectType && item2.Type == 1 )
        //                    {
        //                        item.ChapterPracticeOutput.Add( item2 );
        //                    } else if ( item.SubjectType == 7 && item2.Type == 2 )
        //                    {
        //                        item.ChapterPracticeOutput.Add( item2 );
        //                    }
        //                }
        //            }
        //            return model3;
        //        }


        /// <summary>
        /// 获取章节下的练习题目
        /// </summary>
        /// <returns></returns>
        public List<ExaminationModel> GetChapterPracticeList(string videoId)
        {
            var examinationStr = @"SELECT  MAX(b.SubjectType) SubjectType ,
        CASE b.SubjectType
          WHEN 1 THEN '单选题'
          WHEN 2 THEN '多选题'
          WHEN 3 THEN '判断题'
          WHEN 7 THEN '案例分析题'
        END SubjectName
FROM    t_Course_Subject a
        LEFT JOIN t_Course_Video d ON a.ChapterId = d.Id
        LEFT JOIN dbo.t_Subject_BigQuestion b ON a.SubjectId = b.Id
WHERE   d.Id = @VideoId
GROUP BY b.SubjectType order by b.SubjectType asc ";
            var dy2 = new DynamicParameters();
            dy2.Add(":VideoId", videoId, DbType.String);
            var model3 = Db.QueryList<ExaminationModel>(examinationStr, dy2);

            var list = new List<ChapterPracticeOutput>();
            var strSql = @" SELECT  a.CourseId,a.SubjectId  ,b.Id,
        b.QuestionTitle ,
        '' Description,
        b.QuestionContent ,
        b.Option1 ,
        b.Option2 ,
        b.Option3 ,
        b.Option4 ,
        b.Option5 ,
        b.Option6 ,
        b.Option7 ,
        b.Option8 ,
        b.SubjectType,
        b.Number,
        1 as Type
FROM    t_Course_Subject a
        LEFT JOIN t_Course_Video d ON a.ChapterId = d.Id
        LEFT JOIN dbo.t_Subject_BigQuestion b ON a.SubjectId = b.Id
WHERE   b.SubjectType != 7 
        AND d.IsDelete=0
        AND b.IsDelete=0";
            var dy = new DynamicParameters();
            strSql += " and d.Id=@VideoId ";
            dy.Add(":VideoId", videoId, DbType.String);
            var model = Db.QueryList<ChapterPracticeOutput>(strSql + " ORDER BY b.Id  desc", dy);

            var strSql1 = @" SELECT a.CourseId,a.SubjectId ,c.Id,
        b.QuestionContent Description,
        c.QuestionTitle ,
        c.QuestionContent ,
        c.Option1 ,
        c.Option2 ,
        c.Option3 ,
        c.Option4 ,
        c.Option5 ,
        c.Option6 ,
        c.Option7 ,
        c.Option8 ,
        c.SubjectType,
        c.Number,
        2 as Type
 FROM   t_Course_Subject a
        LEFT JOIN t_Course_Video d ON a.ChapterId = d.Id
        LEFT JOIN dbo.t_Subject_BigQuestion b ON a.SubjectId = b.Id
        LEFT JOIN dbo.t_Subject_Smallquestion c ON c.BigQuestionId = b.Id
 WHERE  b.SubjectType = 7
        AND d.IsDelete=0
        AND b.IsDelete=0
        AND c.IsDelete=0";
            var dy1 = new DynamicParameters();
            strSql1 += " and d.Id=@VideoId ";
            dy1.Add(":VideoId", videoId, DbType.String);
            var model1 = Db.QueryList<ChapterPracticeOutput>(strSql1 + " ORDER BY b.Id  desc", dy1);
            foreach (var item in model)
            {
                var option = new List<Option>();
                string[] str = new string[8] { item.Option1, item.Option2, item.Option3, item.Option4, item.Option5, item.Option6, item.Option7, item.Option8 };
                string[] str1 = new string[8] { "A.", "B.", "C.", "D.", "E.", "F.", "G.", "H." };
                for (int i = 0; i < item.Number; i++)
                {
                    option.Add(new Option { id = i + 1, content = "<span style=\"float:left;\">" + str1[i] + " </span>" + str[i], ischeck = "0" });
                }
                item.option = option;
                switch (item.SubjectType)
                {
                    case 1:
                        item.input_type = "radio";
                        break;
                    case 2:
                        item.input_type = "checkbox";
                        break;
                    case 3:
                        item.input_type = "radio";
                        break;
                }
                item.QuestionContent = item.QuestionContent;
                list.Add(item);

            }
            foreach (var item in model1)
            {
                var option = new List<Option>();
                string[] str = new string[8] { item.Option1, item.Option2, item.Option3, item.Option4, item.Option5, item.Option6, item.Option7, item.Option8 };
                string[] str1 = new string[8] { "A.", "B.", "C.", "D.", "E.", "F.", "G.", "H." };
                for (int i = 0; i < item.Number; i++)
                {
                    option.Add(new Option { id = i + 1, content = "<span style=\"float:left;\">" + str1[i] + " </span>" + str[i], ischeck = "0" });
                }
                item.option = option;
                switch (item.SubjectType)
                {
                    case 1:
                        item.input_type = "radio";
                        break;
                    case 2:
                        item.input_type = "checkbox";
                        break;
                    case 3:
                        item.input_type = "radio";
                        break;
                }
                item.QuestionContent = item.QuestionContent;
                list.Add(item);
            }
            foreach (var item in model3)
            {
                item.ChapterPracticeOutput = new EditableList<ChapterPracticeOutput>();
                foreach (var item2 in list)
                {
                    if (item.SubjectType == item2.SubjectType && item2.Type == 1)
                    {
                        item.ChapterPracticeOutput.Add(item2);
                    }
                    else if (item.SubjectType == 7 && item2.Type == 2)
                    {
                        item.ChapterPracticeOutput.Add(item2);
                    }
                }
            }
            return model3;
        }

        /// <summary>
        /// 通过练习编号获取所有练习试题
        /// </summary>
        /// <param name="chapterQuestionsId"></param>
        /// <returns></returns>
        public List<ChapterPracticeViewListModel> GetChapterPracticeViewList(string chapterQuestionsId)
        {
            var courseChapterQuestions = _iCourseChapterQuestionsRepository.Get(chapterQuestionsId);
            var strSql = @" SELECT *
 FROM   ( SELECT    b.Id ,
                    d.ChapterId ChapterId,
                    b.AddTime
          FROM      t_Course_Subject a
                    LEFT JOIN v_course_Chapter_View d ON a.ChapterId = d.ChapterId
                    LEFT JOIN dbo.t_Subject_BigQuestion b ON a.SubjectId = b.Id
          WHERE     b.SubjectType != 7
          UNION
          SELECT    c.Id ,
                    d.ChapterId ChapterId,
                    b.AddTime
          FROM      t_Course_Subject a
                    LEFT JOIN v_course_Chapter_View d ON a.ChapterId = d.ChapterId
                    LEFT JOIN dbo.t_Subject_BigQuestion b ON a.SubjectId = b.Id
                    LEFT JOIN dbo.t_Subject_Smallquestion c ON c.BigQuestionId = b.Id
          WHERE     b.SubjectType = 7
        ) a
 WHERE  1 = 1  ";
            var dy = new DynamicParameters();
            strSql += " and a.ChapterId=@ChapterId ";
            dy.Add(":ChapterId", courseChapterQuestions.ChapterId, DbType.String);
            var model = Db.QueryList<ChapterPracticeViewListModel>(strSql + " ORDER BY a.AddTime asc", dy);
            return model;
        }

        /// <summary>
        /// 课程章节试题练习主表  生成记录  返回练习信息
        /// </summary>
        /// <param name="chapterId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ChapterQuestionsModel GetChapterQuestions(string chapterId, string userId)
        {

            var strSql = @"
SELECT  b.VideoName as ChapterName,
        c.CourseName
FROM    t_Course_Subject a
        LEFT JOIN dbo.t_Course_Video b ON a.ChapterId = b.Id
        LEFT JOIN t_Course_Info c ON a.CourseId = c.Id where 1=1   ";
            var dy = new DynamicParameters();
            strSql += " and  b.Id=@ChapterId";
            dy.Add(":ChapterId", chapterId, DbType.String);
            var model = Db.QueryFirstOrDefault<ChapterQuestionsModel>(strSql, dy);

            var strSql1 = @"SELECT COUNT(1)
     FROM   ( SELECT    d.Id VideoId ,
                        b.Id
              FROM      t_Course_Subject a
                        LEFT JOIN dbo.t_Course_Video d ON a.ChapterId = d.Id
                        LEFT JOIN dbo.t_Subject_BigQuestion b ON a.SubjectId = b.Id
              WHERE     b.SubjectType != 7
                        AND b.IsDelete = 0
              UNION
              SELECT     d.Id VideoId ,
                        c.Id
              FROM      t_Course_Subject a
                        LEFT JOIN t_Course_Video d ON a.ChapterId = d.Id
                        LEFT JOIN dbo.t_Subject_BigQuestion b ON a.SubjectId = b.Id
                        LEFT JOIN dbo.t_Subject_Smallquestion c ON b.Id = c.BigQuestionId
              WHERE     b.SubjectType = 7
                        AND b.IsDelete = 0
                        AND c.IsDelete = 0
            ) c where 1=1 ";
            var dy1 = new DynamicParameters();
            strSql1 += " and  c.VideoId=@VideoId";
            dy1.Add(":VideoId", chapterId, DbType.String);
            var count = Db.ExecuteScalar<int>(strSql1, dy1);
            model.DateTime = DateTime.Now;
            model.Count = count;
            model.ChapterTiele = model.DateTime.Year + "年" + model.DateTime.Month + "月" + model.DateTime.Day + "日" +
                                 model.ChapterName + "练习";
            // model.ChapterQuestionsId = InsertChapterQuestions(chapterId, userId);
            return model;
        }

        /// <summary>
        /// 通过练习编号获取练习相关的课程信息
        /// </summary>
        /// <returns></returns>
        public ChapterQuestionsModel GetChapterQuestions(string chapterQuestionsId)
        {
            var courseChapterQuestions = _iCourseChapterQuestionsRepository.Get(chapterQuestionsId);
            var strSql = @"
SELECT  b.VideoName as ChapterName,
        c.CourseName
FROM    t_Course_Subject a
        LEFT JOIN dbo.t_Course_Video b ON a.ChapterId = b.Id
        LEFT JOIN t_Course_Info c ON a.CourseId = c.Id where 1=1  ";
            var dy = new DynamicParameters();
            strSql += " and  b.Id=@VideoId";
            dy.Add(":VideoId", courseChapterQuestions.ChapterId, DbType.String);
            var model = Db.QueryFirstOrDefault<ChapterQuestionsModel>(strSql, dy);

            var strSql1 = @"SELECT COUNT(1)
     FROM   ( SELECT    d.Id VideoId ,
                        b.Id
              FROM      t_Course_Subject a
                        LEFT JOIN dbo.t_Course_Video d ON a.ChapterId = d.Id
                        LEFT JOIN dbo.t_Subject_BigQuestion b ON a.SubjectId = b.Id
              WHERE     b.SubjectType != 7
                        AND b.IsDelete = 0
              UNION
              SELECT     d.Id VideoId ,
                        c.Id
              FROM      t_Course_Subject a
                        LEFT JOIN t_Course_Video d ON a.ChapterId = d.Id
                        LEFT JOIN dbo.t_Subject_BigQuestion b ON a.SubjectId = b.Id
                        LEFT JOIN dbo.t_Subject_Smallquestion c ON b.Id = c.BigQuestionId
              WHERE     b.SubjectType = 7
                        AND b.IsDelete = 0
                        AND c.IsDelete = 0
            ) c where 1=1  ";
            var dy1 = new DynamicParameters();
            strSql1 += " and  c.VideoId=@VideoId";
            dy1.Add(":VideoId", courseChapterQuestions.ChapterId, DbType.String);
            var count = Db.ExecuteScalar<int>(strSql1, dy1);
            model.DateTime = courseChapterQuestions.AddTime;
            model.Count = count;
            model.ChapterId = courseChapterQuestions.ChapterId;
            model.PracticeNo = courseChapterQuestions.PracticeNo;
            model.ChapterTiele = model.DateTime.Year + "年" + model.DateTime.Month + "月" + model.DateTime.Day + "日" +
                                 model.ChapterName + "练习";
            return model;
        }

        /// <summary>
        /// 课程章节试题练习主表  插入记录
        /// </summary>
        /// <returns></returns>
        public ChapterModel InsertChapterQuestions(string chapterId, string userId)
        {
            var model = new CourseChapterQuestions
            {
                AddTime = DateTime.Now,
                ChapterId = chapterId,
                Id = Guid.NewGuid().ToString(),
                Status = 0,
                UserId = userId,
                PracticeNo = DateTime.Now.ToString("yyyyMMddHHmm") + RandomHelper.GetRandom(3, 1)[0].ToString()
            };
            _iCourseChapterQuestionsRepository.InsertGetId(model);
            return new ChapterModel { Id = model.Id, PracticeNo = model.PracticeNo };
        }
        /// <summary>
        /// 插入  课程章节试题练习(明细)表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool InsertChapterQuestionsDetail(CourseChapterQuestionsDetailInput input)
        {
            var courseChapterQuestions = _iCourseChapterQuestionsRepository.Get(input.ChapterQuestionsId);
            if (courseChapterQuestions.Status == 0)
            {
                courseChapterQuestions.Status = 1;
                _iCourseChapterQuestionsRepository.Update(courseChapterQuestions);
                var insertStr = "";
                foreach (var item in input.CourseChapterQuestionsDetail)
                {
                    var isCorrect = 1;
                    item.StuAnswer = item.StuAnswer.TrimEnd(',');
                    if (item.Type == 1)
                    {
                        var subjectBigQuestion = _repository.Get(item.QuestionId);
                        if (subjectBigQuestion != null)
                        {
                            if (string.IsNullOrEmpty(item.StuAnswer))
                            {
                                isCorrect = 0;
                            }
                            if (item.StuAnswer != subjectBigQuestion.RightAnswer && !string.IsNullOrEmpty(item.StuAnswer))
                            {
                                isCorrect = 0;
                                _iMyErrorSubjectRepository.InsertGetId(new MyErrorSubject
                                {
                                    AddTime = DateTime.Now,
                                    BigQuestionId = item.QuestionId,
                                    SmallQuestionId = "",
                                    UserId = input.UserId,
                                    StuAnswer = item.StuAnswer,
                                    Id = Guid.NewGuid().ToString()
                                });
                            }

                            insertStr +=
                                $@"INSERT INTO dbo.t_Course_ChapterQuestionsDetail
        ( Id ,
          ChapterQuestionsId ,
          UserId ,
          BigQuestionId ,
          SmallQuestionId ,
          StuAnswer ,
          AddTime,
          IsCorrect
        )
VALUES  ( N'{Guid.NewGuid()}' , -- Id - nvarchar(36)
          N'{input.ChapterQuestionsId}' , -- ChapterQuestionsId - nvarchar(36)
          N'{input.UserId}' , -- UserId - nvarchar(36)
          N'{item.QuestionId}' , -- BigQuestionId - nvarchar(36)
          N'{""}' , -- SmallQuestionId - nvarchar(36)
          N'{item.StuAnswer}' , -- StuAnswer - nvarchar(20)
         '{DateTime.Now}',  -- AddTime - datetime
          {isCorrect}
        ) ;";
                        }
                    }
                    else if (item.Type == 2)
                    {
                        var subjectSmallquestion = _iSubjectSmallquestionRepository.Get(item.QuestionId);
                        if (subjectSmallquestion != null)
                        {
                            if (string.IsNullOrEmpty(item.StuAnswer))
                            {
                                isCorrect = 0;
                            }
                            if (item.StuAnswer != subjectSmallquestion.RightAnswer && !string.IsNullOrEmpty(item.StuAnswer))
                            {
                                isCorrect = 0;
                                _iMyErrorSubjectRepository.InsertGetId(new MyErrorSubject
                                {
                                    AddTime = DateTime.Now,
                                    BigQuestionId = "",
                                    SmallQuestionId = item.QuestionId,
                                    UserId = input.UserId,
                                    StuAnswer = item.StuAnswer,
                                    Id = Guid.NewGuid().ToString()
                                });
                            }
                            insertStr +=
                              $@"INSERT INTO dbo.t_Course_ChapterQuestionsDetail
        ( Id ,
          ChapterQuestionsId ,
          UserId ,
          BigQuestionId ,
          SmallQuestionId ,
          StuAnswer ,
          AddTime,
          IsCorrect
        )
VALUES  ( N'{Guid.NewGuid()}' , -- Id - nvarchar(36)
          N'{input.ChapterQuestionsId}' , -- ChapterQuestionsId - nvarchar(36)
          N'{input.UserId}' , -- UserId - nvarchar(36)
          N'{""}' , -- BigQuestionId - nvarchar(36)
          N'{item.QuestionId}' , -- SmallQuestionId - nvarchar(36)
          N'{item.StuAnswer}' , -- StuAnswer - nvarchar(20)
         '{DateTime.Now}',  -- AddTime - datetime
          {isCorrect}
        ) ;";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(insertStr))
                {
                    Db.ExecuteScalar<bool>(insertStr, null);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 根据章节试题练习编号获取练习结果
        /// </summary>
        /// <param name="chapterQuestionsId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ChapterQuestionsDetailModel GetChapterQuestionsResult(string chapterQuestionsId, string userId)
        {

            var strSql = @" SELECT * ,h.Id CollectionId,g.VideoName,c.VideoId,g.Code,c.KnowledgePointName,
            CASE WHEN h.Id IS NULL THEN 0
             ELSE 1
            END AS IsCollection ,
            CASE c.SubjectType
              WHEN 1 THEN '单选题'
              WHEN 2 THEN '多选题'
              WHEN 3 THEN '判断题'
            END AS SubjectName
     FROM   ( SELECT    a.* ,
                        '' Description ,
                        b.Id QuestionId,
                        CONVERT(NVARCHAR(4000), b.QuestionTextAnalysis) QuestionTextAnalysis ,
                        CONVERT(NVARCHAR(4000), b.QuestionContent) QuestionContent ,
                        b.SubjectType ,
                        b.RightAnswer ,
                        CONVERT(NVARCHAR(4000), b.Option1) Option1 ,
                        CONVERT(NVARCHAR(4000), b.Option2) Option2 ,
                        CONVERT(NVARCHAR(4000), b.Option3) Option3 ,
                        CONVERT(NVARCHAR(4000), b.Option4) Option4 ,
                        CONVERT(NVARCHAR(4000), b.Option5) Option5 ,
                        CONVERT(NVARCHAR(4000), b.Option6) Option6 ,
                        CONVERT(NVARCHAR(4000), b.Option7) Option7 ,
                        CONVERT(NVARCHAR(4000), b.Option8) Option8,
                        b.Number,
                        b.VideoId,
                        c.KnowledgePointName
              FROM      t_Course_ChapterQuestionsDetail a
                        LEFT JOIN t_Subject_BigQuestion b ON a.BigQuestionId = b.Id
                        LEFT JOIN dbo.t_Subject_KnowledgePoint c ON b.KnowledgePointId=c.Id
              WHERE     LEN(a.BigQuestionId)>0
              UNION
              SELECT    a.* ,
                        CONVERT(NVARCHAR(4000), e.QuestionContent) Description ,
                        b.Id QuestionId,
                        CONVERT(NVARCHAR(4000), b.QuestionTextAnalysis) QuestionTextAnalysis ,
                        CONVERT(NVARCHAR(4000), b.QuestionContent) QuestionContent ,
                        b.SubjectType ,
                        b.RightAnswer ,
                        CONVERT(NVARCHAR(4000), b.Option1) Option1 ,
                        CONVERT(NVARCHAR(4000), b.Option2) Option2 ,
                        CONVERT(NVARCHAR(4000), b.Option3) Option3 ,
                        CONVERT(NVARCHAR(4000), b.Option4) Option4 ,
                        CONVERT(NVARCHAR(4000), b.Option5) Option5 ,
                        CONVERT(NVARCHAR(4000), b.Option6) Option6 ,
                        CONVERT(NVARCHAR(4000), b.Option7) Option7 ,
                        CONVERT(NVARCHAR(4000), b.Option8) Option8,
                        b.Number,
                        b.VideoId,
                        c.KnowledgePointName
              FROM      t_Course_ChapterQuestionsDetail a
                        LEFT JOIN t_Subject_Smallquestion b ON a.SmallQuestionId = b.Id
                        LEFT JOIN t_Subject_BigQuestion e ON b.BigQuestionId = e.Id
                        LEFT JOIN dbo.t_Subject_KnowledgePoint c ON e.KnowledgePointId=c.Id
              WHERE     LEN(a.SmallQuestionId)>0
            ) c left join t_My_CollectionItem h on c.QuestionId=h.QuestionId  and h.UserId=@UserId
                LEFT JOIN dbo.t_Course_Video g ON c.VideoId=g.Id
                where 1=1 ";
            var dy = new DynamicParameters();
            strSql += " and c.ChapterQuestionsId=@ChapterQuestionsId ";
            dy.Add(":ChapterQuestionsId", chapterQuestionsId, DbType.String);
            dy.Add(":UserId", userId, DbType.String);
            var model = Db.QueryList<ChapterQuestionsDetailOutput>(strSql + " ORDER BY c.SubjectType asc,c.BigQuestionId  desc,c.Description asc", dy);
            foreach (var item in model)
            {
                var option = new List<Option>();
                string[] str = new string[8] { item.Option1, item.Option2, item.Option3, item.Option4, item.Option5, item.Option6, item.Option7, item.Option8 };
                string[] str1 = new string[8] { "A.", "B.", "C.", "D.", "E.", "F.", "G.", "H." };
                for (int i = 0; i < item.Number; i++)
                {
                    option.Add(new Option { id = i + 1, content = "<span style=\"float:left;\">" + str1[i] + " </span>" + str[i], ischeck = "0" });
                }
                item.Whether = item.RightAnswer == item.StuAnswer ? "<a>正确</a>" : "<i>错误</i>";
                item.IsWhether = item.RightAnswer == item.StuAnswer ? 1 : 0;
                item.RightAnswer =
                    item.RightAnswer.Replace('1', 'A')
                        .Replace('2', 'B')
                        .Replace('3', 'C')
                        .Replace('4', 'D')
                        .Replace('5', 'E')
                        .Replace('6', 'F')
                        .Replace('7', 'G')
                        .Replace('8', 'H');
                item.StuAnswer =
                    item.StuAnswer.Replace('1', 'A')
                        .Replace('2', 'B')
                        .Replace('3', 'C')
                        .Replace('4', 'D')
                        .Replace('5', 'E')
                        .Replace('6', 'F')
                        .Replace('7', 'G')
                        .Replace('8', 'H');
                item.option = option;
                item.QuestionContent = item.QuestionContent;
            }
            var chapterQuestions = GetChapterQuestions(chapterQuestionsId);
            var chapterQuestionsDetailModel = new ChapterQuestionsDetailModel
            {
                ChapterQuestionsDetailOutput = model,
                ChapterName = chapterQuestions.ChapterName,
                ChapterQuestionsId = chapterQuestions.ChapterQuestionsId,
                ChapterTiele = chapterQuestions.ChapterTiele,
                Count = chapterQuestions.Count,
                DateTime = chapterQuestions.DateTime,
                CourseName = chapterQuestions.CourseName,
                PracticeNo = chapterQuestions.PracticeNo,
                ChapterId = chapterQuestions.ChapterId
            };
            return chapterQuestionsDetailModel;
        }


        /// <summary>
        /// 判断课程是否购买
        /// </summary>
        /// <param name="courseId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool IsBuy(string courseId, string userId)
        {
            var strSql = @"SELECT  COUNT(1)
FROM    t_My_Course a
        LEFT JOIN dbo.t_Course_Info c ON a.CourseId = c.Id
WHERE   a.UserId = @RegisterUserId
        AND c.Id = @CourseId  
        AND a.BeginTime<=@DataTime AND a.EndTime>=@DataTime";
            var dy = new DynamicParameters();
            dy.Add(":RegisterUserId", userId, DbType.String);
            dy.Add(":CourseId", courseId, DbType.String);
            dy.Add(":DataTime", DateTime.Now, DbType.DateTime);
            return Db.QueryFirstOrDefault<int>(strSql, dy) > 0;
        }
    }
}