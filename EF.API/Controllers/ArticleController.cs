using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.EF.Service;

namespace EF.API.Controllers
{
    [RoutePrefix( "api/Account" )]
    public class ArticleController : ApiController
    {
        private readonly ArticleService _service;
        public ArticleController( ArticleService service ) {
            _service = service;
        }

        [HttpPost]
        public MessagesOutPut AddOrEdit(ArticleInput input ) {
            return _service.AddOrEdit( input);
        }


    }
}
