using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.MyErrorSubjectModule;
using ZF.Application.WebApiDto.ErrorSubjeModule;
using ZF.Application.WebApiDto.MyCollectionItemModule;

namespace ZF.API.Controllers
{
    public class MyErrorSubjectController : BaseApiController
    {
        private readonly MyErrorSubjectAppService _myErrorSubjectAppService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myErrorSubjectAppService"></param>
        public MyErrorSubjectController( MyErrorSubjectAppService myErrorSubjectAppService )
        {
            _myErrorSubjectAppService = myErrorSubjectAppService;
        }

        /// <summary>
        /// 获取错题集
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<ErrorSubjectsModelOutput> GetList( ErrorSubjectModelInput input )
        {
            input.UserId = UserObject.Id;
            var count = 0;
            var list = _myErrorSubjectAppService.GetList( input, out count );
            return new JqGridOutPut<ErrorSubjectsModelOutput>
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 获取错题集-公众号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public List<ErrorSubjectsModelOutput> GetErrList( ErrorSubjectModelInput input ) {
            input.UserId = UserObject.Id;
            return _myErrorSubjectAppService.GetErrList( input);
        }



        /// <summary>
        /// 删除错题
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut RemoveErrorSubject( string id, string type ) {
            return _myErrorSubjectAppService.RemoveErrorSubject( id,type,UserObject.Id);
        }

        /// <summary>
        /// 保存错题练习记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut SavePracticeSubject( SubjectPracticeModelInput input) {
            input.UserId = UserObject.Id;
            return _myErrorSubjectAppService.saveSubjectPracticeRecord( input);
        }

        /// <summary>
        /// 再次练习结果详情
        /// </summary>
        /// <param name="practiceNo"></param>
        /// <returns></returns>
        [HttpPost]
        public List<SubjectPracticeModelOutput> GetPracticeDetail( string practiceNo ) {
            return _myErrorSubjectAppService.GetPracticeDetail( practiceNo);
        }
    }
}
