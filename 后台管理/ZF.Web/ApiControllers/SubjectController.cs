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
using ZF.Infrastructure.RedisCache;

namespace ZF.Web.ApiControllers
{
    public class SubjectController : BaseController
    {
        private readonly SubjectAppService _subjectAppService;

        private readonly OperatorLogAppService _operatorLogAppService;

        public SubjectController(SubjectAppService subjectAppService, OperatorLogAppService operatorLogAppService)
        {
            _subjectAppService = subjectAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        [HttpPost]
        public MessagesOutPut AddOrEdit(SubjectInput input)
        {
            return _subjectAppService.AddOrEdit(input);
        }

        [HttpPost]
        public JqGridOutPut<SubjectOutput> GetList(SubjectListInput input)
        {
            var count = 0;
            var list = _subjectAppService.GetList(input, out count);
            return new JqGridOutPut<SubjectOutput>()
            {
                Page = input.Page,
                Total = count % input.Rows == 0 ? count / input.Rows : count / input.Rows + 1,
                Records = count,
                Rows = list

            };
        }

        [HttpPost]
        public Subject GetOne(IdInput input)
        {
            return _subjectAppService.Get(input.Id);
        }

        [HttpPost]
        public MessagesOutPut Delete(IdInputIds input)
        {

            RedisCacheHelper.Remove("GetProjectSubject");
            RedisCacheHelper.Remove("SubjectTree");
            RedisCacheHelper.Remove("SubjectList");
            RedisCacheHelper.Remove("GetProjectCourseInfo");
            RedisCacheHelper.Remove("SubjectKnowledgePointTree");
            var array = input.Ids.TrimEnd(',').Split(',');
            foreach (var item in array)
            {
                if (_subjectAppService.IfExistCourse(item))
                {
                    return new MessagesOutPut { Id = 1, Message = "该科目下维护有课程信息，请删除课程后再进行操作！", Success = false };
                }
                if (_subjectAppService.IfExistKnowledgePoint(item))
                {
                    return new MessagesOutPut { Id = 1, Message = "该科目下维护有知识点，请删除知识点后再进行操作！", Success = false };
                }
                var model = _subjectAppService.Get(item);
                if (model != null)
                {
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.DataType,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = "删除科目:" + model.SubjectName
                    });
                    _subjectAppService.LogicDelete(model.Id);
                }
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }
    }
}
