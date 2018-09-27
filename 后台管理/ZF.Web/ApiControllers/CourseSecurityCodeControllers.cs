
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：课程防伪码管理 
    /// </summary>
    public class CourseSecurityCodeController : BaseController
    {
	   private readonly CourseSecurityCodeAppService _courseSecurityCodeAppService;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

	   public CourseSecurityCodeController(CourseSecurityCodeAppService  courseSecurityCodeAppService ,OperatorLogAppService operatorLogAppService)
	   {
			_courseSecurityCodeAppService =courseSecurityCodeAppService;
			_operatorLogAppService = operatorLogAppService;
	   }

	   /// <summary>
       /// 查询列表实体：课程防伪码管理 
       /// </summary>
	   [HttpPost]
        public JqGridOutPut<CourseSecurityCodeOutput> GetList(CourseSecurityCodeListInput input)
        {
            var count = 0;
            var list = _courseSecurityCodeAppService.GetList(input, out count);
            return new JqGridOutPut<CourseSecurityCodeOutput>()
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
            return _courseSecurityCodeAppService.Delete(input.Ids);
        }


        /// <summary>
        /// 新增或修改实体：CourseSuitDetail
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(CourseSecurityCodeInput input)
        {
            var data = _courseSecurityCodeAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }
    }
}

