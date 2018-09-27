
using System.Web.Http;
using ZF.Application.AppService;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.Core.Entity;
using ZF.Infrastructure;

namespace ZF.Web.ApiControllers
{
    /// <summary>
    /// 数据表实体Api接口：SubjectKnowledgePoint 
    /// </summary>
    public class SubjectKnowledgePointController : BaseController
    {
        private readonly SubjectKnowledgePointAppService _subjectKnowledgePointAppService;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        public SubjectKnowledgePointController(SubjectKnowledgePointAppService subjectKnowledgePointAppService, OperatorLogAppService operatorLogAppService)
        {
            _subjectKnowledgePointAppService = subjectKnowledgePointAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 查询列表实体：SubjectKnowledgePoint 
        /// </summary>
        [HttpPost]
        public JqGridOutPut<SubjectKnowledgePointOutput> GetList(SubjectKnowledgePointListInput input)
        {
            var count = 0;
            var list = _subjectKnowledgePointAppService.GetList(input, out count);
            return new JqGridOutPut<SubjectKnowledgePointOutput>()
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
            var array = input.Ids.TrimEnd(',').Split(',');
            foreach (var item in array)
            {
                var model = _subjectKnowledgePointAppService.Get(item);
                if (model != null)
                {
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.SubjectKnowledgePoint,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = "删除科目知识点" + model.KnowledgePointName
                    });
                }
                _subjectKnowledgePointAppService.SubjectKnowledgePointDelete(model);
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
        /// <summary>
        /// 新增或修改实体：SubjectKnowledgePoint
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public MessagesOutPut AddOrEdit(SubjectKnowledgePointInput input)
        {
            var data = _subjectKnowledgePointAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        /// <summary>
        /// 返回一条实体记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [System.Web.Http.HttpPost]
        public SubjectKnowledgePointOutput GetOne(IdInput input)
        {
            var model = _subjectKnowledgePointAppService.GetOnes(input.Id);
            return model;
        }
    }
}

