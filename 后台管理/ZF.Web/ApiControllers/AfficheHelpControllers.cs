
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
    /// 数据表实体Api接口：资讯,帮助管理表 
    /// </summary>
    public class AfficheHelpController : BaseController
    {
	   private readonly AfficheHelpAppService _afficheHelpAppService;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

	   public AfficheHelpController(AfficheHelpAppService  afficheHelpAppService ,OperatorLogAppService operatorLogAppService)
	   {
			_afficheHelpAppService =afficheHelpAppService;
			_operatorLogAppService = operatorLogAppService;
	   }

	   /// <summary>
       /// 查询列表实体：资讯,帮助管理表 
       /// </summary>
	   [HttpPost]
        public JqGridOutPut<AfficheHelpOutput> GetList(AfficheHelpListInput input)
        {
            var count = 0;
            var list = _afficheHelpAppService.GetList(input, out count);
            return new JqGridOutPut<AfficheHelpOutput>()
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
            RedisCacheHelper.Remove("GetAfficheHelp");
            var array = input.Ids.TrimEnd(',').Split(',');
            foreach (var item in array)
            {
                var model = _afficheHelpAppService.Get(item);
                if (model != null)
				{
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId =  (int)Model.AfficheHelp,
                        OperatorType = (int) OperatorType.Delete,
                        Remark = "删除资讯,帮助管理表:"
                    });
				}
                _afficheHelpAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
         /// <summary>
        /// 新增或修改实体：资讯,帮助管理表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(AfficheHelpInput input)
        {
            var data = _afficheHelpAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

		 /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[System.Web.Http.HttpPost]
        public AfficheHelp GetOne(IdInput input)
        {
            var model = _afficheHelpAppService.Get(input.Id);
            return model;
        }
    }
}

