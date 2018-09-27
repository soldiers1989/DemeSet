using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using ZF.Application.BaseDto;
using ZF.Application.Dto;
using ZF.AutoMapper.AutoMapper;
using ZF.Core.Entity;
using ZF.Core.IRepository;
using ZF.Infrastructure;
using ZF.Infrastructure.RedisCache;

namespace ZF.Application.AppService
{
    /// <summary>
    /// 项目服务 
    /// 20180310
    /// 吴永福
    /// </summary>
    public class ProjectAppService : BaseAppService<Project>
    {
        private readonly IProjectRepository _repository;

        /// <summary>
        /// 操作日志服务
        /// </summary>
        private readonly OperatorLogAppService _operatorLogAppService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="operatorLogAppService"></param>
        public ProjectAppService(IProjectRepository repository, OperatorLogAppService operatorLogAppService) : base(repository)
        {
            _repository = repository;
            _operatorLogAppService = operatorLogAppService;
        }

        /// <summary>
        /// 新增项目分类  t_Base_ProjectClass
        /// </summary>
        public MessagesOutPut AddOrEdit(ProjectInput input)
        {
            RedisCacheHelper.Remove("SubjectKnowledgePointTree");
            RedisCacheHelper.Remove("GetProjectCourseInfo");
            RedisCacheHelper.Remove("SubjectTree");
            RedisCacheHelper.Remove("GetProjectSubject");
            Project model;
            if (!string.IsNullOrEmpty(input.Id))
            {
                model = _repository.Get(input.Id);
                #region 修改逻辑
                model.Id = input.Id;
                model.ProjectClassId = input.ProjectClassId;
                model.ProjectName = input.ProjectName;
                model.OrderNo = input.OrderNo;
                model.Remark = input.Remark;
                model.UpdateUserId = UserObject.Id;
                model.UpdateTime = DateTime.Now;
                #endregion
                _repository.Update(model);
                _operatorLogAppService.Add(new OperatorLogInput
                {
                    KeyId = model.Id,
                    ModuleId = (int)Model.Project,
                    OperatorType = (int)OperatorType.Edit,
                    Remark = "修改项目:" + model.ProjectName
                });
                return new MessagesOutPut { Success = true, Message = "修改成功!" };
            }
            model = input.MapTo<Project>();
            model.Id = Guid.NewGuid().ToString();
            model.AddUserId = UserObject.Id;
            model.AddTime = DateTime.Now;
            var keyId = _repository.InsertGetId(model);
            _operatorLogAppService.Add(new OperatorLogInput
            {
                KeyId = keyId,
                ModuleId = (int)Model.Project,
                OperatorType = (int)OperatorType.Add,
                Remark = "新增项目:" + model.ProjectName
            });
            return new MessagesOutPut { Success = true, Message = "新增成功!" };
        }

        /// <summary>
        /// 获取项目分类集合
        /// </summary>
        public List<ProjectOutput> GetList(ProjectListInput input, out int count)
        {
            const string sql = " select  a.Id,a.ProjectName,b.ProjectClassName,a.Remark,a.AddTime,a.OrderNo ";
            var strSql = new StringBuilder(" from t_Base_Project a left join t_Base_ProjectClass b ON a.ProjectClassId=b.Id   where a.IsDelete=0 ");
            var dynamicParameters = new DynamicParameters();
            const string sqlCount = "select count(*) ";
            if (!string.IsNullOrWhiteSpace(input.ProjectName))
            {
                strSql.Append(" and a.ProjectName like  @ProjectName ");
                dynamicParameters.Add(":ProjectName", '%' + input.ProjectName + '%', DbType.String);
            }
            if (!string.IsNullOrWhiteSpace(input.ProjectClassId))
            {
                strSql.Append(" and a.ProjectClassId =  @ProjectClassId ");
                dynamicParameters.Add(":ProjectClassId", input.ProjectClassId , DbType.String);
            }
            count = Db.ExecuteScalar<int>(sqlCount + strSql, dynamicParameters);
            var list = Db.QueryList<ProjectOutput>(GetPageSql(sql + strSql,
                dynamicParameters,
                input.Page,
                input.Rows, input.Sidx, input.Sord), dynamicParameters);
            return list;
        }
    }
}