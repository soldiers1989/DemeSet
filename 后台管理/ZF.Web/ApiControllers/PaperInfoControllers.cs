
using System.Collections.Generic;
using System.Web.Http;
using Topevery.Application.Dto;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;
using ZF.Infrastructure.PublicPaper;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：PaperInfo 
    /// </summary>
    public class PaperInfoController : BaseController
    {
        private readonly PaperInfoAppService _paperInfoAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public PaperInfoController(PaperInfoAppService paperInfoAppService, OperatorLogAppService operatorLogAppService)
        {
            _paperInfoAppService = paperInfoAppService;
            _operatorLogAppService = operatorLogAppService;

        }

        /// <summary>
        /// 查询列表实体：PaperInfo 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<PaperInfoOutput> GetList(PaperInfoListInput input)
        {
            var count = 0;
            var list = _paperInfoAppService.GetList(input, out count);
            return new JqGridOutPut<PaperInfoOutput>()
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
                var model = new PaperInfo();
                model.Id = item;
                model.IsDelete = 1;
                if (model != null)
                {
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.PaperInfo,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = "删除PaperInfo:"
                    });
                }
                _paperInfoAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
        /// <summary>
        /// 新增或修改实体：PaperInfo
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(PaperInfoInput input)
        {
            var data = _paperInfoAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 自动组卷
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut PaperInfoAddList(PaperInfoInput input)
        {
            var row = _paperInfoAppService.PaperInfoAddList(input);
            return row;
        }
        /// <summary>
        /// 手工组卷
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public MessagesOutPut ManualGroupVolumeAddInfo(PublicPaperInfo inputList)
        {
            var row = _paperInfoAppService.ManualGroupVolumeAddInfo(inputList);
            return row;
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public PaperInfo GetOne(IdInput input)
        {
            var model = _paperInfoAppService.Get(input.Id);
            return model;
        }

        /// <summary>
        /// 查询试卷信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public PaperInfoOutput GetInfo(IdInput id)
        {
            var model = _paperInfoAppService.GetInfo(id.Id);
            return model;
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public MessagesOutPut EditInfo(PaperInfoOutput input)
        {
            var row = _paperInfoAppService.EditInfo(input);
            return row;
        }
        /// <summary>
        /// 修改试卷发布状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MessagesOutPut EditInfoState(PaperInfo input)
        {
            return _paperInfoAppService.EditInfoState(input);
        }

        /// <summary>
        /// 查询试卷是否被使用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public bool IfUse( IdInput input )
        {
            return _paperInfoAppService.IfUse( input.Id );
        }
    }
}

