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
    public class ProjectController : BaseController
    {

        private readonly ProjectAppService _projectAppService;

        private readonly OperatorLogAppService _operatorLogAppService;

        public ProjectController(ProjectAppService projectAppService, OperatorLogAppService operatorLogAppService)
        {
            _projectAppService = projectAppService;
            _operatorLogAppService = operatorLogAppService;
        }

        [HttpPost]
        public JqGridOutPut<ProjectOutput> GetList(ProjectListInput input)
        {
            var count = 0;
            var list = _projectAppService.GetList(input, out count);
            return new JqGridOutPut<ProjectOutput>()
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
                var model = _projectAppService.Get(item);
                if (model != null)
                {
                    _operatorLogAppService.Add(new OperatorLogInput
                    {
                        KeyId = model.Id,
                        ModuleId = (int)Model.ProjectClass,
                        OperatorType = (int)OperatorType.Delete,
                        Remark = "删除项目:" + model.ProjectName
                    });
                    _projectAppService.LogicDelete(model.Id);
                }
            }
            RedisCacheHelper.Remove("GetProjectCourseInfo");
            RedisCacheHelper.Remove("SubjectTree");
            RedisCacheHelper.Remove("SubjectKnowledgePointTree");
            RedisCacheHelper.Remove("GetProjectSubject");
            return new MessagesOutPut { Id = 1, Message = "删除成功!", Success = true };
        }


        // GET: SystemSettings/Details/5
        [System.Web.Http.HttpPost]
        public MessagesOutPut AddOrEdit(ProjectInput input)
        {
            var data = _projectAppService.AddOrEdit(input);
            return new MessagesOutPut { Id = data.Id, Message = data.Message, Success = data.Success };
        }

        [System.Web.Http.HttpPost]
        public Project GetOne(IdInput input)
        {
            var model = _projectAppService.Get(input.Id);
            return model;
        }
    }
}
