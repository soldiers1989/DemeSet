using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    public class StructureController : BaseController
    {
        private readonly StructureAppService _structureAppService;

        private readonly OperatorLogAppService _operatorLogAppService;

        public StructureController(StructureAppService structureAppService, OperatorLogAppService operatorLogAppService)
        {
            _structureAppService = structureAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        [System.Web.Http.HttpPost]
        public JqGridOutPut<PaperStructureOutput> GetList(PaperStructureListInput input)
        {
            var count = 0;
            var list = _structureAppService.GetList(input, out count);
            return new JqGridOutPut<PaperStructureOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

        [System.Web.Http.HttpPost]
        public MessagesOutPut AddOrEdit(PaperStructureInput input)
        {
            var data = _structureAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }


        [System.Web.Http.HttpPost]
        public PaperStructure GetOne(IdInput input)
        {
            var model = _structureAppService.Get(input.Id);
            return model;
        }

        /// <summary>
        /// 删除试卷结构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut Delete(IdInputIds input)
        {
            return _structureAppService.Delete(input);
        }

    }
}
