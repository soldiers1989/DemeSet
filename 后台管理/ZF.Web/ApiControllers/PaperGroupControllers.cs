
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：试卷组表 
    /// </summary>
    public class PaperGroupController : BaseController
    {
	   private readonly PaperGroupAppService _paperGroupAppService;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

	   public PaperGroupController(PaperGroupAppService  paperGroupAppService ,OperatorLogAppService operatorLogAppService)
	   {
			_paperGroupAppService =paperGroupAppService;
			_operatorLogAppService = operatorLogAppService;
	   }

	   /// <summary>
       /// 查询列表实体：试卷组表 
       /// </summary>
	   [HttpPost]
        public JqGridOutPut<PaperGroupOutput> GetList(PaperGroupListInput input)
        {
            var count = 0;
            var list = _paperGroupAppService.GetList(input, out count);
            return new JqGridOutPut<PaperGroupOutput>()
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
                var model = _paperGroupAppService.Get(item);
                if (model != null)
				{
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId =  (int)Model.PaperGroup,
                        OperatorType = (int) OperatorType.Delete,
                        Remark = "删除试卷组表:"
                    });
				}
                _paperGroupAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
         /// <summary>
        /// 新增或修改实体：试卷组表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(PaperGroupInput input)
        {
            var data = _paperGroupAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

		 /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[System.Web.Http.HttpPost]
        public PaperGroup GetOne(IdInput input)
        {
            var model = _paperGroupAppService.Get(input.Id);
            return model;
        }

        /// <summary>
        /// 修改试卷发布状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut EditInfoState(PaperGroupInput input)
        {
            return _paperGroupAppService.EditInfoState(input);
        }

    }
}

