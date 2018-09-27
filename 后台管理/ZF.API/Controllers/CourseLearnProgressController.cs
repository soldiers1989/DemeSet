using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.CourseLearnProgressModule;
using ZF.Application.WebApiDto.CourseLearnProgressModule;

namespace ZF.API.Controllers
{

    public class CourseLearnProgressController : BaseApiController
    {
        private readonly CourseLearnProgressAppService _courseLearnProgressAppService;

        public CourseLearnProgressController( CourseLearnProgressAppService courseLearnProgressAppService )
        {
            _courseLearnProgressAppService = courseLearnProgressAppService;
        }

        [HttpPost]
        public MessagesOutPut Add( CourseLearnProgressModelInput input )
        {
            input.UserId = UserObject.Id;
            return _courseLearnProgressAppService.Add( input );
        }


        [HttpPost]
        public bool UdpateState( CourseLearnProgressModelInput input )
        {
            input.UserId = UserObject.Id;
            return _courseLearnProgressAppService.UpdateState( input );
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public bool IfExist( CourseLearnProgressModelInput input )
        {
            input.UserId = UserObject.Id;
            return _courseLearnProgressAppService.IfExist(input);
        }

        /// <summary>
        /// 获取个人学习进度
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public CourseLearnProgressModelOutput GetProgress( CourseLearnProgressModelInput input ) {
            input.UserId = UserObject.Id;
            return _courseLearnProgressAppService.GetProgress( input);
        }

        /// <summary>
        /// 获取视频学习状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public string GetVideoLearnState( CourseLearnProgressModelInput input ) {
            input.UserId = UserObject.Id;
            return _courseLearnProgressAppService.GetVideoLearnState( input);
        }
    }
}
