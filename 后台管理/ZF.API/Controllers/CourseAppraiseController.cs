using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.CourseSecurityCodeModule;
using ZF.Application.WebApiDto.CourseAppraiseModule;
using ZF.Application.WebApiDto.CourseChapterModule;
using ZF.Core.Entity;
using ZF.Infrastructure.IP;
using ZF.Infrastructure.RedisCache;
using CourseAppraiseAppService = ZF.Application.WebApiAppService.CourseAppraiseModule.CourseAppraiseAppService;
using CourseChapterAppService = ZF.Application.WebApiAppService.CourseChapterModule.CourseChapterAppService;
using CourseSecurityCodeAppService = ZF.Application.AppService.CourseSecurityCodeAppService;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CourseAppraiseController : BaseApiController
    {
        private readonly CourseAppraiseAppService _courseAppraiseAppService;

        private readonly CourseChapterAppService _courseChapterAppService;

        private readonly CourseSecurityCodeAppService _courseSecurityCodeAppService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseAppraiseAppService"></param>
        /// <param name="courseChapterAppService"></param>
        public CourseAppraiseController(CourseAppraiseAppService courseAppraiseAppService, CourseChapterAppService courseChapterAppService, CourseSecurityCodeAppService courseSecurityCodeAppService)
        {
            _courseAppraiseAppService = courseAppraiseAppService;
            _courseChapterAppService = courseChapterAppService;
            _courseSecurityCodeAppService = courseSecurityCodeAppService;
        }

        /// <summary>
        /// 添加课程评价
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut Add(CourseAppraiseModelInput input)
        {
            input.UserId = UserObject.Id;
            input.AppraiseIp = IpUtil.GetHostAddress();
            if (_courseAppraiseAppService.IfAppraise(input))
            {
                return new MessagesOutPut { Success = true, Message = "您已经评价过该课程" };
            }
            return _courseAppraiseAppService.Add(input);
        }



        /// <summary>
        /// 是否评价过指定课程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public bool IfAppraise(CourseAppraiseModelInput input)
        {
            input.UserId = UserObject.Id;
            return _courseAppraiseAppService.IfAppraise(input);
        }
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public int GetAverageAppraise( CourseAppraiseModelInput input ) {
        //    return _courseAppraiseAppService.GetAverageAppraise( input );
        //}

        /// <summary>
        ///  通过用户编号  章节编号 试题练习记录
        /// </summary>
        /// <param name="chapterId">章节编号</param>
        /// <returns></returns>
        [HttpPost]
        public List<CourseChapterQuestions> GetCourseChapterQuestionses(string chapterId)
        {
            return _courseChapterAppService.GetCourseChapterQuestionses(chapterId, UserObject.Id);
        }


        /// <summary>
        ///  通过用户编号  试卷编号 获取试卷练习记录
        /// </summary>
        /// <param name="paperId"></param>
        /// <returns></returns>
        [HttpPost]
        public List<MyPaperRecordsOutput> GetMyPaperRecords(string paperId)
        {
            return _courseChapterAppService.GetMyPaperRecords(paperId, UserObject.Id);
        }


        /// <summary>
        ///  通过试卷练习编号获取练习结果
        /// </summary>
        /// <param name="paperRecordsId"></param>
        /// <returns></returns>
        [HttpPost]
        public MyPaperRecordsJgOutput MyPaperRecordsJg(string paperRecordsId)
        {
            return _courseChapterAppService.MyPaperRecordsJg(paperRecordsId);
        }


        /// <summary>
        /// 获取课程章节列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public List<CourseChapterModelOutput> GetCourseChapterList(CourseChapterModelInput input)
        {
            var model = _courseChapterAppService.GetCourseChapterList(input, UserObject.Id);
            return model;
        }

        /// <summary>
        /// 获取课程章节列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public List<CourseChapterModelOutput> GetCourseChapterList1(CourseChapterModelInput input)
        {
            //if (RedisCacheHelper.Exists("GetCourseChapterList1_" + input.CourseId))
            //{
            //    return
            //        RedisCacheHelper.Get<List<CourseChapterModelOutput>>("GetCourseChapterList1_" + input.CourseId);
            //}
            //var model = _courseChapterAppService.GetCourseChapterList(input);
            //RedisCacheHelper.Add("GetCourseChapterList1_" + input.CourseId, model);
            return _courseChapterAppService.GetCourseChapterList(input);
        }


        /// <summary>
        /// 扫码防伪码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="isValueAdded"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut CheckCourseCode(string code, bool? isValueAdded)
        {
            return _courseSecurityCodeAppService.CheckCourseCode(code, UserObject.Id, isValueAdded);
        }


        /// <summary>
        /// 扫码防伪码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut CheckCourseCode(string code)
        {
            return _courseSecurityCodeAppService.CheckCourseCode(code, UserObject.Id, null);
        }

        /// <summary>
        /// 投诉
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [HttpPost]
        public MessagesOutPut Complaints(ComplaintsInput input)
        {
            return _courseSecurityCodeAppService.Complaints(input);
        }
    }
}
