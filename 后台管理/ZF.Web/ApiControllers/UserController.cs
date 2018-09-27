using System;
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;
using ZF.Infrastructure.Des3;

namespace ZF.Web.ApiControllers
{
    public class UserController : BaseController
    {
        private readonly UserAppService _userAppService;

        private readonly OperatorLogAppService _operatorLogAppService;
        public UserController(UserAppService userAppService, OperatorLogAppService operatorLogAppService)
        {
            _userAppService = userAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        [HttpPost]
        public JqGridOutPut<UserListOutputDto> GetList(UserInput input)
        {
            var count = 0;
            var list = _userAppService.GetList(input, out count);
            return new JqGridOutPut<UserListOutputDto>()
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
                var model = _userAppService.Get(item);
                if (model!=null)
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId =(int) Model.User,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = "删除用户"+model.UserName
                    });
                _userAppService.Delete(model);
            }
           
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }


        // GET: SystemSettings/Details/5
        [System.Web.Http.HttpPost]
        public MessagesOutPut AddOrEdit(UserEditInPut input)
        {
            var data = _userAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        [System.Web.Http.HttpPost]
        public User GetOne(IdInput input)
        {
            var model = _userAppService.Get(input.Id);
            return model;
        }


        [System.Web.Http.HttpPost]
        public MessagesOutPut ModifyPassWord(ModifyPassWordInput input)
        {
            var data = _userAppService.ModifyPassWordInput(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }
    }
}
