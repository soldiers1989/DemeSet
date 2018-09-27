using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Application.WebApiAppService.CourseOnTeacherModule;
using ZF.Application.WebApiDto.CourseTeacherModule;
using ZF.Core.Entity;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CourseOnTeacherController : ApiController
    {
        private readonly CourseOnTeacherAppService _courseOnTeacherAppService;
        private static string DefaultDomain = ConfigurationManager.AppSettings["DefuleDomain"];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseOnTeacherAppService"></param>
        public CourseOnTeacherController( CourseOnTeacherAppService courseOnTeacherAppService ) {
            _courseOnTeacherAppService = courseOnTeacherAppService;
        }

        /// <summary>
        /// 获取讲师个人信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public CourseOnTeachers GetInfo( IdInput input ) {
            var model= _courseOnTeacherAppService.Get( input.Id);
            model.TeacherPhoto = DefaultDomain + "/" + model.TeacherPhoto;
            return model;
        }

        /// <summary>
        /// 查询列表实体：CourseOnTeachers 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<CourseOnTeachersOutput> GetList( CourseOnTeachersListInput input )
        {
            var count = 0;
            var list = _courseOnTeacherAppService.GetList( input, out count );
            return new JqGridOutPut<CourseOnTeachersOutput>( )
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }
    }
}
