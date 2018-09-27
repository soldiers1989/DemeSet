
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：SmsendLog 
    /// </summary>
    public class SmsendLogController : BaseController
    {
	   private readonly SmsendLogAppService _smsendLogAppService;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

	   public SmsendLogController(SmsendLogAppService  smsendLogAppService ,OperatorLogAppService operatorLogAppService)
	   {
			_smsendLogAppService =smsendLogAppService;
			_operatorLogAppService = operatorLogAppService;
	   }

	   /// <summary>
       /// 查询列表实体：SmsendLog 
       /// </summary>
	   [HttpPost]
        public JqGridOutPut<SmsendLogOutput> GetList(SmsendLogListInput input)
        {
            var count = 0;
            var list = _smsendLogAppService.GetList(input, out count);
            return new JqGridOutPut<SmsendLogOutput>()
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
                var model = _smsendLogAppService.Get(item);
                if (model != null)
				{
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId =  (int)Model.SmsendLog,
                        OperatorType = (int) OperatorType.Delete,
                        Remark = "删除SmsendLog:"
                    });
				}
                _smsendLogAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
         /// <summary>
        /// 新增或修改实体：SmsendLog
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(SmsendLogInput input)
        {
            var data = _smsendLogAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

		 /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[System.Web.Http.HttpPost]
        public SmsendLog GetOne(IdInput input)
        {
            var model = _smsendLogAppService.Get(input.Id);
            return model;
        }
    }
}

