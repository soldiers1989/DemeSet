
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
    /// 数据表实体Api接口：幻灯片设置表 
    /// </summary>
    public class SlideSettingController : BaseController
    {
	   private readonly SlideSettingAppService _slideSettingAppService;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

	   public SlideSettingController(SlideSettingAppService  slideSettingAppService ,OperatorLogAppService operatorLogAppService)
	   {
			_slideSettingAppService =slideSettingAppService;
			_operatorLogAppService = operatorLogAppService;
	   }

	   /// <summary>
       /// 查询列表实体：幻灯片设置表 
       /// </summary>
	   [HttpPost]
        public JqGridOutPut<SlideSettingOutput> GetList(SlideSettingListInput input)
        {
            var count = 0;
            var list = _slideSettingAppService.GetList(input, out count);
            return new JqGridOutPut<SlideSettingOutput>()
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
            RedisCacheHelper.Remove("GetSlideSettingList");
            var array = input.Ids.TrimEnd(',').Split(',');
            foreach (var item in array)
            {
                var model = _slideSettingAppService.Get(item);
                if (model != null)
				{
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId =  (int)Model.SlideSetting,
                        OperatorType = (int) OperatorType.Delete,
                        Remark = "删除幻灯片设置表:"
                    });
				}
                _slideSettingAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
         /// <summary>
        /// 新增或修改实体：幻灯片设置表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(SlideSettingInput input)
        {
            var data = _slideSettingAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

		 /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[System.Web.Http.HttpPost]
        public SlideSetting GetOne(IdInput input)
        {
            var model = _slideSettingAppService.Get(input.Id);
            return model;
        }
    }
}

