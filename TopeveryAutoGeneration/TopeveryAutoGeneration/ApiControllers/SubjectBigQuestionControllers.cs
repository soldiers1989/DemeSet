
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：SubjectBigQuestion 
    /// </summary>
    public class SubjectBigQuestionController : BaseController
    {
	   private readonly SubjectBigQuestionAppService _subjectBigQuestionAppService;

	    /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

	   public SubjectBigQuestionController(SubjectBigQuestionAppService  subjectBigQuestionAppService ,OperatorLogAppService operatorLogAppService)
	   {
			_subjectBigQuestionAppService =subjectBigQuestionAppService;
			_operatorLogAppService = operatorLogAppService;
	   }

	   /// <summary>
       /// 查询列表实体：SubjectBigQuestion 
       /// </summary>
	   [HttpPost]
        public JqGridOutPut<SubjectBigQuestionOutput> GetList(SubjectBigQuestionListInput input)
        {
            var count = 0;
            var list = _subjectBigQuestionAppService.GetList(input, out count);
            return new JqGridOutPut<SubjectBigQuestionOutput>()
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
                var model = _subjectBigQuestionAppService.Get(item);
                if (model != null)
				{
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId =  (int)Model.SubjectBigQuestion,
                        OperatorType = (int) OperatorType.Delete,
                        Remark = "删除SubjectBigQuestion:"
                    });
				}
                _subjectBigQuestionAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
         /// <summary>
        /// 新增或修改实体：SubjectBigQuestion
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(SubjectBigQuestionInput input)
        {
            var data = _subjectBigQuestionAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

		 /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
		[System.Web.Http.HttpPost]
        public SubjectBigQuestion GetOne(IdInput input)
        {
            var model = _subjectBigQuestionAppService.Get(input.Id);
            return model;
        }
    }
}

