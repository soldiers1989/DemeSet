using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.CoursePackModule;
using ZF.Application.WebApiDto.CourseModule;
using ZF.Application.WebApiDto.CoursePackModule;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CoursePackController : ApiController
    {
        private readonly CoursePackAppService _coursePackAppService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coursePackAppService"></param>
        public CoursePackController( CoursePackAppService coursePackAppService )
        {
            _coursePackAppService = coursePackAppService;
        }



        /// <summary>
        /// 获取套餐课程列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<CoursePackModelOutput> GetSearchList( CoursePackModelInput input )
        {
            var count = 0;
            var list = _coursePackAppService.GetList( input, out count );
            return new JqGridOutPut<CoursePackModelOutput>
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }


        /// <summary>
        /// 获取套餐课程详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public CoursePackModelOutput GetCoursePackDetail( CoursePackModelInput input ) {
            return _coursePackAppService.GetCoursePackDetail( input);
        }



        /// <summary>
        /// 获取面授课详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public FaceToFaceModelOupput GetFaceToFace(IdInput input)
        {
            return _coursePackAppService.GetFaceToFace(input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public bool UpdateViewCount( CoursePackModelInput input )
        {
            return _coursePackAppService.UpdateViewCount( input );
        }
    }
}
