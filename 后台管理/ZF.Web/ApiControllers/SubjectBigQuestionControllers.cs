
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：SubjectBigQuestion 
    /// </summary>
    public class SubjectBigQuestionController : BaseController
    {
        private readonly SubjectBigQuestionAppService _subjectBigQuestionAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public SubjectBigQuestionController( SubjectBigQuestionAppService subjectBigQuestionAppService, OperatorLogAppService operatorLogAppService )
        {
            _subjectBigQuestionAppService = subjectBigQuestionAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：SubjectBigQuestion 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<SubjectBigQuestionOutput> GetList( SubjectBigQuestionListInput input )
        {
            var count = 0;
            var list = _subjectBigQuestionAppService.GetList( input, out count );
            return new JqGridOutPut<SubjectBigQuestionOutput>( )
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }


        [HttpPost]
        public JqGridOutPut<SubjectBigQuestionOutput> GetBigQuestionWithSmallQuestion( SubjectBigQuestionInput input )
        {
            var count = 0;
            var list = _subjectBigQuestionAppService.GetBigQuestionWithSmallQuestion( input, out count );
            return new JqGridOutPut<SubjectBigQuestionOutput>( )
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }


        [HttpPost]
        public JqGridOutPut<SubjectBigQuestionOutput> GetBigQuestionExceptExistInChapter( SubjectBigQuestionInputForCourseChapter input )
        {
            var count = 0;
            var list = _subjectBigQuestionAppService.GetBigQuestionExceptExistInChapter( input, out count );
            return new JqGridOutPut<SubjectBigQuestionOutput>( )
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
            var row = _subjectBigQuestionAppService.LogicDelete( input.Ids );
            return row;
        }
        /// <summary>
        /// 新增或修改实体：SubjectBigQuestion
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit( SubjectBigQuestionInput input )
        {
            var data = _subjectBigQuestionAppService.AddOrEdit( input );
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public SubjectBigQuestionOutput GetOne( IdInput input )
        {
            var model = _subjectBigQuestionAppService.GetOne( input.Id );
            model.Option1 = "<p>" + model.Option1 + "</p>";
            model.Option2 = "<p>" + model.Option2 + "</p>";
            model.Option3 = "<p>" + model.Option3 + "</p>";
            model.Option4 = "<p>" + model.Option4 + "</p>";
            model.Option5 = "<p>" + model.Option5 + "</p>";
            model.Option6 = "<p>" + model.Option6 + "</p>";
            model.Option7 = "<p>" + model.Option7 + "</p>";
            model.Option8 = "<p>" + model.Option8 + "</p>";
            return model;
        }

        ///// <summary>
        ///// 返回一条实体记录
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //[System.Web.Http.HttpPost]
        //public int? GetOneId(IdInput input)
        //{
        //    var model = _subjectBigQuestionAppService.GetOneId(input.Id);
        //    return model;
        //}

        /// <summary>
        /// 查询该试题是否已被使用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public bool IfUse( IdInput input )
        {
            return _subjectBigQuestionAppService.IfUse( input.Id );
        }


        /// <summary>
        /// 查询列表实体：SubjectBigQuestion 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<ErrorSubjectFeedBackOutput> GetErrFeedBackList( ErrorSubjectFeedBackInput input )
        {
            var count = 0;
            var list = _subjectBigQuestionAppService.GetErrFeedBackList( input, out count );
            return new JqGridOutPut<ErrorSubjectFeedBackOutput>( )
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list
            };
        }


        /// <summary>
        /// 审核错题反馈
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut Audit( string Id ) {
            return _subjectBigQuestionAppService.Audit( Id);
        }
    }
}

