using System;
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.Dto;
using ZF.Application.BaseDto;
using ZF.Infrastructure;
using ZF.Core.Entity;


namespace ZF.Web.ApiControllers
{
    public class DataTypeController : BaseController
    {
        private readonly DataTypeAppService _dataTypeAppService;
        private readonly OperatorLogAppService _operatorLogAppService;

        public DataTypeController(DataTypeAppService dataTypeAppService, OperatorLogAppService operatorLogAppService)
        {
            _dataTypeAppService = dataTypeAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询字典分类信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [HttpPost]
        public JqGridOutPut<DataTypeOutput> GetList(DataTypeListInput input)
        {
            var count = 0;
            var list = _dataTypeAppService.GetList(input,out count);

            return new JqGridOutPut<DataTypeOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };

        }

        /// <summary>
        /// 新增或修改字典分类信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(DataTypeInput input)
        {
            var data = _dataTypeAppService.AddOrEdit(input);

            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        [HttpPost]
        public MessagesOutPut Delete(IdInputIds input)
        {
            var array = input.Ids.TrimEnd(',').Split(',');
            foreach (var item in array)
            {
                var model = _dataTypeAppService.Get(item);
                if (model != null)
                {
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.DataType,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = "删除字典分类:" + model.DataTypeName
                    });
                    _dataTypeAppService.LogicDelete(model.Id);
                }
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }


        /// <summary>
        /// 修改的查询方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public DataType GetOne(IdInput input)
        {
            var model = _dataTypeAppService.Get(input.Id);
            return model;
        }
    }
}