using System;
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.Dto;
using ZF.Application.BaseDto;
using ZF.Infrastructure;
using ZF.Core.Entity;
using System.Collections.Generic;


namespace ZF.Web.ApiControllers
{
    public class RegisterUserController : BaseController
    {

        private readonly RegisterUserAppService _registerUserAppService;
        private readonly OperatorLogAppService _operatorLogAppService;

        public RegisterUserController(RegisterUserAppService registerUserAppService, OperatorLogAppService operatorLogAppService)
        {
            _registerUserAppService = registerUserAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        [HttpPost]
        public JqGridOutPut<RegisterUserOutput> GetList(RegisterUserListInput input)
        {
            var count = 0;
            var list = _registerUserAppService.GetList(input, out count);

            return new JqGridOutPut<RegisterUserOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }
        /// <summary>
        /// 根据注册终端统计用户数量
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<RegisterUserOutput> GetRegisterUserCountByType(string addtime)
        {
            var list = _registerUserAppService.GetRegisterUserCountByType(addtime);
            return list;
        }

        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut UpdateUserSate(IdInputIds id)
        {
            return _registerUserAppService.UpdateUserSate(id);
        }

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public RegisterUser GetOne(IdInput input)
        {
            RegisterUser userInfo = _registerUserAppService.GetOne(input.Id);
            return userInfo;
        }
    }
}
