using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.Dto;
using ZF.Application.WebApiAppService.CourseModule;
using ZF.Application.WebApiDto.CourseModule;
using ZF.Application.WebApiDto.MyCollectionModule;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class MyStudyController : BaseApiController
    {

        private readonly CourseInfoAppService _courseInfoAppService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseInfoAppService"></param>
        public MyStudyController( CourseInfoAppService courseInfoAppService )
        {
            _courseInfoAppService = courseInfoAppService;
        }

        /// <summary>
        /// 获取已购买课程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public List<CourseInfoModelOutput> GetPurchaseList( OrderSheetInput input )
        {
            input.RegisterUserId = UserObject.Id;
            return _courseInfoAppService.GetPurchaseList( input );
        }


        /// <summary>
        /// 我的课程 2018-6-4
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<MyCourseModelOutput> GetMyCourse( ) {
            return _courseInfoAppService.GetMyCourse( UserObject.Id);
        }



        /// <summary>
        /// 我的课程 2018-6-4
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<MyCourseModelOutput> MyQuestion()
        {
            return _courseInfoAppService.MyQuestion(UserObject.Id);
        }

        /// <summary>
        /// 推荐课程 2018-6-4
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<CourseInfoModel> GetRecommendCourse( CourseInfoModelInput input) {
            input.UserId = UserObject.Id;
            return _courseInfoAppService.GetRecommendCourse( input);
        }

        /// <summary>
        /// 收藏课程 2018-6-4
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<MyCourseModelOutput> GetCollectedCourse( )
        {
            return _courseInfoAppService.GetCollectedCourse( UserObject.Id );
        }

        /// <summary>
        /// 获取套餐课程学习进度
        /// </summary>
        [HttpPost]
        public CourseInfoModelOutput LoadPackProgress( string courseid ) {
            return _courseInfoAppService.LoadPackProgress( UserObject.Id,courseid);
        }


        /// <summary>
        /// 获取收藏的课程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<CourseInfoModelOutput> GetCollectedCourseList( MyCollectionModuleInput input )
        {
            input.UserId = UserObject.Id;
            return _courseInfoAppService.GetCollectedCourseList( input );
        }
    }
}
