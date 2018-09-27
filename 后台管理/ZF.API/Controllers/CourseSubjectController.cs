using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Application.WebApiAppService.CourseSubjectModule;
using ZF.Application.WebApiDto.CourseSubjectModule;
using ZF.Application.WebApiDto.MyCollectionItemModule;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CourseSubjectController : BaseApiController
    {
        private readonly CourseSubjectAppService _courseSubjectAppService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="courseSubjectAppService"></param>
        public CourseSubjectController( CourseSubjectAppService courseSubjectAppService )
        {
            _courseSubjectAppService = courseSubjectAppService;
        }

        /// <summary>
        /// 获取课程试题列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<CourseSubjectBigQuestionModelOutput> GetList( CourseSubjectModelInput input ) {
            var count = 0;
            var list = _courseSubjectAppService.GetList( input, out count );
            return new JqGridOutPut<CourseSubjectBigQuestionModelOutput>( )
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }


        /// <summary>
        /// 获取我收藏的试题
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<CourseSubjectBigQuestionModelOutput> GetCollectedSubjectList( MyCollectionListInput input ) {
            var count = 0;
            input.UserId = UserObject.Id;
            var list = _courseSubjectAppService.GetCollectedSubject( input,out count);
            return new JqGridOutPut<CourseSubjectBigQuestionModelOutput>( )
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }


        [HttpPost]
        public List<CourseSubjectBigQuestionModelOutput> GetCollectedSubject( MyCollectionListInput input ) {
            input.UserId = UserObject.Id;
            return _courseSubjectAppService.GetCollectedSubjectList( input);
        }


    }
}
