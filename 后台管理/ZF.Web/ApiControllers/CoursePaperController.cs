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
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    public class CoursePaperController : BaseController
    {
        private readonly CoursePaperAppService _coursePaperAppService;
        private readonly OperatorLogAppService _operatorLogAppService;
        public CoursePaperController( CoursePaperAppService coursePaperAppService ,OperatorLogAppService operatorLogAppService)
        {
            _operatorLogAppService = operatorLogAppService;
            _coursePaperAppService = coursePaperAppService;
        }

        /// <summary>
        /// 新增或修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(CoursePaperInput input )
        {
            return _coursePaperAppService.AddOrEdit( input );
        }

        /// <summary>
        /// 获取课程试卷详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public CoursePaper GetOne( IdInput input )
        {
            return _coursePaperAppService.Get( input.Id );
        }

        /// <summary>
        /// 获取课程试卷列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<CoursePaperOutput> GetList( CoursePaperListInput input )
        {
            var count = 0;
            var list = _coursePaperAppService.GetList( input, out count );
            return new JqGridOutPut<CoursePaperOutput>( )
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list

            };
        }

        /// <summary>
        /// 获取课程试卷  可添加试卷列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<CoursePaperOutput> GetListAdd(CoursePaperListInput input)
        {
            var count = 0;
            var list = _coursePaperAppService.GetListAdd(input, out count);
            return new JqGridOutPut<CoursePaperOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list

            };
        }
        

        /// <summary>
        /// 删除课程试卷
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut Delete( IdInputIds input ) {
            return _coursePaperAppService.Delete( input);          
        }
    }
}
