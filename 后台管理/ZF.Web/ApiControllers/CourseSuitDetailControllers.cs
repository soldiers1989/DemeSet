
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
    /// 数据表实体Api接口：CourseSuitDetail 
    /// </summary>
    public class CourseSuitDetailController : BaseController
    {
        private readonly CourseSuitDetailAppService _courseSuitDetailAppService;


        private readonly CoursePackcourseInfoAppService _coursePackcourseInfoAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public CourseSuitDetailController(CourseSuitDetailAppService courseSuitDetailAppService, OperatorLogAppService operatorLogAppService, CoursePackcourseInfoAppService coursePackcourseInfoAppService)
        {
            _courseSuitDetailAppService = courseSuitDetailAppService;
            _operatorLogAppService = operatorLogAppService;
            _coursePackcourseInfoAppService = coursePackcourseInfoAppService;
        }

        /// <summary>
        /// 查询列表实体：CourseSuitDetail 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<CourseInfoOutput> GetList(CourseSuitDetailListInput input)
        {
            var count = 0;
            var list = _courseSuitDetailAppService.GetList(input, out count);
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
            var packCourseId = "";
            var array = input.Ids.TrimEnd(',').Split(',');
            foreach (var item in array)
            {
                var model = _courseSuitDetailAppService.Get(item);
                if (model != null)
                {
                    packCourseId = model.PackCourseId;
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.CourseSuitDetail,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = "删除套餐课程子课程:" + model.PackCourseId
                    });
                }
                _courseSuitDetailAppService.Delete(model);
            }
            RedisCacheHelper.Remove("GetProjectCourseInfo");
            var subjectIds = _courseSuitDetailAppService.GetList(packCourseId);
            var coursePackcourseInfo = _coursePackcourseInfoAppService.Get(packCourseId);
            coursePackcourseInfo.SubjectIds = subjectIds;
            _coursePackcourseInfoAppService.Update(coursePackcourseInfo);
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
        /// <summary>
        /// 新增或修改实体：CourseSuitDetail
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(CourseSuitDetailInput input)
        {
            RedisCacheHelper.Remove("GetProjectCourseInfo");
            var data = _courseSuitDetailAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 批量添加子课程
        /// </summary>
        /// <param name="packCourseId"></param>
        /// <param name="subCourseIds"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddSubCourse(string packCourseId, string subCourseIds)
        {
            RedisCacheHelper.Remove("GetProjectCourseInfo");
            if (!string.IsNullOrWhiteSpace(subCourseIds))
            {
                var idArr = subCourseIds.TrimEnd(',').Split(',');
                foreach (var subCourseId in idArr)
                {
                    _courseSuitDetailAppService.AddOrEdit(new CourseSuitDetailInput
                    {
                        PackCourseId = packCourseId,
                        CourseId = subCourseId
                    });
                }
                var subjectIds = _courseSuitDetailAppService.GetList(packCourseId);
                var coursePackcourseInfo = _coursePackcourseInfoAppService.Get(packCourseId);
                coursePackcourseInfo.SubjectIds = subjectIds;
                _coursePackcourseInfoAppService.Update(coursePackcourseInfo);
                return new MessagesOutPut { Success = true, Message = "添加成功" };
            }
            return new MessagesOutPut { Success = true, Message = "添加成功" };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public CourseSuitDetail GetOne(IdInput input)
        {
            var model = _courseSuitDetailAppService.Get(input.Id);
            return model;
        }

        /// <summary>
        /// 套餐课程是否包含子课程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public bool IsContainsSubCourse( CourseSuitDetailInput input ) {
            return _courseSuitDetailAppService.IsContainsSubCourse( input );
        }


        /// <summary>
        /// 查看课程是否已维护到套餐课程中
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public bool ExistInCoursepack( CourseSuitDetailInput input ) {
            return _courseSuitDetailAppService.ExistInCoursepack( input);
        }

        /// <summary>
        ///
        /// </summary>
        [HttpPost]
        public CourseSuitDetailOutput GetDetail( IdInput input ) {
            return _courseSuitDetailAppService.GetDetail( input);
        }
    }
}

