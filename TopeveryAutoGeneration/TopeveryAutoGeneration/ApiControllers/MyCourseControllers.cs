
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：MyCourse 
    /// </summary>
    public class MyCourseController : BaseController
    {
	   private readonly MyCourseAppService _myCourseAppService;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

	   public MyCourseController(MyCourseAppService  myCourseAppService ,OperatorLogAppService operatorLogAppService)
	   {
			_myCourseAppService =myCourseAppService;
			_operatorLogAppService = operatorLogAppService;
	   }

	   /// <summary>
       /// 查询列表实体：MyCourse 
       /// </summary>
	   [HttpPost]
        public JqGridOutPut<MyCourseOutput> GetList(MyCourseListInput input)
        {
            var count = 0;
            var list = _myCourseAppService.GetList(input, out count);
            return new JqGridOutPut<MyCourseOutput>()
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
                var model = _myCourseAppService.Get(item);
                if (model != null)
				{
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId =  (int)Model.MyCourse,
                        OperatorType = (int) OperatorType.Delete,
                        Remark = "删除MyCourse:"
                    });
				}
                _myCourseAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
         /// <summary>
        /// 新增或修改实体：MyCourse
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(MyCourseInput input)
        {
            var data = _myCourseAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

		 /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[System.Web.Http.HttpPost]
        public MyCourse GetOne(IdInput input)
        {
            var model = _myCourseAppService.Get(input.Id);
            return model;
        }
    }
}

