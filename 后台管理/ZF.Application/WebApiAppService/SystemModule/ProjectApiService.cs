using System.Collections.Generic;
using System.Data;
using Castle.Components.DictionaryAdapter;
using Dapper;
using ZF.Application.WebApiDto;
using ZF.Application.WebApiDto.SystemModule;
using ZF.Core.Entity;
using ZF.Core.IRepository;

namespace ZF.Application.WebApiAppService.SystemModule
{

    /// <summary>
    /// 项目分类 项目 科目 webapi服务
    /// </summary>
    public class ProjectApiService : BaseAppService<Project>
    {
        private readonly IProjectRepository _repository;

        private readonly IProjectClassRepository _projectClassRepository;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="projectClassRepository"></param>
        public ProjectApiService(IProjectRepository repository, IProjectClassRepository projectClassRepository) : base(repository)
        {
            _repository = repository;
            _projectClassRepository = projectClassRepository;
        }

        /// <summary>
        /// 获取所有项目分类
        /// </summary>
        /// <returns></returns>
        public List<ProjectClassModel> GetProjectClassAll()
        {
            var strSql = "select a.* from t_Base_ProjectClass a where a.IsDelete=0 order by a.OrderNo asc ";
            return Db.QueryList<ProjectClassModel>(strSql, null);
        }

        /// <summary>
        /// 获取所有项目分类
        /// </summary>
        /// <returns></returns>
        public ProjectClass GetOne(string projectClassId)
        {
            if (string.IsNullOrEmpty(projectClassId))
            {
                return new ProjectClass();
            }
            return _projectClassRepository.Get(projectClassId);
        }

        /// <summary>
        /// 获取项目详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<ProjectMode> GetProjectAll(ProjectModeInput input)
        {
            var strSql = "select a.* from t_Base_Project a where a.IsDelete=0  ";
            var dy = new DynamicParameters();
            if (!string.IsNullOrEmpty(input.ProjectClassId))
            {
                strSql += " and a.ProjectClassId=@ProjectClassId ";
                dy.Add(":ProjectClassId", input.ProjectClassId, DbType.String);
            }
            return Db.QueryList<ProjectMode>(strSql + " ORDER BY orderno asc  ", dy);
        }

        /// <summary>
        /// 获取所有项目
        /// </summary>
        /// <returns></returns>
        public List<ProjectMode> GetProjectAll()
        {
            var strSql = "select a.* from t_Base_Project a where a.IsDelete=0 order by OrderNo DESC ";
            var dy = new DynamicParameters();
            return Db.QueryList<ProjectMode>(strSql, dy);
        }

        /// <summary>
        /// 获取科目详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public List<SubjectModel> GetSubjectAll(SubjectModelInput input)
        {
            var strSql = "select a.* from t_Base_Subject a where a.IsDelete=0 ";
            var dy = new DynamicParameters();
            if (!string.IsNullOrEmpty(input.ProjectId))
            {
                strSql += " and a.ProjectId=@ProjectId ";
                dy.Add(":ProjectId", input.ProjectId, DbType.String);
                return Db.QueryList<SubjectModel>(strSql + " ORDER BY orderno asc  ", dy);
            }
            return new List<SubjectModel>();
        }

        /// <summary>
        /// 获取所有科目  不包含经济基础
        /// </summary>
        /// <returns></returns>
        public List<SubjectModel> GetSubjectAll()
        {
            var strSql = "select a.*,b.ProjectClassId from t_Base_Subject a left join t_Base_Project b on a.ProjectId=b.Id where a.IsDelete=0 and a.IsEconomicBase=0   ORDER BY a.OrderNo ASC";
            var dy = new DynamicParameters();
            return Db.QueryList<SubjectModel>(strSql, dy);
        }

    }
}