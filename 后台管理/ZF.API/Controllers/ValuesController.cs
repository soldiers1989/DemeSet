using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [System.Web.Mvc.AllowAnonymous]
    public class ValuesController : BaseApiController
    {
        // GET api/values/5
        [HttpGet]
        public string Get()
        {
            return "value";
        }

        private readonly ModuleAppService _moduleAppService;

        private readonly OperatorLogAppService _operatorLogAppService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="moduleAppService"></param>
        /// <param name="operatorLogAppService"></param>
        public ValuesController(ModuleAppService moduleAppService, OperatorLogAppService operatorLogAppService)
        {
            _moduleAppService = moduleAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 获取List集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [System.Web.Mvc.AllowAnonymous]
        public JqGridOutPut<Module> GetList(ListModuleInput input)
        {
            if (input == null)
            {
                input = new ListModuleInput();
            }
            var count = 0;
            var list = _moduleAppService.GetList(input, out count);
            return new JqGridOutPut<Module>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }
    }

}
