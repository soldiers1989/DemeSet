
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：ProjectClass 
    /// </summary>
    public class ProjectClassController : BaseController
    {
	   private readonly ProjectClassAppService _projectClassAppService;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

	   public ProjectClassController(ProjectClassAppService  projectClassAppService ,OperatorLogAppService operatorLogAppService)
	   {
			_projectClassAppService =projectClassAppService;
			_operatorLogAppService = operatorLogAppService;
	   }

	   /// <summary>
       /// 查询列表实体：ProjectClass 
       /// </summary>
	   [HttpPost]
        public JqGridOutPut<ProjectClassOutput> GetList(ProjectClassListInput input)
        {
            var count = 0;
            var list = _projectClassAppService.GetList(input, out count);
            return new JqGridOutPut<ProjectClassOutput>()
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
                var model = _projectClassAppService.Get(item);
                if (model != null)
				{
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId =  (int)Model.ProjectClass,
                        OperatorType = (int) OperatorType.Delete,
                        Remark = "删除ProjectClass:"
                    });
				}
                _projectClassAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
         /// <summary>
        /// 新增或修改实体：ProjectClass
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(ProjectClassInput input)
        {
            var data = _projectClassAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

		 /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[System.Web.Http.HttpPost]
        public ProjectClass GetOne(IdInput input)
        {
            var model = _projectClassAppService.Get(input.Id);
            return model;
        }
    }
}

