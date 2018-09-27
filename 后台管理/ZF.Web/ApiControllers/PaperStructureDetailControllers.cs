
using System.Web.Http;
using Topevery.Application.Dto;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：PaperStructureDetail 
    /// </summary>
    public class PaperStructureDetailController : BaseController
    {
        private readonly PaperStructureDetailAppService _paperStructureDetailAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public PaperStructureDetailController(PaperStructureDetailAppService paperStructureDetailAppService, OperatorLogAppService operatorLogAppService)
        {
            _paperStructureDetailAppService = paperStructureDetailAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：PaperStructureDetail 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<PaperStructureDetailOutput> GetList(PaperStructureDetailListInput input)
        {
            var count = 0;
            var list = _paperStructureDetailAppService.GetList(input, out count);
            return new JqGridOutPut<PaperStructureDetailOutput>()
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
            return _paperStructureDetailAppService.Delete(input);
        }
        /// <summary>
        /// 新增或修改实体：PaperStructureDetail
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(PaperStructureDetailInput input)
        {
            var data = _paperStructureDetailAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public PaperStructureDetail GetOne(IdInput input)
        {
            var model = _paperStructureDetailAppService.Get(input.Id);
            return model;
        }
    }
}

