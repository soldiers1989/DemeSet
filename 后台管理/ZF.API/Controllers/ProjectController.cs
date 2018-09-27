using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZF.Application.WebApiAppService;
using ZF.Application.WebApiAppService.SystemModule;
using ZF.Application.WebApiDto;
using ZF.Application.WebApiDto.SystemModule;

namespace ZF.API.Controllers
{
    /// <summary>
    /// 项目分类 项目 科目webapi接口
    /// </summary>
    public class ProjectController : ApiController
    {
        private readonly ProjectApiService _projectApiService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="projectApiService"></param>
        public ProjectController(ProjectApiService projectApiService)
        {
            _projectApiService = projectApiService;
        }

        /// <summary>
        /// 获取所有项目分类
        /// </summary>
        /// <returns></returns>

        [System.Web.Http.HttpPost]
        public List<ProjectClassModel> GetProjectClassAll()
        {
            return _projectApiService.GetProjectClassAll();
        }

        /// <summary>
        /// 获取项目详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [System.Web.Http.HttpPost]
        public List<ProjectMode> GetProjectAll(ProjectModeInput input)
        {
            return _projectApiService.GetProjectAll(input);
        }

        /// <summary>
        /// 获取科目详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [System.Web.Http.HttpPost]
        public List<SubjectModel> GetSubjectAll(SubjectModelInput input)
        {
            return _projectApiService.GetSubjectAll(input);
        }
    }
}
