
using System.Collections.Generic;
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;
using ZF.Infrastructure.RedisCache;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：CourseChapter 
    /// </summary>
    public class CourseChapterController : BaseController
    {
        private readonly CourseChapterAppService _courseChapterAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public CourseChapterController(CourseChapterAppService courseChapterAppService, OperatorLogAppService operatorLogAppService)
        {
            _courseChapterAppService = courseChapterAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：CourseChapter 
        /// </summary>
        [HttpPost]
        public List<CourseChapterOutput> GetList(CourseChapterListInput input)
        {
            var list = _courseChapterAppService.GetList(input);
            return list;
        }


        /// <summary>
        /// 根据id 删除实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut Delete(IdInputIds input)
        {
            var array = input.Ids.TrimEnd(',').Split(',');
            foreach (var item in array)
            {
                var model = _courseChapterAppService.Get(item);
                if (model != null)
                {
                    RedisCacheHelper.Remove("GetCourseChapterList1_" + model.CourseId);
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.CourseChapter,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = string.Format("删除课程章节:{0}-{1}", model.CourseId, model.CapterName)
                    });
                }
                _courseChapterAppService.LogicDelete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }


        /// <summary>
        /// 新增或修改实体：CourseChapter
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(CourseChapterInput input)
        {
            var data = _courseChapterAppService.AddOrEdit(input);
            RedisCacheHelper.Remove("GetCourseChapterList1_" + input.CourseId);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public CourseChapter GetOne(IdInput input)
        {
            var model = _courseChapterAppService.Get(input.Id);
            return model;
        }
    }
}

