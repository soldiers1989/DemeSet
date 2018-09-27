using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.CourseChapterModule;
using ZF.Application.WebApiDto.ChapterExerciseModule;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ChapterExerciseRecordController : BaseApiController
    {
        private readonly CourseChapterAppService _courseChapterAppService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseChapterAppService"></param>
        public ChapterExerciseRecordController(CourseChapterAppService courseChapterAppService)
        {
            _courseChapterAppService = courseChapterAppService;
        }

        /// <summary>
        /// 获取章节练习记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<CourseChapterExerciseOuput> GetExerciseRecords(CourseChapterExerciseInput input)
        {
            int count = 0;
            input.UserId = UserObject.Id;
            var list = _courseChapterAppService.GetExerciseRecords(input, out count);
            return new JqGridOutPut<CourseChapterExerciseOuput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }




    }
}
