
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：试卷组试卷关系表 
    /// </summary>
    public class PaperGroupRelationController : BaseController
    {
	   private readonly PaperGroupRelationAppService _paperGroupRelationAppService;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

	   public PaperGroupRelationController(PaperGroupRelationAppService  paperGroupRelationAppService ,OperatorLogAppService operatorLogAppService)
	   {
			_paperGroupRelationAppService =paperGroupRelationAppService;
			_operatorLogAppService = operatorLogAppService;
	   }

	   /// <summary>
       /// 查询列表实体：试卷组试卷关系表 
       /// </summary>
	   [HttpPost]
        public JqGridOutPut<PaperGroupRelationOutput> GetList(PaperGroupRelationListInput input)
        {
            var count = 0;
            var list = _paperGroupRelationAppService.GetList(input, out count);
            return new JqGridOutPut<PaperGroupRelationOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 查询列表实体：试卷组试卷关系表 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<PaperGroupRelationOutput> GetList1(PaperGroupRelationListInput input)
        {
            var count = 0;
            var list = _paperGroupRelationAppService.GetList1(input, out count);
            return new JqGridOutPut<PaperGroupRelationOutput>()
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
                var model = _paperGroupRelationAppService.Get(item);
                if (model != null)
				{
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId =  (int)Model.PaperGroupRelation,
                        OperatorType = (int) OperatorType.Delete,
                        Remark = "删除试卷组试卷关系表:"
                    });
				}
                _paperGroupRelationAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
         /// <summary>
        /// 新增或修改实体：试卷组试卷关系表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(PaperGroupRelationInput input)
        {
            var data = _paperGroupRelationAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

		 /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[System.Web.Http.HttpPost]
        public PaperGroupRelation GetOne(IdInput input)
        {
            var model = _paperGroupRelationAppService.Get(input.Id);
            return model;
        }
    }
}

