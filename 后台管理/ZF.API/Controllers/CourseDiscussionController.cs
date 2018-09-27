using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.WebApiAppService.CourseDiscussionModule;
using ZF.Application.WebApiDto.CoursediscussionModule;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CourseDiscussionController : ApiController
    {
        public static CourseDiscussionAppService _courseDiscussionAppService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseDiscussionAppService"></param>
        public CourseDiscussionController( CourseDiscussionAppService courseDiscussionAppService ) {
            _courseDiscussionAppService = courseDiscussionAppService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public List<CourseDiscussionModelOutput> GetList( CourseDiscussionModelInput input ) {
            var count = 0;
            var list = _courseDiscussionAppService.GetList(input,out count );
            return list;
        }
    }
}
