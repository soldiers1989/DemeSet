using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.CourseAppraiseModule;
using ZF.Application.WebApiAppService.CourseChapterModule;
using ZF.Application.WebApiDto.CourseAppraiseModule;
using ZF.Application.WebApiDto.CourseChapterModule;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CourseChapterController : ApiController
    {

        private readonly CourseChapterAppService _courseChapterAppService;

        private readonly CourseAppraiseAppService _courseAppraiseAppService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="courseChapterAppService"></param>
        /// <param name="courseAppraiseAppService"></param>
        public CourseChapterController( CourseChapterAppService courseChapterAppService, CourseAppraiseAppService courseAppraiseAppService)
        {
            _courseChapterAppService = courseChapterAppService;
            _courseAppraiseAppService = courseAppraiseAppService;
        }

        /// <summary>
        /// 获取课程章节列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public List<CourseChapterModelOutput> GetCourseChapterList( CourseChapterModelInput input )
        {
            return _courseChapterAppService.GetCourseChapterList( input);
        }



        /// <summary>
        /// 是否开启评价功能
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool IsPj()
        {
            return _courseAppraiseAppService.IsPj();
        }

        /// <summary>
        /// 获取评价列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<CourseAppraiseModelOutput> GetList(CourseAppraiseModelInput input)
        {
            var count = 0;
            var list = _courseAppraiseAppService.GetList(input, out count);
            return new JqGridOutPut<CourseAppraiseModelOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 获取章节信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public CourseChapterModelOutput GetModel( CourseChapterModelInput input) {
            return _courseChapterAppService.GetModel( input);
        }



        /// <summary>
        /// 获取章节信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public CourseChapterOutput GetCourseChapter(IdInput input)
        {
            return _courseChapterAppService.GetCourseChapter(input);
        }
    }
}