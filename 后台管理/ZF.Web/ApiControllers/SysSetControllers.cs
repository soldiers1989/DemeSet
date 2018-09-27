
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
    /// 数据表实体Api接口：网站相关配置表 
    /// </summary>
    public class SysSetController : BaseController
    {
	   private readonly SysSetAppService _sysSetAppService;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

	   public SysSetController(SysSetAppService  sysSetAppService ,OperatorLogAppService operatorLogAppService)
	   {
			_sysSetAppService =sysSetAppService;
			_operatorLogAppService = operatorLogAppService;
	   }

	   /// <summary>
       /// 查询列表实体：网站相关配置表 
       /// </summary>
	   [HttpPost]
        public JqGridOutPut<SysSetOutput> GetList(SysSetListInput input)
        {
            var count = 0;
            var list = _sysSetAppService.GetList(input, out count);
            return new JqGridOutPut<SysSetOutput>()
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
                var model = _sysSetAppService.Get(item);
                if (model != null)
				{
                    RedisCacheHelper.Remove("GetArguValue" + model.ArguName);
                    RedisCacheHelper.Remove(model.ArguName);
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId =  (int)Model.SysSet,
                        OperatorType = (int) OperatorType.Delete,
                        Remark = "删除网站相关配置表:"+ model.ArguName
                    });
				}
                _sysSetAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
         /// <summary>
        /// 新增或修改实体：网站相关配置表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(SysSetInput input)
        {
            var data = _sysSetAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

		 /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[System.Web.Http.HttpPost]
        public SysSet GetOne(IdInput input)
        {
            var model = _sysSetAppService.Get(input.Id);
            return model;
        }
    }
}

