using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.AppService;
using static ZF.Application.AppService.MenuAppService;

namespace ZF.API.Controllers
{
    public class MenuController : ApiController
    {
        private readonly MenuAppService _menuAppService;

        public MenuController( MenuAppService menuAppService )
        {
            _menuAppService = menuAppService;
        }
        /// <summary>
        /// 菜單列表
        /// </summary>
        /// <returns></returns>
        public List<MenuModule> GetMenuModuleList( )
        {
            return  _menuAppService.GetMenuModuleList( );
           
        }
    }
}
