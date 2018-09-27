
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：文本配置表(联系我们,注册协议) 
    /// </summary>
    public class DanyeController : BaseController
    {
	   private readonly DanyeAppService _danyeAppService;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

	   public DanyeController(DanyeAppService  danyeAppService ,OperatorLogAppService operatorLogAppService)
	   {
			_danyeAppService =danyeAppService;
			_operatorLogAppService = operatorLogAppService;
	   }

	   /// <summary>
       /// 查询列表实体：文本配置表(联系我们,注册协议) 
       /// </summary>
	   [HttpPost]
        public JqGridOutPut<DanyeOutput> GetList(DanyeListInput input)
        {
            var count = 0;
            var list = _danyeAppService.GetList(input, out count);
            return new JqGridOutPut<DanyeOutput>()
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
                var model = _danyeAppService.Get(item);
                if (model != null)
				{
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId =  (int)Model.Danye,
                        OperatorType = (int) OperatorType.Delete,
                        Remark = "删除文本配置表(联系我们,注册协议):"
                    });
				}
                _danyeAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
         /// <summary>
        /// 新增或修改实体：文本配置表(联系我们,注册协议)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(DanyeInput input)
        {
            var data = _danyeAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="inptInput"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public Danye GetOne(DanyeInput inptInput)
        {
            var model = _danyeAppService.GetOne(inptInput);
            return model;
        }
    }
}

