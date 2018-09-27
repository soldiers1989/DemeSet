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
    public class ProjectClassificationController : BaseController
    {

        private readonly ProjectClassAppService _projectClassAppService;


        private readonly OperatorLogAppService _operatorLogAppService;

        public ProjectClassificationController(ProjectClassAppService projectClassAppService, OperatorLogAppService operatorLogAppService)
        {
            _projectClassAppService = projectClassAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        [HttpPost]
        public JqGridOutPut<ProjectClassOutput> GetList(ProjectClassListInput input)
        {
            var count = 0;
            var list = _projectClassAppService.GetList(input, out count);
            return new JqGridOutPut<ProjectClassOutput>()
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
            RedisCacheHelper.Remove("GetProjectCourseInfo");
            RedisCacheHelper.Remove("SubjectTree");
            RedisCacheHelper.Remove("SubjectKnowledgePointTree");
            var array = input.Ids.TrimEnd(',').Split(',');
            foreach (var item in array)
            {
                var model = _projectClassAppService.Get(item);
                if (model != null)
                {
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.ProjectClass ,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = "删除项目分类:" + model.ProjectClassName
                    });
                    _projectClassAppService.LogicDelete(model.Id);
                }
            }
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }


        // GET: SystemSettings/Details/5
        [System.Web.Http.HttpPost]
        public MessagesOutPut AddOrEdit(ProjectClassInput input)
        {
            var data = _projectClassAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        [System.Web.Http.HttpPost]
        public ProjectClass GetOne(IdInput input)
        {
            var model = _projectClassAppService.Get(input.Id);
            return model;
        }
    }
}
