
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：推广公司表 
    /// </summary>
    public class PromoteCompanyController : BaseController
    {
        private readonly PromoteCompanyAppService _promoteCompanyAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public PromoteCompanyController(PromoteCompanyAppService promoteCompanyAppService, OperatorLogAppService operatorLogAppService)
        {
            _promoteCompanyAppService = promoteCompanyAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：推广公司表 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<PromoteCompanyOutput> GetList(PromoteCompanyListInput input)
        {
            var count = 0;
            var list = _promoteCompanyAppService.GetList(input, out count);
            return new JqGridOutPut<PromoteCompanyOutput>()
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
                var model = _promoteCompanyAppService.Get(item);
                if (model != null)
                {
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.PromoteCompany,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = "删除推广公司表:" + model.Name
                    });
                }
                _promoteCompanyAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
        /// <summary>
        /// 新增或修改实体：推广公司表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(PromoteCompanyInput input)
        {
            var data = _promoteCompanyAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public PromoteCompany GetOne(IdInput input)
        {
            var model = _promoteCompanyAppService.Get(input.Id);
            return model;
        }
    }
}

