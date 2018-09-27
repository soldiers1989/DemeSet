using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.QuestionCheckerModule;
using ZF.Application.WebApiDto.CourseSubjectModule;

namespace ZF.API.Controllers
{
    public class QuestionCheckerController : BaseApiController
    {
        private readonly QuestionCheckerAppService _service;
        public QuestionCheckerController(QuestionCheckerAppService service )
        {
            _service = service;
        }

        /// <summary>
        /// 添加纠错反馈
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut Add( QuestionCheckerInput input ) {
            input.UserId = UserObject.Id;
            return _service.Add( input);

        }
    }
}
