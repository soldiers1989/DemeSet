
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：CourseSubject 
    /// </summary>
    public class CourseSubjectController : BaseController
    {
	   private readonly CourseSubjectAppService _courseSubjectAppService;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

	   public CourseSubjectController(CourseSubjectAppService  courseSubjectAppService ,OperatorLogAppService operatorLogAppService)
	   {
			_courseSubjectAppService =courseSubjectAppService;
			_operatorLogAppService = operatorLogAppService;
	   }

	   /// <summary>
       /// 查询列表实体：CourseSubject 
       /// </summary>
	   [HttpPost]
        public JqGridOutPut<CourseSubjectOutput> GetList(CourseSubjectListInput input)
        {
            var count = 0;
            var list = _courseSubjectAppService.GetList(input, out count);
            return new JqGridOutPut<CourseSubjectOutput>()
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
                var model = _courseSubjectAppService.Get(item);
                if (model != null)
				{
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId =  (int)Model.CourseSubject,
                        OperatorType = (int) OperatorType.Delete,
                        Remark = "删除CourseSubject:"
                    });
				}
                _courseSubjectAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
         /// <summary>
        /// 新增或修改实体：CourseSubject
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(CourseSubjectInput input)
        {
            var data = _courseSubjectAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

		 /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[System.Web.Http.HttpPost]
        public CourseSubject GetOne(IdInput input)
        {
            var model = _courseSubjectAppService.Get(input.Id);
            return model;
        }
    }
}

