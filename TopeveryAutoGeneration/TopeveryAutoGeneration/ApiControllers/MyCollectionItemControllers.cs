
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：MyCollectionItem 
    /// </summary>
    public class MyCollectionItemController : BaseController
    {
	   private readonly MyCollectionItemAppService _myCollectionItemAppService;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

	   public MyCollectionItemController(MyCollectionItemAppService  myCollectionItemAppService ,OperatorLogAppService operatorLogAppService)
	   {
			_myCollectionItemAppService =myCollectionItemAppService;
			_operatorLogAppService = operatorLogAppService;
	   }

	   /// <summary>
       /// 查询列表实体：MyCollectionItem 
       /// </summary>
	   [HttpPost]
        public JqGridOutPut<MyCollectionItemOutput> GetList(MyCollectionItemListInput input)
        {
            var count = 0;
            var list = _myCollectionItemAppService.GetList(input, out count);
            return new JqGridOutPut<MyCollectionItemOutput>()
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
                var model = _myCollectionItemAppService.Get(item);
                if (model != null)
				{
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId =  (int)Model.MyCollectionItem,
                        OperatorType = (int) OperatorType.Delete,
                        Remark = "删除MyCollectionItem:"
                    });
				}
                _myCollectionItemAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
         /// <summary>
        /// 新增或修改实体：MyCollectionItem
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(MyCollectionItemInput input)
        {
            var data = _myCollectionItemAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

		 /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[System.Web.Http.HttpPost]
        public MyCollectionItem GetOne(IdInput input)
        {
            var model = _myCollectionItemAppService.Get(input.Id);
            return model;
        }
    }
}

