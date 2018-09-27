
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：CourseResource 
    /// </summary>
    public class CourseResourceController : BaseController
    {
	   private readonly CourseResourceAppService _courseResourceAppService;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

	   public CourseResourceController(CourseResourceAppService  courseResourceAppService ,OperatorLogAppService operatorLogAppService)
	   {
			_courseResourceAppService =courseResourceAppService;
			_operatorLogAppService = operatorLogAppService;
	   }

	   /// <summary>
       /// 查询列表实体：CourseResource 
       /// </summary>
	   [HttpPost]
        public JqGridOutPut<CourseResourceOutput> GetList(CourseResourceListInput input)
        {
            var count = 0;
            var list = _courseResourceAppService.GetList(input, out count);
            return new JqGridOutPut<CourseResourceOutput>()
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
            var array = input.Ids.TrimEnd(',').Split(',');
            foreach (var item in array)
            {
                var model = _courseResourceAppService.Get(item);
                if (model != null)
				{
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId =  (int)Model.CourseResource,
                        OperatorType = (int) OperatorType.Delete,
                        Remark = "删除CourseResource:"
                    });
				}
                _courseResourceAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
         /// <summary>
        /// 新增或修改实体：CourseResource
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(CourseResourceInput input)
        {
            var data = _courseResourceAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

		 /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[System.Web.Http.HttpPost]
        public CourseResource GetOne(IdInput input)
        {
            var model = _courseResourceAppService.Get(input.Id);
            return model;
        }
    }
}

