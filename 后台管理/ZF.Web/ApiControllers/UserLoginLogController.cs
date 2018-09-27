using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;

namespace ZF.Web.ApiControllers
{
    public class UserLoginLogController : BaseController
    {
        private readonly UserLoginLogAppService _userLoginLogAppService;

        private readonly OperatorLogAppService _operatorLogAppService;

        public UserLoginLogController( UserLoginLogAppService userLoginLogAppService, OperatorLogAppService operatorLogAppService )
        {
            _userLoginLogAppService = userLoginLogAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 获取登陆日志列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<UserLoginLogOutput> GetList( UserLoginLogListInput input ) {

            var count = 0;
            //非管理员只显示当前用户登陆信息
            if ( UserObject.IsAdmin == 0 ) {
                input.UserId = UserObject.Id;
            }
            var list = _userLoginLogAppService.GetList( input, out count );
            return new JqGridOutPut<UserLoginLogOutput>( )
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list

            };
        }

        
        /// <summary>
        /// 统计用户登陆数据
        /// </summary>
        /// <param name="yearPart"></param>
        /// <returns></returns>
        [HttpPost]
        public UserLoginLogCountOutput GetStatics(int yearPart) {
            return _userLoginLogAppService.GetStatics(yearPart);
        }
    }
}
