
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：科目关联书籍管理 
    /// </summary>
    public class SubjectBookController : BaseController
    {
        private readonly SubjectBookAppService _subjectBookAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public SubjectBookController(SubjectBookAppService subjectBookAppService, OperatorLogAppService operatorLogAppService)
        {
            _subjectBookAppService = subjectBookAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：科目关联书籍管理 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<SubjectBookOutput> GetList(SubjectBookListInput input)
        {
            var count = 0;
            var list = _subjectBookAppService.GetList(input, out count);
            return new JqGridOutPut<SubjectBookOutput>()
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
                var model = _subjectBookAppService.Get(item);
                if (model != null)
                {
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.SubjectBook,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = "删除科目关联书籍管理:" + model.BookName
                    });
                }
                _subjectBookAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
        /// <summary>
        /// 新增或修改实体：科目关联书籍管理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(SubjectBookInput input)
        {
            var data = _subjectBookAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public SubjectBook GetOne(IdInput input)
        {
            var model = _subjectBookAppService.Get(input.Id);
            return model;
        }
    }
}

