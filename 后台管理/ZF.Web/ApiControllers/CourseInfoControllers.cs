using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;
using ZF.Infrastructure.RedisCache;
using System.Collections.Generic;
using ZF.Infrastructure.TwoDimensionalCode;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：CourseInfo 
    /// </summary>
    public class CourseInfoController : BaseController
    {
        private readonly CourseInfoAppService _courseInfoAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public CourseInfoController(CourseInfoAppService courseInfoAppService, OperatorLogAppService operatorLogAppService)
        {
            _courseInfoAppService = courseInfoAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：CourseInfo 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<CourseInfoOutput> GetList(CourseInfoListInput input)
        {
            var count = 0;
            var list = _courseInfoAppService.GetList(input, out count);
            return new JqGridOutPut<CourseInfoOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 查询列表实体：CourseInfo 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<SecurityCodeStatisticsOutput> GetSecurityCodeStatistics(SecurityCodeUsageInput input)
        {
            var count = 0;
            var list = _courseInfoAppService.GetSecurityCodeStatistics(input, out count);
            return new JqGridOutPut<SecurityCodeStatisticsOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 查询列表实体：CourseInfo 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<SecurityCodeUsageOutput> GetSecurityCodeUsage(SecurityCodeUsageInput input)
        {
            var count = 0;
            var list = _courseInfoAppService.GetSecurityCodeUsage(input, out count);
            return new JqGridOutPut<SecurityCodeUsageOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }
        

        [HttpPost]
        public List<CourseInfoOutput> GetTwoDimensionalCodeList(CourseInfoListInput input)
        {
            return _courseInfoAppService.GetTwoDimensionalCode(input);
        }

        /// <summary>
        /// 获取不包括在指定套餐课程中的课程列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<CourseInfoOutput> GetListExceptInPackCourse(CourseInfoListExceptInPackCourse input)
        {
            var count = 0;
            var list = _courseInfoAppService.GetListExceptInPackCourse(input, out count);
            return new JqGridOutPut<CourseInfoOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }



        /// <summary>
        /// 获取所有上架的课程  以及套餐课程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<CourseInfoOutput> GetPackcourseInfoInfoList(CourseInfoListInput input)
        {
            var count = 0;
            var list = _courseInfoAppService.GetPackcourseInfoInfoList(input, out count);
            return new JqGridOutPut<CourseInfoOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }


        /// <summary>
        /// 根据id 删除实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut Delete(IdInputIds input)
        {
            RedisCacheHelper.Remove("GetProjectCourseInfo");
            var array = input.Ids.TrimEnd(',').Split(',');
            foreach (var item in array)
            {
                var model = _courseInfoAppService.Get(item);
                if (model != null)
                {
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.CourseInfo,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = string.Format("删除课程信息:{0}-{1}", model.Id, model.CourseName)
                    });
                }
                _courseInfoAppService.LogicDelete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
        /// <summary>
        /// 新增或修改实体：CourseInfo
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(CourseInfoInput input)
        {
            var data = _courseInfoAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 更新上下架状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut UpdateState(CourseInfoInput input)
        {
            var data = _courseInfoAppService.UpdateState(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 批量上架
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut BatchUpper(IdInputIds id)
        {
            var idArr = id.Ids.Split(',');
            foreach (var item in idArr)
            {
                _courseInfoAppService.UpdateState(new CourseInfoInput { Id = item, State = 1 });
            }
            return new MessagesOutPut { Message = "操作成功", Success = true };
        }

        /// <summary>
        /// 批量下架
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut BatchLower(IdInputIds id)
        {
            var idArr = id.Ids.Split(',');
            foreach (var item in idArr)
            {
                _courseInfoAppService.UpdateState(new CourseInfoInput { Id = item, State = 0 });
            }
            return new MessagesOutPut { Message = "操作成功", Success = true };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public CourseInfo GetOne(IdInput input)
        {
            var model = _courseInfoAppService.Get(input.Id);
            return model;
        }


        /// <summary>
        /// 判断课程中是否有指定讲师
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool ExistTeacherInCourse(CourseInfoInput input)
        {
            return _courseInfoAppService.ExistTeacherInCourse(input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public bool ExistInOrderOrCart(CourseInfoInput input)
        {
            return _courseInfoAppService.ExistInOrderOrCart(input);
        }

        /// <summary>
        /// 更新课程时长
        /// </summary>
        /// <returns></returns>
        public bool updateCourseLongTime(string chapterId)
        {
            return _courseInfoAppService.UpdateCourseLongTime(chapterId);
        }

        /// <summary>
        /// 获取课程二维码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public CourseInfoCode GetTwoDimensionalCode(CourseInfoInput input)
        {
            return _courseInfoAppService.GetTwoDimensionalCode(input);
        }

    }
}

