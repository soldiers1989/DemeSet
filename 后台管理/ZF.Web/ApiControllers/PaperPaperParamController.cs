
using System.Web.Http;
using Topevery.Application.Dto;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;
namespace ZF.Web.ApiControllers
{
    public class PaperPaperParamController : BaseController
    {
        private readonly PaperPaperParamAppService _paperPaperParamAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public PaperPaperParamController(PaperPaperParamAppService paperPaperParamAppService, OperatorLogAppService operatorLogAppService)
        {
            _paperPaperParamAppService = paperPaperParamAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：PaperPaperParam 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<PaperPaperParamOutput> GetList(PaperPaperParamListInput input)
        {
            var count = 0;
            var list = _paperPaperParamAppService.GetList(input, out count);
            return new JqGridOutPut<PaperPaperParamOutput>()
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
            return _paperPaperParamAppService.Delete(input);
        }
        /// <summary>
        /// 新增或修改实体：PaperPaperParam
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(PaperPaperParamInput input)
        {
            var data = _paperPaperParamAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 修改参数发布状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]

        public MessagesOutPut UpdateState(PaperPaperParamInput input)
        {
            return _paperPaperParamAppService.UpdateState(input);
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public PaperPaperParam GetOne(IdInput input)
        {
            var model = _paperPaperParamAppService.Get(input.Id);
            return model;
        }

        /// <summary>
        /// 根据参数ID查询结构ID与科目ID
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public PaperPaperParamOutput GetListById(PaperPaperParamListInput input)
        {
            var model = _paperPaperParamAppService.GetListById(input);
            return model;
        }
    }
}
