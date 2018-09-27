using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;

namespace ZF.Web.ApiControllers
{
    public class ArticleController : BaseController
    {
        private readonly ArticleAppService _service;
        public ArticleController( ArticleAppService service )
        {
            _service = service;
        }

        /// <summary>
        /// 查询列表实体
        /// </summary>
        [HttpPost]
        public JqGridOutPut<ArticleOutput> GetList( ArticleListInput input )
        {
            var count = 0;
            var list = _service.GetList( input, out count );
            return new JqGridOutPut<ArticleOutput>( )
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
        public MessagesOutPut Delete( IdInputIds input )
        {
            var array = input.Ids.TrimEnd( ',' ).Split( ',' );
            foreach ( var item in array )
            {
                var model = _service.Get( item );

                _service.Delete( model );
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
        /// <summary>
        /// 新增或修改实体：资讯,帮助管理表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit( ArticleInput input )
        {
            var data = _service.AddOrEdit( input );
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public Article GetOne( IdInput input )
        {
            var model = _service.Get( input.Id );
            return model;
        }

    }
}
