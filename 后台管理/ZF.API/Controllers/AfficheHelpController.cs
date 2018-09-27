using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.AfficheHelpModule;
using ZF.Application.WebApiDto.AfficheHelpModule;

namespace ZF.API.Controllers
{
    public class AfficheHelpController : BaseApiController
    {


        private readonly AfficheHelpAppService _afficheHelpAppService;
        /// <summary>
        /// 查询列表实体：资讯,帮助管理表 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<AfficheHelpOutput> GetList(AfficheHelpListInput input)
        {
            var count = 0;
            input.Sidx = "IsTop,AddTime ";
            var list = _afficheHelpAppService.GetList(input, out count);
            return new JqGridOutPut<AfficheHelpOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }
    }
}
