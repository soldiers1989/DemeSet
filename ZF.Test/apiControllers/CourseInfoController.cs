using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;

namespace ZF.Test.apiControllers
{
    public class CourseInfoController : ApiController
    {
        private readonly CourseInfoAppService _courseInfoAppService;



        public CourseInfoController( CourseInfoAppService courseInfoAppService )
        {
            _courseInfoAppService = courseInfoAppService;
        }

        [HttpPost]
        public JqGridOutPut<CourseInfoOutput> GetCourseInfoList( CourseInfoListInput input ) {
            var count = 0;
            var list = _courseInfoAppService.GetList( input, out count );
            return new JqGridOutPut<CourseInfoOutput>( )
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }



    }
}
