
using System.Collections.Generic;
using System.Web.Http;
using Topevery.Application.Dto;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;
using ZF.Infrastructure.zTree;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：PaperParamDetail 
    /// </summary>
    public class PaperParamDetailController : BaseController
    {
        private readonly PaperParamDetailAppService _paperParamDetailAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public PaperParamDetailController(PaperParamDetailAppService paperParamDetailAppService, OperatorLogAppService operatorLogAppService)
        {
            _paperParamDetailAppService = paperParamDetailAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：PaperParamDetail 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<PaperParamDetailOutput> GetList(PaperParamDetailListInput input)
        {
            var count = 0;
            var list = _paperParamDetailAppService.GetList(input, out count);
            return new JqGridOutPut<PaperParamDetailOutput>()
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
            return _paperParamDetailAppService.Delete(input);
        }
        /// <summary>
        /// 新增或修改实体：PaperParamDetail
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(PaperParamDetailInput input)
        {
            var data = _paperParamDetailAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public PaperParamDetail GetOne(IdInput input)
        {
            var model = _paperParamDetailAppService.Get(input.Id);
            return model;
        }

        /// <summary>
        /// 手工组卷
        /// </summary>
        /// <param name="input">PaperParamId</param>
        /// <returns></returns>
        public List<Tree.zTree> PaperTreeList(PaperParamDetailListInput input)
        {
            var model = _paperParamDetailAppService.PaperTreeList(input);
            return model;
        }
    }
}

