using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.WebApiDto.CourseVideoModule;
using ZF.Core.Entity;
using ZF.Infrastructure.AlipayService;
using ZF.Infrastructure.AlipayService.Model;
using CourseVideoAppService = ZF.Application.WebApiAppService.CourseVideoModule.CourseVideoAppService;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CourseVideoController : ApiController
    {
        private readonly CourseVideoAppService _courseVideoAppService;

      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseVideoAppService"></param>
        public CourseVideoController(CourseVideoAppService courseVideoAppService)
        {
            _courseVideoAppService = courseVideoAppService;
        }
        /// <summary>
        /// 获取指定课程视频信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public CourseVideo GetOne(CourseVideoModelInput input)
        {
            return _courseVideoAppService.GetOne(input);
        }


        /// <summary>
        /// 获取视频信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public VideoModel GetVideo(IdInput input)
        {
            return Aliyun.GetVideoPlayAuth(input.Id);
        }

        [HttpPost]
        public CourseVideo GetVideoInfo( IdInput input ) {
            return _courseVideoAppService.GetVideoInfo( input);
        }
    }
}