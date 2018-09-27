using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.BaseDto;
using ZF.Application.WebApiAppService.MyPaperRecordModule;
using ZF.Application.WebApiDto.ChapterExerciseModule;
using ZF.Application.WebApiDto.CoursePaperRecordModule;
using static ZF.Application.WebApiDto.CoursePaperRecordModule.CoursePaperRecordOutput;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CoursePaperRecordController : BaseApiController
    {
        public readonly MyPaperRecordAppService _paperRecordAppService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="myPaperRecordAppService"></param>
        public CoursePaperRecordController(MyPaperRecordAppService myPaperRecordAppService)
        {
            _paperRecordAppService = myPaperRecordAppService;
        }

        /// <summary>
        /// 获取试卷测评记录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<CoursePaperRecordOutput> GetList(CoursePaperRecordInput input)
        {

            int count = 0;
            input.UserId = UserObject.Id;
            var list = _paperRecordAppService.GetList(input, out count);
            return new JqGridOutPut<CoursePaperRecordOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 试卷下拉选择列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<string> GetPaperRecordSelect() {
            return _paperRecordAppService.GetPaperRecordSelect( UserObject.Id );
        }

        /// <summary>
        /// 测评试卷按试卷名称分类
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public JqGridOutPut<CoursePaperClassByPaper> GetPaperClassList( CoursePaperRecordInput input)
        {
            int count = 0;
            input.UserId = UserObject.Id;
            var list = _paperRecordAppService.GetPaperClassList( input, out count );
            return new JqGridOutPut<CoursePaperClassByPaper>( )
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }

        /// <summary>
        /// 获取测评统计结果
        /// </summary>
        /// <param name="paperId"></param>
        /// <param name="paperRecordsId"></param>
        /// <returns></returns>
        [HttpPost]
        public CoursePaperRecordMoreOutput Get( string paperId, string paperRecordsId ) {
            return _paperRecordAppService.Get( paperId,paperRecordsId);
        }

        /// <summary>
        /// 测评试卷总览信息
        /// </summary>
        /// <param name="paperId"></param>
        /// <returns></returns>
        [HttpPost]
        public PaperInfoOutput GetPaperInfo( string paperId ) {
            return _paperRecordAppService.GetPaperInfo( paperId);
        }
    }
}
