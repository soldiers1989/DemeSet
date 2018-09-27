using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.WebApiAppService.CourseResourceModule;
using ZF.Application.WebApiDto.CourseResourceModule;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CourseResourceController : ApiController
    {
        private readonly CourseResourceAppService _courseResourceAppService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="courseResourceAppService"></param>
        public CourseResourceController( CourseResourceAppService courseResourceAppService )
        {
            _courseResourceAppService = courseResourceAppService;
        }


        /// <summary>
        /// 获取课程资源列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public List<CourseResourceModelOutput> GetList( CourseResourceModelInput input ) {
            return _courseResourceAppService.GetList( input);
        }
    }
}
