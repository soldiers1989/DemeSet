using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;

namespace ZF.Web.ApiControllers
{
    public class OperatorLogController : BaseController
    {
        private readonly OperatorLogAppService _operatorLogAppService;

        public OperatorLogController(OperatorLogAppService operatorLogAppService)
        {
            _operatorLogAppService = operatorLogAppService;
        }

        [HttpPost]
        public JqGridOutPut<OperatorLogOutput> GetList(GetListOperatorLogInput input)
        {
            var count = 0;
            var list = _operatorLogAppService.GetList(input, out count);
            return new JqGridOutPut<OperatorLogOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }
    }
}
