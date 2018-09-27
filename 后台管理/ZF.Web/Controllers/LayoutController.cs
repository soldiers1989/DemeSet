using System.Web.Mvc;
using ZF.Application.AppService;

namespace ZF.Web.Controllers
{
    public class LayoutController : BaseController
    {
        private readonly MenuAppService _menuAppService;

        public LayoutController(MenuAppService menuAppService)
        {
            _menuAppService = menuAppService;
        }

        public ActionResult HeadMenu()
        {
            var model = _menuAppService.GetMenuModuleList( );
            return PartialView(model);
        }
    }
}