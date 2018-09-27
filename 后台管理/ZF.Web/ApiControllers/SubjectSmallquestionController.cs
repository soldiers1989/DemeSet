
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：SubjectSmallquestion
    /// </summary>
    public class SubjectSmallquestionController : BaseController
    {
        private readonly SubjectSmallquestionAppService _subjectSmallquestionAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public SubjectSmallquestionController(SubjectSmallquestionAppService subjectSmallquestionAppService, OperatorLogAppService operatorLogAppService)
        {
            _subjectSmallquestionAppService = subjectSmallquestionAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：SubjectSmallquestion 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<SubjectSmallquestionOutput> GetList(SubjectSmallquestionListInput input)
        {
            var count = 0;
            var list = _subjectSmallquestionAppService.GetList(input, out count);
            return new JqGridOutPut<SubjectSmallquestionOutput>()
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
        public MessagesOutPut Delete(IdInputIds input)
        {
            var row = _subjectSmallquestionAppService.LogicDelete(input.Ids);
            return row;
        }
        /// <summary>
        /// 新增或修改实体：SubjectSmallquestion
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(SubjectSmallquestionInput input)
        {
            var data = _subjectSmallquestionAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public SubjectSmallquestionOutput GetOne(IdInput input)
        {
            var model = _subjectSmallquestionAppService.GetOne(input.Id);
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
    }
}

