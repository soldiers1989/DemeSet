
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;
using ZF.Infrastructure.SmsService;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：SmsendLog 
    /// </summary>
    public class SmsendLogController : BaseController
    {
        private readonly SmsendLogAppService _smsendLogAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public SmsendLogController(SmsendLogAppService smsendLogAppService, OperatorLogAppService operatorLogAppService)
        {
            _smsendLogAppService = smsendLogAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：SmsendLog 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<SmsendLogOutput> GetList(SmsendLogListInput input)
        {
            var count = 0;
            var list = _smsendLogAppService.GetList(input, out count);
            return new JqGridOutPut<SmsendLogOutput>()
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
                var model = _smsendLogAppService.Get(item);
                if (model != null)
                {
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.SmsendLog,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = "删除SmsendLog:"
                    });
                }
                _smsendLogAppService.Delete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public SmsendLog GetOne(IdInput input)
        {
            var model = _smsendLogAppService.Get(input.Id);
            return model;
        }

        /// <summary>
        /// 短信发送
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut SendSmsInfo(SmsModel input)
        {
            var row = _smsendLogAppService.SendSmsInfo(input);
            return row;
        }
    }
}

