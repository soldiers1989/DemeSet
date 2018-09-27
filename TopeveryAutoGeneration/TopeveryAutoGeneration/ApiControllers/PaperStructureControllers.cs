
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：PaperStructure 
    /// </summary>
    public class PaperStructureController : BaseController
    {
	   private readonly PaperStructureAppService _paperStructureAppService;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

	   public PaperStructureController(PaperStructureAppService  paperStructureAppService ,OperatorLogAppService operatorLogAppService)
	   {
			_paperStructureAppService =paperStructureAppService;
			_operatorLogAppService = operatorLogAppService;
	   }

	   /// <summary>
       /// 查询列表实体：PaperStructure 
       /// </summary>
	   [HttpPost]
        public JqGridOutPut<PaperStructureOutput> GetList(PaperStructureListInput input)
        {
            var count = 0;
            var list = _paperStructureAppService.GetList(input, out count);
            return new JqGridOutPut<PaperStructureOutput>()
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
                var model = _paperStructureAppService.Get(item);
                if (model != null)
				{
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId =  (int)Model.PaperStructure,
                        OperatorType = (int) OperatorType.Delete,
                        Remark = "删除PaperStructure:"
                    });
				}
                _paperStructureAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
         /// <summary>
        /// 新增或修改实体：PaperStructure
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(PaperStructureInput input)
        {
            var data = _paperStructureAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

		 /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[System.Web.Http.HttpPost]
        public PaperStructure GetOne(IdInput input)
        {
            var model = _paperStructureAppService.Get(input.Id);
            return model;
        }
    }
}

