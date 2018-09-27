using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.AccessControl;
using System.Text;
using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.SheetModule;
using ZF.Application.WebApiAppService.SubjectModule;
using ZF.Application.WebApiDto.CourseSubjectModule;
using ZF.Application.WebApiDto.SheetDtoModule;
using ZF.Application.WebApiDto.SubjectModule;
using ZF.Core.Entity;
using ZF.Infrastructure.RedisCache;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 试题相关控制器
    /// </summary>
    public class EconomyController : BaseApiController
    {
        private SubjectBigQuestionAppService _subjectBigQuestionAppService;

        private SheetApiService _sheetApiService;

        private static string DefuleDomain = ConfigurationManager.AppSettings["DefuleDomain"];

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="subjectBigQuestionAppService"></param>
        public EconomyController(SubjectBigQuestionAppService subjectBigQuestionAppService, SheetApiService sheetApiService)
        {
            _subjectBigQuestionAppService = subjectBigQuestionAppService;
            _sheetApiService = sheetApiService;
        }

        /// <summary>
        /// 获取试题详情
        /// </summary>
        /// <param name="questionId">试题编号</param>
        /// <param name="type">试题类型</param>
        /// <param name="paperRecordsId">作答记录编码</param>
        /// <returns></returns>
        public object GetQuestion(string questionId, int type, string paperRecordsId)
        {
            if (RedisCacheHelper.Exists("GetQuestion_" + questionId))
            {
                return RedisCacheHelper.Get<SubjectBigQuestionOutput>("GetQuestion_" + questionId);
            }
            var model =
                _subjectBigQuestionAppService.GetSubjectBigQuestionOne(new SubjectBigQuestionInput
                {
                    BigQuestionId = questionId,
                    Type = type,
                    //UserId = UserObject.Id,
                    //PaperRecordsId = paperRecordsId
                });
            RedisCacheHelper.Add("GetQuestion_" + questionId, model);
            return model;
        }

        /// <summary>
        /// 获取试题详情
        /// </summary>
        /// <param name="questionId">试题编号</param>
        /// <param name="type">试题类型</param>
        /// <param name="paperRecordsId">作答记录编码</param>
        /// <returns></returns>
        public object GetQuestionView(string questionId, int type, string paperRecordsId)
        {
            var model =
               _subjectBigQuestionAppService.GetSubjectBigQuestionOneView(new SubjectBigQuestionInput
               {
                   BigQuestionId = questionId,
                   Type = type,
                   UserId = UserObject.Id,
                   PaperRecordsId = paperRecordsId
               });
            return model;
        }

        /// <summary>
        /// 根据试卷编号获取该试卷下的题目
        /// </summary>
        /// <param name="paperId"></param>
        /// <returns></returns>
        public List<PaperInfoOutput> GetPaperInfo(string paperId)
        {
            if (RedisCacheHelper.Exists("PaperInfo_" + paperId))
            {
                return RedisCacheHelper.Get<List<PaperInfoOutput>>("PaperInfo_" + paperId);
            }
            var model = _subjectBigQuestionAppService.GetPaperInfo(paperId);
            RedisCacheHelper.Add("PaperInfo_" + paperId, model);
            return model;

        }


        /// <summary>
        /// 根据试卷编号获取该试卷下的题目
        /// </summary>
        /// <param name="paperId"></param>
        /// <param name="paperRecordsId"></param>
        /// <returns></returns>
        public List<PaperInfoOutputView> GetPaperInfoView(string paperId, string paperRecordsId)
        {
            if (RedisCacheHelper.Exists("GetPaperInfoView_" + paperId + paperRecordsId))
            {
                return RedisCacheHelper.Get<List<PaperInfoOutputView>>("GetPaperInfoView_" + paperId + paperRecordsId);
            }
            var model = _subjectBigQuestionAppService.GetPaperInfoView(paperId, paperRecordsId);
            RedisCacheHelper.Add("GetPaperInfoView_" + paperId + paperRecordsId, model);
            return model;
        }


        /// <summary>
        /// 提交试卷
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        [HttpPost]
        public bool AnswerQuestion(List<AnswerQuestionInput> inputs)
        {
            //foreach (var item in inputs)
            //{
            return _subjectBigQuestionAppService.AnswerQuestion(inputs, UserObject.Id);
            //}
        }

        /// <summary>
        /// 根据试卷编号获取试卷信息
        /// </summary>
        /// <param name="paperId"></param>
        /// <param name="paperRecordsId"></param>
        /// <returns></returns>
        [HttpPost]
        public PaperInfoModel GetExaminationPaper(string paperId, string paperRecordsId)
        {
            if (RedisCacheHelper.Exists("ExaminationPaper_" + paperId))
            {
                var model1 = RedisCacheHelper.Get<PaperInfoModel>("ExaminationPaper_" + paperId);
                model1.PracticeNo = _subjectBigQuestionAppService.GetPaperRecords(paperRecordsId).PracticeNo;
                model1.Score = _subjectBigQuestionAppService.GetPaperRecords(paperRecordsId).Score;
                model1.UserName = UserObject.NickNamw;
                model1.ImgUrl = UserObject.HeadImage;
                return model1;
            }
            var model = _subjectBigQuestionAppService.GetExaminationPaper(paperId);
            model.PracticeNo = _subjectBigQuestionAppService.GetPaperRecords(paperRecordsId).PracticeNo;
            model.Score = _subjectBigQuestionAppService.GetPaperRecords(paperRecordsId).Score;
            model.UserName = UserObject.NickNamw;
            model.ImgUrl = UserObject.HeadImage;
            RedisCacheHelper.Add("ExaminationPaper_" + paperId, model);
            return model;
        }


        /// <summary>
        /// 新增试卷作答记录
        /// </summary>
        /// <param name="paperId"></param>
        /// <returns></returns>
        [HttpPost]
        public MyPaperRecordsOutput InsertIntoPaperRecords(string paperId)
        {
            return _subjectBigQuestionAppService.InsertIntoPaperRecords1(paperId, UserObject.Id);
        }

        /// <summary>
        /// 新增试卷作答记录
        /// </summary>
        /// <param name="paperId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpPost]
        public MyPaperRecordsOutput InsertIntoPaperRecords(string paperId, string courseId)
        {
            //判断是否购买该课程
            if (_sheetApiService.IfAreadyPay(new SheetDetailModelInput { CourseType = 0, CourseId = courseId, UserId = UserObject.Id }))
            {
                return _subjectBigQuestionAppService.InsertIntoPaperRecords(paperId, UserObject.Id);
            }
            return null;
        }


        /// <summary>
        /// 获取章节下的练习题目
        /// </summary>
        /// <param name="chapterId"></param>
        /// <returns></returns>

        [HttpPost]
        public object GetChapterPracticeList(string chapterId)
        {
            return _subjectBigQuestionAppService.GetChapterPracticeList(chapterId);
        }


        /// <summary>
        /// 获取章节下的练习题目  通过练习编号
        /// </summary>
        /// <param name="chapterQuestionsId"></param>
        /// <returns></returns>

        [HttpPost]
        public List<ChapterPracticeViewListModel> GetChapterPracticeViewList(string chapterQuestionsId)
        {
            return _subjectBigQuestionAppService.GetChapterPracticeViewList(chapterQuestionsId);
        }


        /// <summary>
        /// 获取章节试题-分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<CourseSubjectBigQuestionModelOutput> GetChapterQuestionViewListByPage(QuestionListInput input)
        {
            var count = 0;
            var list = _subjectBigQuestionAppService.GetChapterQuestionViewListByPage(input, out count);
            return new JqGridOutPut<CourseSubjectBigQuestionModelOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list

            };
        }

        /// <summary>
        /// 课程章节试题练习主表  生成记录  返回练习编号
        /// </summary>
        /// <param name="chapterId"></param>
        /// <returns></returns>
        [HttpPost]
        public ChapterModel InsertChapterQuestions(string chapterId)
        {
            var model = _subjectBigQuestionAppService.InsertChapterQuestions(chapterId, UserObject.Id);
            return model;
        }

        /// <summary>
        /// 课程章节试题练习主表  生成记录  返回练习编号
        /// </summary>
        /// <param name="chapterId"></param>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpPost]
        public ChapterModel InsertChapterQuestions(string chapterId, string courseId)
        {
            //判断是否购买该课程
            if (_sheetApiService.IfAreadyPay(new SheetDetailModelInput { CourseType = 0, CourseId = courseId, UserId = UserObject.Id }))
            {
                var model = _subjectBigQuestionAppService.InsertChapterQuestions(chapterId, UserObject.Id);
                return model;
            }
            return null;
        }

        /// <summary>
        /// 课程章节试题练习主表  生成记录  返回练习信息
        /// </summary>
        /// <param name="chapterId"></param>
        /// <returns></returns>

        [HttpPost]
        public ChapterQuestionsModel GetChapterQuestions(string chapterId)
        {
            var model = _subjectBigQuestionAppService.GetChapterQuestions(chapterId, UserObject.Id);
            return model; ;
        }




        /// <summary>
        /// 课程章节试题练习(明细)表  保存练习记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [HttpPost]
        public bool InsertChapterQuestionsDetail(CourseChapterQuestionsDetailInput input)
        {
            input.UserId = UserObject.Id;
            return _subjectBigQuestionAppService.InsertChapterQuestionsDetail(input);
        }

        /// <summary>
        /// 根据章节试题练习编号获取练习结果
        /// </summary>
        /// <param name="chapterQuestionsId"></param>
        /// <returns></returns>

        [HttpPost]
        public ChapterQuestionsDetailModel GetChapterQuestionsResult(string chapterQuestionsId)
        {
            //if (RedisCacheHelper.Exists("GetChapterQuestionsResult_" + chapterQuestionsId))
            //{
            //    return RedisCacheHelper.Get<ChapterQuestionsDetailModel>("GetChapterQuestionsResult_" + chapterQuestionsId);
            //}
            var model = _subjectBigQuestionAppService.GetChapterQuestionsResult(chapterQuestionsId, UserObject.Id);
            RedisCacheHelper.Add("GetChapterQuestionsResult_" + chapterQuestionsId, model);
            return model;
        }
    }
}
